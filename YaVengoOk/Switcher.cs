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
        private readonly List<string> processNames = new()
        {
            "code",
            "devenv",
            "GitHubDesktop",
            "msedge",
            "azuredatastudio"
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
            int attempts = 0;
            int total = processNames.Count;

            while (attempts < total)
            {
                string processName = processNames[currentIndex];
                var hwnd = GetWindowHandleIfProcessRunning(processName);

                currentIndex = (currentIndex + 1) % processNames.Count;

                if (hwnd.HasValue && IsWindow(hwnd.Value))
                {
                    // Simular una interacción humana (presionar ALT brevemente)
                    sim.Keyboard.KeyPress(VirtualKeyCode.MENU);
                    Thread.Sleep(50);

                    // Restaurar ventana (por si está minimizada)
                    ShowWindow(hwnd.Value, SW_RESTORE);
                    Thread.Sleep(50);

                    // Llevar la ventana al frente
                    SetForegroundWindow(hwnd.Value);
                    break;
                }

                attempts++;
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

    }
}
