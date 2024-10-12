global using Point2 = PolygonEditor.Geometry.Vec2;

using PolygonEditor.Geometry.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry
{
    public struct Vec2
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vec2(int x, int y) {  X = x; Y = y; }
        public Vec2(Vertex v) { X = v.X; Y = v.Y; }
        public Vec2(System.Drawing.Point p) { X = p.X; Y = p.Y; }
        public Vec2() { X = Y = 0; }

        public static Vec2 operator+(Vec2 lhs, Vec2 rhs) { return new Vec2(lhs.X + rhs.X, lhs.Y + rhs.Y); }
        public static Vec2 operator-(Vec2 lhs, Vec2 rhs) { return new Vec2(lhs.X - rhs.X, lhs.Y - rhs.Y); }

        public static Vec2 operator*(int n, Vec2 v) { return new Vec2(n * v.X, n * v.Y); }
        public static Vec2 operator*(Vec2 v, int n) { return n * v; }

        public static Vec2 operator/(Vec2 v, int d) { return new Vec2(v.X / d, v.Y / d); }

        public static Vec2 operator*(float f, Vec2 v) { return new Vec2((int)(f * v.X), (int)(f * v.Y)); }
        public static Vec2 operator*(Vec2 v, float f) { return f * v; }

        public static bool operator==(Vec2 lhs, Vec2 rhs) { return lhs.X == rhs.X && lhs.Y == rhs.Y; }
        public static bool operator!=(Vec2 lhs, Vec2 rhs) { return !(lhs == rhs); }

        public static implicit operator Point(Vec2 v) { return new Point(v.X, v.Y); }
        public static implicit operator Vec2(Point p) { return new Vec2(p); }

        public override readonly bool Equals(object? obj)
        {
            if(obj == null || obj is not Vec2)
                return false;
            return ((Vec2)obj) == this;
        }

        public override readonly int GetHashCode()
        {
            return X.GetHashCode() * Y.GetHashCode();
        }
    }
}
