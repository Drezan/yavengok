using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YaVengoOk
{
    public class Switcher
    {
        private readonly InputSimulator sim = new InputSimulator();
        private readonly Random random = new Random();
        private record WeightedProcess(string Name, int Weight);
        private readonly List<WeightedProcess> weightedProcesses = new()
        {
            new WeightedProcess("code", 5),
            new WeightedProcess("devenv", 4),
            new WeightedProcess("msedge", 3),
            new WeightedProcess("GitHubDesktop", 1),
            new WeightedProcess("azuredatastudio", 1)
        };
        private List<IntPtr> validWindowHandles = new();
        private int currentIndex = 0;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;
        private const int SW_MAXIMIZE = 3;


        public void StartSwitching(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                int waitTime = random.Next(2, 3) * 60 * 1000; // 5-9 mins.
                Thread.Sleep(waitTime);

                if (token.IsCancellationRequested) break;

                TrySwitchToNextAvailableWindow();
            }
        }

        private void TrySwitchToNextAvailableWindow()
        {
            var weightedList = BuildWeightedProcessList();
            var tried = new HashSet<string>();

            while (tried.Count < weightedProcesses.Count)
            {
                string processName = weightedList[random.Next(weightedList.Count)];

                if (tried.Contains(processName)) continue;
                tried.Add(processName);

                var hwnd = GetWindowHandleIfProcessRunning(processName);

                if (hwnd.HasValue && IsWindow(hwnd.Value))
                {
                    sim.Keyboard.KeyPress(VirtualKeyCode.MENU);
                    Thread.Sleep(50);

                    ShowWindow(hwnd.Value, SW_RESTORE);
                    Thread.Sleep(50);

                    ShowWindow(hwnd.Value, SW_MAXIMIZE);
                    Thread.Sleep(50);

                    SetForegroundWindow(hwnd.Value);
                    break;
                }
            }
        }
        private IntPtr? GetWindowHandleIfProcessRunning(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (var proc in processes)
            {
                if (proc.MainWindowHandle != IntPtr.Zero)
                {
                    return proc.MainWindowHandle;
                }

                // Si está minimizado, esperar y reintentar brevemente
                Thread.Sleep(100);
                if (proc.MainWindowHandle != IntPtr.Zero)
                {
                    return proc.MainWindowHandle;
                }
            }
            return null;
        }

        private List<string> BuildWeightedProcessList()
        {
            return weightedProcesses
                .SelectMany(p => Enumerable.Repeat(p.Name, p.Weight))
                .ToList();
        }

    }
}
