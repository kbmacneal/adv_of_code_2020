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
            string[] input = (await File.ReadAllLinesAsync("inputs\\11.test.txt"));

            int changes = 1;
            //int sim_count = 1;

            while (changes != 0)
            {
                //sw.Start();
                (input, changes) = run_sim(input);
                //sw.Stop();
                //Console.WriteLine("Ran sim {0}, {1} changes in {2}ms", sim_count, changes, sw.ElapsedMilliseconds);
                //sw.Reset();
                //sim_count++;
            }

            Part1Answer = string.Join("\r\n", input).Count(e => e == '#').ToString();

            changes = 1;
            input = (await File.ReadAllLinesAsync("inputs\\11.test.txt"));
            while (changes != 0)
            {
                //sw.Start();
                (input, changes) = run_sim(input, true);
                //sw.Stop();
                //Console.WriteLine("Ran sim {0}, {1} changes in {2}ms", sim_count, changes, sw.ElapsedMilliseconds);
                //sw.Reset();
                //sim_count++;
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

                            if (surrounding.Count(e => e == '#') >= (part2 ? 5 : 4))
                            {
                                sb[x] = 'L';
                                raw_changes++;
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
                //up
                var n = 1;
                while (true)
                {
                    if (y - n < 0) break;
                    bool stopped = false;

                    switch (spaces[y - n][x])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y - n][x]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }
                //down
                n = 1;
                while (true)
                {
                    if (y + n > spaces.Length - 1) break;
                    bool stopped = false;

                    switch (spaces[y + n][x])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y + n][x]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }
                //left
                n = 1;
                while (true)
                {
                    if (x - n < 0) break;
                    bool stopped = false;

                    switch (spaces[y][x - n])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y][x - n]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }
                //right
                n = 1;
                while (true)
                {
                    if (x + n > spaces[y].Length - 1) break;
                    bool stopped = false;

                    switch (spaces[y][x + n])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y][x + n]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }

                //up-left
                n = 1;
                while (true)
                {
                    if (y - n < 0) break;
                    if (x - n < 0) break;
                    bool stopped = false;

                    switch (spaces[y - n][x - n])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y - n][x - n]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }
                //up-right
                n = 1;
                while (true)
                {
                    if (y - n < 0) break;
                    if (x + n > spaces[y].Length - 1) break;
                    bool stopped = false;

                    switch (spaces[y - n][x + n])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y - n][x + n]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }
                //down-left
                n = 1;
                while (true)
                {
                    if (y + n > spaces.Length - 1) break;
                    if (x - n < 0) break;
                    bool stopped = false;

                    switch (spaces[y + n][x - n])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y + n][x - n]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }
                //down-right
                n = 1;
                while (true)
                {
                    if (y + n > spaces.Length - 1) break;
                    if (x + n > spaces[y].Length - 1) break;
                    bool stopped = false;

                    switch (spaces[y + n][x - n])
                    {
                        case 'L':
                        case '#':
                            chars.Add(spaces[y + n][x + n]);
                            stopped = true;
                            break;

                        default:
                            break;
                    }
                    if (stopped) break;
                    n++;
                }
            }

            return chars.ToArray();
        }
    }
}