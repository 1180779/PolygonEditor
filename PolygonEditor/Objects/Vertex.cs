using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolygonEditor.Objects
{
    public class Vertex : MovableItem, INotifyPropertyChanged
    {
        
        private const int RADIUS = 5;

        private Point _point;

        public event PropertyChangedEventHandler? PropertyChanged;
        public const string CHANGED_P = "p";
        public Point Point { get { return _point; } set { _point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(CHANGED_P)); } }
        public const string CHANGED_X = "x";
        public int X { get { return _point.X; } set { _point.X = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(CHANGED_X)); } }
        public const string CHANGED_Y = "y";
        public int Y { get { return _point.Y; } set { _point.Y = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(CHANGED_Y)); } }

        private Line? _prev = null;
        public Line? Prev 
        { 
            get { return _prev; } 
            set 
            {
                if (_prev != null)
                    PropertyChanged -= _prev.VertexChanged;

                _prev = value; 
                if(value != null) 
                    PropertyChanged += value.VertexChanged; 
            } 
        }
        private Line? _next = null;
        public Line? Next 
        { 
            get { return _next; }
            set 
            {
                if (_next != null)
                    PropertyChanged -= _next.VertexChanged;

                _next = value;
                if (value != null)
                    PropertyChanged += value.VertexChanged;
            } 
        }

        public override int S_RADIUS => RADIUS + Item.SA_RADIUS;

        public Vertex(Point p) { Point = p; }
        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
            Prev = null;
            Next = null;
        }

        public override void Draw(Graphics g, Pen p, Brush b) 
        {
            g.FillEllipse(b, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }
        public override void DrawSelected(Graphics g, Pen p, Brush b, Brush s)
        {
            g.FillEllipse(s, X - S_RADIUS, Y - S_RADIUS, S_RADIUS * 2, S_RADIUS * 2);
            g.FillEllipse(b, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
            g.DrawEllipse(p, X - RADIUS, Y - RADIUS, RADIUS * 2, RADIUS * 2);
        }

        public override void Move(Point PML, Point ML)
        {
            _point.X += ML.X - PML.X;
            _point.Y += ML.Y - PML.Y;
        }

        public override bool Selected(Point ML)
        {
            return Geometry.Dist2(_point, ML) <= RADIUS * RADIUS;
        }
    }
}
