using adv_of_code_2020.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day14 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [DebuggerDisplay("mask={string.Join(\"\",mask)}|addr={address}|value={value}")]
        private class instruction
        {
            public char[] mask { get; set; }
            public long address { get; set; }
            public long value { get; set; }
            public string[] memory { get; set; }

            public instruction(string Mask, int Address, int Value)
            {
                mask = Mask.ToCharArray().Reverse().ToArray();
                address = Address;
                value = Value;
                memory = new string[mask.Length];

                memory.Fill("0");
            }
        }

        private class computer
        {
            public long[] memory { get; set; }
        }

        public async Task Run()
        {
            string newline_separator = "\n";
            string[] input = (await File.ReadAllTextAsync("inputs\\14.txt")).Split("mask = ", StringSplitOptions.RemoveEmptyEntries);

            List<instruction> instructions = new List<instruction>();

            foreach (var instr in input)
            {
                var split = instr.Split(newline_separator, StringSplitOptions.RemoveEmptyEntries);

                var mask = split[0];

                for (int i = 1; i < split.Length; i++)
                {
                    var addr = Int32.Parse(split[i].Split(" = ")[0].Replace("mem[", "").Replace("]", ""));

                    var val = Int32.Parse(split[i].Split(" = ")[1]);

                    instructions.Add(new instruction(mask, addr, val));
                }
            }

            var c = new computer() { memory = new long[instructions.Max(e => e.address)] };

            foreach (var instr in instructions)
            {
                var result = new StringBuilder();

                char[] mask = instr.mask;

                char[] binary = string.Join("", Convert.ToString(instr.value, 2).ToCharArray().Reverse()).PadRight(36, '0').ToCharArray();

                for (int i = 0; i < binary.Length; i++)
                {
                    result.Append(mask[i] != 'X' ? mask[i].ToString() : binary[i].ToString());
                }

                c.memory[instr.address - 1] = result.ToString().FromBinary();
            }

            Part1Answer = c.memory.Sum().ToString();

            List<(long, long)> addresses = new List<(long, long)>();

            foreach (var instr in instructions)
            {
                var result = new StringBuilder();

                char[] mask = instr.mask;

                char[] binary = string.Join("", Convert.ToString(instr.address, 2).ToCharArray().Reverse()).PadRight(36, '0').ToCharArray();

                for (int i = 0; i < binary.Length; i++)
                {
                    result.Append(mask[i] == 'X' ? mask[i].ToString() : (mask[i] == '1' ? mask[i].ToString() : binary[i].ToString()));
                }

                var post_masked = MaskedToAddresses(result.ToString());

                IEnumerable<(long, long)> pairs = post_masked.Select(e => (e, instr.value));

                addresses = addresses.Concat(pairs).ToList();
            }

            Dictionary<long, long> memory = new Dictionary<long, long>();

            var addr_ints = addresses.Select(e => e.Item1).Distinct();

            addr_ints.ToList().ForEach(e => memory.Add(e, 0));

            foreach (var address in addresses)
            {
                memory[address.Item1] = address.Item2;
            }

            Part2Answer = memory.Select(e => e.Value).Sum().ToString();
        }

        private long[] MaskedToAddresses(string mask)
        {
            long[] rtn = new long[Convert.ToInt32(Math.Pow(2, mask.Count(e => e == 'X')))];

            var test = GenerateCombinations(mask);

            rtn = GenerateCombinations(mask).Select(e => e.FromBinary()).ToArray();

            return rtn;
        }

        private List<string> GenerateCombinations(string value)
        {
            if (!value.Any(c => c.Equals('X')))
            {
                return new List<string> { value };
            }
            else
            {
                var zeroMask = ReplaceFirstMatch(value, "X", "0");
                var oneMask = ReplaceFirstMatch(value, "X", "1");
                return GenerateCombinations(zeroMask).Concat(GenerateCombinations(oneMask)).ToList();
            }
        }

        private string ReplaceFirstMatch(string value, string mask, string replacement)
        {
            var firstMaskIndex = value.IndexOf(mask);
            if (firstMaskIndex < 0)
            {
                return value;
            }
            return value.Remove(firstMaskIndex, 1).Insert(firstMaskIndex, replacement);
        }
    }
}