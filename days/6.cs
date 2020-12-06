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

            Part1Answer = groups.Select(e => e.Distinct().Count()).Sum().ToString();

            var group_expanded = input.Split("\n\n").Select(e => e.Split("\n"));

            Part2Answer = group_expanded.Select(x => x.Aggregate(new HashSet<char>(x.First()), (h, e) => { h.IntersectWith(e); return h; }).Count()).Sum().ToString();
        }
    }
}