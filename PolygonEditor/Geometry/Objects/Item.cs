using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public abstract class Item
    {
        public const int SA_RADIUS = 5; // selection added radius
        public abstract int S_RADIUS { get; } // selection radius

        public abstract bool IsSelected(Point2 p);
        public abstract void DrawSelection(Graphics g, Pen p, Brush s);
        public abstract void Draw(DirectBitmap dbitmap, Graphics g, Pen p, Brush b);
        public abstract void DrawLibrary(DirectBitmap dbitmap, Graphics g, Pen p, Brush b);
        public abstract void DrawSelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s);
        public abstract void DrawLibrarySelected(DirectBitmap dbitmap, Graphics g, Pen p, Brush b, Brush s);
        
        public abstract bool Locked { get; }
        public abstract void Lock();
        public abstract void Unlock();
        public abstract void Move(Vec2 v);
    }
}
