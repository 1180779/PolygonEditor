using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace PolygonEditor.Objects
{
    public class Polygon : Item, IMovable
    {
        public List<Vertex> Vertices = new();
        public Point[] Points = new Point[0];
        public List<Line> Lines = new();
        public List<BezierLine> BezierLines = new();

        Vertex? selectedVertex;
        BezierLine? selectedBezierLine;
        public Polygon() 
        {
            selectedVertex = null;
            selectedBezierLine = null;
        }
        public void GeneratePointsFromVerices()
        {
            Points = new Point[Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
                Points[i] = Vertices[i].Point;
        }
        public void DrawSelected(Graphics g, Pen p, Brush b)
        {
            Draw(g, p, b);
            g.FillPolygon(b, Points);
        }
        public override void Draw(Graphics g, Pen p, Brush b) 
        {
            foreach(var item in BezierLines)
                item.Draw(g, p, b);
            foreach(var item in Lines)
                item.Draw(g, p, b);
            foreach (var item in Vertices) 
                item.Draw(g, p, b);
        }

        public void Move(Point PML, Point ML)
        {
            foreach (var item in Vertices)
                item.Move(PML, ML);
            foreach(var item in BezierLines)
                item.Move(PML, ML);
        }

        public bool Selected(Point ML)
        {
            return PointInPolygon(ML);
        }

        public bool SelectedVertex(Point ML) 
        {
            selectedVertex = null;
            selectedBezierLine = null;
            foreach(var v in Vertices)
            {
                if (v.Selected(ML)) 
                {
                    selectedVertex = v;
                    return true;
                }
            }
            foreach(var b in BezierLines)
            {
                if(b.Selected(ML))
                {
                    selectedBezierLine = b;
                    return true;
                }
            }
            return false;
        }

        public void MoveVertex(Point PML, Point ML) 
        {
            selectedVertex?.Move(PML, ML);
            selectedBezierLine?.Move(PML, ML);
        }

        public void MoveWHoleOrVertex(Point PML, Point ML)
        {
            if(selectedVertex == null && selectedVertex == null)
                Move(PML, ML);
            else
                MoveVertex(PML, ML);
        }

        // https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/
        // Function to check if a point is inside a polygon
        public bool PointInPolygon(Point p)
        {
            int numVertices = Vertices.Count;
            double x = p.X, y = p.Y;
            bool inside = false;

            // Store the first point in the polygon and initialize the second point
            Point p1 = Vertices[0].Point, p2;

            // Loop through each edge in the polygon
            for (int i = 1; i <= numVertices; i++)
            {
                // Get the next point in the polygon
                p2 = Vertices[i % numVertices].Point;

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
