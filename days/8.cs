using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day8 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";

        private class instruction
        {
            public string Command { get; set; }
            public int Value { get; set; }

            public instruction(string line)
            {
                Command = line.Split(" ")[0];
                Value = Int32.Parse(line.Split(" ")[1]);
            }

            public instruction(string command, int value)
            {
                Command = command;
                Value = value;
            }
        }

        public async Task Run()
        {
            List<instruction> input = (await File.ReadAllLinesAsync("inputs\\8.txt")).Select(x => new instruction(x)).ToList();

            Part1Answer = run_sim(input).ToString();

            for (int i = 0; i < input.Count(); i++)
            {
                instruction old = input[i];

                if (input[i].Command == "jmp") input[i] = new instruction("nop", input[i].Value);
                else if (input[i].Command == "nop") input[i] = new instruction("jmp", input[i].Value);
                else continue;

                var acc = run_sim(input, 2);
                if (acc != 0)
                {
                    Part2Answer = acc.ToString(); ;
                    return;
                }
                else
                {
                    input[i] = old;
                    continue;
                }
            }
        }

        private int run_sim(List<instruction> input, int part = 1)
        {
            List<int> indices = new List<int>();

            int acc = 0;

            for (int i = 0; i < input.Count(); i++)
            {
                indices.Add(i);

                if (indices.Count(e => e == i) > 1)
                {
                    if (part == 1)
                    {
                        return acc;
                    }
                    else
                    {
                        return 0;
                    }
                }

                switch (input[i].Command)
                {
                    case "acc":
                        acc += input[i].Value;
                        break;

                    case "jmp":
                        i += input[i].Value - 1;
                        break;

                    default:
                        break;
                }
            }
            if (part == 1)
            {
                return 0;
            }
            else
            {
                return acc;
            }
        }
    }
}