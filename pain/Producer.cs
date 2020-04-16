using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pain
{
    class Producer
    {
        private int width, heigth;
       
        private List<Ball> balls = new List<Ball>();

        private static Random rand = null;
        public Color FgColor { get; private set; }
        public Color BgColor { get; private set; }
        private Thread t = null;
        private bool stop = false;
        private CommonData d;
        public bool EndMove = false;
        public bool IsAlive { get { return t != null && t.IsAlive; } }

        public static readonly int valNum = 3;
        private int valIndex; //NUM
        //Ball b;
       
        public Producer(CommonData d, Rectangle rect1, int valIndex,List<Ball> ball )
        {
            balls = ball;
            Update(rect1);
            this.valIndex = Math.Abs(valIndex % valNum);
            this.d=d;
            //var rect = new Rectangle(0, 0, width, heigth);
            //Ball b = new Ball(d, rect, valIndex);
            //b.Start();
            //Monitor.Enter(balls);
            //balls.Add(b);
            //Ball.Count++;
            //Monitor.Exit(balls);
            //stop = true;
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
                ThreadStart th = new ThreadStart(Produce);
                t = new Thread(th);
                t.Start();
            }
           
        }
        

        private void Produce()
        {
            while (true)
            {
                var rect = new Rectangle(0, 0, width, heigth);
                var b1 = new Ball(d, rect, valIndex);
                Monitor.Enter(balls);
                balls.Add(b1);
                Ball.Count++;
                Monitor.Exit(balls);



                if (!EndMove && balls!=null)
                {
                    EndMove = true;
                    Monitor.Enter(balls);
                    foreach (var b in balls)
                    {


                        b.Start();

                       


                        //if (balls.Exists(x => x != b))
                        //{

                        //    EndMove = false;
                        //}

                    }
                    Monitor.Exit(balls);
                }
                //if (!EndMove)
                //{
                //    foreach (var b in balls)
                //    {
                //        b.Move();
                //    }
                //}


            }
            
                
               
           
        }
        public void Abort()
        {
            try
            {
                t.Abort();
                t.Join();
            }
            catch (Exception e)
            {
                //cons = null;
            }
        }

        public void  EndMoved()
        {
            EndMove = false;
        }
        public void EndMoved1()
        {
            EndMove = true;
        }
    }

    
}

/*
           Monitor.Enter(balls);
           foreach (var b in balls)
           {
               if (valIndex == 0)
               {
                   if (b.X < width / 2)
                   {
                       b.Move();
                   }
                   else { d.Add(valIndex, b); }

               }
               if (valIndex == 1)
               {
                   if (b.X > width / 2)
                   {
                       b.Move();
                   }
                   else { d.Add(valIndex, b); }

               }
               if (valIndex == 2)
               {
                   if (b.Y < heigth / 2)
                   {
                       b.Move();
                   }
                   else { d.Add(valIndex, b); }

               }
               Monitor.Exit(balls);*/
