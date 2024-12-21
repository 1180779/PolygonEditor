using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static PolygonEditor.Geometry.Objects.Edge;

namespace PolygonEditor.Geometry.Objects
{
    public class Edge : Item
    {
        public Edge(Vertex a, Vertex b) { A = a; B = b; }
        public Vertex A { get; set; }
        public Vertex B { get; set; }
        public Point2 Middle
        {
            get
            {
                if (A == null || B == null) throw new InvalidOperationException();
                return (A.Point + B.Point) / 2;
            }
        }
        public float Length { get { return Geometry.Dist(A, B); } }
        public float Length2 { get { return Geometry.Dist2(A, B); } }

        public override int S_RADIUS => SA_RADIUS * 2;


        public override bool IsSelected(Point2 p)
        {
            return Geometry.Dist2(p, this) <= S_RADIUS * S_RADIUS;
        }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            MyDrawing.PlotLine(A.Point, B.Point, dbitmap, p.Color);
        }
        public override void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            g.DrawLine(p, A.Point, B.Point);
        }

        // https://stackoverflow.com/questions/36966671/draw-an-ellipse-with-a-specified-fatness-between-2-points
        private static void DrawEllipse(Graphics G, Brush b, Point center, Size size, float angle)
        {
            int h2 = size.Height / 2;
            int w2 = size.Width / 2;
            Rectangle rect = new(new Point(center.X - w2, center.Y - h2), size);

            G.TranslateTransform(center.X, center.Y);
            G.RotateTransform(angle);
            G.TranslateTransform(-center.X, -center.Y);
            G.FillEllipse(b, rect);
            G.ResetTransform();
        }
        public override void DrawSelection(Graphics g, Pen p, Brush s)
        {
            float angle = -(float)(Math.Atan2(A.Y - B.Y, B.X - A.X) * 180f / Math.PI);
            int longSide = (int)Math.Sqrt((A.Y - B.Y) * (A.Y - B.Y) + (B.X - A.X) * (B.X - A.X));
            Point C = new((A.X + B.X) / 2, (A.Y + B.Y) / 2);
            Size size = new(longSide, S_RADIUS);
            DrawEllipse(g, s, C, size, angle);
        }
        public override void DrawSelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            DrawSelection(g, p, s);
            MyDrawing.PlotLine(A.Point, B.Point, dbitmap, p.Color);
        }

        public override void DrawLibrarySelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            DrawSelection(g, p, s);
            DrawLibrary(dbitmap, g, p, b);
        }

        public override bool Locked { get { return A.Locked && B.Locked; } }
        public override void Lock()
        {
            A.Lock();
            B.Lock();
        }
        public override void Unlock()
        {
            A.Unlock();
            B.Unlock();
        }
        public override void Move(Vec2 v)
        {
            A.Move(v);
            B.Move(v);
        }
    }
}
