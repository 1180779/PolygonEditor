﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public class BezierLine : Line
    {
        bool validState = false;

        Point2 LastA;
        Point2 LastB;
        public static readonly Pen controlLinePen = new Pen(Color.Blue);
        static BezierLine()
        {
            controlLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
        }

        public BezierControlVertex? selectedControlVertex = null;
        public List<Vertex> Vertices = new() { };
        public List<Point2> Approximation = [];
        private void FindApproximation()
        {
            if (A == null || B == null || P2 == null || P3 == null)
                throw new InvalidOperationException();
            Approximation.Clear();

            float step = 0.05f;
            Point2f I1A, I1B, I1C, I2A, I2B, I3A;
            Point2f a = A.Point;
            Point2f p2 = P2.Point;
            Point2f p3 = P3.Point;
            Point2f b = B.Point;
            for (float t = 0; t <= 1.01f; t += step)
            {
                float t2 = 1 - t;
                I1A = t2 * a + t * p2;
                I1B = t2 * p2 + t * p3;
                I1C = t2 * p3 + t * b;

                I2A = t2 * I1A + t * I1B;
                I2B = t2 * I1B + t * I1C;

                I3A = t2 * I2A + t * I2B;
                Approximation.Add((Point2)I3A);
            }
        }
        public void Init()
        {
            if (validState)
                return;
            validState = true;

            Vertices.Add(A);
            Vertices.Add(P2);
            Vertices.Add(P3);
            Vertices.Add(B);

            LastA = A.Point;
            LastB = B.Point;

            A.Prev.A.PropertyChanged += ConVertexChangePos;
            B.Next.B.PropertyChanged += ConVertexChangePos;

            Vec2 v = B.Point - A.Point;
            Vec2 w = new Vec2(-v.Y, v.X);
            P2.Point = P2.Point + v / 2 + w / 2;
            P3.Point = P3.Point - v / 2 - w / 2;

            FindApproximation();
        }

        public void Clear()
        {
            if (!validState)
                return;
            validState = false;

            Vertices.Clear();

            A.Prev.A.PropertyChanged -= ConVertexChangePos;
            B.Next.B.PropertyChanged -= ConVertexChangePos;

            Approximation.Clear();
        }
        public BezierControlVertex? P2 { get; set; }
        public BezierControlVertex? P3 { get; set; }
        public BezierControlLine? AP2 { get; set; }
        public BezierControlLine? P2P3 { get; set; }
        public BezierControlLine? P3B { get; set; }

        public override int S_RADIUS => throw new NotImplementedException();

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
        public BezierLine() : base() { Init(); }
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
        }
        public override void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b)
        {
            //for (int i = 0; i < Approximation.Count - 1; i++)
            //    g.DrawLine(p, Approximation[i], Approximation[i + 1]);
            //g.DrawLine(p, Approximation[Approximation.Count - 1], B.Point);
            g.DrawBezier(p, A.Point, P2.Point, P3.Point, B.Point);

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
                g.FillPolygon(s, new System.Drawing.Point[] { A.Point, P2.Point, P3.Point, B.Point });
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

        public override void Move(Vec2 v)
        {
            if (selectedControlVertex == null)
            {
                base.Move(v);
                P2.Move(v);
                P3.Move(v);
                FindApproximation();
                return;
            }

            BezierControlVertex otherControlVertex = selectedControlVertex == P2 ? P3 : P2;
            otherControlVertex.Lock();
            A.Lock();
            B.Lock();
            selectedControlVertex.MoveLockUnlockLater(v);
            
            // handle continuity on vertecies adjecend to bezier lin
            if(selectedControlVertex == P2)
            {
                if (A.ContinuityType == Vertex.Continuity.G0) { }
                else if (A.ContinuityType == Vertex.Continuity.G1)
                {
                    Vertex u = A.Prev.A;
                    if(!Geometry.PointsInLine(u.Point, P2.Point, A.Point)) 
                    {
                        u.Point = (Geometry.ProjectPointOntoLine(u.Point, P2.Point, A.Point));
                    }
                }
                else if(A.ContinuityType == Vertex.Continuity.C1)
                {
                    Vertex u = A.Prev.A;
                    Vec2 w = P2.Point - A.Point;
                    u.Point = A.Point - 3 * w;
                }
            }
            else
            {
                if (B.ContinuityType == Vertex.Continuity.G0) { }
                else if(B.ContinuityType == Vertex.Continuity.G1)
                {
                    Vertex u = B.Next.B;
                    if (!Geometry.PointsInLine(u.Point, B.Point, P3.Point))
                    { 
                        u.Point = (Geometry.ProjectPointOntoLine(u.Point, P3.Point, B.Point));
                    }
                }
                else if (B.ContinuityType == Vertex.Continuity.C1)
                {
                    Vertex u = B.Next.B;
                    Vec2 w = P3.Point - B.Point;
                    u.Point = B.Point - 3 * w;
                }
            }
            selectedControlVertex.Unlock();
            otherControlVertex.Unlock();
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
            Vertex v = (Vertex)sender;
            if (v == A)
            {
                Point2 tempA = LastA;
                LastA = A.Point;
                if (A.ContinuityType == Vertex.Continuity.G0) { }
                if (A.ContinuityType == Vertex.Continuity.G1)
                {
                    A.Prev.A.Point = Geometry.ProjectPointOntoLine(A.Prev.A.Point, A.Point, P2.Point);
                }
                else if (A.ContinuityType == Vertex.Continuity.C1)
                {
                    Vertex.MoveLockDelayNotify(A.Prev.A, P2, A.Point - tempA);
                }
            }
            else if (v == B)
            {
                Point2 tempB = LastB;
                LastB = B.Point;
                if (B.ContinuityType == Vertex.Continuity.G0) { }
                else if (B.ContinuityType == Vertex.Continuity.G1)
                {
                    B.Next.B.Point = Geometry.ProjectPointOntoLine(B.Next.B.Point, P3.Point, B.Point);
                }
                else if (B.ContinuityType == Vertex.Continuity.C1)
                {
                    Vertex.MoveLockDelayNotify(B.Next.B, P3, B.Point - tempB);
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
            Vertex v = (Vertex)sender;
            if (v == A.Prev.A)
            {
                if (A.ContinuityType == Vertex.Continuity.G0) { }
                else if (A.ContinuityType == Vertex.Continuity.G1)
                {
                    if(!Geometry.PointsInLine(P2.Point, A.Point, v.Point))
                    {
                        P2.Point = Geometry.ProjectPointOntoLine(P2.Point, A.Point, v.Point);
                    }
                }
                else if (A.ContinuityType == Vertex.Continuity.C1)
                {
                    Vertex u = A.Prev.A;
                    Vec2 w = u.Point - A.Point;
                    P2.Point = A.Point - w / 3;
                }
            }
            else if (v == B.Next.B)
            {
                if (B.ContinuityType == Vertex.Continuity.G0) { }
                else if (B.ContinuityType == Vertex.Continuity.G1)
                {
                    if (!Geometry.PointsInLine(v.Point, A.Point, B.Point))
                    {
                        P3.Point = Geometry.ProjectPointOntoLine(P3.Point, v.Point, B.Point);
                    }
                }
                else if (B.ContinuityType == Vertex.Continuity.C1)
                {
                    Vertex u = B.Next.B;
                    Vec2 w = u.Point - B.Point;
                    P3.Point = B.Point - w / 3;
                }
            }
            else
                throw new InvalidOperationException();
            LastA = A.Point;
            LastB = B.Point;
        }

        public Line BezierToLine()
        {
            Clear();
            Line l = new Line(A, B);
            A.Next = l;
            B.Prev = l;
            return l;
        }
    }
}
