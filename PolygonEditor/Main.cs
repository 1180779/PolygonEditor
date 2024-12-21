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

using PolygonEditor.Geometry;

namespace PolygonEditor
{
    public partial class Main : Form
    {
        bool libraryDrawing = true;
        bool moving = false; // moving mouse
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

            PolluteRadioCons();
            PolluteRadioConVertexCon();

            drawArea = new DirectBitmap(1920, 1080);
            canvas.Image = drawArea.Bitmap;

            Point center = new(canvas.Width / 2, canvas.Height / 2);
            pFactory = new(center);
            polygons.Add(pFactory.GeneratePredefined());
            canvas.Invalidate();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // REMOVING VERTICES HELPER FUNCTIONS
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

        private void RemoveSelectedVertex()
        {
            if (selectedP == null)
                return;
            if (!selectedP.RemoveVertex())
            {
                throw new Exception();
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

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // DRAWING
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

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
        private void DrawLibraryPolygons(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            foreach (var poly in polygons)
            {
                if (poly == selectedP)
                    poly.DrawLibrarySelected(dbitmap, g, p, b, s);
                else
                    poly.DrawLibrary(dbitmap, g, p, b);
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            using Graphics g = Graphics.FromImage(drawArea.Bitmap);
            g.Clear(Color.White);
            Pen p = new(Color.Black);
            if (libraryDrawing)
                DrawLibraryPolygons(drawArea, g, p, Brushes.Black, Brushes.LightGreen);
            else
                DrawPolygons(drawArea, g, p, Brushes.Black, Brushes.LightGreen);
            p.Dispose();
        }

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // MOUSE DOWN EVENTS
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                textBoxLength.Text = "";

                EnableRadioRestrictions(false);
                UncheckRadioRestrictions();

                EnableRadioCons(false);
                UncheckRadioCons();

                moving = true;

                // disable buttons
                buttonAddVertex.Enabled = false;
                buttonRemoveVertex.Enabled = false;
                buttonLineToBezier.Enabled = false;
                buttonBezierToLine.Enabled = false;
                buttonRemovePolygon.Enabled = false;

                ML = e.Location;
                Polygon p;
                for (int i = polygons.Count - 1; i >= 0; i--)
                {
                    p = polygons[i];
                    if (p.SelectedItem(ML) || p.IsSelected(ML)) // selected polygon or polygon part
                    {
                        // p is the selected polygon
                        selectedP = p;
                        canvas.Invalidate();

                        if (selectedP.selectedVertex != null) // selected vertex
                        {
                            buttonRemoveVertex.Enabled = true;
                            if (selectedP.selectedVertex.HasOneBezier)
                            {
                                EnableRadioCons();
                                if (selectedP.selectedVertex.ContinuityType == PVertex.Continuity.G0)
                                    radioConG0.Checked = true;
                                else if (selectedP.selectedVertex.ContinuityType == PVertex.Continuity.G1)
                                    radioConG1.Checked = true;
                                else if (selectedP.selectedVertex.ContinuityType == PVertex.Continuity.C1)
                                    radioConC1.Checked = true;
                            }
                        }
                        else if (selectedP.selectedLine != null) // selected line
                        {
                            textBoxLength.Text = selectedP.selectedLine.Length.ToString();
                            buttonAddVertex.Enabled = true;

                            EnableRadioRestrictions();
                            buttonLineToBezier.Enabled = true;

                            if (selectedP.selectedLine.Restriction == PEdge.LineRestriction.None)
                                radioRestrNoRestr.Checked = true;
                            else if (selectedP.selectedLine.Restriction == PEdge.LineRestriction.Horizontal)
                                radioRestrHorizontal.Checked = true;
                            else if (selectedP.selectedLine.Restriction == PEdge.LineRestriction.Vertical)
                                radioRestrVertical.Checked = true;
                            else if (selectedP.selectedLine.Restriction == PEdge.LineRestriction.ConstantLength)
                                radioRestrConst.Checked = true;
                        }
                        else if (selectedP.selectedBezierLine != null) // selected bezier line
                        {
                            buttonBezierToLine.Enabled = true;
                        }
                        else // selected polygon
                        {
                            buttonRemovePolygon.Enabled = true;
                        }
                        return;
                    }
                }
                // no polygon selected
                selectedP = null;
                canvas.Invalidate();
            }
        }

        private void MoveCanvas(Vec2 v)
        {
            foreach (var p in polygons)
                p.Move(v);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!moving)
                return;

            PML = ML;
            ML = e.Location;
            if (selectedP == null)
            {
                MoveCanvas(ML - PML);
            }
            else
            {
                selectedP.MoveWHoleOrVertex(ML - PML);
            }
            canvas.Invalidate();
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            // polygon still selected, but no longer moving
            if (e.Button == MouseButtons.Left)
                moving = false;
            canvas.Invalidate();
        }

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // KEY EVENTS 
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            // TO DO:
            // add proper key support

            //if (e.KeyCode == Keys.Delete)
            //{
            //    RemoveSelectedVertex();
            //}
            //if (e.KeyCode == Keys.N && selectedP != null)
            //{
            //    AddNewVertex();
            //}
        }

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // BUTTONS -> ADD AND REMOVE VERTICES 
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

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


        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // RADIO BUTTONS -> RESTRICTIONS ON LINES 
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

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
        private void UncheckRadioRestrictions()
        {
            foreach (var radioRestr in radioRestrs)
                radioRestr.Checked = false;
        }

        private Dictionary<RadioButton, PEdge.LineRestriction> radioRestrLineRestr = new() { };
        private void PolluteRadioRestrLineRestr()
        {
            radioRestrLineRestr.Add(radioRestrNoRestr, PEdge.LineRestriction.None);
            radioRestrLineRestr.Add(radioRestrHorizontal, PEdge.LineRestriction.Horizontal);
            radioRestrLineRestr.Add(radioRestrVertical, PEdge.LineRestriction.Vertical);
            radioRestrLineRestr.Add(radioRestrConst, PEdge.LineRestriction.ConstantLength);
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
            selectedP!.ConvertLineToBezier();
            selectedP = null;
            UncheckRadioRestrictions();
            EnableRadioRestrictions(false);
            buttonLineToBezier.Enabled = false;
            canvas.Invalidate();
        }

        private void buttonBezierToLine_Click(object sender, EventArgs e)
        {
            selectedP!.ConvertBezierToLine();
            selectedP = null;
            buttonBezierToLine.Enabled = false;
        }

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // RADIO BUTTONS -> RESTRICTIONS ON VERTICES
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

        private List<RadioButton> radioCons = new();
        private void PolluteRadioCons()
        {
            radioCons.Add(radioConG0);
            radioCons.Add(radioConG1);
            radioCons.Add(radioConC1);
        }

        private Dictionary<RadioButton, PVertex.Continuity> radioConVertexCon = new() { };
        private void PolluteRadioConVertexCon()
        {
            radioConVertexCon.Add(radioConG0, PVertex.Continuity.G0);
            radioConVertexCon.Add(radioConG1, PVertex.Continuity.G1);
            radioConVertexCon.Add(radioConC1, PVertex.Continuity.C1);
        }
        private void UncheckRadioConsButOne(RadioButton radioConOn)
        {
            foreach (var radioCon in radioCons)
            {
                if (radioConOn == radioCon)
                    continue;
                radioCon.Checked = false;
            }
        }
        private void UncheckRadioCons()
        {
            foreach (var radioCon in radioCons)
                radioCon.Checked = false;
        }

        private void EnableRadioCons(bool state = true)
        {
            foreach (var radioCon in radioCons)
                radioCon.Enabled = state;
        }

        private void radioCon_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioCon = (RadioButton)sender;
            if (radioCon.Checked == false)
                return;
            selectedP!.selectedVertex!.ContinuityType = radioConVertexCon[radioCon];
            UncheckRadioConsButOne(radioCon);
        }

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // RADIO BUTTONS -> DRAWING ALGORITHM
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

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

        ///////////////////////////////////////////////////////////////////////////
        //                                                                       //
        // NEW POLYGON BUTTONS
        //                                                                       //
        ///////////////////////////////////////////////////////////////////////////

        private void buttonNewPolygon_Click(object sender, EventArgs e)
        {
            int verticesCount;
            int R;
            try
            {
                verticesCount = int.Parse(textBoxVerteciesC.Text);
                R = int.Parse(textBoxRadius.Text);

            }
            catch
            {
                return;
            }

            polygons.Add(pFactory.GenerateEquilateral(verticesCount, R));
        }

        private void buttonRemovePolygon_Click(object sender, EventArgs e)
        {
            if (selectedP != null && selectedP.selectedVertex == null &&
                selectedP.selectedLine == null && selectedP.selectedBezierLine == null)
            {
                polygons.Remove(selectedP);
                selectedP = null;
                buttonRemovePolygon.Enabled = false;
            }
        }
    }
}
