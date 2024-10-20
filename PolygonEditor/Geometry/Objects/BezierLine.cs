using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public class BezierLine : PEdge, IStructureItem
    {
        public BezierControlVertex P2 { get; set; }
        public BezierControlVertex P3 { get; set; }
        public BezierControlLine AP2 { get; set; }
        public BezierControlLine P2P3 { get; set; }
        public BezierControlLine P3B { get; set; }


        public BezierControlVertex? selectedControlVertex = null;
        public List<PVertex> Vertices = new() { };
        public List<Point2> Approximation = [];

        bool validState = false;

        Point2 LastA;
        Point2 LastB;

        public override int S_RADIUS => throw new NotImplementedException();

        public BezierLine(PVertex a, PVertex b) : base(a, b)
        {
            if (a == null || b == null)
                throw new InvalidOperationException();
            if (A.Prev == null || B.Next == null)
                throw new InvalidOperationException();

            Vec2 v = B.Point - A.Point;
            Vec2 w = new Vec2(-v.Y, v.X);
            P2 = new BezierControlVertex(A.Point + v / 2 + w / 2);
            P3 = new BezierControlVertex(B.Point - v / 2 - w / 2);

            AP2 = new BezierControlLine(A, P2);
            P2P3 = new BezierControlLine(P2, P3);
            P3B = new BezierControlLine(P3, B);

            Vertices.Add(A);
            Vertices.Add(P2);
            Vertices.Add(P3);
            Vertices.Add(B);

            A.Next = this;
            B.Prev = this;

            A.Prev.A.PropertyChanged += ConVertexChangePos;
            B.Next.B.PropertyChanged += ConVertexChangePos;

            FindApproximation();

            validState = true;
        }

        ~BezierLine()
        {
            Clear();
        }

        
        public static readonly Pen controlLinePen = new Pen(Color.Blue);
        static BezierLine()
        {
            controlLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
        }

        private void FindApproximation()
        {
            if (A == null || B == null || P2 == null || P3 == null)
                throw new InvalidOperationException();
            Approximation.Clear();

            //float maxL = float.Max(float.Max(float.Max(Length2, Geometry.Dist2(A, P2)), Geometry.Dist2(P2, P3)), Geometry.Dist2(P3, B));
            //float step = 1.0f / (maxL / 10000 + 20);//0.05f;
            float step = 0.02f;
            float step2 = step * step;
            float step3 = step * step2;

            // change base
            Point2f A0 = A.Point;
            Point2f A1 = 3 * (P2.Point - A.Point);
            Point2f A2 = 3 * (P3.Point - 2*P2.Point + A.Point);
            Point2f A3 = B.Point - 3*P3.Point + 3*P2.Point - A.Point;

            Point2f Ptd = A0;
            Point2f dPtd = A3* step3 + A2*step2 + A1*step;
            Point2f d2Ptd = 6*A3* step3 + 2*A2*step2;
            Point2f d3Ptd = 6*A3*step3;

            Approximation.Add((Point2)Ptd);
            for(float t = 0.0f; t < 1.0f - 1.5*step; t += step)
            {
                Ptd = Ptd + dPtd;
                dPtd = dPtd + d2Ptd;
                d2Ptd = d2Ptd + d3Ptd;
                Approximation.Add((Point2)Ptd);
            }

            // De Casteljau’s algorithm

            //float step = 0.05f;
            //Point2f I1A, I1B, I1C, I2A, I2B, I3A;
            //Point2f a = A.Point;
            //Point2f p2 = P2.Point;
            //Point2f p3 = P3.Point;
            //Point2f b = B.Point;
            //for (float t = 0; t <= 1.01f; t += step)
            //{
            //    float t2 = 1 - t;
            //    I1A = t2 * a + t * p2;
            //    I1B = t2 * p2 + t * p3;
            //    I1C = t2 * p3 + t * b;

            //    I2A = t2 * I1A + t * I1B;
            //    I2B = t2 * I1B + t * I1C;

            //    I3A = t2 * I2A + t * I2B;
            //    Approximation.Add((Point2)I3A);
            //}
        }

        // remove linering PropertyChange delegates from adjecend vertices
        // to be called after every bezier line removal from polygon
        public void Clear()
        {
            if (!validState)
                return;

            validState = false;

            Vertices.Clear();

            A.Prev!.A.PropertyChanged -= ConVertexChangePos;
            B.Next!.B.PropertyChanged -= ConVertexChangePos;

            Approximation.Clear();
        }
        
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            for (int i = 0; i < Approximation.Count - 1; i++) 
                MyDrawing.PlotLine(Approximation[i], Approximation[i + 1], dbitmap, p.Color);
            MyDrawing.PlotLine(Approximation[Approximation.Count - 1], B.Point, dbitmap, p.Color);

            AP2.Draw(dbitmap, g, controlLinePen, b);
            P2P3.Draw(dbitmap, g, controlLinePen, b);
            P3B.Draw(dbitmap, g, controlLinePen, b);

            A.Draw(dbitmap, g, p, b);
            B.Draw(dbitmap, g, p, b);
            P2.Draw(dbitmap, g, p, b);
            P3.Draw(dbitmap, g, p, b);

            LastA = A.Point;
            LastB = B.Point;
        }
        public override void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            for (int i = 0; i < Approximation.Count - 1; i++)
                g.DrawLine(p, Approximation[i], Approximation[i + 1]);
            g.DrawLine(p, Approximation[Approximation.Count - 1], B.Point);

            AP2.DrawLibrary(dbitmap, g, controlLinePen, b);
            P2P3.DrawLibrary(dbitmap, g, controlLinePen, b);
            P3B.DrawLibrary(dbitmap, g, controlLinePen, b);

            A.DrawLibrary(dbitmap, g, p, b);
            B.DrawLibrary(dbitmap, g, p, b);
            P2.DrawLibrary(dbitmap, g, p, b);
            P3.DrawLibrary(dbitmap, g, p, b);
        }
        public override void DrawSelection(Graphics g, Pen p, Brush s)
        {
            if (selectedControlVertex != null)
                selectedControlVertex.DrawSelection(g, p, s);
            else
                g.FillPolygon(s, new Point[] { A.Point, P2.Point, P3.Point, B.Point });
        }
        public override void DrawSelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            DrawSelection(g, p, s);
            Draw(dbitmap, g, p, b);
        }
        public override void DrawLibrarySelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s)
        {
            DrawSelection(g, p, s);
            DrawLibrary(dbitmap, g, p, b);
        }

        public override void Lock()
        {
            base.Lock();
            P2.Lock();
            P3.Lock();
        }
        public override void Unlock()
        {
            base.Unlock();
            P2.Unlock();
            P3.Unlock();
        }
        public override void Move(Vec2 v)
        {
            if (selectedControlVertex == null)
            {
                base.MoveInStructure(v);
                MoveInStructure(v);
                Lock();
                NotifyInStructure();
                Unlock();
                FindApproximation();
                return;
            }

            BezierControlVertex otherControlVertex = selectedControlVertex == P2 ? P3 : P2;
            A.Lock();
            B.Lock();
            selectedControlVertex.MoveLockUnlockLater(v);
            
            // handle continuity on vertecies adjecend to bezier line
            if(selectedControlVertex == P2)
            {
                if (A.ContinuityType == PVertex.Continuity.G0) { }
                else if (A.ContinuityType == PVertex.Continuity.G1)
                {
                    PVertex u = A.Prev!.A;
                    if(!Geometry.PointsInLine(u.Point, P2.Point, A.Point)) 
                    {
                        u.Point = (Geometry.ProjectPointOntoLine(u.Point, P2.Point, A.Point));
                    }
                }
                else if(A.ContinuityType == PVertex.Continuity.C1)
                {
                    PVertex u = A.Prev!.A;
                    Vec2 w = P2.Point - A.Point;
                    u.Point = A.Point - 3 * w;
                }
            }
            else
            {
                if (B.ContinuityType == PVertex.Continuity.G0) { }
                else if(B.ContinuityType == PVertex.Continuity.G1)
                {
                    PVertex u = B.Next!.B;
                    if (!Geometry.PointsInLine(u.Point, B.Point, P3.Point))
                    { 
                        u.Point = (Geometry.ProjectPointOntoLine(u.Point, P3.Point, B.Point));
                    }
                }
                else if (B.ContinuityType == PVertex.Continuity.C1)
                {
                    PVertex u = B.Next!.B;
                    Vec2 w = P3.Point - B.Point;
                    u.Point = B.Point - 3 * w;
                }
            }
            selectedControlVertex.Unlock();
            A.Unlock();
            B.Unlock();
            FindApproximation();
        }
        public void MoveInPolygon(Vec2 v)
        {
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            P2.Move(v);
            P3.Move(v);
            FindApproximation();
        }

        public bool IsSelectedItem(Point2 p)
        {
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            if (P2.IsSelected(p))
            {
                selectedControlVertex = P2;
                return true;
            }
            if (P3.IsSelected(p))
            {
                selectedControlVertex = P3;
                return true;
            }
            return false;
        }

        public override bool IsSelected(Point2 p)
        {
            if (P2 == null || P3 == null)
                throw new InvalidOperationException();
            selectedControlVertex = null;
            return IsSelectedItem(p) || Geometry.PointInPolygon(p, Vertices);
        }

        public override void VertexChangedPos(object? sender, PropertyChangedEventArgs e)
        {
            if(sender == null) 
                throw new InvalidOperationException();

            Vertex v = (Vertex)sender;
            if (v == A)
            {
                Point2 tempA = LastA;
                LastA = A.Point;
                if (A.ContinuityType == PVertex.Continuity.G0) { }
                if (A.ContinuityType == PVertex.Continuity.G1)
                {
                    A.Prev!.A.Point = Geometry.ProjectPointOntoLine(A.Prev.A.Point, A.Point, P2.Point);
                }
                else if (A.ContinuityType == PVertex.Continuity.C1)
                {
                    PVertex.MoveLockDelayNotify(A.Prev!.A, P2, A.Point - tempA);
                }
            }
            else if (v == B)
            {
                Point2 tempB = LastB;
                LastB = B.Point;
                if (B.ContinuityType == PVertex.Continuity.G0) { }
                else if (B.ContinuityType == PVertex.Continuity.G1)
                {
                    B.Next!.B.Point = Geometry.ProjectPointOntoLine(B.Next.B.Point, P3.Point, B.Point);
                }
                else if (B.ContinuityType == PVertex.Continuity.C1)
                {
                    PVertex.MoveLockDelayNotify(B.Next!.B, P3, B.Point - tempB);
                }
            }
            else
                throw new InvalidOperationException();
            LastA = A.Point;
            LastB = B.Point;
            FindApproximation();
        }

        public void ConVertexChangePos(object? sender, PropertyChangedEventArgs e) 
        {
            if(sender == null)
                throw new InvalidOperationException();

            PVertex v = (PVertex)sender;
            if (v == A.Prev!.A)
            {
                if (A.ContinuityType == PVertex.Continuity.G0) { }
                else if (A.ContinuityType == PVertex.Continuity.G1)
                {
                    if(!Geometry.PointsInLine(P2.Point, A.Point, v.Point))
                    {
                        Point2 newP2 = Geometry.ProjectPointOntoLine(P2.Point, A.Point, v.Point);
                        // TO DO: push P2 out if between A and v
                        if (Geometry.PointOnEdge(newP2, A.Point, v.Point))
                        {
                            // push out of edge
                            P2.Point = A.Point - (newP2 - A.Point);
                        }
                        else
                        {
                            P2.Point = newP2;
                        }
                        FindApproximation();
                    }
                }
                else if (A.ContinuityType == PVertex.Continuity.C1)
                {
                    Vertex u = A.Prev.A;
                    Vec2 w = u.Point - A.Point;
                    P2.Point = A.Point - w / 3;
                    FindApproximation();
                }
            }
            else if (v == B.Next!.B)
            {
                if (B.ContinuityType == PVertex.Continuity.G0) { }
                else if (B.ContinuityType == PVertex.Continuity.G1)
                {
                    if (!Geometry.PointsInLine(v.Point, A.Point, B.Point))
                    {
                        Point2 newP3 = Geometry.ProjectPointOntoLine(P3.Point, v.Point, B.Point);
                        if (Geometry.PointOnEdge(newP3, v.Point, B.Point))
                        {
                            // push out of edge
                            P3.Point = B.Point - (newP3 - B.Point);
                        }
                        else
                        {
                            P3.Point = newP3;
                        }
                        FindApproximation();
                    }
                }
                else if (B.ContinuityType == PVertex.Continuity.C1)
                {
                    PVertex u = B.Next.B;
                    Vec2 w = u.Point - B.Point;
                    P3.Point = B.Point - w / 3;
                    FindApproximation();
                }
            }
            else
                throw new InvalidOperationException();
            LastA = A.Point;
            LastB = B.Point;
        }

        public PEdge ToEdge()
        {
            Clear();
            PEdge l = new PEdge(A, B);
            A.Next = l;
            B.Prev = l;
            return l;
        }

        public new void MoveInStructure(Vec2 v) 
        {
            P2.MoveInStructure(v);
            P3.MoveInStructure(v);
        }

        public new void NotifyInStructure()
        {
            base.NotifyInStructure();
            P2.NotifyInStructure();
            P3.NotifyInStructure();
        }
    }
}
