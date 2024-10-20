using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry.Objects
{
    public class PolygonFactory
    {
        public readonly Point2 Center;
        public PolygonFactory(Point2 center)
        {
            Center = center;
        }

        public Polygon GenerateEquilateral(int n = 5, int R = 100)
        {
            return new Polygon(Center);
        }

        public Polygon GeneratePredefined()
        {
            Polygon res = GenerateEquilateral();
            res.selectedLine = res.Lines[1];
            res.ConvertLineToBezier();
            res.Lines[2].Restriction = PEdge.LineRestriction.ConstantLength;
            res.Lines[3].Restriction = PEdge.LineRestriction.Horizontal;
            return res;
        }

        public Polygon Generate(int n = 5, int R = 100)
        {
            Polygon res = new Polygon(Center, n, R);

            int bLineIdx = Random.Shared.Next(n);
            res.selectedLine = res.Lines[bLineIdx];
            res.ConvertLineToBezier();
            return res;
        }
    }
}
