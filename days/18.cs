using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day18 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        public async Task Run()
        {
            string[] input = await File.ReadAllLinesAsync("inputs\\18.txt");

            long part1 = 0;
            long part2 = 0;

            foreach (var line in input)
            {
                part1 += EvalatePartOne(line, false);
                part2 += EvalatePartOne(line, true);
            }

            Part1Answer = part1.ToString();
            Part2Answer = part2.ToString();
        }

        private static long EvalatePartOne(string line, bool isPartTwo)
        {
            while (line.Contains("("))
            {
                int level = 0;
                int rightParantesesIndex = 0;
                int leftParantesesIndex = 0;
                bool isLeftSet = false;
                for (int i = line.IndexOf('('); i < line.Length; i++)
                {
                    char currentChar = line[i];
                    if (currentChar == '(')
                    {
                        if (!isLeftSet)
                        {
                            leftParantesesIndex = i;
                            isLeftSet = true;
                        }

                        level++;
                    }
                    if (currentChar == ')')
                    {
                        level--;
                    }
                    if (level == 0)
                    {
                        rightParantesesIndex = i;
                        break;
                    }
                }

                int phaseStartIndex = leftParantesesIndex + 1;
                int phaseLenght = rightParantesesIndex - leftParantesesIndex - 1;
                long phaseResult = EvalatePartOne(line.Substring(phaseStartIndex, phaseLenght), isPartTwo);
                line = line.Substring(0, leftParantesesIndex) + phaseResult + line.Substring(rightParantesesIndex + 1);
            }

            if (isPartTwo)
            {
                string regex = @"(\d+) \+ (\d+)";
                Regex rgx = new Regex(regex);
                while (Regex.IsMatch(line, regex))
                {
                    Match m = Regex.Match(line, regex);
                    long t1 = long.Parse(m.Groups[1].Value);
                    long t2 = long.Parse(m.Groups[2].Value);
                    line = rgx.Replace(line, $"{t1 + t2}", 1);
                }
            }

            long counter = 0;
            bool isFirst = true;
            string[] operators = line.Trim().Split(" ");
            for (int i = 0; i < operators.Length; i++)
            {
                string current = operators[i];
                if (current == "+")
                {
                    counter += long.Parse(operators[i + 1]);
                }
                else if (current == "*")
                {
                    counter *= long.Parse(operators[i + 1]);
                }
                else
                {
                    if (isFirst)
                    {
                        counter = long.Parse(current);
                        isFirst = false;
                    }
                }
            }

            return counter;
        }
    }
}