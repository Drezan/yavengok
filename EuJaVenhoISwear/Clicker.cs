using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using WindowsInput;

namespace EuJaVenhoISwear
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

                while (DateTime.Now < loopActiveNow && !token.IsCancellationRequested)
                {
                    PerformClickSequence();
                    await Task.Delay(random.Next(8000, 25000), token);
                }

                var stopUntil = DateTime.Now.AddMinutes(10);

                while (DateTime.Now < stopUntil && !token.IsCancellationRequested)
                {
                    Thread.Sleep(random.Next(1000));
                }
            }
        }

        private void PerformClickSequence()
        {
            int clickType = random.Next(0, 10);

            if (clickType <= 5)
            {
                // Un click Simple
                sim.Mouse.LeftButtonClick();
            }
            else if (clickType <= 7)
            {
                // Doble Click Izquierdo
                sim.Mouse.LeftButtonClick();
                Thread.Sleep(random.Next(60, 150)); //Pequeña pausa para hacerlo un poco realista
                sim.Mouse.LeftButtonClick();
            }
            else
            {
                sim.Mouse.RightButtonClick();
                Thread.Sleep(random.Next(1000, 3000)); //Pequeña pausa para hacerlo un poco realista
                sim.Mouse.MoveMouseBy(5, 5);
                sim.Mouse.LeftButtonClick();
            }
        }
    }
}
