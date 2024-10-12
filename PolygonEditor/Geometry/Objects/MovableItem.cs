using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public abstract class MovableItem : Item
    {
        public abstract void Move(Vec2 v);
    }
}
