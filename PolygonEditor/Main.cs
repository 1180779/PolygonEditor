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
        bool moving = false; // moving item with left mouse button
        Polygon? selectedP = null; // selected polygon (item within polygon) with left mouse click

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
                    if (p == selectedP)
                    {
                        p.DrawSelected(g, pen, Brushes.Black, Brushes.LightGreen);
                    }
                    else
                    {
                        p.Draw(g, pen, Brushes.Black);
                    }
                }
                pen.Dispose();
            }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ML = e.Location;
                foreach (var p in polygons)
                {
                    if (p.SelectedItem(ML) || p.Selected(ML))
                    {
                        // p is the selected polygon
                        selectedP = p;
                        moving = true;
                        canvas.Invalidate();
                        return;
                    }
                }
                // no polygon selected
                selectedP = null;
                moving = false;
                canvas.Invalidate();
            }
        }


        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedP == null || !moving)
                return;

            PML = ML;
            ML = e.Location;
            selectedP.MoveWHoleOrVertex(PML, ML);
            canvas.Invalidate();
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            // polygon still selected, but no longer moving
            if (e.Button == MouseButtons.Left)
                moving = false;
            canvas.Invalidate();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && selectedP != null)
            {
                if (!selectedP.RemoveVertex())
                {
                    Signal_Production();
                }
                else
                {
                    selectedP = null;
                    canvas.Invalidate();
                }
            }
            if (e.KeyCode == Keys.N && selectedP != null)
            {
                if (selectedP.AddVertex())
                {
                    selectedP = null;
                    canvas.Invalidate();
                }
            }
        }
    }
}
