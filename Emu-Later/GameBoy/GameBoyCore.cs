using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuLater
{
    class GameBoyCore : IEmulatorCore
    {
        private const int width = 160;
        public int Width
        {
            get { return width; }
        }

        private const int height = 144;
        public int Height
        {
            get { return height; }
        }

        ScreenLevel[,] screen = new ScreenLevel[width, height];
        private Color[] brushes = { Color.Black, Color.DarkGray, Color.LightGray, Color.White };
        Random rand = new Random();

        public void DumpRAM()
        {
            throw new NotImplementedException();
        }

        public void LoadROM(string path)
        {
            //throw new NotImplementedException();
            CorruptScreen();
        }


        public void Render(Graphics g)
        {
            g.Clear(Color.DarkGreen);
            for (int x = 0; x < screen.GetLength(0); x++)
            {
                for (int y = 0; y < screen.GetLength(1); y++)
                {
                    g.FillRectangle(new SolidBrush(brushes[(int)screen[x,y]]), x * GameForm.SCALE, y * GameForm.SCALE, GameForm.SCALE, GameForm.SCALE);
                }
            }
        }

        int kernalsize = 3;
        Color GetBlurredPixel(int x, int y)
        {

            if (kernalsize > 1 && x > 0 && y > 0 && x < Width - 1 && y < Height - 1)
            {
                int r =
                brushes[(int)screen[x - 1, y - 1]].R +
                brushes[(int)screen[x - 1, y]].R +
                brushes[(int)screen[x - 1, y + 1]].R +
                brushes[(int)screen[x, y + 1]].R +
                brushes[(int)screen[x + 1, y + 1]].R +
                brushes[(int)screen[x + 1, y]].R +
                brushes[(int)screen[x + 1, y - 1]].R +
                brushes[(int)screen[x, y - 1]].R +
                brushes[(int)screen[x, y]].R;

                int g =
                brushes[(int)screen[x - 1, y - 1]].G +
                brushes[(int)screen[x - 1, y]].G +
                brushes[(int)screen[x - 1, y + 1]].G +
                brushes[(int)screen[x, y + 1]].G +
                brushes[(int)screen[x + 1, y + 1]].G +
                brushes[(int)screen[x + 1, y]].G +
                brushes[(int)screen[x + 1, y - 1]].G +
                brushes[(int)screen[x, y - 1]].G +
                brushes[(int)screen[x, y]].G;

                int b =
                brushes[(int)screen[x - 1, y - 1]].B +
                brushes[(int)screen[x - 1, y]].B +
                brushes[(int)screen[x - 1, y + 1]].B +
                brushes[(int)screen[x, y + 1]].B +
                brushes[(int)screen[x + 1, y + 1]].B +
                brushes[(int)screen[x + 1, y]].B +
                brushes[(int)screen[x + 1, y - 1]].B +
                brushes[(int)screen[x, y - 1]].B +
                brushes[(int)screen[x, y]].B;
                r /= 7;
                g /= 7;
                b /= 7;
                return Color.FromArgb(Clamp(r, 0, 255), Clamp(g, 0, 255), Clamp(b, 0, 255));
            }
            else
                return brushes[(int)screen[x, y]];
        }

        int Clamp(int val, int min, int max)
        {
            if (val < min)
                return min;
            else if (val > max)
                return max;
            else return val;
        }

        int t = 0;
        public void Run()
        {
            CorruptScreen();
        }

        private void CorruptScreen()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    screen[i, j] = (ScreenLevel)rand.Next(4);
                }
            }
        }

        enum ScreenLevel
        {
            Black,
            Dark,
            Light,
            White,
        }
    }
}
