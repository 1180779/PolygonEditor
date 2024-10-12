﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonEditor.Geometry.Objects
{
    public class Vertex : MovableItem, INotifyPropertyChanged
    {
        public bool Locked { get; private set; } = false;
        private void Lock() { Locked = true; }
        private void Unclock() { Locked = false; }


        public const int MAXDEPTH = 100;
        public const int MAXITERATIONS = 10000;
        public int Depth { get; private set; } = 0;
        public int Iterations { get; private set; } = 0;
        protected const int RADIUS = 5;
        private void NotifyPropertyChanged()
        {
            if (Depth == 0)
                Iterations = 0;
            Iterations++;
            Depth++;
            if (Depth < MAXDEPTH)
            {
                if (Iterations <= MAXITERATIONS)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
            Depth--;

        }


        private Point2 _point;
        public event PropertyChangedEventHandler? PropertyChanged;
        public Point2 Point
        {
            get { return _point; }
            set
            {
                if (Locked)
                {
                    NotifyPropertyChanged();
                    return;
                }
                if (_point.X != value.X || _point.Y != value.Y)
                {
                    _point = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int X
        {
            get { return _point.X; }
            set
            {
                if (Locked)
                {
                    NotifyPropertyChanged();
                    return;
                }
                if (_point.X != value)
                {
                    _point.X = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int Y
        {
            get { return _point.Y; }
            set
            {
                if (Locked)
                {
                    NotifyPropertyChanged();
                    return;
                }
                if (_point.Y != value)
                {
                    _point.Y = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Line? _prev = null;
        public Line? Prev
        {
            get { return _prev; }
            set
            {
                if (_prev != null)
                    PropertyChanged -= _prev.VertexChangedPos;

                _prev = value;
                if (value != null)
                    PropertyChanged += value.VertexChangedPos;
            }
        }
        private Line? _next = null;
        public Line? Next
        {
            get { return _next; }
            set
            {
                if (_next != null)
                    PropertyChanged -= _next.VertexChangedPos;

                _next = value;
                if (value != null)
                    PropertyChanged += value.VertexChangedPos;
            }
        }

        public override int S_RADIUS => RADIUS + SA_RADIUS;

        public Vertex(Point2 p) { Point = p; }
        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
            Prev = null;
            Next = null;
        }

        public override bool IsSelected(Point2 p)
        {
            return Geometry.Dist2(_point, p) <= RADIUS * RADIUS;
        }
        public override void DrawSelection(Graphics g, Pen p, Brush s)
        {
            g.FillEllipse(s, X - S_RADIUS, Y - S_RADIUS, S_RADIUS * 2, S_RADIUS * 2);
        }
        public override void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b) => DrawLibrary(g, p, b);
        public override void DrawLibrary(Graphics g, Pen p, Brush b)
        {
            g.FillEllipse(b, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }
        public override void DrawSelected(DirectBitmap dbitmapg, Graphics g, Pen p, Brush b, Brush s) => DrawLibrarySelected(g, p, b, s);
        public override void DrawLibrarySelected(Graphics g, Pen p, Brush b, Brush s)
        {
            DrawSelection(g, p, s);
            g.FillEllipse(b, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }

        public void MoveLock(Vec2 v)
        {
            Lock();
            _point += v;
            NotifyPropertyChanged();
            Unclock();
        }
        public override void Move(Vec2 v) => Point += v;
        public void MoveNoNotify(Vec2 v) => _point += v;

        //
        //
        //
        //

        public static void MoveDelayNotify(Vertex A, Vertex B, Vec2 v)
        {
            A._point += v;
            B._point += v;
            A.NotifyPropertyChanged();
            B.NotifyPropertyChanged();
        }

        public static void MoveLockDelayNotify(Vertex A, Vertex B, Vec2 v)
        {
            A._point += v;
            B._point += v;
            A.Lock();
            B.Lock();
            A.NotifyPropertyChanged();
            B.NotifyPropertyChanged();
            A.Unclock();
            B.Unclock();
        }

    }
}