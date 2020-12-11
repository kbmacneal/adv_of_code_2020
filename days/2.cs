using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day2 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        private class password
        {
            public int min { get; set; }
            public int max { get; set; }
            public char letter { get; set; }
            public string value { get; set; }
            public Boolean isValid => value.Where(e => e == letter).Count() <= max && value.Where(e => e == letter).Count() >= min;
            public Boolean isPart2Valid => value[min - 1] == letter ^ value[max - 1] == letter;

            public password(string line)
            {
                string[] split = line.Split(' ');
                min = Int32.Parse(split[0].Split('-')[0]);
                max = Int32.Parse(split[0].Split('-')[1]);
                letter = split[1][0];
                value = split[2];
            }
        }

        public async Task Run()
        {
            List<password> passwords = File.ReadAllLines("inputs\\2.txt").Select(e => new password(e)).ToList();

            Part1Answer = passwords.Where(e => e.isValid).Count().ToString();
            Part2Answer = passwords.Where(e => e.isPart2Valid).Count().ToString();
        }
    }
}