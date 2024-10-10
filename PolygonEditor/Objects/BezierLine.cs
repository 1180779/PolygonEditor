using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public class BezierLine : Line, IMovable
    {
        public Vertex? P2 { get; set; }
        public Vertex? P3 { get; set; } 
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

        public void SetP2(Vertex p2) 
        {
            if(P2 == null) 
            {
                P2 = p2;
                return;
            }
            throw new InvalidOperationException();
        }
        public void SetP3(Vertex p3)
        {
            if (P3 == null)
            {
                P3 = p3;
                return;
            }
            throw new InvalidOperationException();
        }

        public override void Draw(Graphics g, Pen p, Brush b)
        {
            if (A == null || P2 == null || P3 == null || B == null)
                throw new InvalidOperationException();
            g.DrawBezier(p, A.Point, P2.Point, P3.Point, B.Point);
        }

        public void Move(Point prevML, Point ML)
        {
            // move one of the control points 
            // and chandle the necessary changes
            throw new NotImplementedException();
        }

        public bool Selected(Point ML)
        {
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            return P2.Selected(ML) || P3.Selected(ML);
        }
    }
}
