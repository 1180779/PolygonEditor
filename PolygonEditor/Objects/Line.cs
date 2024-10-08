using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public class Line : Item
    {
        public Vertex? A { get; set; }
        public Vertex? B { get; set; }

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
    }
}
