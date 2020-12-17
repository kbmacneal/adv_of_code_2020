using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace adv_of_code_2020.Classes
{
    [DebuggerDisplay("X={X} Y={Y}")]
    public struct Point3D : IEquatable<Point3D>
    {
        public int X { get; }

        public int Y { get; }
        public int Z { get; set; }


        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj) => obj is Point3D other ? Equals(other) : base.Equals(obj);

        public override int GetHashCode() => X ^ Y ^ Z;

        public bool Equals(Point3D other) => X == other.X && Y == other.Y;

        public static bool operator ==(Point3D a, Point3D b) => a.Equals(b);

        public static bool operator !=(Point3D a, Point3D b) => !a.Equals(b);

        public static Point3D operator +(Point3D a, Point3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Point3D operator -(Point3D a, Point3D b) => new Point3D(a.X - b.X, a.Y - b.Y, a.Z-b.Z);

        public static Point3D operator *(Point3D a, int b) => new Point3D(a.X * b, a.Y * b, a.Z*b);

        public static Point3D Empty { get; } = new Point3D(0, 0,0);

        public List<Point3D> GetSurrounding(List<Point3D> others)
        {
            List<Point3D> surrounding = new List<Point3D>();

            List<Point3D> comparitors = new List<Point3D>() { this, new Point3D(X - 1, Y, Z), new Point3D(X + 1, Y, Z) };

            foreach (var point in comparitors)
            {
                
            }

            return surrounding;
        }
    }
}