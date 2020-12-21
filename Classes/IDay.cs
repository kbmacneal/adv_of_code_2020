using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public interface IDay
    {
        public string Part1Answer { get; set; }
        public string Part2Answer { get; set; }
        public Stopwatch sw { get; set; }

        public async Task Run()
        {
        }

        public static async Task<string[]> ReadInputLinesAsync(int day_num)
        {
            return await File.ReadAllLinesAsync(string.Format("inputs\\{0}.txt", day_num.ToString()));
        }

        public static async Task<string> ReadInputStringAsync(int day_num)
        {
            return await File.ReadAllTextAsync(string.Format("inputs\\{0}.txt", day_num.ToString()));
        }
    }
}