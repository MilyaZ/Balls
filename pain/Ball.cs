using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pain
{
    class Ball
    {
        private int width, heigth;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Num { get; private set; }

        public int BallD { get; private set; } = 50;

        private static Random rand = null;
        public Color FgColor { get; private set; }
        public Color BgColor { get; private set; }
        private int dx, dy;
        private Thread t = null;
        private bool stop = false;
        public static int Count  = 0;
        private static int MaxCount = 10;
        public bool IsAlive { get { return t != null && t.IsAlive; } }

        private CommonData d;
        public Ball(CommonData d, Rectangle r, int num)
        {
            this.d = d;
            Update(r); //чтобы можно было менять размеры панели на ходу
            Num = num;
            if (rand == null) rand = new Random();
            int color = rand.Next(10, 255);
            if (num == 0)
            {
                BgColor = Color.FromArgb(color,rand.Next(0,100), rand.Next(0,100), 255);
                //X = -BallD;
                X = 0;
                Y = r.Height / 2;
                dx = rand.Next(1, 6);
                dy = 0;
            }
            if (num == 1)
            {
                
                BgColor = Color.FromArgb(color, rand.Next(0,100), 255, rand.Next(0,100));
                X = r.Width-BallD;
                Y = r.Height / 2;
                dx = rand.Next(-6, -1);
                dy = 0;
            }
            if (num == 2)
            {
                
                BgColor = Color.FromArgb(color,255, rand.Next(0,100), rand.Next(0,100));
                X = r.Width / 2;
                Y = 0;
                dx = 0;
                dy = rand.Next(1, 6);
            }

        }
        public void Update(Rectangle r)
        {
            width = r.Width;
            heigth = r.Height;
        }
        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }
        public void Stop() 
        {
            if (t != null)
            {
                stop = true;
                t.Abort();
                t.Join();
            }
        }
        public void Move()
        {
            while (!stop)
            {
                if (d != null)
                {
                    Thread.Sleep(30);
                    if (Num == 0)
                    {
                        if (X < width / 2)
                        {
                            X += dx;
                            Y += dy;
                        }
                        else
                        {
                            d.Add(Num, this); 
                            this.Stop();
                        }

                    }
                    if (Num == 1)
                    {
                        if (X > width / 2)
                        {
                            X += dx;
                            Y += dy;
                        }
                        else
                        {
                            d.Add(Num, this);
                            this.Stop();
                        }

                    }
                    if (Num == 2)
                    {
                        if (Y < heigth / 2)
                        {
                            X += dx;
                            Y += dy;
                        }
                        else
                        {
                            d.Add(Num, this);
                            this.Stop();
                        }

                    }
                }
            }
        }
    }
}
