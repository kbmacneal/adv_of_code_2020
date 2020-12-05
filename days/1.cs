using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day1 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";

        public async Task Run()
        {
            int[] numbers = File.ReadAllLines("inputs\\1.txt").Select(e => Int32.Parse(e)).ToArray();

            bool part1 = false;

            bool part2 = false;

            //part 1

            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = 1; j < numbers.Length; j++)
                {
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        Part1Answer = (numbers[i] * numbers[j]).ToString();
                        part1 = true;
                        break;
                    }
                }
                if (part1) break;
            }

            //part 2

            for (int i = 0; i < numbers.Length - 2; i++)
            {
                for (int j = 1; j < numbers.Length - 1; j++)
                {
                    for (int k = 2; k < numbers.Length; k++)
                    {
                        if (numbers[i] + numbers[j] + numbers[k] == 2020)
                        {
                            Part2Answer = (numbers[i] * numbers[j] * numbers[k]).ToString();
                            part2 = true;
                            break;
                        }
                    }
                    if (part2) break;
                }
                if (part2) break;
            }
        }
    }
}