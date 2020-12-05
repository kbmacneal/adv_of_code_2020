using CommandLine;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
                            .OrderByDescending(e => e.GetType().Name);

                       Type t = types.First();

                       IDay day = (IDay)Activator.CreateInstance(t);

                       string result = day.Run().GetAwaiter().GetResult();

                       Console.WriteLine(result);
                   }

                   if (o.day > 0)
                   {
                       var type = typeof(IDay);
                       Type t = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                            .Where(e=>e.Name.Contains("Day" + o.day.ToString()))
                            .Where(e => e.Name != "IDay")
                            .OrderByDescending(e => e.GetType().Name)
                            .First();

                       IDay day = (IDay)Activator.CreateInstance(t);

                       string result = day.Run().GetAwaiter().GetResult();

                       Console.WriteLine(result);
                   }

                   if (!o.latest && o.day == 0)
                   {
                       Console.WriteLine("Input Day");

                       if (Int32.TryParse(Console.ReadLine(), out var day_num))
                       {
                           var cls = Assembly.GetEntryAssembly().GetType("adv_of_code_2020.Day" + day_num.ToString());

                           MethodInfo method = cls.GetMethod("Run");

                           Task<string> result = (Task<string>)method.Invoke(null, null);

                           result.Wait();

                           Console.WriteLine(result.Result);

                           result.Dispose();
                       }
                   }
               });
        }
    }
}