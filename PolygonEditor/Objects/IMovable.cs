using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public interface IMovable
    {
        public void Move(Point prevML, Point ML);
        public bool Selected(Point ML);
    }
}
