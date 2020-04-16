using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pain
{
    class CommonData
    {
        List<Ball> b;
        Producer[] p;
        public CommonData(List<Ball> b, Producer[] p)
        {
            this.b = b;
            this.p = p;
        }
        public static readonly int maxSize = 4;
        private Queue<Ball>[] vals =
        {
            new Queue<Ball>(),
            new Queue<Ball>(),
            new Queue<Ball>()
        };
        public void Add(int index, Ball value)
        {
           
            index = Math.Abs(index % 3);
            var q = vals[index];
            Monitor.Enter(q);
            try
            {
                while (q.Count >= maxSize)
                {
                    Monitor.Wait(q);
                }
                q.Enqueue(value);
                p[index].EndMoved();
                //добавление в очередь

                Monitor.PulseAll(q);
            }
            catch (Exception e)
            {

            }
            finally
            {
                
                Monitor.Exit(q);
            }
        }
        public Ball[] GetNextTriplet()
        {
            Ball[] res = { null, null, null };
            for (int i = 0; i < vals.Length; i++)
            {
                var q = vals[i];
                Monitor.Enter(q);
                try
                {
                    while (q.Count == 0)
                    {
                        Monitor.Wait(q);
                    }

                    res[i] = q.Dequeue();
                    
                    Monitor.PulseAll(q);
                }
                catch (Exception e)
                {
                }
                finally
                {
                    Monitor.Exit(q);
                }
            }
            return res;
        }

        

    }
}

