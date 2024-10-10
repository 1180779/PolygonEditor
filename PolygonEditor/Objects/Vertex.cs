using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public class Vertex : Item, IMovable
    {
        private const int RADIUS = 5;
        private Point _point;
        public Point Point { get { return _point; } set { _point = value; } }
        public int X { get { return _point.X; } set { _point.X = value; } }
        public int Y { get { return _point.Y; } set { _point.Y = value; } }
        public Line? Prev { get; private set; }
        public Line? Next { get; private set; }

        public Vertex(Point p) { Point = p; }
        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
            Prev = null;
            Next = null;
        }

        public void SetPrev(Line next)
        {
            if (Prev == null)
            {
                Prev = next;
                return;
            }
            throw new InvalidOperationException();
        }

        public void SetNext(Line next)
        {
            if (Next == null)
            {
                Next = next;
                return;
            }
            throw new InvalidOperationException();
        }

        public override void Draw(Graphics g, Pen p, Brush b) 
        {
            g.FillEllipse(b, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }

        public void Move(Point PML, Point ML)
        {
            _point.X += ML.X - PML.X;
            _point.Y += ML.Y - PML.Y;
        }

        public bool Selected(Point ML)
        {
            return Geometry.Dist2(_point, ML) <= RADIUS * RADIUS;
        }
    }
}
