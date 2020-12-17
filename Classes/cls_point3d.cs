using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace adv_of_code_2020.Classes
{
    [DebuggerDisplay("X={X} Y={Y} Z={Z}")]
    public class Point3D : IEquatable<Point3D>
    {
        public int X { get; set; }

        public int Y { get; set; }
        public int Z { get; set; }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj) => obj is Point3D other ? Equals(other) : base.Equals(obj);

        public override int GetHashCode() => X ^ Y ^ Z;

        public bool Equals(Point3D other) => X == other.X && Y == other.Y && Z == other.Z;

        public static bool operator ==(Point3D a, Point3D b) => a.Equals(b);

        public static bool operator !=(Point3D a, Point3D b) => !a.Equals(b);

        public static Point3D operator +(Point3D a, Point3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Point3D operator -(Point3D a, Point3D b) => new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Point3D operator *(Point3D a, int b) => new Point3D(a.X * b, a.Y * b, a.Z * b);

        public static Point3D Empty { get; } = new Point3D(0, 0, 0);

        public IEnumerable<Point3D> GetSurrounding()
        {
            List<Point3D> points = new List<Point3D>();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        points.Add(new Point3D(X + x, Y + y, Z + z));
                    }
                }
            }

            return points;
        }
    }
}