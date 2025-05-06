using MetroFramework.Forms;

namespace YaVengoOk
{
    partial class Form1 : MetroForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Start = new Button();
            Stop = new Button();
            panelStatus = new Panel();
            lblTimer = new Label();
            Timer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // Start
            // 
            Start.BackColor = Color.FromArgb(51, 51, 51);
            Start.FlatStyle = FlatStyle.Flat;
            Start.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Start.ForeColor = Color.SpringGreen;
            Start.Location = new Point(60, 87);
            Start.Name = "Start";
            Start.Size = new Size(386, 83);
            Start.TabIndex = 0;
            Start.Text = "Start";
            Start.UseMnemonic = false;
            Start.UseVisualStyleBackColor = false;
            Start.Click += buttonStart_Click;
            // 
            // Stop
            // 
            Stop.BackColor = Color.FromArgb(51, 51, 51);
            Stop.FlatStyle = FlatStyle.Flat;
            Stop.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Stop.ForeColor = Color.SpringGreen;
            Stop.Location = new Point(60, 335);
            Stop.Name = "Stop";
            Stop.Size = new Size(386, 83);
            Stop.TabIndex = 1;
            Stop.Text = "Stop";
            Stop.UseVisualStyleBackColor = false;
            Stop.Click += Stop_Click;
            // 
            // panelStatus
            // 
            panelStatus.BackColor = Color.Gray;
            panelStatus.Location = new Point(244, 206);
            panelStatus.Name = "panelStatus";
            panelStatus.Size = new Size(16, 16);
            panelStatus.TabIndex = 2;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTimer.ForeColor = Color.SpringGreen;
            lblTimer.Location = new Point(129, 225);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(253, 81);
            lblTimer.TabIndex = 0;
            lblTimer.Text = "00:00:00";
            // 
            // Timer
            // 
            Timer.Interval = 1000;
            Timer.Tick += timeCounter_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 255);
            ClientSize = new Size(509, 509);
            ControlBox = false;
            Controls.Add(lblTimer);
            Controls.Add(panelStatus);
            Controls.Add(Stop);
            Controls.Add(Start);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Padding = new Padding(0, 60, 0, 0);
            Style = MetroFramework.MetroColorStyle.Green;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Start;
        private Button Stop;
        private Panel panelStatus;
        private Label lblTimer;
        private System.Windows.Forms.Timer Timer;
    }
}
