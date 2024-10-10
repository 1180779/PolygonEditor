using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public class Line : MovableItem
    {
        public enum LineRestriction { None, Horizontal, Vertical, ConstantLength }
        public LineRestriction restriction = LineRestriction.None;
        public Point Middle 
        { 
            get 
            { 
                if (A == null || B == null) throw new InvalidOperationException(); 
                return new Point((A.X + B.X) / 2, (A.Y + B.Y) / 2); 
            }
        }
        public Vertex? A { get; set; }
        public Vertex? B { get; set; }

        public override int S_RADIUS => Item.SA_RADIUS * 2;

        public Line(Vertex a, Vertex b) 
        {
            A = a;
            B = b;
        }
        public Line(Vertex a) 
        {
            A = a;
            B = null;
        }
        public Line() 
        {
            A = null;
            B = null;
        }
        public void SetA(Vertex a)
        {
            if (A == null)
            {
                A = a;
                return;
            }
            throw new InvalidOperationException();
        }
        public void SetB(Vertex b) 
        {
            if(B == null) 
            {
                B = b;
                return;
            }
            throw new InvalidOperationException();
        }

        public override void Draw(Graphics g, Pen p, Brush b) 
        {
            if(A == null || B == null)
                throw new InvalidOperationException();
            g.DrawLine(p, A.Point, B.Point);
        }

        public void VertexChanged(object? sender, PropertyChangedEventArgs e) 
        {
            if(A == null || B == null)
                throw new InvalidOperationException();
            if (sender == null)
                throw new Exception();
            if (restriction == LineRestriction.None)
                return;

            Vertex v = (Vertex)sender;
            Vertex other = v == A ? B : A;
            if(restriction == LineRestriction.Vertical) 
            {
                if(other.X != v.X)
                    other.X = v.X;
            }
            else if(restriction == LineRestriction.Vertical)
            {
                if(other.Y != v.Y)
                    other.Y = v.Y;
            }
            else if (restriction == LineRestriction.ConstantLength) 
            {
                throw new NotImplementedException();
            }
        }

        // https://stackoverflow.com/questions/36966671/draw-an-ellipse-with-a-specified-fatness-between-2-points
        public override void DrawSelected(Graphics g, Pen p, Brush b, Brush s)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();

            float angle = -(float)(Math.Atan2(A.Y - B.Y, B.X - A.X) * 180f / Math.PI);
            int longSide = (int)(Math.Sqrt((A.Y - B.Y) * (A.Y - B.Y) + (B.X - A.X) * (B.X - A.X)));
            Point C = new Point((A.X + B.X) / 2, (A.Y + B.Y) / 2);
            Size size = new Size((int)longSide, S_RADIUS);
            DrawEllipse(g, s, C, size, angle);
            Draw(g, p, b);
        }

        static void DrawEllipse(Graphics G, Brush b, Point center, Size size, float angle)
        {
            int h2 = size.Height / 2;
            int w2 = size.Width / 2;
            Rectangle rect = new Rectangle(new Point(center.X - w2, center.Y - h2), size);

            G.TranslateTransform(center.X, center.Y);
            G.RotateTransform(angle);
            G.TranslateTransform(-center.X, -center.Y);
            G.FillEllipse(b, rect);
            G.ResetTransform();
        }

        public override bool Selected(Point ML)
        {
            if (ML.X < int.Min(A.X, B.X) || ML.X > int.Max(A.X, B.X) || ML.Y < int.Min(A.Y, B.Y) || ML.Y > int.Max(A.Y, B.Y))
                return false;
            return Geometry.Dist2(ML, this) <= S_RADIUS * S_RADIUS;
        }

        public override void Move(Point PML, Point ML)
        {
            if(A == null || B == null)
                throw new InvalidOperationException();
            A.Move(PML, ML);
            B.Move(PML, ML);
        }
    }
}
