global using Point2f = PolygonEditor.Geometry.Vec2f;

using PolygonEditor.Geometry.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Geometry
{
    public class Vec2f
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vec2f(float x, float y) { X = x; Y = y; }
        public Vec2f(Vertex v) { X = v.X; Y = v.Y; }
        public Vec2f(System.Drawing.Point p) { X = p.X; Y = p.Y; }
        public Vec2f() { X = Y = 0; }

        public static Vec2f operator +(Vec2f lhs, Vec2f rhs) { return new Vec2f(lhs.X + rhs.X, lhs.Y + rhs.Y); }
        public static Vec2f operator -(Vec2f lhs, Vec2f rhs) { return new Vec2f(lhs.X - rhs.X, lhs.Y - rhs.Y); }

        public static Vec2f operator *(int n, Vec2f v) { return new Vec2f(n * v.X, n * v.Y); }
        public static Vec2f operator *(Vec2f v, int n) { return n * v; }

        public static Vec2f operator /(Vec2f v, int d) { return new Vec2f(v.X / d, v.Y / d); }
        public static Vec2f operator /(Vec2f v, float f) { return new Vec2f(v.X / f, v.Y / f); }

        public static Vec2f operator *(float f, Vec2f v) { return new Vec2f(f * v.X, f * v.Y); }
        public static Vec2f operator *(Vec2f v, float f) { return f * v; }

        public static bool operator ==(Vec2f lhs, Vec2f rhs) { return lhs.X == rhs.X && lhs.Y == rhs.Y; }
        public static bool operator !=(Vec2f lhs, Vec2f rhs) { return !(lhs == rhs); }

        public static explicit operator Point(Vec2f v) { return new Point((int)v.X, (int)v.Y); }
        public static implicit operator Vec2f(Point p) { return new Vec2f(p); }
        public static explicit operator Vec2(Vec2f v) { return new Vec2((int)v.X, (int)v.Y); }
        public static implicit operator Vec2f(Vec2 v) { return new Vec2f(v); }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Vec2f)
                return false;
            return ((Vec2f)obj) == this;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * Y.GetHashCode();
        }
    }
}
