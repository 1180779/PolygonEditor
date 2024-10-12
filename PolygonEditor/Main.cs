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
using PolygonEditor.Geometry.Objects;

namespace PolygonEditor
{
    public partial class Main : Form
    {
        bool libraryDrawing = true;
        bool moving = false; // moving item with left mouse button
        Polygon? selectedP = null; // selected polygon (item within polygon) with left mouse click
        PolygonFactory pFactory;

        DirectBitmap drawArea;
        List<Polygon> polygons = [];

        Point2 ML = new(-1, 0); // mouse location
        Point2 PML = new(-1, 0); // prevoius mouse location

        public Main()
        {
            InitializeComponent();

            PolluteRadioRestrs();
            PolluteRadioRestrLineRestr();

            drawArea = new DirectBitmap(canvas.Width, canvas.Height);
            canvas.Image = drawArea.Bitmap;

            Point center = new(canvas.Width / 2, canvas.Height / 2);
            pFactory = new(center);
            polygons.Add(pFactory.Generate());
            canvas.Invalidate();
        }
        private void DrawPolygons(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {

            foreach (var poly in polygons)
            {
                if (poly == selectedP)
                    poly.DrawSelected(dbitmap, g, p, b, s);
                else
                    poly.Draw(dbitmap, g, p, b);
            }

        }
        private void DrawLibraryPolygons(Graphics g, Pen p, Brush b, Brush s)
        {
            foreach (var poly in polygons)
            {
                if (poly == selectedP)
                    poly.DrawLibrarySelected(g, p, b, s);
                else
                    poly.DrawLibrary(g, p, b);
            }
        }
        public static void TestSignal()
        {
            About a = new();
            a.ShowDialog();
        }
        private void RemoveSelectedVertex()
        {
            if (selectedP == null)
                return;
            if (!selectedP.RemoveVertex())
            {
                // add new dialog 
                TestSignal();
            }
            else
            {
                selectedP = null;
                canvas.Invalidate();
            }
        }
        private void AddNewVertex()
        {
            if (selectedP == null)
                return;
            if (selectedP.AddVertex())
            {
                selectedP = null;
                canvas.Invalidate();
            }
        }
        private List<RadioButton> radioRestrs = [];
        private void PolluteRadioRestrs()
        {
            radioRestrs.EnsureCapacity(4);
            radioRestrs.Add(radioRestrNoRestr);
            radioRestrs.Add(radioRestrHorizontal);
            radioRestrs.Add(radioRestrVertical);
            radioRestrs.Add(radioRestrConst);
        }
        private void EnableRadioRestrictions(bool state = true)
        {
            foreach (var radioRestr in radioRestrs)
                radioRestr.Enabled = state;
        }
        private void UncheckRadioRestrictionsButOne(RadioButton radioRestrOn)
        {
            foreach (var radioRestr in radioRestrs)
            {
                if (radioRestr == radioRestrOn)
                    continue;
                radioRestr.Checked = false;
            }
        }
        private void UncheckRadioRestrictions()
        {
            foreach (var radioRestr in radioRestrs)
                radioRestr.Checked = false;
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            using Graphics g = Graphics.FromImage(drawArea.Bitmap);
            g.Clear(Color.White);
            Pen p = new(Color.Black);
            if (libraryDrawing)
                DrawLibraryPolygons(g, p, Brushes.Black, Brushes.LightGreen);
            else
                DrawPolygons(drawArea, g, p, Brushes.Black, Brushes.LightGreen);
            p.Dispose();
        }
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                EnableRadioRestrictions(false);
                UncheckRadioRestrictions();

                // disable buttons
                buttonRemoveVertex.Enabled = false;
                buttonAddVertex.Enabled = false;
                buttonLineToBezier.Enabled = false;

                ML = e.Location;
                foreach (var p in polygons)
                {
                    if (p.SelectedItem(ML) || p.IsSelected(ML))
                    {
                        // p is the selected polygon
                        selectedP = p;
                        moving = true;
                        canvas.Invalidate();

                        if (selectedP.selectedVertex != null)
                        {
                            buttonRemoveVertex.Enabled = true;
                        }
                        else if (selectedP.selectedLine != null)
                        {
                            buttonAddVertex.Enabled = true;

                            EnableRadioRestrictions();
                            buttonLineToBezier.Enabled = true;

                            if (selectedP.selectedLine.Restriction == Line.LineRestriction.None)
                                radioRestrNoRestr.Checked = true;
                            else if (selectedP.selectedLine.Restriction == Line.LineRestriction.Horizontal)
                                radioRestrHorizontal.Checked = true;
                            else if (selectedP.selectedLine.Restriction == Line.LineRestriction.Vertical)
                                radioRestrVertical.Checked = true;
                            else if (selectedP.selectedLine.Restriction == Line.LineRestriction.ConstantLength)
                                radioRestrConst.Checked = true;
                        }
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
            selectedP.MoveWHoleOrVertex(ML - PML);
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
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedVertex();
            }
            if (e.KeyCode == Keys.N && selectedP != null)
            {
                AddNewVertex();
            }
        }

        private void buttonRemoveVertex_Click(object sender, EventArgs e)
        {
            RemoveSelectedVertex();
            buttonRemoveVertex.Enabled = false;
        }

        private void buttonAddVertex_Click(object sender, EventArgs e)
        {
            AddNewVertex();
            buttonAddVertex.Enabled = false;
        }

        private Dictionary<RadioButton, Line.LineRestriction> radioRestrLineRestr = new() { };
        private void PolluteRadioRestrLineRestr()
        {
            radioRestrLineRestr.Add(radioRestrNoRestr, Line.LineRestriction.None);
            radioRestrLineRestr.Add(radioRestrHorizontal, Line.LineRestriction.Horizontal);
            radioRestrLineRestr.Add(radioRestrVertical, Line.LineRestriction.Vertical);
            radioRestrLineRestr.Add(radioRestrConst, Line.LineRestriction.ConstantLength);
        }
        private void radioRestr_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioRestr = (RadioButton)sender;
            if (radioRestr.Checked == false)
                return;
            selectedP!.selectedLine!.Restriction = radioRestrLineRestr[radioRestr];
            UncheckRadioRestrictionsButOne(radioRestr);
        }
        private void buttonLineToBezier_Click(object sender, EventArgs e)
        {
            selectedP.ConvertLineToBezier();
            selectedP = null;
            UncheckRadioRestrictions();
            EnableRadioRestrictions(false);
            buttonLineToBezier.Enabled = false;
            canvas.Invalidate();
        }
        private void radioAlgLib_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            if (r.Checked == true)
            {
                radioAlgMy.Checked = false;
                libraryDrawing = true;
            }
        }

        private void radioAlgMy_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            if (r.Checked == true)
            {
                radioAlgLib.Checked = false;
                libraryDrawing = false;
            }
        }

        private void buttonNewPolygon_Click(object sender, EventArgs e)
        {
            polygons.Add(pFactory.Generate(int.Parse(textBoxVerteciesC.Text), int.Parse(textBoxRadius.Text)));
        }

        private void buttonRemovePolygon_Click(object sender, EventArgs e)
        {
            if (selectedP != null && selectedP.selectedVertex == null &&
                selectedP.selectedLine == null && selectedP.selectedBezierLine == null)
            {
                polygons.Remove(selectedP);
                selectedP = null;
            }
        }
    }
}
