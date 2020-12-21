using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day21 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string[] input = (await IDay.ReadInputLinesAsync(21));

            //allergen, list of associated ingredients
            Dictionary<string, List<string>> allergens = new();
            Dictionary<string, (string allergen, int number)> ingredients = new Dictionary<string, (string, int)>();

            foreach (string food in input)
            {
                string foodIngredients = food.Split(" (contains ")[0];
                string foodAllergens = food.Replace(")", " ").Split(" (contains ")[1];
                foreach (string allergen in foodAllergens.Split(new char[] { ' ', ',', ')' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (allergens.ContainsKey(allergen))
                    {
                        allergens[allergen].RemoveAll(a => allergens[allergen].Except(foodIngredients.Split(' ')).Contains(a));
                    }
                    else
                    {
                        allergens.Add(allergen, foodIngredients.Split(' ').ToList());
                    }
                }

                foreach (string ingredient in foodIngredients.Split(' '))
                {
                    if (ingredients.ContainsKey(ingredient))
                    {
                        ingredients[ingredient] = ("", ingredients[ingredient].number + 1);
                    }
                    else
                    {
                        ingredients[ingredient] = ("", 1);
                    }
                }
            }

            while (allergens.Values.Where(e => e.Count > 1).Count() > 0)
            {
                foreach (List<string> ingredient in allergens.Values.Where(e => e.Count == 1).ToArray())
                {
                    foreach (string multiple in allergens.Keys.Where(a => allergens[a].Count > 1 && allergens[a].Contains(ingredient[0])))
                    {
                        allergens[multiple].Remove(ingredient[0]);
                    }
                }
            }

            foreach (string allergen in allergens.Keys)
            {
                string ingredient = allergens[allergen][0];
                ingredients[ingredient] = (allergen, ingredients[ingredient].number);
            }

            Part1Answer = ingredients.Where(i => i.Value.allergen == "").Sum(i => i.Value.number).ToString();

            string canonical = string.Empty;
            foreach (KeyValuePair<string, (string, int)> food in ingredients.Where(i => i.Value.allergen != "").OrderBy(i => i.Value.allergen))
                canonical += food.Key + ',';

            Part2Answer = canonical[0..^1].ToString();
        }
    }
}