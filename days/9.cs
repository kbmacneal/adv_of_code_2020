using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day9 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        private class combination
        {
            private long _a { get; set; }
            private long _b { get; set; }
            public long c { get; set; }

            public combination(long a, long b)
            {
                _a = a;
                _b = b;
                c = a + b;
            }
        }

        public async Task Run()
        {
            long[] input = (await File.ReadAllLinesAsync("inputs\\9.txt")).Select(e => Int64.Parse(e)).ToArray();

            var preamble_size = 25;

            long part2_temp = 0;

            for (int i = preamble_size; i < input.Length; i++)
            {
                long[] preamble = input.Slice(i - preamble_size, preamble_size).ToArray();

                List<combination> combinations = new List<combination>();

                for (int j = 0; j < preamble.Count(); j++)
                {
                    for (int k = 0; k < preamble.Count(); k++)
                    {
                        if (k != j)
                        {
                            combinations.Add(new combination(preamble[k], preamble[j]));
                        }
                    }
                }

                if (combinations.FirstOrDefault(e => e.c == input[i]) == null)
                {
                    Part1Answer = input[i].ToString();
                    part2_temp = input[i];
                    break;
                }
            }

            var reversed_input = input.Reverse().ToArray();

            for (int i = 0; i < reversed_input.Length; i++)
            {
                if (reversed_input[i] > part2_temp) continue;

                var start = reversed_input[i];

                long sum = start;
                long min = start;
                long max = start;

                for (int j = i + 1; j < reversed_input.Length; j++)
                {
                    sum += reversed_input[j];
                    min = Math.Min(min, reversed_input[j]);
                    max = Math.Max(max, reversed_input[j]);

                    if (sum == part2_temp)
                    {
                        Part2Answer = (min + max).ToString();
                        return;
                    }

                    if (sum > part2_temp) break;
                }
            }
        }
    }
}