using CommandLine;
using System;
using System.Linq;

namespace adv_of_code_2020
{
    internal class Program
    {
        public class RuntimeOptions
        {
            [Option('d', "day", HelpText = "[Number of day] Run the solution for the given day.")]
            public int day { get; set; }

            [Option('l', "latest", HelpText = "Run the solution using the latest day.")]
            public bool latest { get; set; }

            [Option('a', "all", HelpText = "Run all days, in order")]
            public bool all { get; set; }
        }

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<RuntimeOptions>(args)
                .WithParsed<RuntimeOptions>(o =>
               {
                   if (o.day > 0 && o.latest)
                   {
                       Console.WriteLine("Cannot use -d and -l together.");
                   }

                   if (o.latest)
                   {
                       var type = typeof(IDay);
                       var types = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(p => type.IsAssignableFrom(p))
                            .Where(e => e.Name != "IDay")
                            .OrderBy(e => e.GetType().Name);

                       Type t = types.Last();

                       IDay day = (IDay)Activator.CreateInstance(t);

                       day.Run().GetAwaiter().GetResult();

                       Console.WriteLine("Part 1: " + day.Part1Answer + Environment.NewLine + "Part 2: " + day.Part2Answer);
                   }

                   if (o.day > 0)
                   {
                       var type = typeof(IDay);
                       Type t = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                            .Where(e => e.Name.Contains("Day" + o.day.ToString()))
                            .Where(e => e.Name != "IDay")
                            .OrderByDescending(e => e.GetType().Name)
                            .First();

                       IDay day = (IDay)Activator.CreateInstance(t);

                       day.Run().GetAwaiter().GetResult();

                       Console.WriteLine("Part 1: " + day.Part1Answer + Environment.NewLine + "Part 2: " + day.Part2Answer);
                   }

                   if (!o.latest && o.day == 0 && !o.all)
                   {
                       Console.WriteLine("Input Day");

                       if (Int32.TryParse(Console.ReadLine(), out var day_num))
                       {
                           var type = typeof(IDay);
                           Type t = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                                .Where(e => e.Name.Contains("Day" + day_num.ToString()))
                                .Where(e => e.Name != "IDay")
                                .OrderByDescending(e => e.GetType().Name)
                                .First();

                           IDay day = (IDay)Activator.CreateInstance(t);

                           day.Run().GetAwaiter().GetResult();

                           Console.WriteLine("Part 1: " + day.Part1Answer + Environment.NewLine + "Part 2: " + day.Part2Answer);
                       }
                   }

                   if (o.all)
                   {
                       var type = typeof(IDay);
                       var types = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(p => type.IsAssignableFrom(p))
                            .Where(e => e.Name != "IDay")
                            .OrderBy(e => e.GetType().Name);

                       foreach (Type t in types)
                       {
                           IDay day = (IDay)Activator.CreateInstance(t);

                           day.Run().GetAwaiter().GetResult();

                           Console.WriteLine(t.Name);

                           Console.WriteLine("Part 1: " + day.Part1Answer + Environment.NewLine + "Part 2: " + day.Part2Answer);
                       }
                   }
               });
        }
    }
}