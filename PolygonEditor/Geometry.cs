using PolygonEditor.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor
{
    public static class Geometry
    {
        public static int Dist2(Vertex A, Vertex B) => Dist2(A.Point, B.Point);
        public static int Dist2(Point A, Point B)
        {
            return (A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y);
        }

        public static int Dist(Vertex A, Vertex B) => Dist2(A.Point, B.Point);
        public static int Dist(Point A, Point B)
        {
            return (int) Math.Sqrt(Dist2(A, B));
        }

        public static int Dist2(Point A, Line L) 
        {
            if(L.A == null || L.B == null)
                throw new InvalidOperationException();
            long n = (L.B.Y - L.A.Y) * A.X - (L.B.X - L.A.X) * A.Y + L.B.X * L.A.Y - L.B.Y * L.A.X; // numerator
            n = n * n;
            long d = (L.B.Y - L.A.Y) * (L.B.Y - L.A.Y) + (L.B.X - L.A.X) * (L.B.X - L.A.X); // denominator
            return (int) (n / d);
        }
    }
}
