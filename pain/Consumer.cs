using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pain
{
    class Consumer
    {

        private Thread t;
        private CommonData d;
        private static Consumer cons = null;
      
        List<Ring> ring;
        Rectangle rect;
        List<Ball> balls;
        Producer[] p;

        private Consumer(CommonData d, List<Ring> r, List<Ball> b, Rectangle rect, Producer[] p)
        {
            this.d = d;
            this.balls = b;
            this.rect = rect;
            this.ring = r;
            this.p = p;
            Start();
        }
        public static Consumer getInstance(CommonData d,List<Ring>r, List<Ball>b, Rectangle rect, Producer[] p)
        {
           
            if (cons == null) cons = new Consumer(d,r,b,rect,p);
            
            return cons;
            // вся эта магия, чтобы cons был единственной
        }
        private void Start()
        {
            ThreadStart th = new ThreadStart(Consume);
            t = new Thread(th);
            t.Start();
        }
        
        private void Consume()
        {
            //Producer.EndMove = true;
            while (true)
            {
                var value = d.GetNextTriplet();
                int bright = 0;
                int red = 0;
                int green = 0;
                int blue = 0;
                if (!value.Contains(null))
                {
                    //foreach (var v in value)
                    //{

                    //    bright += v.BgColor.A;
                    //    red += v.BgColor.R;
                    //    green += v.BgColor.G;
                    //    blue += v.BgColor.B;
                    //    v = null;
                    //}
                    for(int k =0; k < Producer.valNum; k++)
                    {
                        bright += value[k].BgColor.A;
                        red += value[k].BgColor.R;
                        green += value[k].BgColor.G;
                        blue += value[k].BgColor.B;
                        Monitor.Enter(balls);

                        balls.Remove(value[k]);

                        Monitor.Exit(balls);
                        value[k] = null;

                       
                    }
                    

                    Color s = Color.FromArgb(bright % 255, red % 255, green % 255, blue % 255);
                    
                   
                    Ring r = new Ring(s, rect);
                    //Producer.EndMove = false;
                    r.Start();
                    Monitor.Enter(ring);
                    ring.Add(r);
                    Monitor.Exit(ring);
                    value = null;
                    for (int i = 0; i < Producer.valNum; i++)
                    {
                        p[i].EndMoveF();
                        //Thread.Sleep(1000);
                    }
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
                cons = null;
            }
        }
    }
}
