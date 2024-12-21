using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing.Drawing2D;

namespace PolygonEditor.Geometry.Objects
{
    public class BezierControlLine : PEdge
    {
        public readonly float[] DashPattern = { 5 };
        public BezierControlLine(PVertex A, PVertex B) : base(A, B) { }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            MyDrawing.PlotLine(A.Point, B.Point, dbitmap, p.Color, 3);
        }
    }
}
