using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry
{
    public static class MyDrawing
    {
        // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm#All_cases
        private static void PlotLineLow(Point a, Point b, DirectBitmap dbitmap, Color c, int dist = 1) // dist - gap between pixels
        {
            int tdist = 0;
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            int yi = 1;
            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            int d = 2 * dy - dx;
            int y = a.Y;

            for (int x = a.X; x < b.X; ++x)
            {
                tdist++;
                if (tdist == dist)
                {
                    dbitmap.SetPixel(x, y, c);
                    tdist = 0;
                }
                if (d > 0)
                {
                    y += yi;
                    d += 2 * (dy - dx);
                }
                else
                {
                    d += 2 * dy;
                }
            }
        }
        private static void PlotLineHigh(Point a, Point b, DirectBitmap dbitmap, Color c, int dist = 1)
        {
            int tdist = 0;
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            int d = 2 * dx - dy;
            int x = a.X;

            for (int y = a.Y; y < b.Y; ++y)
            {
                tdist++;
                if (tdist == dist)
                {
                    dbitmap.SetPixel(x, y, c);
                    tdist = 0;
                }
                if (d > 0)
                {
                    x += xi;
                    d += 2 * (dx - dy);
                }
                else
                {
                    d += 2 * dx;
                }
            }
        }
        public static void PlotLine(Point a, Point b, DirectBitmap dbitmap, Color c, int dist = 1)
        {
            if (Math.Abs(b.Y - a.Y) < Math.Abs(b.X - a.X))
            {
                if (a.X > b.X)
                    PlotLineLow(b, a, dbitmap, c, dist);
                else
                    PlotLineLow(a, b, dbitmap, c, dist);
            }
            else
            {
                if (a.Y > b.Y)
                    PlotLineHigh(b, a, dbitmap, c, dist);
                else
                    PlotLineHigh(a, b, dbitmap, c, dist);
            }
        }
    }
}
