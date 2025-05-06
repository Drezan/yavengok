using WindowsInput;
using WindowsInput.Native;

namespace YaVengoOk
{
    public class Switcher
    {
        private readonly InputSimulator sim = new InputSimulator();
        private readonly Random random = new Random();

        public void StartSwitching(CancellationToken token)
        {
            bool isSwitched = false;

            while (!token.IsCancellationRequested)
            {
                int waitTime = random.Next(10, 16) * 60 * 1000; // 10-15 mins.
                int waited = 0;

                while (waited < waitTime && !token.IsCancellationRequested)
                {
                    Thread.Sleep(1000); // Espera 1 segundo
                    waited += 1000; // Aumenta el tiempo esperado
                }

                if (token.IsCancellationRequested) break;

                //Cambiar ventana
                PerformSwitchSequence();
                isSwitched = !isSwitched;
            }
        }

        private void PerformSwitchSequence()
        {
            sim.Keyboard.KeyDown(VirtualKeyCode.MENU);
            Thread.Sleep(100);
            sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
            Thread.Sleep(100);
            sim.Keyboard.KeyUp(VirtualKeyCode.MENU);

            Clicker.IsOnOriginalWindow = !Clicker.IsOnOriginalWindow;
            Clicker.OnWindowFocusChanged?.Invoke(Clicker.IsOnOriginalWindow);
        }
    }
}
