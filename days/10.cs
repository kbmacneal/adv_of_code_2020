using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day10 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";

        public async Task Run()
        {
            int[] input = (await File.ReadAllLinesAsync("inputs\\10.txt")).Select(e => Int32.Parse(e)).OrderBy(e => e).ToArray();

            int rating = input.Max() + 3;

            List<int> jolt_diffs = new List<int>();

            //the jump from 0 to 1
            jolt_diffs.Add(1);

            for (int i = 1; i < input.Length; i++)
            {
                jolt_diffs.Add(input[i] - input[i - 1]);
            }

            //the jump from the last one to the max joltage, always 3
            jolt_diffs.Add(3);

            var ones = jolt_diffs.Where(e => e == 1).Count();
            var threes = jolt_diffs.Where(e => e == 3).Count();

            Part1Answer = (ones * threes).ToString();

            List<int> p2_input = input.ToList();
            p2_input.Add(0);
            p2_input.Add(rating);
            p2_input.Sort();

            var graph = new Dictionary<int, List<int>>();

            //get the list of all numbers and associate them with all potential precursors from the list.
            for (var i = p2_input.Count - 1; i >= 0; i--)
            {
                graph.Add(p2_input[i], new List<int>());

                for (int j = 1; j < 4; j++)
                {
                    if (p2_input.Contains(p2_input[i] - j)) graph[p2_input[i]].Add(p2_input[i] - j);
                }
            }

            //the number we're looking at associated with the total number of precursors
            var processed = new Dictionary<int, long>();

            for (var i = 0; i < p2_input.Count; i++)
            {
                long count = i == 0 ? 1 : 0;

                //accumulate the precursor ints as we roll up the input, essentially keeping a running tally of the number of precursor combinations
                foreach (var g in graph[p2_input[i]])
                {
                    count += processed[g];
                }

                processed.Add(p2_input[i], count);
            }

            Part2Answer = processed[p2_input.Last()].ToString();

            return;
        }
    }
}