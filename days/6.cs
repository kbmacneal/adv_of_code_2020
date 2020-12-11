using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day6 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        public async Task Run()
        {
            //if this doesnt work, change the line endings. visual studio sometimes changes the \n from the original file to an \r\n
            string line_endings = "\r\n";
            string input = await File.ReadAllTextAsync("inputs\\6.txt");

            var groups = input.Split(line_endings + line_endings).Select(e => e.Replace(line_endings, ""));

            Part1Answer = groups.Select(e => e.Distinct().Count()).Sum().ToString();

            var group_expanded = input.Split(line_endings + line_endings).Select(e => e.Split(line_endings));

            Part2Answer = group_expanded.Select(x => x.Aggregate(new HashSet<char>(x.First()), (h, e) => { h.IntersectWith(e); return h; }).Count()).Sum().ToString();
        }
    }
}