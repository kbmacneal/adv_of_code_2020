using System.Diagnostics;
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
    }
}