using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public abstract class Item
    {
        public abstract void Draw(Graphics g, Pen p, Brush b);

    }
}
