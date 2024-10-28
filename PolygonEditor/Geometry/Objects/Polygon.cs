using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Net.Http.Headers;

namespace PolygonEditor.Geometry.Objects
{
    public class Polygon : Item
    {
        public List<PVertex> VerticesInOrder = [];
        public List<PVertex> Vertices = [];
        public List<PEdge> Lines = [];
        public List<BezierLine> BezierLines = [];

        public PVertex? selectedVertex = null;
        public PEdge? selectedLine = null;
        public BezierLine? selectedBezierLine = null;

        public override int S_RADIUS => throw new NotImplementedException();

        public Polygon(Point2 center, int n = 5, int R = 100) 
        {
            float b = 360 / n;
            List<PVertex> vertices = new(n);
            for (int i = 0; i < n; i++)
            {
                PVertex v = new(center);
                v.Y += (int)(R * Math.Sin(b * i / 360 * 2 * Math.PI));
                v.X += (int)(R * Math.Cos(b * i / 360 * 2 * Math.PI));
                vertices.Add(v);
            }

            List<PEdge> lines = new(n);
            for (int i = 0; i < n; i++)
            {
                PEdge line = new PEdge(vertices[i], vertices[(i + 1) % n]);
                lines.Add(line);
                vertices[i].Next = line;
                vertices[(i + 1) % n].Prev = line;
            }

            for (int i = 0; i < n; i++)
            {
                Vertices.Add(vertices[i]);
                Lines.Add(lines[i]);
            }

            ReconstructVerticesInOrder();
        }

        public Point[] GetPointsNoBezier()
        {
            Point[] points = new Point[Vertices.Count];
            for (int i = 0; i < Vertices.Count; ++i)
                points[i] = VerticesInOrder[i].Point;
            return points;
        }

        public Point[] GetPoints()
        {
            int bezierVerticesCount = 0;
            foreach (var bezier in BezierLines)
                bezierVerticesCount += bezier.Approximation.Count;
            Point[] points = new Point[Vertices.Count + bezierVerticesCount];

            int i = 0;
            Vertex v = Vertices[0];
            Vertex w = v;
            do
            {
                points[i] = w.Point;
                i++;
                if (w.Next is BezierLine)
                {
                    BezierLine bLine = (BezierLine)w.Next;
                    for (int j = 0; j < bLine.Approximation.Count; ++j)
                        points[i + j] = bLine.Approximation[j];
                    i += bLine.Approximation.Count;
                }
                w = w.Next!.B!;
            } while (w != v);
            return points;
        }

        public override void DrawSelection(Graphics g, Pen p, Brush s)
        {
            Point[] points = GetPoints();
            g.FillPolygon(s, points);
        }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            Point[] points = GetPoints();
            g.FillPolygon(Brushes.White, points);
            foreach (var item in BezierLines)
                item.Draw(dbitmap, g, p, b);
            foreach (var item in Lines)
                item.Draw(dbitmap, g, p, b);
            foreach (var item in Vertices)
                item.Draw(dbitmap, g, p, b);
        }
        public override void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            Point[] points = GetPoints();
            g.FillPolygon(Brushes.White, points);
            foreach (var item in BezierLines)
                item.DrawLibrary(dbitmap, g, p, b);
            foreach (var item in Lines)
                item.DrawLibrary(dbitmap, g, p, b);
            foreach (var item in Vertices)
                item.DrawLibrary(dbitmap, g, p, b);
        }
        public override void DrawSelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            if (selectedVertex == null && selectedLine == null && selectedBezierLine == null)
                DrawSelection(g, p, s);
            foreach (var item in BezierLines)
            {
                if (item == selectedBezierLine)
                    item.DrawSelected(dbitmap, g, p, b, s);
                else
                    item.Draw(dbitmap, g, p, b);
            }
            foreach (var item in Lines)
            {
                if (item == selectedLine)
                    item.DrawSelected(dbitmap, g, p, b, s);
                else
                    item.Draw(dbitmap, g, p, b);
            }
            foreach (var item in Vertices)
            {
                if (item == selectedVertex)
                    item.DrawSelected(dbitmap, g, p, b, s);
                else
                    item.Draw(dbitmap, g, p, b);
            }
        }
        public override void DrawLibrarySelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            if (selectedVertex == null && selectedLine == null && selectedBezierLine == null)
                DrawSelection(g, p, s);
            foreach (var item in BezierLines)
            {
                if (item == selectedBezierLine)
                    item.DrawLibrarySelected(dbitmap, g, p, b, s);
                else
                    item.DrawLibrary(dbitmap, g, p, b);
            }
            foreach (var item in Lines)
            {
                if (item == selectedLine)
                    item.DrawLibrarySelected(dbitmap, g, p, b, s);
                else
                    item.DrawLibrary(dbitmap, g, p, b);
            }
            foreach (var item in Vertices)
            {
                if (item == selectedVertex)
                    item.DrawLibrarySelected(dbitmap, g, p, b, s);
                else
                    item.DrawLibrary(dbitmap, g, p, b);
            }
        }


        public override void Move(Vec2 v)
        {
            foreach (var item in Vertices)
                item.MoveNoNotify(v);
            foreach (var item in BezierLines)
                item.MoveInPolygon(v);
        }

        public override bool IsSelected(Point2 p)
        {
            return PointInPolygon(p);
        }

        public bool SelectedItem(Point2 p)
        {
            selectedVertex = null;
            selectedLine = null;
            selectedBezierLine = null;
            foreach (var v in Vertices)
            {
                if (v.IsSelected(p))
                {
                    selectedVertex = v;
                    return true;
                }
            }
            foreach (var l in Lines)
            {
                if (l.IsSelected(p))
                {
                    selectedLine = l;
                    return true;
                }
            }
            foreach (var b in BezierLines)
            {
                if (b.IsSelected(p))
                {
                    selectedBezierLine = b;
                    return true;
                }
            }
            return false;
        }

        public void MoveItem(Vec2 v)
        {
            selectedVertex?.MoveLock(v);
            selectedLine?.Move(v);
            selectedBezierLine?.Move(v);
        }

        public void MoveWHoleOrVertex(Vec2 v)
        {
            if (selectedVertex == null && selectedLine == null && selectedBezierLine == null)
                Move(v);
            else
                MoveItem(v);
        }

        public bool RemoveVertex()
        {
            if (selectedVertex == null)
                return false;
            if (Vertices.Count == 3)
                return false;

            PVertex vprev = selectedVertex!.Prev!.A!;
            PVertex vnext = selectedVertex!.Next!.B!;

            PEdge l = new((PVertex)vprev, (PVertex)vnext);

            Vertices.Remove(selectedVertex);
            if(selectedVertex.Prev is BezierLine)
            {
                BezierLine bLine = (BezierLine)selectedVertex.Prev;
                bLine.Clear();
                if (!BezierLines.Remove(bLine))
                    throw new Exception();
            }
            else
            {
                if (!Lines.Remove(selectedVertex.Prev))
                    throw new Exception();
            }
            if (selectedVertex.Next is BezierLine)
            {
                BezierLine bLine = (BezierLine)selectedVertex.Next;
                bLine.Clear();
                if (!BezierLines.Remove(bLine))
                    throw new Exception();
            }
            else
            {
                if (!Lines.Remove(selectedVertex.Next))
                throw new Exception();
            }

            vprev.Next = l;
            vnext.Prev = l;

            selectedVertex.Prev.B = vnext;
            selectedVertex.Next.A = vprev;

            Lines.Add(l);
            selectedVertex = null;
            ReconstructVerticesInOrder();
            return true;
        }

        public bool AddVertex()
        {
            if (selectedLine == null)
                return false;

            PVertex A = selectedLine!.A!;
            PVertex C = selectedLine!.B!;

            PVertex B = new(selectedLine.Middle);
            PEdge prev = new(A, B);
            PEdge next = new(B, C);
            B.Prev = prev;
            B.Next = next;

            if(selectedLine.A.Prev is BezierLine)
            {
                BezierLine bline = (BezierLine)(selectedLine.A.Prev);
                C.PropertyChanged -= bline.ConVertexChangePos;
                B.PropertyChanged += bline.ConVertexChangePos;
            }
            if (selectedLine.B.Next is BezierLine) 
            {
                BezierLine bline = (BezierLine)(selectedLine.B.Next);
                A.PropertyChanged -= bline.ConVertexChangePos;
                B.PropertyChanged += bline.ConVertexChangePos;
            }

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
            PVertex v = Vertices[0];
            for (int i = 0; i < Vertices.Count; i++)
            {
                VerticesInOrder.Add(v);
                v = v.Next!.B;
            }
        }

        public void ConvertLineToBezier()
        {
            if (selectedLine == null) 
                return;
            BezierLine bLine = selectedLine.ToBezier();
            BezierLines.Add(bLine);
            Lines.Remove(selectedLine);

            if (selectedLine.A.Prev is BezierLine)
                selectedLine.A.ContinuityType = PVertex.Continuity.G0;
            else if (selectedLine.B.Next is BezierLine)
                selectedLine.B.ContinuityType = PVertex.Continuity.G0;

            selectedLine = null;
        }

        public void ConvertBezierToLine()
        {
            if(selectedBezierLine == null) 
                return;
            PEdge l = selectedBezierLine.ToEdge();
            Lines.Add(l);
            BezierLines.Remove(selectedBezierLine);

            selectedBezierLine.A.ContinuityType = PVertex.Continuity.G0;
            selectedBezierLine.B.ContinuityType = PVertex.Continuity.G0;
            
            selectedBezierLine = null;
        }

        public override bool Locked => throw new NotImplementedException();
        public override void Lock()
        {
            throw new NotImplementedException();
        }

        public override void Unlock()
        {
            throw new NotImplementedException();
        }

        private bool PointInPolygon(Point2 p) => Geometry.PointInPolygon(p, VerticesInOrder);
    }
}
