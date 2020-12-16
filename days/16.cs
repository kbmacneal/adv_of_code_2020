using adv_of_code_2020.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day16 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [DebuggerDisplay("{lower} - {upper}")]
        private class IntRange
        {
            public int upper { get; set; }
            public int lower { get; set; }

            public Boolean isvalid(int n) => n >= lower && n <= upper;

            public IntRange(string l, string u)
            {
                upper = Int32.Parse(u);
                lower = Int32.Parse(l);
            }
        }

        private class ticket
        {
            public int[] values { get; set; }
            public Boolean isValid { get; set; } = true;
        }

        private class ruleset
        {
            public string name { get; set; }
            public List<IntRange> intRanges { get; set; }

            public ruleset(string line)
            {
                name = line.Split(":")[0];
                intRanges = new List<IntRange>();

                foreach (var range in line.Split(":")[1].Split(" or "))
                {
                    var lower = range.Split("-")[0];
                    var upper = range.Split("-")[1];
                    intRanges.Add(new IntRange(lower, upper));
                }
            }
        }

        public async Task Run()
        {
            string separator = "\n\n";
            string[] input = (await File.ReadAllTextAsync("inputs\\16.txt")).Split(separator);

            List<ticket> tickets = new List<ticket>();
            List<ruleset> rulesets = new List<ruleset>();

            int error_rate = 0;

            ticket myticket = new ticket() { values = input[1].Split("\n")[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray() };

            foreach (var rule in input[0].Split("\n"))
            {
                rulesets.Add(new ruleset(rule));
            }

            foreach (var ticket in input.Last().Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                tickets.Add(new ticket() { values = ticket.Split(",").Select(Int32.Parse).ToArray() });
            }

            foreach (var ticket in tickets)
            {
                foreach (var n in ticket.values)
                {
                    if (!IsValidForAtLeastOneRuleset(n, rulesets))
                    {
                        error_rate += n;
                        ticket.isValid = false;
                    }
                }
            }

            Part1Answer = error_rate.ToString();

            List<int> departure_fields = new List<int>();

            Dictionary<int, List<string>> field_names = new Dictionary<int, List<string>>();

            for (int i = 0; i < myticket.values.Length; i++)
            {
                field_names.Add(i, rulesets.Select(e => e.name).ToList());
            }

            foreach (var ticket in tickets.Where(e => e.isValid))
            {
                for (int i = 0; i < ticket.values.Length; i++)
                {
                    var valid_ranges = RangesThatAreValid(ticket.values[i], rulesets);

                    foreach (var valid_range in rulesets.Select(e => e.name).Where(e => !valid_ranges.Contains(e)))
                    {
                        field_names[i].Remove(valid_range);
                    }
                }
            }

            field_names = clean_list(field_names);

            List<int> products = new List<int>();

            foreach (var index in field_names.Where(e => e.Value.First().StartsWith("departure") && e.Value.Count == 1).Select(e => e.Key))
            {
                products.Add(myticket.values[index]);
            }

            Dictionary<string, int> ticket_map = field_names.ToDictionary(e => e.Value.First(), e => e.Key);

            Part2Answer = products.Product().ToString();
        }

        public Dictionary<int, List<string>> clean_list(Dictionary<int, List<string>> field_names)
        {
            var changes = 1;

            while (changes > 0)
            {
                changes = 0;

                foreach (var field in field_names.Where(e => e.Value.Count() == 1))
                {
                    foreach (var other in field_names.Where(e => e.Key != field.Key))
                    {
                        if (other.Value.Count(e => e == field.Value.First()) > 0)
                        {
                            changes++;
                            other.Value.Remove(field.Value.First());
                        }
                    }
                }
            }

            return field_names;
        }

        private Boolean IsValidForAtLeastOneRuleset(int n, IEnumerable<ruleset> rulesets)
        {
            var valid_rulesets = 0;

            foreach (var range in rulesets.Select(e => e.intRanges))
            {
                foreach (var set in range)
                {
                    if (set.isvalid(n)) valid_rulesets++;
                }
            }

            return valid_rulesets > 0;
        }

        private IEnumerable<string> RangesThatAreValid(int n, IEnumerable<ruleset> rulesets)
        {
            var valid_rulesets = new List<string>();

            foreach (var range in rulesets)
            {
                foreach (var set in range.intRanges)
                {
                    if (set.isvalid(n))
                    {
                        valid_rulesets.Add(range.name);
                    }
                }
            }

            return valid_rulesets;
        }
    }
}