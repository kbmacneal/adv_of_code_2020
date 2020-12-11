using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day11 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        public async Task Run()
        {
            string[] input = (await File.ReadAllLinesAsync("inputs\\11.txt"));

            int changes = 1;

            while (changes != 0)
            {
                (input, changes) = run_sim(input);
            }

            Part1Answer = string.Join("\r\n", input).Count(e => e == '#').ToString();

            changes = 1;
            input = (await File.ReadAllLinesAsync("inputs\\11.txt"));
            while (changes != 0)
            {
                (input, changes) = run_sim(input, true);
            }

            Part2Answer = string.Join("\r\n", input).Count(e => e == '#').ToString();
        }

        private (string[], int) run_sim(string[] spaces, bool part2 = false)
        {
            int raw_changes = 0;

            var changes = new string[spaces.Length];

            for (int y = 0; y < spaces.Length; y++)
            {
                Span<char> sb = stackalloc char[spaces[y].Length];
                sb = spaces[y].ToCharArray();

                for (int x = 0; x < spaces[y].Length; x++)
                {
                    var surrounding = get_surrounding(spaces, x, y, part2);

                    switch (spaces[y][x])
                    {
                        case '#':

                            if (part2)
                            {
                                if (surrounding.Count(e => e == '#') >= 5)
                                {
                                    sb[x] = 'L';
                                    raw_changes++;
                                }
                            }
                            else
                            {
                                if (surrounding.Count(e => e == '#') >= 4)
                                {
                                    sb[x] = 'L';
                                    raw_changes++;
                                }
                            }

                            break;

                        case 'L':
                            if (surrounding.Count(e => e == '#') == 0)
                            {
                                sb[x] = '#';
                                raw_changes++;
                            }

                            break;

                        default:
                            break;
                    }
                }
                changes[y] = string.Join("", sb.ToArray());
            }

            return (changes, raw_changes);
        }

        private char[] get_surrounding(string[] spaces, int x, int y, bool ispart2 = false)
        {
            List<char> chars = new List<char>();

            if (!ispart2)
            {
                if (y != 0) chars.Add(spaces[y - 1][x]);
                if (x != 0) chars.Add(spaces[y][x - 1]);
                if (x + 1 < spaces[y].Length) chars.Add(spaces[y][x + 1]);
                if (y + 1 < spaces.Length) chars.Add(spaces[y + 1][x]);

                if (y != 0 && x != 0) chars.Add(spaces[y - 1][x - 1]);
                if (y + 1 < spaces.Length && x != 0) chars.Add(spaces[y + 1][x - 1]);
                if (y + 1 < spaces.Length && x + 1 < spaces[y].Length) chars.Add(spaces[y + 1][x + 1]);
                if (y != 0 && x + 1 < spaces[y].Length) chars.Add(spaces[y - 1][x + 1]);
            }
            else
            {
                int n = 1;
                char[] dirs = new char[] { '.', '.', '.', '.', '.', '.', '.', '.' };
                bool process = true;
                while (process)
                {
                    process = false;
                    //up
                    if (y - n >= 0 && dirs[0] == '.')
                    {
                        dirs[0] = spaces[y - n][x];
                        process = true;
                    }
                    //left
                    if (x - n >= 0 && dirs[1] == '.')
                    {
                        dirs[1] = spaces[y][x - n];
                        process = true;
                    }
                    //right
                    if (x + n < spaces[y].Length && dirs[2] == '.')
                    {
                        dirs[2] = spaces[y][x + n];
                        process = true;
                    }
                    //down
                    if (y + n < spaces.Length && dirs[3] == '.')
                    {
                        dirs[3] = spaces[y + n][x];
                        process = true;
                    }
                    //up left
                    if (y - n >= 0 && x - n >= 0 && dirs[4] == '.')
                    {
                        dirs[4] = spaces[y - n][x - n];
                        process = true;
                    }
                    //down left
                    if (y + n < spaces.Length && x - n >= 0 && dirs[5] == '.')
                    {
                        dirs[5] = spaces[y + n][x - n];
                        process = true;
                    }
                    //down right
                    if (y + n < spaces.Length && x + n < spaces[y].Length && dirs[6] == '.')
                    {
                        dirs[6] = spaces[y + n][x + n];
                        process = true;
                    }
                    //up right
                    if (y - n >= 0 && x + n < spaces[y].Length && dirs[7] == '.')
                    {
                        dirs[7] = spaces[y - n][x + n];
                        process = true;
                    }

                    n++;
                }
                chars = dirs.Where(e => e != '.').ToList();
            }

            return chars.ToArray();
        }
    }
}