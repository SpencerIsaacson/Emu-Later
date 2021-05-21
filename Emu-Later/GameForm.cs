using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using static EmuLater.WindowsMessageFunctions;

namespace EmuLater
{
    public partial class GameForm : Form
    {
        private IEmulatorCore core;
        public const int SCALE = 16;
        private bool continueRender = true;
        private bool showStats = false;
        private FrameCounter FPS = new FrameCounter();
        private bool paused = false;

        public GameForm()
        {
            InitializeComponent();
            core = new Chip8Core();
            core.LoadROM("Chip8/c8games/INVADERS");
        }

        void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while(ApplicationIsIdle)
            {
                if (!paused)
                    core.Run();
            }
        }

        private void Render()
        {
            BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(drawPanel.CreateGraphics(), drawPanel.DisplayRectangle);

            FPS.Start();

            while (continueRender)
            {
                FPS.Update();
                core.Render(bg.Graphics);
                if (showStats)
                {
                    bg.Graphics.DrawString(String.Format("{0:F2} FPS", FPS.Get()), SystemFonts.DefaultFont, Brushes.Yellow, 0, 0);
                }
                bg.Render();
            }
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            continueRender = false;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(core.Width * SCALE, core.Height * SCALE);
            Application.Idle += OnApplicationEnterIdle;
            Task.Run(() => { Render(); });
        }

        private void dumpRAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.DumpRAM();
        }

        private void togglePauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paused = !paused;
            togglePauseToolStripMenuItem.Text = (paused) ? "Play" : "Pause";
        }

        private void showStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showStats = !showStats;
        }
    }
}