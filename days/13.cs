using adv_of_code_2020.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day13 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        private class bus
        {
            public int id { get; set; }
            public int target { get; set; }
            public int nextMultipleAfter => this.id.NextMultipleAfter(target);

            public bus(int ID, int Target)
            {
                id = ID;
                target = Target;
            }
        }

        public async Task Run()
        {
            string[] input = await File.ReadAllLinesAsync("inputs\\13.txt");

            var target = Int32.Parse(input[0]);

            List<bus> busses = input[1].Split(',').Where(e => e != "x").Select(Int32.Parse).Select(e => new bus(e, target)).ToList();

            Part1Answer = (busses.Min(e => e.nextMultipleAfter).AbsDifference(target) * busses.OrderBy(e => e.nextMultipleAfter).First().id).ToString();

            string[] times = input[1].Split(",");

            var earliestTime = long.Parse(times[0]);
            var increment = earliestTime;
            for (int i = 1; i < times.Length; i++)
            {
                if (times[i] == "x") continue;

                var curTime = long.Parse(times[i]);

                var modValue = curTime - (i % curTime);

                while (earliestTime % curTime != modValue)
                {
                    earliestTime += increment;
                }

                increment = new long[] { increment, curTime }.GetLCM();
            }

            Part2Answer = earliestTime.ToString();
        }
    }
}