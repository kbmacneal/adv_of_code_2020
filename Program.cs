using CommandLine;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    internal class Program
    {
        public static IConfigurationRoot configuration;

        public class RuntimeOptions
        {
            [Option('d', "day", HelpText = "[Number of day] Run the solution for the given day.")]
            public int day { get; set; }

            [Option('l', "latest", HelpText = "Run the solution using the latest day.")]
            public bool latest { get; set; }

            [Option('a', "all", HelpText = "Run all days, in order")]
            public bool all { get; set; }
        }

        private static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel
                 .Debug().Enrich
                 .FromLogContext()
                 .CreateLogger();

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            try
            {
                // Start!
                MainAsync(args).Wait();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task MainAsync(string[] args)
        {
            Stopwatch sw = new Stopwatch();

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
                            .OrderBy(e => Int32.Parse(e.Name.Replace("Day", "")));

                       var day_num = Int32.Parse(types.Last().Name.Replace("Day", ""));

                       GetDayInput(day_num);

                       Type t = types.Last();

                       IDay day = (IDay)Activator.CreateInstance(t);

                       sw.Start();
                       day.Run().GetAwaiter().GetResult();
                       sw.Stop();
                       Log.Information("Part 1: " + day.Part1Answer);
                       Log.Information("Part 2: " + day.Part2Answer);
                       Log.Information(String.Format("Solution ran in {0}ms", sw.ElapsedMilliseconds));
                       sw.Reset();
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

                       var day_num = Int32.Parse(t.Name.Replace("Day", ""));

                       GetDayInput(day_num);

                       IDay day = (IDay)Activator.CreateInstance(t);

                       sw.Start();
                       day.Run().GetAwaiter().GetResult();
                       sw.Stop();
                       Log.Information("Part 1: " + day.Part1Answer);
                       Log.Information("Part 2: " + day.Part2Answer);
                       Log.Information(String.Format("Solution ran in {0}ms", sw.ElapsedMilliseconds));
                       sw.Reset();
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

                           GetDayInput(day_num);

                           IDay day = (IDay)Activator.CreateInstance(t);

                           sw.Start();
                           day.Run().GetAwaiter().GetResult();
                           sw.Stop();
                           Log.Information("Part 1: " + day.Part1Answer);
                           Log.Information("Part 2: " + day.Part2Answer);
                           Log.Information(String.Format("Solution ran in {0}ms", sw.ElapsedMilliseconds));
                           sw.Reset();
                       }
                   }

                   if (o.all)
                   {
                       var type = typeof(IDay);
                       var types = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(p => type.IsAssignableFrom(p))
                            .Where(e => e.Name != "IDay")
                            .OrderBy(e => Int32.Parse(e.Name.Replace("Day", "")));

                       foreach (Type t in types)
                       {
                           var day_num = Int32.Parse(t.Name.Replace("Day", ""));

                           GetDayInput(day_num);

                           IDay day = (IDay)Activator.CreateInstance(t);

                           sw.Start();
                           day.Run().GetAwaiter().GetResult();
                           sw.Stop();
                           Log.Information(t.Name);
                           Log.Information("Part 1: " + day.Part1Answer);
                           Log.Information("Part 2: " + day.Part2Answer);
                           Log.Information(String.Format("Solution ran in {0}ms", sw.ElapsedMilliseconds));
                           sw.Reset();
                       }
                   }
               });
        }

        private static void GetDayInput(int day_num)
        {
            if (!File.Exists(string.Format("inputs\\{0}.txt", day_num)))
            {
                var result = "https://adventofcode.com/2020/day/"
                .AppendPathSegment(day_num.ToString())
                .AppendPathSegment("input")
                .WithCookie("session", configuration.GetSection("session").Value)
                .GetStringAsync().GetAwaiter().GetResult();

                File.WriteAllTextAsync(string.Format("inputs\\{0}.txt", day_num), result).GetAwaiter().GetResult();
            }
        }
    }
}