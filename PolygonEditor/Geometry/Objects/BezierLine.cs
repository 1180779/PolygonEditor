using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public class BezierLine : Line
    {
        public static readonly Pen controlLinePen = new Pen(Color.Blue);
        static BezierLine()
        {
            controlLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
        }

        public BezierControlVertex? selectedControlVertex = null;
        public List<Vertex> Vertices = new() { };
        public List<Point2> Approximation = [];
        private void FindApproximation()
        {
            if (A == null || B == null || P2 == null || P3 == null)
                throw new InvalidOperationException();
            Approximation.Clear();

            float step = 0.05f;
            Point2f I1A, I1B, I1C, I2A, I2B, I3A;
            Point2f a = A.Point;
            Point2f p2 = P2.Point;
            Point2f p3 = P3.Point;
            Point2f b = B.Point;
            for (float t = 0; t <= 1.01f; t += step)
            {
                float t2 = 1 - t;
                I1A = t2 * a + t * p2;
                I1B = t2 * p2 + t * p3;
                I1C = t2 * p3 + t * b;

                I2A = t2 * I1A + t * I1B;
                I2B = t2 * I1B + t * I1C;

                I3A = t2 * I2A + t * I2B;
                Approximation.Add((Point2)I3A);
            }
        }
        public void Init()
        {
            Vertices.Add(A);
            Vertices.Add(P2);
            Vertices.Add(P3);
            Vertices.Add(B);

            A.PropertyChanged += VertexChangedPos;
            B.PropertyChanged += VertexChangedPos;
            P2.PropertyChanged += VertexChangedPos;
            P3.PropertyChanged += VertexChangedPos;

            Vec2 v = B.Point - A.Point;
            Vec2 w = new Vec2(-v.Y, v.X);
            P2.Point = P2.Point + v / 2 + w / 2;
            P3.Point = P3.Point - v / 2 - w / 2;

            FindApproximation();
        }

        public void Destroy()
        {
            A.PropertyChanged -= VertexChangedPos;
            B.PropertyChanged -= VertexChangedPos;
            P2.PropertyChanged -= VertexChangedPos;
            P3.PropertyChanged -= VertexChangedPos;
        }
        public BezierControlVertex? P2 { get; set; }
        public BezierControlVertex? P3 { get; set; }
        public BezierControlLine? AP2 { get; set; }
        public BezierControlLine? P2P3 { get; set; }
        public BezierControlLine? P3B { get; set; }

        public override int S_RADIUS => throw new NotImplementedException();

        public BezierLine(Vertex a, Vertex b) : base(a, b)
        {
            P2 = null;
            P3 = null;
        }
        public BezierLine(Vertex a) : base(a)
        {
            P2 = null;
            P3 = null;
        }
        public BezierLine() : base() { Init(); }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            for (int i = 0; i < Approximation.Count - 1; i++) 
            {
                MyDrawing.PlotLine(Approximation[i], Approximation[i + 1], dbitmap, p.Color);
            }
            MyDrawing.PlotLine(Approximation[Approximation.Count - 1], B.Point, dbitmap, p.Color);

            AP2.Draw(dbitmap, g, controlLinePen, b);
            P2P3.Draw(dbitmap, g, controlLinePen, b);
            P3B.Draw(dbitmap, g, controlLinePen, b);

            A.Draw(dbitmap, g, p, b);
            B.Draw(dbitmap, g, p, b);
            P2.Draw(dbitmap, g, p, b);
            P3.Draw(dbitmap, g, p, b);
        }
        public override void DrawLibrary(Graphics g, Pen p, Brush b)
        {
            if (A == null || P2 == null || P3 == null || B == null)
                throw new InvalidOperationException();
            g.DrawBezier(p, A.Point, P2.Point, P3.Point, B.Point);

            AP2.DrawLibrary(g, controlLinePen, b);
            P2P3.DrawLibrary(g, controlLinePen, b);
            P3B.DrawLibrary(g, controlLinePen, b);

            A.DrawLibrary(g, p, b);
            B.DrawLibrary(g, p, b);
            P2.DrawLibrary(g, p, b);
            P3.DrawLibrary(g, p, b);
        }
        public override void DrawSelection(Graphics g, Pen p, Brush s)
        {
            if (selectedControlVertex != null)
                selectedControlVertex.DrawSelection(g, p, s);
            else
                g.FillPolygon(s, new System.Drawing.Point[] { A.Point, P2.Point, P3.Point, B.Point });
        }
        public override void DrawSelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            DrawSelection(g, p, s);
            Draw(dbitmap, g, p, b);
        }
        public override void DrawLibrarySelected(Graphics g, Pen p, Brush b, Brush s)
        {
            DrawSelection(g, p, s);
            DrawLibrary(g, p, b);
        }

        public override void Move(Vec2 v)
        {
            if (selectedControlVertex != null)
            {
                selectedControlVertex.Move(v);
                return;
            }
            // move one of the control points 
            // and chandle the necessary changes
            base.Move(v);
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            P2.Move(v);
            P3.Move(v);
        }
        public void MoveInPolygon(Vec2 v)
        {
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            P2.Move(v);
            P3.Move(v);
        }

        public bool IsSelectedItem(Point2 p)
        {
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            if (P2.IsSelected(p))
            {
                selectedControlVertex = P2;
                return true;
            }
            if (P3.IsSelected(p))
            {
                selectedControlVertex = P3;
                return true;
            }
            return false;
        }

        public override bool IsSelected(Point2 p)
        {
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            selectedControlVertex = null;
            return IsSelectedItem(p) || Geometry.PointInPolygon(p, Vertices);
        }

        public override void VertexChangedPos(object? sender, PropertyChangedEventArgs e)
        {
            FindApproximation();
        }
    }
}
