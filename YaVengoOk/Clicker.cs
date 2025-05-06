using System.Runtime.InteropServices;
using WindowsInput;

namespace YaVengoOk
{
    public class Clicker
    {
        private readonly InputSimulator sim = new InputSimulator();
        private readonly Random random = new Random();
        private IntPtr _targetWindowHandle;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        public static bool IsOnOriginalWindow = true;
        public static Action<bool>? OnWindowFocusChanged;

        public void InitializeTargetWindow()
        {
            _targetWindowHandle = GetForegroundWindow();
        }

        public async Task StartClicking(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_targetWindowHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(_targetWindowHandle);
                }

                var loopActiveNow = DateTime.Now.AddMinutes(5);

                int activeMinutes = random.Next(10, 13);
                var activeUntil = DateTime.Now.AddMinutes(activeMinutes);

                while (DateTime.Now < activeUntil && !token.IsCancellationRequested)
                {
                    PerformClickSequence();
                    await Task.Delay(random.Next(4000, 12000), token);
                }

                int longPauseSeconds = random.Next(180, 201);
                await Task.Delay(TimeSpan.FromSeconds(longPauseSeconds), token);
            }
        }

        private void PerformClickSequence()
        {
            if (!IsOnOriginalWindow || GetForegroundWindow() != _targetWindowHandle)
                return;

            int clickType = random.Next(0, 10);

            if (clickType <= 6)
            {
                // Un click Simple
                sim.Mouse.LeftButtonClick();
            }
            else if (clickType <= 8)
            {
                // Doble Click Izquierdo
                sim.Mouse.LeftButtonClick();
                Thread.Sleep(random.Next(60, 150)); //Pequeña pausa para hacerlo un poco realista
                sim.Mouse.LeftButtonClick();
            }
            else
            {
                var originalPos = Cursor.Position;

                sim.Mouse.RightButtonClick();
                Thread.Sleep(random.Next(1000, 3000)); //Pequeña pausa para hacerlo un poco realista
                sim.Mouse.MoveMouseBy(-30, 0);
                Thread.Sleep(100);
                sim.Mouse.LeftButtonClick();

                Cursor.Position = originalPos;
            }
        }
    }
}
