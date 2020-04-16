using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pain
{
    class Animator
    {

        private Graphics mainG;
        private int width, heigth;
        private List<Ball> balls;
        private List<Ring> rings;
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;
        private object obj = new object();
        public Animator(Graphics g, Rectangle r,List<Ball> b, List<Ring> ring)
        {
            Update(g, r, b,ring);
        }
        public void Update(Graphics g, Rectangle r, List<Ball> ball, List<Ring> ring)
        {
            rings = ring;
            balls = ball;
            mainG = g;
            width = r.Width;
            heigth = r.Height;
            Monitor.Enter(obj);
            bg = BufferedGraphicsManager.Current.Allocate(mainG, new Rectangle(0, 0, width, heigth));
            Monitor.Exit(obj);
            Monitor.Enter(balls);
            foreach (var b in balls)
            {
                b.Update(r);
            }
            Monitor.Exit(balls);

        }
        private void Animate()
        {
            while (!stop)
             {
                Monitor.Enter(obj);
                Graphics g = bg.Graphics;
                g.Clear(Color.White);
                Monitor.Exit(obj);

                Monitor.Enter(rings);

                foreach (var r in rings)
                {
                    Brush br = new SolidBrush(r.RingColor);
                    g.FillEllipse(br, r.X, r.Y, 2 * r.Radius, 2 * r.Radius);
                }
                Monitor.Exit(rings);

                Monitor.Enter(balls);
                foreach (var b in balls)
                {
                    Brush br = new SolidBrush(b.BgColor);
                    g.FillEllipse(br, b.X, b.Y, b.BallD, b.BallD);
                    Pen p = new Pen(b.FgColor, 2);
                    g.DrawEllipse(p, b.X, b.Y, b.BallD, b.BallD);
                }
                Monitor.Exit(balls);
                try
                {
                    Monitor.Enter(obj);
                    bg.Render();
                    Monitor.Exit(obj);
                }
                catch (Exception e) { }
                Thread.Sleep(30);

            }
        }


        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
            }
          

        }
        public void Stop()
        {
            stop = true;
            Monitor.Enter(balls);
            foreach (var b in balls)
            {
                b.Stop();
            }
            balls.Clear();
            Monitor.Exit(balls);
            Monitor.Enter(rings);
            foreach (var r in rings)
            {
                r.Stop();
            }
            rings.Clear();
            Monitor.Exit(rings);

        }
    }
}
