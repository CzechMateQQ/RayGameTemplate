using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MatrixHeirarchy
{
    class Timer
    {
        Stopwatch stopwatch = new Stopwatch();

        private long currentTime = 0;
        private long lastTime = 0;

        private float deltaTime = 0.005f;

        public Timer()
        {
            stopwatch.Start();
        }

        public void Reset()
        {
            stopwatch.Reset();
        }

        public float Seconds
        {
            get { return stopwatch.ElapsedMilliseconds / 1000.0f; }
        }

        public float GetDeltaTime()
        {
            lastTime = currentTime;
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;
            return deltaTime;
        }
    }
}
