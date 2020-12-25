using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day25 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string[] input = await IDay.ReadInputLinesAsync(25);

            long cardPK = long.Parse(input[0]), doorPK = long.Parse(input[1]);
            long cardLoop = GetLoopVal(cardPK), doorLoop = GetLoopVal(doorPK);
            long cardEncryptionKey = GetKey(doorPK, cardLoop), doorEncryptionKey = GetKey(cardPK, doorLoop);

            Part1Answer = cardEncryptionKey.ToString();
        }

        private long GetLoopVal(long pk)
        {
            long value = 1, loopNo = 0;
            while (value != pk)
            {
                value = Calc(value, 7);
                loopNo++;
            }
            return loopNo;
        }

        private long GetKey(long pk, long loopNo)
        {
            long value = 1;
            for (long i = 0; i < loopNo; i++)
                value = Calc(value, pk);
            return value;
        }

        private long Calc(long value, long seed)
        {
            value *= seed;
            value %= 20201227;
            return value;
        }
    }
}