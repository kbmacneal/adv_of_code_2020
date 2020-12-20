using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace adv_of_code_2020.Classes
{
    public enum Ordinal
    {
        North,
        South,
        West,
        East,
        Up = North,
        Down = South,
        Left = West,
        Right = East
    }

    public enum RotationDirection
    {
        Clockwise,
        Anticlockwise,
        Deosil = Clockwise,
        Widdershins = Anticlockwise
    }

    /// <summary>
    /// Common rotation commands for ease of use.
    /// </summary>
    public enum Rotation
    {
        None,
        Right90,
        Right180,
        Left90
    }

    public enum Flip2D
    {
        None,
        Horizontal,
        Vertical,
    }

    public class Oriented<T>
    {
        public T North { get; set; }
        public T South { get; set; }
        public T West { get; set; }
        public T East { get; set; }

        public Oriented(Func<Ordinal, T> factory)
        {
            North = factory(Ordinal.North);
            South = factory(Ordinal.South);
            West = factory(Ordinal.West);
            East = factory(Ordinal.East);
        }

        public T Get(Ordinal ordinal) => ordinal switch
        {
            Ordinal.North => North,
            Ordinal.South => South,
            Ordinal.West => West,
            Ordinal.East => East,
            _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
        };

        public Oriented<R> Map<R>(Func<T, Ordinal, R> map)
        {
            return new Oriented<R>(o => map(Get(o), o));
        }

        public Oriented<T> Rotate(Rotation rotation) => new Oriented<T>(side => Get(side.Rotate(rotation)));

        public List<T> ToList()
        {
            return new List<T> { North, South, West, East };
        }
    }

    public static class OrdinalExtensions
    {
        public static Ordinal Rotate(this Ordinal ordinal, int degrees, RotationDirection direction = RotationDirection.Clockwise)
        {
            if (degrees % 90 != 0)
            {
                throw new NotImplementedException(
                    "Ordinal Rotation is only supported in intervals of 90 degrees. For more accurate turns, use vectors and not ordinals :)");
            }

            var baseTurn = direction == RotationDirection.Clockwise ? Rotation.Right90 : Rotation.Left90;

            return Enumerable.Range(0, degrees / 90).Aggregate(ordinal, (ord, i) => ord.Rotate(baseTurn));
        }

        public static Ordinal Rotate(this Ordinal ordinal, Rotation rotation) => rotation switch
        {
            Rotation.None => ordinal,
            Rotation.Right90 => ordinal switch
            {
                Ordinal.North => Ordinal.East,
                Ordinal.South => Ordinal.West,
                Ordinal.West => Ordinal.South,
                Ordinal.East => Ordinal.North,
                _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
            },
            Rotation.Right180 => ordinal.Rotate(Rotation.Right90),
            Rotation.Left90 => ordinal.Rotate(Rotation.Right180).Rotate(Rotation.Right90),
            _ => throw new ArgumentOutOfRangeException(nameof(rotation))
        };
    }
}