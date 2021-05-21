namespace EmuLater
{
    partial class GameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.drawPanel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpRAMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.togglePauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawPanel
            // 
            this.drawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPanel.Location = new System.Drawing.Point(0, 0);
            this.drawPanel.Name = "drawPanel";
            this.drawPanel.Size = new System.Drawing.Size(264, 247);
            this.drawPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(264, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.dumpRAMToolStripMenuItem,
            this.togglePauseToolStripMenuItem,
            this.showStatsToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // dumpRAMToolStripMenuItem
            // 
            this.dumpRAMToolStripMenuItem.Name = "dumpRAMToolStripMenuItem";
            this.dumpRAMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dumpRAMToolStripMenuItem.Text = "Dump RAM";
            this.dumpRAMToolStripMenuItem.Click += new System.EventHandler(this.dumpRAMToolStripMenuItem_Click);
            // 
            // togglePauseToolStripMenuItem
            // 
            this.togglePauseToolStripMenuItem.Name = "togglePauseToolStripMenuItem";
            this.togglePauseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.togglePauseToolStripMenuItem.Text = "Play";
            this.togglePauseToolStripMenuItem.Click += new System.EventHandler(this.togglePauseToolStripMenuItem_Click);
            // 
            // showStatsToolStripMenuItem
            // 
            this.showStatsToolStripMenuItem.Name = "showStatsToolStripMenuItem";
            this.showStatsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showStatsToolStripMenuItem.Text = "Show Stats";
            this.showStatsToolStripMenuItem.Click += new System.EventHandler(this.showStatsToolStripMenuItem_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 247);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.drawPanel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "GameForm";
            this.Text = "Emu-Later";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel drawPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dumpRAMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem togglePauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showStatsToolStripMenuItem;
    }
}

