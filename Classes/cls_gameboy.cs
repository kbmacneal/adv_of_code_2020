using System;
using System.Collections.Generic;
using System.Linq;

namespace adv_of_code_2020.Classes
{
    public enum Command
    {
        acc,
        jmp,
        nop
    }

    public class instruction
    {
        public Command Command { get; set; }
        public int Value { get; set; }

        public instruction(string line)
        {
            Command = Enum.Parse<Command>(line.Split(" ")[0]);
            Value = Int32.Parse(line.Split(" ")[1]);
        }

        public instruction(Command command, int value)
        {
            Command = command;
            Value = value;
        }
    }

    internal class cls_gameboy
    {
        private List<instruction> _input { get; set; }

        public cls_gameboy(List<instruction> instructions)
        {
            _input = instructions;
        }

        public int run_sim(Boolean infinite_loop_check = false)
        {
            if (_input == null) throw new ArgumentNullException("_input");

            List<int> indices = new List<int>();

            int acc = 0;

            for (int i = 0; i < _input.Count(); i++)
            {
                indices.Add(i);

                if (indices.Count(e => e == i) > 1)
                {
                    if (infinite_loop_check)
                    {
                        return acc;
                    }
                    else
                    {
                        return -1;
                    }
                }
                switch (_input[i].Command)
                {
                    case Command.acc:
                        acc += _input[i].Value;
                        break;

                    case Command.jmp:
                        i += _input[i].Value - 1;
                        break;

                    case Command.nop:
                        break;

                    default:
                        break;
                }
            }
            if (infinite_loop_check)
            {
                return -1;
            }
            else
            {
                return acc;
            }
        }
    }
}