using PolygonEditor.Geometry.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry
{
    public static class Geometry
    {
        public static int Dist2(Vertex A, Vertex B) => Dist2(A.Point, B.Point);
        public static int Dist2(Point2 A, Point2 B)
        {
            return (A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y);
        }

        public static float Dist(Vertex A, Vertex B) => Dist(A.Point, B.Point);
        public static float Dist(Point2 A, Point2 B)
        {
            return (float)Math.Sqrt(Dist2(A, B));
        }

        public static int DistToLine2(Point A, Line L)
        {
            if (L.A == null || L.B == null)
                throw new InvalidOperationException();
            long n = (L.B.Y - L.A.Y) * A.X - (L.B.X - L.A.X) * A.Y + L.B.X * L.A.Y - L.B.Y * L.A.X; // numerator
            n *= n;
            long d = (L.B.Y - L.A.Y) * (L.B.Y - L.A.Y) + (L.B.X - L.A.X) * (L.B.X - L.A.X); // denominator
            return (int)(n / d);
        }


        public static int DotProduct(Point2 A, Point2 B)
        {
            return A.X * B.X + A.Y * B.Y;
        }

        // https://www.sunshine2k.de/coding/java/PointOnLine/PointOnLine.html
        public static Point2 ProjectPointOntoLine(Point2 p, Point2 v1, Point2 v2)
        {
            Point2 e1 = v2 - v1;
            Point2 e2 = p - v1;
            float valDp = DotProduct(e1, e2);
            float lenLineE1 = DotProduct(e1, e1);
            float lenLineE2 = DotProduct(e2, e2);
            float cos = valDp / (lenLineE1 * lenLineE2);
            float projLenOfLine = cos * lenLineE2;
            return v1 + (Vec2) ((projLenOfLine * (Vec2f) e1) / lenLineE2);
        }
        public static int Dist2(Point2 A, Line L)
        {
            if (L.A == null || L.B == null)
                throw new InvalidOperationException();

            int l2 = Dist2(L.A, L.B);
            if (l2 == 0)
                return Dist2(A, L.A.Point);
            float t = DotProduct(A - L.A.Point, L.B.Point - L.A.Point) / (float)l2;
            t = float.Max(0, float.Min(1, t));
            Point2 projection = L.A.Point + t * (L.B.Point - L.A.Point);
            return Dist2(A, projection);
        }

        // https://stackoverflow.com/questions/300871/best-way-to-find-a-point-on-a-circle-closest-to-a-given-point
        public static Point2 PointToNewCircle(Point2 C, Point2 P, float R)
        {
            if (P == C)
                return new(C.X + (int)R, C.Y);

            Point2 V = new(P.X - C.X,
                                P.Y - C.Y);
            float d = (float)Math.Sqrt(DotProduct(V, V));
            return new(C.X + (int)(V.X / d * R),
                            C.Y + (int)(V.Y / d * R));
        }

        // https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/
        // Function to check if a point is inside a polygon
        public static bool PointInPolygon(Point2 p, List<Vertex> vertices)
        {
            int numVertices = vertices.Count;
            double x = p.X, y = p.Y;
            bool inside = false;

            // Store the first point in the polygon and initialize the second point
            Point2 p1 = vertices[0].Point, p2;

            // Loop through each edge in the polygon
            for (int i = 1; i <= numVertices; i++)
            {
                // Get the next point in the polygon
                p2 = vertices[i % numVertices].Point;

                // Check if the point is above the minimum y coordinate of the edge
                if (y > Math.Min(p1.Y, p2.Y))
                {
                    // Check if the point is below the maximum y coordinate of the edge
                    if (y <= Math.Max(p1.Y, p2.Y))
                    {
                        // Check if the point is to the left of the maximum x coordinate of the edge
                        if (x <= Math.Max(p1.X, p2.X))
                        {
                            // Calculate the x-intersection of the line connecting the point to the edge
                            double xIntersection = (y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;

                            // Check if the point is on the same line as the edge or to the left of the x-intersection
                            if (p1.X == p2.X || x <= xIntersection)
                            {
                                // Flip the inside flag
                                inside = !inside;
                            }
                        }
                    }
                }

                // Store the current point as the first point for the next iteration
                p1 = p2;
            }
            // Return the value of the inside flag
            return inside;
        }
    }
}
