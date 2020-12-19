using adv_of_code_2020.Classes;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day19 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string input = await File.ReadAllTextAsync("inputs\\19.txt");

			var segments = new List<string[]> { input.Split("\n\n")[0].Split("\n"), input.Split("\n\n")[1].Split("\n") };

			var rulesBase = segments[0]
				.Select(x => x.Split(':', StringSplitOptions.TrimEntries))
				.ToDictionary(x => x[0], x => x[1]);
			var processed = new Dictionary<string, string>();

			string BuildRegex(string input)
			{
				if (processed.TryGetValue(input, out var s))
					return s;

				var orig = rulesBase[input];
				if (orig.StartsWith('\"'))
					return processed[input] = orig.Replace("\"", "");

				if (!orig.Contains("|"))
					return processed[input] = string.Join("", orig.Split().Select(BuildRegex));

				return processed[input] =
					"(" +
					string.Join("", orig.Split().Select(x => x == "|" ? x : BuildRegex(x))) +
					")";
			}

			var regex = new Regex("^" + BuildRegex("0") + "$");
			Part1Answer = segments[1].Count(regex.IsMatch).ToString();

			regex = new Regex($@"^({BuildRegex("42")})+(?<open>{BuildRegex("42")})+(?<close-open>{BuildRegex("31")})+(?(open)(?!))$");
			Part2Answer = segments[1].Count(regex.IsMatch).ToString();
        }

		
	}
}