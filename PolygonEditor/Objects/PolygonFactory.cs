using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Objects
{
    public class PolygonFactory
    {
        public readonly Point Center;
        public PolygonFactory(Point center) 
        {
            Center = center;
        }
        public Polygon Generate(int n = 5, int R = 100) 
        {
            float b = 360 / n;
            List<Vertex> vertices = new(n);
            for (int i = 0; i < n; i++) {
                Vertex v = new Vertex(Center);
                v.Y += (int) (R * Math.Sin(b * i / 360 * 2 * Math.PI));
                v.X += (int) (R * Math.Cos(b * i / 360 * 2 * Math.PI));
                vertices.Add(v);
            }

            List<Line> lines = new(n);
            for(int i = 0; i < n - 1; i++) 
            {
                lines.Add(new Line(vertices[i], vertices[i + 1]));
            }
            lines.Add(new Line(vertices[n - 1], vertices[0]));

            Polygon res = new Polygon();
            for (int i = 0; i < n; i++)
            {
                res.Vertices.Add(vertices[i]);
                res.Lines.Add(lines[i]);
            }
            res.GeneratePointsFromVerices();
            return res;
        }
    }
}
