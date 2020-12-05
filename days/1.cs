using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day1 : IDay
    {
        public async Task<string> Run()
        {
            int[] numbers = File.ReadAllLines("inputs\\1.txt").Select(e => Int32.Parse(e)).ToArray();

            StringBuilder answers = new StringBuilder();

            bool part1 = false;

            bool part2 = false;

            //part 1

            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = 1; j < numbers.Length; j++)
                {
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        answers.AppendLine("Part 1: " + numbers[i] * numbers[j]);
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
                            answers.AppendLine("Part 2: " + numbers[i] * numbers[j] * numbers[k]);
                            part2 = true;
                            break;
                        }
                    }
                    if (part2) break;
                }
                if (part2) break;
            }

            return answers.ToString();
        }
    }
}