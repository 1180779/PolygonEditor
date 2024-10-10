using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Net.Http.Headers;

namespace PolygonEditor.Objects
{
    public class Polygon : MovableItem
    {
        public List<Vertex> VerticesInOrder = new();
        public List<Vertex> Vertices = new();
        public List<Line> Lines = new();
        public List<BezierLine> BezierLines = new();

        Vertex? selectedVertex = null;
        Line? selectedLine = null;
        BezierLine? selectedBezierLine = null;

        public override int S_RADIUS => throw new NotImplementedException();

        public Polygon() 
        {
        }

        public Point[] GetPoints()
        {
            Point[] points = new Point[Vertices.Count];
            for (int i = 0; i < Vertices.Count; ++i)
                points[i] = VerticesInOrder[i].Point;
            return points;
        }
        public override void DrawSelected(Graphics g, Pen p, Brush b, Brush s)
        {
            if (selectedVertex == null && selectedLine == null && selectedBezierLine == null)
                g.FillPolygon(s, GetPoints());
            foreach (var item in BezierLines)
            {
                if (item == selectedBezierLine)
                    item.DrawSelected(g, p, b, s);
                else
                    item.Draw(g, p, b);
            }
            foreach (var item in Lines)
            {
                if(item == selectedLine)
                    item.DrawSelected(g, p, b, s);
                else
                    item.Draw(g, p, b);
            }
            foreach (var item in Vertices)
            {
                if (item == selectedVertex)
                    item.DrawSelected(g, p, b, s);
                else
                    item.Draw(g, p, b);
            }
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

        public override void Move(Point PML, Point ML)
        {
            foreach (var item in Vertices)
                item.Move(PML, ML);
            foreach(var item in BezierLines)
                item.Move(PML, ML);
        }

        public override bool Selected(Point ML)
        {
            return PointInPolygon(ML);
        }

        public bool SelectedItem(Point ML) 
        {
            selectedVertex = null;
            selectedLine = null;
            selectedBezierLine = null;
            foreach(var v in Vertices)
            {
                if (v.Selected(ML)) 
                {
                    selectedVertex = v;
                    return true;
                }
            }
            foreach(var l in Lines)
            {
                if (l.Selected(ML))
                {
                    selectedLine = l;
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

        public void MoveItem(Point PML, Point ML) 
        {
            selectedVertex?.Move(PML, ML);
            selectedLine?.Move(PML, ML);
            selectedBezierLine?.Move(PML, ML);
        }

        public void MoveWHoleOrVertex(Point PML, Point ML)
        {
            if(selectedVertex == null && selectedLine == null && selectedVertex == null)
                Move(PML, ML);
            else
                MoveItem(PML, ML);
        }

        public bool RemoveVertex() 
        {
            if (selectedVertex == null && selectedBezierLine == null)
                return false;

            if (selectedVertex != null) 
            {
                if (Vertices.Count == 3)
                    return false;
                Vertex vprev = selectedVertex.Prev.A;
                Vertex vnext = selectedVertex.Next.B;
                selectedVertex.Prev.B = vnext;
                selectedVertex.Next.A = vprev;

                Line l = new Line(vprev, vnext);
                vprev.Next = l;
                vnext.Prev = l;

                Vertices.Remove(selectedVertex);
                if (!Lines.Remove(selectedVertex.Prev))
                    throw new Exception();
                if(!Lines.Remove(selectedVertex.Next))
                    throw new Exception();
                Lines.Add(l);
                selectedVertex = null;
                ReconstructVerticesInOrder();
                return true;
            }
            return false;
        }

        public bool AddVertex() 
        {
            if (selectedLine == null)
                return false;

            Vertex A = selectedLine.A;
            Vertex C = selectedLine.B;

            Vertex B = new Vertex(selectedLine.Middle);
            Line prev = new Line(A, B);
            Line next = new Line(B, C);
            B.Prev = prev;
            B.Next = next;
            
            Vertices.Add(B);
            Lines.Remove(selectedLine);
            Lines.Add(prev);
            Lines.Add(next);
            A.Next = prev;
            C.Prev = next;
            selectedLine = null;
            ReconstructVerticesInOrder();
            return true;
        }

        public void ReconstructVerticesInOrder() 
        {
            VerticesInOrder.Clear();
            VerticesInOrder.EnsureCapacity(Vertices.Count);
            Vertex v = Vertices[0];
            for (int i = 0; i < Vertices.Count; i++) 
            {
                VerticesInOrder.Add(v);
                v = v.Next.B;
            }
        }

        // https://www.geeksforgeeks.org/how-to-check-if-a-given-point-lies-inside-a-polygon/
        // Function to check if a point is inside a polygon
        private bool PointInPolygon(Point p)
        {
            int numVertices = VerticesInOrder.Count;
            double x = p.X, y = p.Y;
            bool inside = false;

            // Store the first point in the polygon and initialize the second point
            Point p1 = Vertices[0].Point, p2;

            // Loop through each edge in the polygon
            for (int i = 1; i <= numVertices; i++)
            {
                // Get the next point in the polygon
                p2 = VerticesInOrder[i % numVertices].Point;

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
