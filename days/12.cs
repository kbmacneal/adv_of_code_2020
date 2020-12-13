using adv_of_code_.Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day12 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        private enum Direction
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }

        private class ferry
        {
            public Point p { get; set; }
            public Direction direction { get; set; } = Direction.East;
        }

        public async Task Run()
        {
            string[] input = await File.ReadAllLinesAsync("inputs\\12.txt");

            Part1Answer = part1(input).ToString();

            Part2Answer = part2(input).ToString();
        }

        private int part1(string[] input)
        {
            ferry f = new ferry() { p = new Point(0, 0) };
            foreach (var instr in input)
            {
                switch (instr[0])
                {
                    case 'N':
                        f.p = f.p.moveUp(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'S':
                        f.p = f.p.moveDown(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'E':
                        f.p = f.p.moveRight(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'W':
                        f.p = f.p.moveLeft(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'L':
                        var degrees = Int32.Parse(instr.Substring(1));

                        var ticks = degrees / 90;
                        switch (ticks)
                        {
                            case 1:
                                switch (f.direction)
                                {
                                    case Direction.North:
                                        f.direction = Direction.West;
                                        break;

                                    case Direction.East:
                                        f.direction = Direction.North;
                                        break;

                                    case Direction.South:
                                        f.direction = Direction.East;
                                        break;

                                    case Direction.West:
                                        f.direction = Direction.South;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            case 2:
                                switch (f.direction)
                                {
                                    case Direction.North:
                                        f.direction = Direction.South;
                                        break;

                                    case Direction.East:
                                        f.direction = Direction.West;
                                        break;

                                    case Direction.South:
                                        f.direction = Direction.North;
                                        break;

                                    case Direction.West:
                                        f.direction = Direction.East;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            case 3:
                                switch (f.direction)
                                {
                                    case Direction.North:
                                        f.direction = Direction.East;
                                        break;

                                    case Direction.East:
                                        f.direction = Direction.South;
                                        break;

                                    case Direction.South:
                                        f.direction = Direction.West;
                                        break;

                                    case Direction.West:
                                        f.direction = Direction.North;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    case 'R':

                        degrees = Int32.Parse(instr.Substring(1));

                        ticks = degrees / 90;
                        switch (ticks)
                        {
                            case 1:
                                switch (f.direction)
                                {
                                    case Direction.North:
                                        f.direction = Direction.East;
                                        break;

                                    case Direction.East:
                                        f.direction = Direction.South;
                                        break;

                                    case Direction.South:
                                        f.direction = Direction.West;
                                        break;

                                    case Direction.West:
                                        f.direction = Direction.North;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            case 2:
                                switch (f.direction)
                                {
                                    case Direction.North:
                                        f.direction = Direction.South;
                                        break;

                                    case Direction.East:
                                        f.direction = Direction.West;
                                        break;

                                    case Direction.South:
                                        f.direction = Direction.North;
                                        break;

                                    case Direction.West:
                                        f.direction = Direction.East;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            case 3:
                                switch (f.direction)
                                {
                                    case Direction.North:
                                        f.direction = Direction.West;
                                        break;

                                    case Direction.East:
                                        f.direction = Direction.North;
                                        break;

                                    case Direction.South:
                                        f.direction = Direction.East;
                                        break;

                                    case Direction.West:
                                        f.direction = Direction.South;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    case 'F':
                        switch (f.direction)
                        {
                            case Direction.North:
                                f.p = f.p.moveUp(Int32.Parse(instr.Substring(1))).Last();
                                break;

                            case Direction.East:
                                f.p = f.p.moveRight(Int32.Parse(instr.Substring(1))).Last();
                                break;

                            case Direction.South:
                                f.p = f.p.moveDown(Int32.Parse(instr.Substring(1))).Last();
                                break;

                            case Direction.West:
                                f.p = f.p.moveLeft(Int32.Parse(instr.Substring(1))).Last();
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }
            return f.p.ManhDist();
        }

        private int part2(string[] input)
        {
            ferry f = new ferry() { p = new Point(0, 0) };
            Point waypoint = new Point(10, 1);

            foreach (var instr in input)
            {
                switch (instr[0])
                {
                    case 'N':
                        waypoint = waypoint.moveUp(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'S':
                        waypoint = waypoint.moveDown(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'E':
                        waypoint = waypoint.moveRight(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'W':
                        waypoint = waypoint.moveLeft(Int32.Parse(instr.Substring(1))).Last();
                        break;

                    case 'L':
                        var degrees = Int32.Parse(instr.Substring(1));

                        var ticks = degrees / 90;
                        for (int i = 0; i < ticks; i++)
                        {
                            //each step counter-clockwise chage X to -Y and Y to X
                            //e.g X = 4 Y = 10 ===> X= - 10 Y = 4
                            int oldWaypointX = waypoint.X;
                            int oldwaypointY = waypoint.Y;

                            waypoint = new Point(-oldwaypointY, oldWaypointX);
                        }
                        break;

                    case 'R':

                        degrees = Int32.Parse(instr.Substring(1));

                        ticks = degrees / 90;

                        for (int i = 0; i < ticks; i++)
                        {
                            //each step counter-clockwise chage X to Y and Y to -X
                            //e.g X = 4 Y = 10 ===> X = 10 Y = -4
                            int oldWaypointX = waypoint.X;
                            int oldwaypointY = waypoint.Y;

                            waypoint = new Point(oldwaypointY, -oldWaypointX);
                        }

                        break;

                    case 'F':
                        var times = Int32.Parse(instr.Substring(1));

                        var total_x = times * waypoint.X;
                        var total_y = times * waypoint.Y;

                        if (total_x > 0)
                        {
                            f.p = f.p.moveRight(total_x).Last();
                        }
                        else
                        {
                            f.p = f.p.moveLeft(-1 * total_x).Last();
                        }

                        if (total_y > 0)
                        {
                            f.p = f.p.moveUp(total_y).Last();
                        }
                        else
                        {
                            f.p = f.p.moveDown(-1 * total_y).Last();
                        }

                        break;

                    default:
                        break;
                }
            }

            return f.p.ManhDist();
        }
    }
}