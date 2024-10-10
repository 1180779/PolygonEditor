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
        bool change = false;
        Polygon? movingP = null;

        DirectBitmap drawArea;
        List<Polygon> polygons = new();

        Point ML = new Point(-1, 0); // mouse location
        Point PML = new Point(-1, 0); // prevoius mouse location
        private void Signal_Production()
        {
            About a = new About();
            a.ShowDialog();
        }
        public Main()
        {
            InitializeComponent();

            drawArea = new DirectBitmap(canvas.Width, canvas.Height);
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                g.Clear(Color.White);
            }
            canvas.Image = drawArea.Bitmap;



            Point center = new Point(canvas.Width / 2, canvas.Height / 2);
            PolygonFactory f = new(center);
            Polygon p = f.Generate();

            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                Pen pen = new Pen(Color.Black);
                p.Draw(g, pen, Brushes.Black);
                pen.Dispose();
            }
            polygons.Add(p);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(drawArea.Bitmap))
            {
                g.Clear(Color.White);
                Pen pen = new Pen(Color.Black);
                foreach (var p in polygons)
                {
                    p.Draw(g, pen, Brushes.Black);
                }
                pen.Dispose();
            }
        }
        
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            ML = e.Location;
            foreach (var p in polygons)
            {
                if (p.SelectedVertex(ML) || p.Selected(ML)) 
                {
                    movingP = p;
                    change = true;
                    return;
                }
            }
        }


        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(!change) 
                return;

            PML = ML;
            ML = e.Location;
            movingP?.MoveWHoleOrVertex(PML, ML);
            canvas.Invalidate();
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            canvas.Invalidate();
            change = false;
            movingP = null;
        }
    }
}
