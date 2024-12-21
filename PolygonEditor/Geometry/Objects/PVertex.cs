using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public class PVertex : Vertex, INotifyPropertyChanged, IStructureItem
    {
        public PVertex(Point2 p) : base(p) { }

        public enum Continuity { G0, G1, C1 };
        private Continuity _continuity = Continuity.G0;
        public Continuity ContinuityType
        {
            get { return _continuity; }
            set
            {
                if (_continuity != value)
                {
                    _continuity = value;
                    (Prev!.A!).NotifyPropertyChanged();
                    (Next!.B!).NotifyPropertyChanged();
                }
            }
        }

        public const int MAXDEPTH = 10;
        public const int MAXITERATIONS = 25;
        public int Depth { get; private set; } = 0;
        public int Iterations { get; private set; } = 0;
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged()
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
        public override Point2 Point
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
        public override int X
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
        public override int Y
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

        protected new PEdge? _prev = null;
        protected new PEdge? _next = null;
        public new PEdge? Prev
        {
            get { return _prev; }
            set
            {
                if (_prev != null)
                    PropertyChanged -= _prev.VertexChangedPos;

                base.Prev = value;
                _prev = value;
                if (value != null)
                    PropertyChanged += value.VertexChangedPos;
            }
        }
        public new PEdge? Next
        {
            get { return _next; }
            set
            {
                if (_next != null)
                    PropertyChanged -= _next.VertexChangedPos;

                base.Next = value;
                _next = value;
                if (value != null)
                    PropertyChanged += value.VertexChangedPos;
            }
        }

        public override void Move(Vec2 v) => Point += v;
        public void MoveLock(Vec2 v)
        {
            if (Locked)
                return;
            Lock();
            _point += v;
            NotifyPropertyChanged();
            Unlock();
        }
        public void MoveLockUnlockLater(Vec2 v)
        {
            if (Locked)
                return;
            Lock();
            _point += v;
            NotifyPropertyChanged();
        }
        public void MoveNoNotify(Vec2 v)
        {
            if (!Locked)
                _point += v;
        }

        public static void MoveLockDelayNotify(PVertex A, PVertex B, Vec2 v)
        {
            if (!A.Locked && !B.Locked)
            {
                A.Lock();
                B.Lock();
                A._point += v;
                B._point += v;
                A.NotifyPropertyChanged();
                B.NotifyPropertyChanged();
                A.Unlock();
                B.Unlock();
            }
            else if (!A.Locked)
            {
                A.MoveLock(v);
            }
            else
            {
                B.MoveLock(v);
            }
        }

        public void MoveInStructure(Vec2 v) 
        {
            _point += v;
        }
        public void NotifyInStructure()
        {
            NotifyPropertyChanged();
        }
    }
}
