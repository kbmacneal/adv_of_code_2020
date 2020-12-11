using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day5 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        private class instruction
        {
            public string full_instruction { get; set; }
            public string forward_aft_instr { get; set; }
            public string l_r_instr { get; set; }
            public int row_min { get; set; } = 0;
            public int row_max { get; set; } = 127;
            public int row_size => row_max - row_min + 1;
            public int col_min { get; set; } = 0;
            public int col_max { get; set; } = 8;
            public int col_size => col_max - col_min + 1;
            public int row { get; set; } = 0;
            public int col { get; set; } = 0;
            public int id => row * 8 + col;

            public instruction(string line)
            {
                full_instruction = line;
                forward_aft_instr = string.Join("", line.Take(7));
                l_r_instr = string.Join("", line.Skip(7).Take(3));
            }
        }

        public async Task Run()
        {
            string[] input = await File.ReadAllLinesAsync("inputs\\5.txt");

            List<instruction> instructions = input.ToList().Select(e => new instruction(e)).ToList();

            foreach (instruction instr in instructions)
            {
                foreach (char i in instr.forward_aft_instr.ToCharArray())
                {
                    switch (i)
                    {
                        case 'F':
                            instr.row_max -= instr.row_size / 2;
                            break;

                        case 'B':
                            instr.row_min += instr.row_size / 2;
                            break;

                        default:
                            break;
                    }
                }

                foreach (char i in instr.l_r_instr.ToCharArray())
                {
                    switch (i)
                    {
                        case 'L':
                            instr.col_max -= instr.col_size / 2;
                            break;

                        case 'R':
                            instr.col_min += instr.col_size / 2;
                            break;

                        default:
                            break;
                    }
                }
            }

            foreach (instruction instruction in instructions)
            {
                if (instruction.row_min == instruction.row_max)
                {
                    instruction.row = instruction.row_min;
                }
                else
                {
                    throw new InvalidDataException();
                }

                instruction.col = instruction.col_min;
            }

            Part1Answer = instructions.Max(e => e.id).ToString();

            var seat_numbers = Enumerable.Range(0, instructions.Max(e => e.id));

            var open_seats = seat_numbers.Where(e => !instructions.Select(f => f.id).Contains(e));

            Part2Answer = open_seats.Max().ToString();
        }
    }
}