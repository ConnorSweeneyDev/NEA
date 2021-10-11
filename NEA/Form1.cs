using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace NEA
{
    public partial class NEA_Proj : Form
    {
        private Random rand;
        private Pen pen;
        private SolidBrush brush;
        private Bitmap DrawingArea;
    
        public NEA_Proj()
        {
            InitializeComponent();
            rand = new Random((int)DateTime.Now.Ticks);
            pen = new Pen(Color.White);
            brush = new SolidBrush(Color.White);
        }
        
        private void InitialiseDrawingArea()
        {
            Graphics g = Graphics.FromImage(DrawingArea);
            g.Clear(Color.Black);
        }

        private void LoadProj(object sender, EventArgs e)
        {
            DrawingArea = new Bitmap(this.Width, this.Height, PixelFormat.Format24bppRgb);
            InitialiseDrawingArea();
        }

        private void CloseProj(object sender, FormClosedEventArgs e)
        {
            DrawingArea.Dispose();
        }

        private void PaintProj(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(DrawingArea, 0, 0, DrawingArea.Width,DrawingArea.Height);
        }

        private void NEA_ProjMouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(DrawingArea);
            List<Buffer> buffer = new List<Buffer>();

            for (int i = 0; i < 20; i++)
            {
                int radius = rand.Next(10, 20);
                int PosX = rand.Next(0, this.Width);
                int PosY = rand.Next(0, this.Height);
                int loop = 0;

                buffer.Add(new Buffer() { X = PosX, Y = PosY });

                while (loop == 1)
                {
                    radius = rand.Next(10, 20);
                    PosX = rand.Next(0, this.Width);
                    PosY = rand.Next(0, this.Height);
                }

                brush.Color = Color.FromArgb((rand.Next(25, 255)),
                                             (rand.Next(25, 255)),
                                             (rand.Next(25, 255)));
                
                g.FillEllipse(brush, PosX, PosY, radius, radius);
            }
            Invalidate();
        }
    }

    public class Buffer
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}