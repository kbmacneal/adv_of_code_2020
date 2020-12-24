using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day24 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string[] input = await IDay.ReadInputLinesAsync(24);

            var regex = new Regex(@"e|se|sw|w|nw|ne");
            List<List<string>> directions = new();

            foreach (var line in input)
            {
                directions.Add(regex.Matches(line).Select(e => e.Value).ToList());
            }

            decimal R = 1M;
            var x = 1.5M * R * (decimal)Math.Cos(Math.PI / 3);
            var y = 1.5M * R * (decimal)Math.Sin(Math.PI / 3);

            var deltas = new Dictionary<string, (decimal x, decimal y)>()
            {
                {"e", (1.5M * R, 0)},
                {"w", (-1.5M * R, 0)},
                {"ne", (x, y * -1)},
                {"nw", (x * -1, y * -1)},
                {"se", (x, y)},
                {"sw", (x * -1, y)},
            };

            var tiles = exec_instruction(directions, deltas);

            Part1Answer = tiles.Count(e => !e.Value).ToString();

            for (var i = 0; i < 100; i++)
            {
                var tileCounts = new Dictionary<(decimal x, decimal y), (bool colour, int blackNeighbours)>();

                foreach (var (pos, colour) in tiles)
                {
                    foreach (var (_, (dx, dy)) in deltas)
                    {
                        var neighbour = pos;
                        neighbour.x += dx;
                        neighbour.y += dy;

                        if (!tileCounts.ContainsKey(neighbour))
                        {
                            var newColour = true;

                            if (tiles.ContainsKey(neighbour))
                            {
                                newColour = tiles[neighbour];
                            }

                            tileCounts[neighbour] = (newColour, 0);
                        }

                        var val = tileCounts[neighbour];

                        if (!colour)
                        {
                            val.blackNeighbours++;
                        }

                        tileCounts[neighbour] = val;
                    }
                }

                var newTiles = new Dictionary<(decimal x, decimal y), bool>();
                foreach (var (pos, (colour, blackNeighbours)) in tileCounts)
                {
                    switch (colour)
                    {
                        case false when blackNeighbours == 0 || blackNeighbours > 2:
                            break;

                        case true when blackNeighbours == 2:
                        case false:
                            newTiles.Add(pos, false);
                            break;
                    }
                }

                tiles = newTiles;
            }

            Part2Answer = tiles.Values.Count(x => !x).ToString();
        }

        private Dictionary<(decimal x, decimal y), bool> exec_instruction(List<List<string>> directions, Dictionary<string, (decimal x, decimal y)> deltas)
        {
            var tiles = new Dictionary<(decimal x, decimal y), bool>();

            foreach (var instr in directions)
            {
                (decimal x, decimal y) pos = (0, 0);

                foreach (var (x, y) in instr.Select(e => deltas[e]))
                {
                    pos.x += x;
                    pos.y += y;
                }

                if (tiles.ContainsKey(pos))
                {
                    tiles[pos] = !tiles[pos];
                }
                else
                {
                    tiles[pos] = false;
                }
            }

            return tiles;
        }
    }
}