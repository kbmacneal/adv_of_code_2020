using adv_of_code_2020.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day8 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        public async Task Run()
        {
            List<instruction> input = (await File.ReadAllLinesAsync("inputs\\8.txt")).Select(x => new instruction(x)).ToList();

            cls_gameboy gmb = new cls_gameboy(input);

            Part1Answer = gmb.run_sim(true).ToString();

            for (int i = 0; i < input.Count(); i++)
            {
                instruction old = input[i];

                if (input[i].Command == Command.jmp) input[i] = new instruction(Command.nop, input[i].Value);
                else if (input[i].Command == Command.nop) input[i] = new instruction(Command.jmp, input[i].Value);
                else continue;

                var acc = gmb.run_sim(false);
                if (acc != -1)
                {
                    Part2Answer = acc.ToString();
                    return;
                }
                else
                {
                    input[i] = old;
                    continue;
                }
            }
        }
    }
}