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

            Dictionary<int, List<int>> runner = new Dictionary<int, List<int>>();

            var i = 0;

            var target = 30000000;

            for (; i < input.Length; i++)
            {
                runner.Add(input[i], new List<int>() { i });
            }

            var current_number = runner.Last().Key;

            for (; i < target; i++)
            {
                if (runner.ContainsKey(current_number))
                {
                    if (runner[current_number].Count() > 1)
                    {
                        var newnumber = runner[current_number].DifferenceBetweenLastTwo();

                        if (runner.ContainsKey(newnumber))
                        {
                            runner[newnumber].Add(i);
                            current_number = newnumber;
                        }
                        else
                        {
                            runner[newnumber] = new List<int>() { i };
                            current_number = newnumber;
                        }
                    }
                    else
                    {
                        runner[0].Add(i);
                        current_number = 0;
                    }
                }
                else
                {
                    runner[0].Add(i);
                    current_number = 0;
                }
            }

            Part1Answer = runner.First(e => e.Value.Contains(2020 - 1)).Key.ToString();

            Part2Answer = runner.First(e => e.Value.Contains(30000000 - 1)).Key.ToString();

            //the above takes about 18 seconds to run, below about a second

            //int[] numbers = new int[target];

            //numbers.Fill(-1);

            //var i = 1;

            //for (; i < input.Length + 1; i++)
            //{
            //    numbers[input[i - 1]] = i;
            //}

            //var curNumber = 0;

            //for (; i < 2020; i++)
            //{
            //    var prevTime = numbers[curNumber];
            //    numbers[curNumber] = i;
            //    curNumber = prevTime != -1 ? i - prevTime : 0;
            //}

            //Part1Answer = curNumber.ToString();

            //for (; i < target; i++)
            //{
            //    var prevTime = numbers[curNumber];
            //    numbers[curNumber] = i;
            //    curNumber = prevTime != -1 ? i - prevTime : 0;
            //}

            //Part2Answer = curNumber.ToString();
        }
    }
}