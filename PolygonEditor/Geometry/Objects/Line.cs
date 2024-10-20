using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PolygonEditor.Geometry.Objects.Line;

namespace PolygonEditor.Geometry.Objects
{
    public class Line : MovableItem
    {
        public readonly Size IconRect = new(14, 14);
        public readonly Dictionary<LineRestriction, Icon> LineIcons = new()
        {
            { LineRestriction.Horizontal, SystemIcons.Information },
            { LineRestriction.Vertical, SystemIcons.Exclamation },
            { LineRestriction.ConstantLength, SystemIcons.Hand },
        };
        public enum LineRestriction { None, Horizontal, Vertical, ConstantLength }
        private LineRestriction _restriction = LineRestriction.None;
        public LineRestriction Restriction
        {
            get { return _restriction; }
            set
            {
                if (value == LineRestriction.ConstantLength)
                {
                    if (A == null || B == null)
                        throw new InvalidOperationException();
                    R = Geometry.Dist(A, B);
                }
                _restriction = value;
                A.NotifyPropertyChanged();
                B.NotifyPropertyChanged();
            }
        }
        public Point2 Middle
        {
            get
            {
                if (A == null || B == null) throw new InvalidOperationException();
                return (A.Point + B.Point) / 2;
            }
        }
        public Vertex? A { get; set; }
        public Vertex? B { get; set; }
        public float R { get; private set; } = 0;

        public override int S_RADIUS => SA_RADIUS * 2;

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

        public override bool IsSelected(Point2 p)
        {
            return Geometry.Dist2(p, this) <= S_RADIUS * S_RADIUS;
        }
        private void DrawIcon(Graphics g)
        {
            if (Restriction == LineRestriction.None)
                return;
            g.DrawIcon(LineIcons[Restriction], new Rectangle(Middle.X - IconRect.Width / 2, Middle.Y - IconRect.Height / 2,
                                                            IconRect.Width, IconRect.Height));
        }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            MyDrawing.PlotLine(A.Point, B.Point, dbitmap, p.Color);
            DrawIcon(g);
        }
        public override void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            g.DrawLine(p, A.Point, B.Point);
            DrawIcon(g);
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
            DrawIcon(g);
        }

        public override void DrawLibrarySelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            DrawSelection(g, p, s);
            DrawLibrary(dbitmap, g, p, b);
            DrawIcon(g);
        }

        public override void Move(Vec2 v)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            Vertex.MoveLockDelayNotify(A, B, v);
        }
        // 
        // 
        // 
        // 
        // 

        public BezierLine ConvertToBezierLine()
        {
            BezierLine bLine = new(A, B)
            {
                P2 = new BezierControlVertex(A.Point),
                P3 = new BezierControlVertex(B.Point)
            };

            bLine.AP2 = new BezierControlLine(A, bLine.P2);
            bLine.P2P3 = new BezierControlLine(bLine.P2, bLine.P3);
            bLine.P3B = new BezierControlLine(bLine.P3, B);
            A.Next = bLine;
            B.Prev = bLine;

            bLine.Init();
            return bLine;
        }
        // 
        // 
        // 
        // 
        // 

        public virtual void VertexChangedPos(object? sender, PropertyChangedEventArgs e)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            if (sender == null)
                throw new Exception();
            if (Restriction == LineRestriction.None)
                return;

            Vertex v = (Vertex)sender;
            Vertex other = v == A ? B : A;
            if (Restriction == LineRestriction.Horizontal)
            {
                other.Y = v.Y;
            }
            else if (Restriction == LineRestriction.Vertical)
            {
                other.X = v.X;
            }
            else if (Restriction == LineRestriction.ConstantLength)
            {
                other.Point = Geometry.PointToNewCircle(v.Point, other.Point, R);
            }
        }
    }
}
