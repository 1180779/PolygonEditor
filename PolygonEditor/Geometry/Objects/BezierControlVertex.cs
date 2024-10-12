using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public class BezierControlVertex : Vertex
    {
        public BezierControlVertex(Point p) : base(p) { }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b) => DrawLibrary(g, p, b);
        public override void DrawLibrary(Graphics g, Pen p, Brush b)
        {
            g.FillEllipse(Brushes.White, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }
        public override void DrawSelected(DirectBitmap dbitmapg, Graphics g, Pen p, Brush b, Brush s) => DrawLibrarySelected(g, p, b, s);
        public override void DrawLibrarySelected(Graphics g, Pen p, Brush b, Brush s)
        {
            base.DrawSelection(g, p, s);
            DrawLibrary(g, p, b);
        }
    }
}
