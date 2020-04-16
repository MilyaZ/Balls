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
        private List<Ball> balls;
        List<Ring> ring;
        Rectangle rect;

        Producer[] p;

        private Consumer(CommonData d, List<Ball> balls, List<Ring> r, Rectangle rect, Producer[] p)
        {
            this.d = d;
            this.balls = balls;
            this.rect = rect;
            this.ring = r;
            this.p = p;
            Start();
        }
        public static Consumer getInstance(CommonData d, List<Ball> balls,List<Ring>r,Rectangle rect, Producer[] p)
        {
           
            if (cons == null) cons = new Consumer(d,balls,r,rect,p);
            
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
                int red = 0;
                int green = 0;
                int blue = 0;

                foreach (var v in value)
                {
                    red += v.BgColor.R;
                    green += v.BgColor.G;
                    blue += v.BgColor.B;
                    Monitor.Enter(balls);
                    v.Stop();
                    balls.Remove(v);
                    
                    Monitor.Exit(balls);
                }
                Color s = Color.FromArgb(220, red%255, green % 255, blue % 255);
                for (int i =0; i < Producer.valNum; i++)
                {
                    p[i].EndMoved();
                }
                //Producer.EndMove = false;
                Ring r = new Ring(s, rect);
                //Producer.EndMove = false;
                r.Start();
                Monitor.Enter(ring);
                ring.Add(r);
                Monitor.Exit(ring);

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
