using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public class PEdge : Edge, IStructureItem
    {
        public PEdge(PVertex a, PVertex b) : base(a, b) {
            if (a == null || b == null)
                throw new InvalidOperationException();
            A = a;
            B = b;

            // unnecessary, just to make compiler happy
            _A = a; 
            _B = b;          
        }


        private PVertex _A;
        private PVertex _B;
        public new PVertex A { get { return _A; } set { _A = value; base.A = value; } }
        public new PVertex B { get { return _B; } set { _B = value; base.B = value; } }


        public readonly Size IconRect = new(14, 14);
        public float R { get; private set; } = 0;
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

        private void DrawIcon(Graphics g)
        {
            if (Restriction == LineRestriction.None)
                return;
            g.DrawIcon(LineIcons[Restriction], new Rectangle(Middle.X - IconRect.Width / 2, Middle.Y - IconRect.Height / 2,
                                                            IconRect.Width, IconRect.Height));
        }

        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            base.Draw(dbitmap, g, p, b);
            DrawIcon(g);
        }
        public override void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            base.DrawLibrary(dbitmap, g, p, b);
            DrawIcon(g);
        }

        public override void DrawSelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            base.DrawSelected(dbitmap, g, p, b, s);
            DrawIcon(g);
        }

        public override void DrawLibrarySelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            base.DrawLibrarySelected(dbitmap, g, p, b, s);
            DrawIcon(g);
        }

        public override void Move(Vec2 v)
        {
            if (A == null || B == null)
                throw new InvalidOperationException();
            PVertex.MoveLockDelayNotify(A, B, v);
        }

        public BezierLine ToBezier()
        {
            BezierLine bLine = new(A, B);

            return bLine;
        }

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

        public void MoveInStructure(Vec2 v)
        {
            A.MoveInStructure(v);
            B.MoveInStructure(v);
        }
        public void NotifyInStructure()
        {
            A.NotifyInStructure();
            B.NotifyInStructure();
        }
    }
}
