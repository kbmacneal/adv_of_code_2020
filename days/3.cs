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

            public void GetTreesForSlope(List<string> course)
            {
                int tree_count = 0;

                for (int y = 0, x = 0; y < course.Count; y += this.y, x += this.x)
                {
                    if (course[y][x % course[y].Length] == '#') tree_count++;
                }

                trees = tree_count;
            }
        }

        public static async Task<string> Run()
        {
            StringBuilder answer = new StringBuilder();

            List<string> course = File.ReadAllLines("inputs\\3.txt").ToList();

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

            slopes.ForEach(e => e.GetTreesForSlope(course));

            answer.AppendLine("Part 1: " + slopes.First(e=>e.x ==3).trees.ToString());

            answer.AppendLine("Part 2: " + slopes.Select(e => e.trees).Product());

            return answer.ToString();
        }
    }
}