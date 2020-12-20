using adv_of_code_2020.Classes;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day20 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [DebuggerDisplay("{id}")]
        private class piece
        {
            public int id { get; set; }
            public string[] sides { get; set; } = new string[4];
            public string[] side_combinations { get; set; } = new string[8];
            public List<piece> matches { get; set; } = new List<piece>();
            public string[] raw { get; set; }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string[] input = (await File.ReadAllTextAsync("inputs\\20.txt")).Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            List<piece> pieces = new List<piece>();

            foreach (var piece in input)
            {
                var split = piece.Split("\n");
                var id = Int32.Parse(split[0].Split(" ")[1].Replace(":", ""));

                var sides = new string[4];

                sides[0] = split[1];
                sides[3] = split.Last();
                sides[1] = String.Join("", split.Skip(1).Select(e => e.First()));
                sides[2] = String.Join("", split.Skip(1).Select(e => e.Last()));

                var sides_flipped = new string[8];
                sides.CopyTo(sides_flipped, 0);

                for (int i = 0; i < 4; i++)
                {
                    sides_flipped[i + 4] = String.Join("", sides[i].Reverse());
                }

                pieces.Add(new Day20.piece() { id = id, raw = split, sides = sides, side_combinations = sides_flipped });
            }

            foreach (var piece in pieces)
            {
                foreach (var side in piece.side_combinations)
                {
                    foreach (var other in pieces.Where(e=>e.id != piece.id))
                    {
                        if (other.side_combinations.Contains(side)) piece.matches.Add(other);
                    }
                }
            }

            Part1Answer = pieces.Where(e => e.matches.Count() == pieces.Select(f => f.matches.Count).Min()).Select(e=>e.id).Product().ToString();
        }
    }
}