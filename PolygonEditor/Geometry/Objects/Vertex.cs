using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonEditor.Geometry.Objects
{
    public class Vertex : Item
    {
        public bool HasOneBezier { get { return (_prev is BezierLine && _next is not BezierLine) 
                    || (_prev is not BezierLine && _next is BezierLine); } }

        protected const int RADIUS = 5;
        
        protected Point2 _point;
        public virtual Point2 Point
        {
            get { return _point; }
            set
            {
                if (Locked)
                    return;
                _point = value;
            }
        }
        public virtual int X
        {
            get { return _point.X; }
            set
            {
                if (Locked)
                    return;
                _point.X = value;

            }
        }
        public virtual int Y
        {
            get { return _point.Y; }
            set
            {
                if (Locked)
                    return;
                _point.Y = value;
            }
        }

        protected Edge? _prev = null;
        protected Edge? _next = null;
        public virtual Edge? Prev
        {
            get { return _prev; }
            set { _prev = value; }
        }
        public virtual Edge? Next
        {
            get { return _next; }
            set { _next = value; }
        }

        public override int S_RADIUS => RADIUS + SA_RADIUS;

        public Vertex(Point2 p) { Point = p; }
        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
            Prev = null;
            Next = null;
        }

        public override bool IsSelected(Point2 p)
        {
            return Geometry.Dist2(_point, p) <= RADIUS * RADIUS;
        }
        public override void DrawSelection(Graphics g, Pen p, Brush s)
        {
            g.FillEllipse(s, X - S_RADIUS, Y - S_RADIUS, S_RADIUS * 2, S_RADIUS * 2);
        }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b) => DrawLibrary(dbitmap, g, p, b);
        public override void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            g.FillEllipse(b, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }
        public override void DrawSelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s) => DrawLibrarySelected(dbitmap, g, p, b, s);
        public override void DrawLibrarySelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            DrawSelection(g, p, s);
            g.FillEllipse(b, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }

        private bool _locked = false;
        public override bool Locked { get { return _locked; } }
        public override void Lock() { _locked = true; }
        public override void Unlock() { _locked = false; }
        public override void Move(Vec2 v) => Point += v;
    }
}
