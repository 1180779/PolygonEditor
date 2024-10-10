using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public abstract class Item
    {
        public const int SA_RADIUS = 5; // selection added radius
        public abstract int S_RADIUS { get; } // selection radius

        public abstract void Draw(Graphics g, Pen p, Brush b);
        public abstract bool Selected(Point ML);
        public abstract void DrawSelected(Graphics g, Pen p, Brush b, Brush s);
    }
}
