using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day2
    {
        private class password
        {
            public int min { get; set; }
            public int max { get; set; }
            public char letter { get; set; }
            public string value { get; set; }
            public Boolean isValid { get; set; } = false;
            public Boolean isPart2Valid { get; set; } = false;
        }

        public static async Task<string> Run()
        {
            StringBuilder answer = new StringBuilder();

            List<password> passwords = new List<password>();

            System.IO.File.ReadAllLines("inputs\\2.txt").ToList().ForEach(e => passwords.Add(parse_line(e)));

            passwords.ForEach(e => e.isValid = (check_for_valid(e)));

            answer.AppendLine("Part 1: " + passwords.Where(e => e.isValid).Count());

            passwords.ForEach(e => e.isPart2Valid = (check_for_part2(e)));

            answer.AppendLine("Part 2: " + passwords.Where(e => e.isPart2Valid).Count());

            return answer.ToString();
        }

        private static password parse_line(string line)
        {
            string[] split = line.Split(' ');

            password p = new password()
            {
                min = Int32.Parse(split[0].Split('-')[0]),
                max = Int32.Parse(split[0].Split('-')[1]),
                letter = split[1][0],
                value = split[2]
            };

            return p;
        }

        private static Boolean check_for_valid(password p)
        {
            int[] password_array = p.value.ToCharArray().Select(e => (int)e).ToArray();

            int letter_int = (int)p.letter;

            return password_array.Where(e => e == letter_int).Count() <= p.max && password_array.Where(e => e == letter_int).Count() >= p.min;
        }

        private static Boolean check_for_part2(password p)
        {
            int[] password_array = p.value.ToCharArray().Select(e => (int)e).ToArray();

            int letter_int = (int)p.letter;

            return password_array[p.min - 1] == letter_int ^ password_array[p.max - 1] == letter_int;
        }
    }
}