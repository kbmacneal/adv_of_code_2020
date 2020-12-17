using adv_of_code_2020.Classes;
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
            Dictionary<Point3D, Boolean> cubes = new Dictionary<Point3D, bool>();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    cubes.Add(new Point3D(x, y, 0), input[y][x] == '#');
                }
            }

            for (int i = 0; i < 6; i++)
            {
                cubes = run_sim(cubes);
            }

            Part1Answer = cubes.Count(e => e.Value).ToString();
        }

        public Dictionary<Point3D, Boolean> run_sim(Dictionary<Point3D, Boolean> cubes)
        {
            List<Point3D> cubes_to_add = new List<Point3D>();
            List<Point3D> fliptofalse = new List<Point3D>();
            List<Point3D> fliptotrue = new List<Point3D>();

            var state = cubes;

            foreach (var cube in cubes)
            {
                var surrounding = cube.Key.GetSurrounding();

                foreach (var surround in surrounding)
                {
                    if (!cubes.ContainsKey(surround)) cubes_to_add.Add(surround);
                }
            }

            foreach (var cube in cubes_to_add)
            {
                cubes[cube] = false;
            }

            foreach (var cube in cubes)
            {
                var surrounding = cube.Key.GetSurrounding().Where(e => e != cube.Key);

                var actual_surrounding = cubes.Where(e => surrounding.Contains(e.Key));

                if (cube.Value)
                {
                    if (new int[] { 2, 3 }.Contains(actual_surrounding.Count(e => e.Value)))
                    {
                        fliptofalse.Add(cube.Key);
                    }
                }
                else if (!cube.Value)
                {
                    if (actual_surrounding.Count(e => e.Value) == 3)
                    {
                        fliptotrue.Add(cube.Key);
                    }
                }
            }

            foreach (var cube in fliptofalse)
            {
                state[cube] = false;
            }

            foreach (var cube in fliptotrue)
            {
                state[cube] = true;
            }

            return state;
        }
    }
}