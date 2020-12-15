using adv_of_code_2020.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day15 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        //[DebuggerDisplay("mask={string.Join(\"\",mask)}|addr={address}|value={value}")]

        public async Task Run()
        {
            int[] input = (await File.ReadAllTextAsync("inputs\\15.txt")).Split(",").Select(Int32.Parse).ToArray();

            var target = 30000000;

            int[] numbers = new int[target];

            numbers.Fill(-1);

            var i = 1;

            for (; i < input.Length + 1; i++)
            {
                numbers[input[i - 1]] = i;
            }

            var curNumber = 0;

            for (; i < 2020; i++)
            {
                var prevTime = numbers[curNumber];
                numbers[curNumber] = i;
                curNumber = prevTime != -1 ? i - prevTime : 0;
            }

            Part1Answer = curNumber.ToString();

            for (; i < target; i++)
            {
                var prevTime = numbers[curNumber];
                numbers[curNumber] = i;
                curNumber = prevTime != -1 ? i - prevTime : 0;
            }

            Part2Answer = curNumber.ToString();
        }
    }
}