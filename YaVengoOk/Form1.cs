using System.Runtime.InteropServices;

namespace YaVengoOk
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private CancellationTokenSource? _cts;
        private TimeSpan timePassed;

        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HTCAPTION = 0x0002;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        public Form1()
        {
            InitializeComponent();

            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;

            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.ControlBox = true;

            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.FormBorderStyle = FormBorderStyle.None;

            this.Padding = new Padding(0, 30, 0, 30);

            this.MouseDown += Form1_MouseDown;

            MakePanelCircle(panelStatus);
            panelStatus.BackColor = Color.Gray;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetDarkTheme(this);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start Clicker and Switcher");

            if (_cts == null || _cts.IsCancellationRequested)
            {
                Console.WriteLine("Clicking");

                _cts = new CancellationTokenSource();
                panelStatus.BackColor = Color.LimeGreen;
                Timer.Start();
                Task.Run(() => 
                {
                    try
                    {
                        var clicker = new Clicker();
                        clicker.InitializeTargetWindow();
                        clicker.StartClicking(_cts.Token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Clicker Error: " + ex.Message);
                    }
                });
                Task.Run(() => 
                {
                    try
                    {
                        var switcher = new Switcher();
                        switcher.StartSwitching(_cts.Token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Switcher Error: " + ex.Message);
                    }
                    
                });
            }

            Console.WriteLine("Token Cancelled or Loop ended.");
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                Timer.Stop();
                lblTimer.Text = "00:00:00";
                timePassed = TimeSpan.Zero;
                panelStatus.BackColor = Color.Gray;
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;
                Console.WriteLine("Stop and Cleaning done.");
            }
        }

        private void SetDarkTheme(Control control)
        {
            foreach (Control child in control.Controls)
            {
                if (child is MetroFramework.Interfaces.IMetroControl metroControl)
                {
                    metroControl.Theme = MetroFramework.MetroThemeStyle.Dark;
                    metroControl.Style = MetroFramework.MetroColorStyle.Green;
                }
                SetDarkTheme(child);
            }
        }

        private void MakePanelCircle(Panel panel)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, panel.Width, panel.Height);
            panel.Region = new Region(path);
        }

        private void timeCounter_Tick(object sender, EventArgs e)
        {
            timePassed = timePassed.Add(TimeSpan.FromSeconds(1));
            lblTimer.Text = timePassed.ToString(@"hh\:mm\:ss");
        }
    }
}
