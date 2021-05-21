using System;
using System.Diagnostics;

namespace EmuLater
{
    class FrameCounter
    {
        private int frames = 0;
        private Stopwatch stopwatch = new Stopwatch();
        private double fps;

        public void Start()
        {
            stopwatch.Restart();
        }
        public void Update()
        {
            frames++;

            if (stopwatch.Elapsed.TotalSeconds > 1)
            {
                fps = frames / stopwatch.Elapsed.TotalSeconds;
                frames = 0;
                stopwatch.Restart();
            }
        }

        public double Get()
        {
            return fps;
        }
    }
}
