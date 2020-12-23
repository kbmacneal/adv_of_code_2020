using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day23 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string[] input = (await IDay.ReadInputLinesAsync(23));

            LinkedList<int> cups = new LinkedList<int>();
            LinkedList<int> cups2 = new LinkedList<int>();

            Dictionary<int, LinkedListNode<int>> nodeLoc = new Dictionary<int, LinkedListNode<int>>();
            Dictionary<int, LinkedListNode<int>> nodeLoc2 = new Dictionary<int, LinkedListNode<int>>();

            Dictionary<int, bool> isIn = new Dictionary<int, bool>();
            Dictionary<int, bool> isIn2 = new Dictionary<int, bool>();

            for (int i = 0; i < input[0].Length; i++)
            {
                int num = int.Parse(input[0][i].ToString());
                nodeLoc[num] = cups.AddLast(num);
                nodeLoc2[num] = cups2.AddLast(num);
                isIn[num] = true;
                isIn2[num] = true;
            }

            int moreCup = cups.Max() + 1;

            while (moreCup <= 1000000)
            {
                nodeLoc2[moreCup] = cups2.AddLast(moreCup);
                isIn2[moreCup] = true;
                moreCup++;
            }

            ShuffleCups(cups, nodeLoc, isIn, 100);

            LinkedListNode<int> index = nodeLoc[1];
            for (int i = 1; i < cups.Count; i++)
            {
                index = index.Next ?? cups.First;
                Part1Answer += index.Value;
            }

            ShuffleCups(cups2, nodeLoc2, isIn2, 10000000);

            index = nodeLoc2[1];

            long product = 1;

            for (int i = 0; i < 2; i++)
            {
                index = index.Next ?? cups.First;
                Console.WriteLine(index.Value);
                product *= index.Value;
            }

            Part2Answer = product.ToString();
        }

        private static void ShuffleCups(LinkedList<int> cups, Dictionary<int, LinkedListNode<int>> nodeLoc, Dictionary<int, bool> isIn, int moves)
        {
            LinkedListNode<int> index = cups.First;
            Stack<int> removed = new Stack<int>();
            int isInMax = cups.Max();
            int currentCup = index.Value;

            for (int move = 0; move < moves; move++)
            {
                currentCup = index.Value;

                for (int i = 0; i < 3; i++)
                {
                    int num = (index.Next ?? cups.First).Value;
                    removed.Push(num);
                    isIn.Remove(num);
                    cups.Remove(index.Next ?? cups.First);
                }

                int target = currentCup - 1;

                while (removed.Contains(target) || !isIn.TryGetValue(target, out bool res))
                {
                    target--;
                    if (target < 1)
                    {
                        target = isInMax;
                    }
                }

                LinkedListNode<int> index2 = nodeLoc[target];

                for (int i = 0; i < 3; i++)
                {
                    int num = removed.Pop();
                    nodeLoc[num] = cups.AddAfter(index2, num);
                    isIn[num] = true;
                }

                index = index.Next ?? cups.First;
            }
        }
    }
}