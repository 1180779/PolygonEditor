using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public abstract class MovableItem : Item
    {
        public abstract void Move(Point PML, Point ML);
    }
}
