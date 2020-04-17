using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BallMove
{
    class Producer
    {

        public delegate void Progress(int value);

        public event Progress EventProgress;

        private int width, heigth;
       
        public Color FgColor { get; private set; }
        public Color BgColor { get; private set; }
        private Thread t = null;
        private CommonData d;
        public bool EndMove = false;
        public bool IsAlive { get { return t != null && t.IsAlive; } }
        public static readonly int valNum = 3;
        private int valIndex; //NUM
        List<Ball> balls;
        private List<Queue<Ball>> needBall = new List<Queue<Ball>>() {new Queue<Ball>(), new Queue<Ball>(), new Queue<Ball>() };
    public Producer(CommonData d, Rectangle rect1, int valIndex,List<Ball>balls)
        {
           this.balls = balls;
            Update(rect1);
            this.valIndex = Math.Abs(valIndex % valNum);
            this.d=d;
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
                //stop = false;
                ThreadStart th = new ThreadStart(Produce);
                t = new Thread(th);
                t.Start();
            }
           
        }
        

        private void Produce()
        {
            while (true)
            {
                EventProgress?.Invoke(needBall[valNum - 1].Count);
                if (!EndMove && needBall[valNum-1].Count != 0)
                {

                    EndMove = true;
                    Thread.Sleep(300);
                    var first = needBall[valNum - 1].Dequeue();
                    first.Start();
                }
                if (needBall[valNum - 1].Count <= 4)
                {
                    Thread.Sleep(1000);
                    if(needBall[valNum - 1].Count>=2) Thread.Sleep(3000);
                   
                    var rect = new Rectangle(0, 0, width, heigth);
                    var b1 = new Ball(d, rect, valIndex);
                    Monitor.Enter(balls);
                    //balls.Add(b1);
                    needBall[valNum - 1].Enqueue(b1);
                    Monitor.Enter(balls);
                    balls.Add(b1);
                    Monitor.Exit(balls);
                    Ball.Count++;
                    Monitor.Exit(balls);
                }
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
               
            }
        }

        public void  EndMoveF()
        {
            EndMove = false;
        }
    }
}

