using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day17 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        public async Task Run()
        {
            string[] input = await File.ReadAllLinesAsync("inputs\\17.txt");
            Dictionary<(int x, int y, int z), Boolean> state = new Dictionary<(int x, int y, int z), Boolean>();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    state.Add((x, y, 0), input[y][x] == '#');
                }
            }

            var count = new Dictionary<(int x, int y, int z), int>(1024);
            var dirs = Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .Select(z => (x, y, z))))
                .Where(d => d != (0, 0, 0))
                .ToArray();

            for (int i = 0; i < 6; i++)
            {
                count.Clear();

                foreach (var p in state.Keys)
                {
                    count[p] = 0;
                }

                foreach (var ((x, y, z), alive) in state.Where(kvp => kvp.Value))
                {
                    foreach (var (dx, dy, dz) in dirs)
                    {
                        count[(x + dx, y + dy, z + dz)] = count.GetValueOrDefault((x + dx, y + dy, z + dz)) + 1;
                    }
                }

                foreach (var (p, c) in count)
                {
                    state[p] = (state.GetValueOrDefault(p), c) switch
                    {
                        (true, >= 2 and <= 3) => true,
                        (false, 3) => true,
                        _ => false,
                    };
                }
            }

            Part1Answer = state.Where(kvp => kvp.Value).Count().ToString();

            DoPartB(await File.ReadAllBytesAsync("inputs\\17.txt"));
        }

        private void DoPartB(byte[] input)
        {
            var state = new Dictionary<(int x, int y, int z, int w), bool>(8192);
            int _x = 0, _y = 0;
            foreach (var c in input)
            {
                if (c == '\n') { _x = 0; _y++; }
                else state[(_x++, _y, 0, 0)] = c == '#';
            }

            var count = new Dictionary<(int x, int y, int z, int w), int>(8192);
            var dirs = Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .SelectMany(z => Enumerable.Range(-1, 3)
                            .Select(w => (x, y, z, w)))))
                .Where(d => d != (0, 0, 0, 0))
                .ToArray();
            for (int i = 0; i < 6; i++)
            {
                count.Clear();

                foreach (var p in state.Keys)
                {
                    count[p] = 0;
                }

                foreach (var ((x, y, z, w), alive) in state.Where(kvp => kvp.Value))
                {
                    foreach (var (dx, dy, dz, dw) in dirs)
                    {
                        count[(x + dx, y + dy, z + dz, w + dw)] = count.GetValueOrDefault((x + dx, y + dy, z + dz, w + dw)) + 1;
                    }
                }

                foreach (var (p, c) in count)
                {
                    state[p] = (state.GetValueOrDefault(p), c) switch
                    {
                        (true, >= 2 and <= 3) => true,
                        (false, 3) => true,
                        _ => false,
                    };
                }
            }

            Part2Answer = state.Where(kvp => kvp.Value).Count().ToString();
        }
    }
}