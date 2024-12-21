using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public interface IStructureItem
    {
        public abstract void MoveInStructure(Vec2 v);
        public abstract void NotifyInStructure();
    }
}
