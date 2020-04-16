using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pain
{
    public partial class Form1 : Form
    {
    

        Animator a;
        private List<Ball> balls = new List<Ball>();
        private List<Ring> rings = new List<Ring>();
        Consumer c;
        Producer[] p;
        CommonData d;
        public Form1()
        {
            InitializeComponent();
            a = new Animator(panel1.CreateGraphics(), panel1.ClientRectangle,balls,rings);
        }


        private void panel1_Click_1(object sender, EventArgs e)
        {
            a.Start();
           
            p = new Producer[3];
            d = new CommonData();
            for (int i = 0; i < Producer.valNum; i++)
            {
                p[i] = new Producer(d, panel1.ClientRectangle, i, balls);
                p[i].Start();

            }
            c = Consumer.getInstance(d, rings,balls, panel1.ClientRectangle,p);//вместо конструктора
            //Thread.Sleep(30000);
            //c.Abort();
            //for (int i = 0; i < Producer.valNum; i++)
            //{
            //    p[i].Abort();
            //}


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            a.Stop();
            c.Abort();
            for(int i = 0; i < Producer.valNum; i++)
            {
                p[i].Abort();
            }
            
        }
    }
}
