using adv_of_code_.Classes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day3
    {
        private class slope
        {
            public int x { get; set; }
            public int y { get; set; }
            public long trees { get; set; } = 0;
        }

        public static async Task<string> Run()
        {
            StringBuilder answer = new StringBuilder();

            List<string> course = File.ReadAllLines("inputs\\3.txt").ToList();

            var pos_x = -3;

            var tree_count = 0;

            for (int y = 0; y < course.Count; y++)
            {
                if (y != course.Count - 1)
                {
                    pos_x = (pos_x + 3) % course[y + 1].Length;
                }

                if (course[y][pos_x] == '#') tree_count++;
            }

            answer.AppendLine("Part 1: " + tree_count.ToString());

            List<slope> slopes = new List<slope>()
            {
                new slope
                {
                    x = 1,y=1
                },
                new slope
                {
                    x = 5,y=1
                },
                new slope
                {
                    x = 7,y=1
                },
                new slope
                {
                    x = 1,y=2
                },
                new slope
                {
                    x = 3,y=1
                }
            };

            foreach (slope slope in slopes)
            {
                pos_x = -1 * slope.x;
                tree_count = 0;

                slope.trees = GetTreesForSlope(slope.x, slope.y, course);
            }

            answer.AppendLine("Part 2: " + slopes.Select(e => e.trees).Product());

            return answer.ToString();
        }

        private static long GetTreesForSlope(int dx, int dy, List<string> course)
        {
            int tree_count = 0;

            for (int y = 0, x = 0; y < course.Count; y += dy, x += dx)
            {
                if (course[y][x % course[y].Length] == '#') tree_count++;
            }

            return tree_count;
        }


    }
}