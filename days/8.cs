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

        public async Task Run()
        {
            List<KeyValuePair<string, int>> input = (await File.ReadAllLinesAsync("inputs\\8.txt")).Select(x => new KeyValuePair<string, int>(x.Split(" ")[0], Int32.Parse(x.Split(" ")[1]))).ToList();

            Part1Answer = run_sim(input).ToString();

            for (int i = 0; i < input.Count(); i++)
            {
                KeyValuePair<string, int> old = input[i];

                if (input[i].Key == "jmp") input[i] = new KeyValuePair<string, int>("nop", input[i].Value);
                else if (input[i].Key == "nop") input[i] = new KeyValuePair<string, int>("jmp", input[i].Value);
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

        private int run_sim(List<KeyValuePair<string, int>> input, int part = 1)
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

                switch (input[i].Key)
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