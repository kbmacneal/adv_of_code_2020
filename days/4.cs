using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day4 : IDay
    {
        private class passport
        {
            public string byr { get; set; } = "";
            public string iyr { get; set; } = "";
            public string eyr { get; set; } = "";
            public string hgt { get; set; } = "";
            public string hcl { get; set; } = "";
            public string ecl { get; set; } = "";
            public string pid { get; set; } = "";
            public string cid { get; set; } = "";
            public Boolean isValid => byr != "" && iyr != "" && eyr != "" && hgt != "" && hcl != "" && ecl != "" && pid != "";

            public Boolean isPart2Valid => validate();

            private string[] valid_eye_colors { get; } = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            private Boolean validate()
            {
                if (Int32.Parse(byr) < 1920 || Int32.Parse(byr) > 2002)
                    return false;

                if (Int32.Parse(iyr) < 2010 || Int32.Parse(iyr) > 2020)
                    return false;

                if (Int32.Parse(eyr) < 2020 || Int32.Parse(eyr) > 2030)
                    return false;

                if (hgt.EndsWith("cm"))
                {
                    if (Int32.Parse(hgt.Replace("cm", "")) < 150 || Int32.Parse(hgt.Replace("cm", "")) > 193)
                        return false;
                }
                else if (hgt.EndsWith("in"))
                {
                    if (Int32.Parse(hgt.Substring(0, hgt.Length - 2)) < 59 || Int32.Parse(hgt.Substring(0, hgt.Length - 2)) > 76)
                        return false;
                }
                else
                    return false;

                // ^anchor for start of string
                //# the literal #
                //( start of group
                //?: indicate a non - capturing group that doesn't generate backreferences
                //[0 - 9a - fA - F]   hexadecimal digit
                // { 3 } three times
                //) end of group
                //{ 1,2} repeat either once or twice
                //$ anchor for end of string
                Regex rex = new Regex("#[a-f0-9]{6}");
                if (!rex.IsMatch(hcl) || hcl.Length != 7)
                    return false;

                if (!valid_eye_colors.Contains(ecl) || ecl.Length != 3)
                    return false;

                Regex rex2 = new Regex("[0-9]{9}");
                if (!rex2.IsMatch(pid) || pid.Length != 9)
                    return false;

                return true;
            }

            public passport(string[] input)
            {
                foreach (string line in input)
                {
                    string[] split = line.Split(":");

                    switch (split[0])
                    {
                        case "byr":
                            byr = split[1];
                            break;

                        case "iyr":
                            iyr = split[1];
                            break;

                        case "eyr":
                            eyr = split[1];
                            break;

                        case "hgt":
                            hgt = split[1];
                            break;

                        case "hcl":
                            hcl = split[1];
                            break;

                        case "ecl":
                            ecl = split[1];
                            break;

                        case "pid":
                            pid = split[1];
                            break;

                        case "cid":
                            cid = split[1];
                            break;

                        case "\r":
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        public async Task<string> Run()
        {
            StringBuilder answer = new StringBuilder();

            string[] input = File.ReadAllText("inputs\\4.txt").Split("\n\n");

            List<passport> passports = input.ToList().Select(i => new passport(i.Replace(" ", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries))).ToList();

            answer.AppendLine("Part 1: " + passports.Where(e => e.isValid).Count());

            answer.AppendLine("Part 2: " + passports.Where(e => e.isValid).Where(e => e.isPart2Valid).Count());

            return answer.ToString();
        }
    }
}