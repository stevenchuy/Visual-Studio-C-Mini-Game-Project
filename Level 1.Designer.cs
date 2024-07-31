namespace MultiLevel_Game_Project_V._3
{
    partial class Level_1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Timer = new System.Windows.Forms.Timer(components);
            playerLabel = new Label();
            timerLabel = new Label();
            robotLabel = new Label();
            highestScoreLabel = new Label();
            SuspendLayout();
            // 
            // Timer
            // 
            Timer.Enabled = true;
            Timer.Interval = 30;
            Timer.Tick += TimerEvent;
            // 
            // playerLabel
            // 
            playerLabel.AutoSize = true;
            playerLabel.Location = new Point(79, 26);
            playerLabel.Name = "playerLabel";
            playerLabel.Size = new Size(112, 25);
            playerLabel.TabIndex = 0;
            playerLabel.Text = "Player Score:";
            // 
            // timerLabel
            // 
            timerLabel.AutoSize = true;
            timerLabel.Location = new Point(636, 26);
            timerLabel.Name = "timerLabel";
            timerLabel.Size = new Size(59, 25);
            timerLabel.TabIndex = 1;
            timerLabel.Text = "Time: ";
            // 
            // robotLabel
            // 
            robotLabel.AutoSize = true;
            robotLabel.Location = new Point(1211, 30);
            robotLabel.Name = "robotLabel";
            robotLabel.Size = new Size(92, 25);
            robotLabel.TabIndex = 2;
            robotLabel.Text = "Bot Score:";
            // 
            // highestScoreLabel
            // 
            highestScoreLabel.AutoSize = true;
            highestScoreLabel.Location = new Point(89, 68);
            highestScoreLabel.Name = "highestScoreLabel";
            highestScoreLabel.Size = new Size(131, 25);
            highestScoreLabel.TabIndex = 3;
            highestScoreLabel.Text = "Highest Score: ";
            // 
            // Level_1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1478, 744);
            Controls.Add(highestScoreLabel);
            Controls.Add(robotLabel);
            Controls.Add(timerLabel);
            Controls.Add(playerLabel);
            Name = "Level_1";
            Text = "Level_1";
            WindowState = FormWindowState.Maximized;
            Paint += PaintEvent;
            KeyDown += KeyIsDown;
            KeyUp += KeyIsUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer Timer;
        private Label playerLabel;
        private Label timerLabel;
        private Label robotLabel;
        private Label highestScoreLabel;
    }
}