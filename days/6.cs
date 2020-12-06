using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day6 : IDay
    {
        public string Part1Answer { get; set; }
        public string Part2Answer { get; set; }


        public async Task Run()
        {
            string input = await File.ReadAllTextAsync("inputs\\6.txt");

            var groups = input.Split("\n\n").Select(e => e.Replace("\n", ""));

            int count = 0;

            foreach(var group in groups)
            {
                count += group.Distinct().Count();
            }

            Part1Answer = count.ToString();

            count = 0;

            var group_expanded = input.Split("\n\n").Select(e=>e.Split("\n"));

            foreach (var group in group_expanded)
            {
                count += group.Aggregate(new HashSet<char>(group.First()), (h, e) => { h.IntersectWith(e); return h; }).Count();
            }

            Part2Answer = count.ToString();
        }
    }
}