using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day7 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";

        public async Task Run()
        {
            string[] input = await File.ReadAllLinesAsync("inputs\\7.txt");

            Dictionary<string, string> RuleDict = input.Select(e => e.Substring(0, e.Length - 1).Replace(" bags", "").Replace(" bag", "")).ToDictionary(e => e.Split(" contain ")[0], f => f.Split(" contain ")[1]);

            Part1Answer = RuleDict.Select(e => e.Key).Where(e => HasShinyGold(e, RuleDict)).Count().ToString();

            Part2Answer = Part2(RuleDict).ToString();
        }

        private bool HasShinyGold(string str, Dictionary<string, string> RuleDict)
        {
            if (RuleDict[str].Contains("shiny gold"))
                return true;
            else
            {
                foreach (var value in RuleDict[str].Split(", "))
                {
                    if (value != "no other")
                    {
                        if (HasShinyGold(value.Substring(2), RuleDict))
                            return true;
                    }
                }
            }
            return false;
        }

        private int Part2(Dictionary<string, string> RuleDict, string bagColor = "shiny gold")
        {
            var totalBags = 0;
            foreach (var s in RuleDict[bagColor].Split(", "))
            {
                if (s != "no other")
                {
                    var num = Convert.ToInt32(s.Substring(0, 1));
                    totalBags += num + num * Part2(RuleDict, s.Substring(2));
                }
            }
            return totalBags;
        }
    }
}