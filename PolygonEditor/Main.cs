using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PolygonEditor.Objects;

namespace PolygonEditor
{
    public partial class Main : Form
    {
        DirectBitmap drawArea;
        public Main()
        {
            InitializeComponent();

            drawArea = new DirectBitmap(canvas.Width, canvas.Height);
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap)) 
            {
                g.Clear(Color.White);
            }
            canvas.Image = drawArea.Bitmap;



            List<Item> Items = new();
            Point center = new Point(canvas.Width / 2, canvas.Height / 2);
            Vertex c = new Vertex(center);

            int R = 100;
            int[] angles = { 150, 30, -90 };

            Vertex[] vertices = new Vertex[3];
            for (int i = 0; i < 3; ++i) 
            {
                vertices[i] = new Vertex(center);
                vertices[i].X += (int)(R * Math.Cos((double) angles[i] / 360 * 2 * Math.PI));
                vertices[i].Y += (int)(R * Math.Sin((double) angles[i] / 360 * 2 * Math.PI));
            }

            Line[] lines = new Line[3];
            for(int i = 0; i < 3; ++i) 
            {
                lines[i] = new Line(vertices[i], vertices[(i + 1) % 3]);
            }
            for(int i = 0; i < 3; ++i)
            {
                Items.Add(vertices[i]);
                Items.Add(lines[i]);
            }

            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                Pen p = new Pen(Color.Black);
                foreach(var item in Items) 
                {
                    item.Draw(g, p);
                }
                c.Draw(g, p);

                p.Dispose();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        
    }
}
