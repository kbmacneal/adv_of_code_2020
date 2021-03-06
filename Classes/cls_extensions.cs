﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace adv_of_code_2020.Classes
{
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
        {
            if (values.Count() == 1)
                return new[] { values };
            return values.SelectMany(v => Permutations(values.Where(x => x.Equals(v) == false)), (v, p) => p.Prepend(v));
        }

        public static IEnumerable<T> Cartesian<T>(this IEnumerable<IEnumerable<T>> items)
        {
            var slots = items
               // initialize enumerators
               .Select(x => x.GetEnumerator())
               // get only those that could start in case there is an empty collection
               .Where(x => x.MoveNext())
               .ToArray();

            while (true)
            {
                // yield current values
                yield return (T)slots.Select(x => x.Current);

                // increase enumerators
                foreach (var slot in slots)
                {
                    // reset the slot if it couldn't move next
                    if (!slot.MoveNext())
                    {
                        // stop when the last enumerator resets
                        if (slot == slots.Last()) { yield break; }
                        slot.Reset();
                        slot.MoveNext();
                        // move to the next enumerator if this reseted
                        continue;
                    }
                    // we could increase the current enumerator without reset so stop here
                    break;
                }
            }
        }

        public static int DifferenceBetweenLastTwo(this List<int> l)
        {
            var temp = l.Skip(l.Count - 2).Take(2);

            return Math.Abs(temp.First() - temp.Last());
        }

        public static long FromBinary(this string i)
        {
            long result = 0;

            for (int n = 0; n < i.Length; n++)
            {
                result += i[n] == '1' ? Convert.ToInt64(Math.Pow(2, n)) : 0;
            }

            return result;
        }

        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }

        public static long Product(this IEnumerable<int> product)
        {
            long answer = 1;

            foreach (int item in product)
            {
                answer *= item;
            }
            return answer;
        }

        public static long Product(this IEnumerable<long> product)
        {
            long answer = 1;

            foreach (int item in product)
            {
                answer *= item;
            }
            return answer;
        }

        public static Int32 Abs(this Int32 num)
        {
            return Math.Abs(num);
        }

        public static void ReplaceKey<T, U>(this Dictionary<T, U> source, T key, T newKey)
        {
            if (!source.TryGetValue(key, out var value))
                throw new ArgumentException("Key does not exist", nameof(key));
            source.Remove(key);
            source.Add(newKey, value);
        }

        public static void ReplaceValue<T, U>(this Dictionary<T, U> source, T key, U newVal)
        {
            source.Remove(key);
            source.Add(key, newVal);
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueGenerator)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = valueGenerator(key);
                dictionary.Add(key, value);
            }

            return value;
        }

        public static IEnumerable<int> CumulativeSum(this IEnumerable<int> sequence)
        {
            int sum = 0;
            foreach (var item in sequence)
            {
                sum += item;
                yield return sum;
            }
        }

        public static int Floor(this Decimal d)
        {
            return (int)Math.Floor(d);
        }

        public static int Floor(this Double d)
        {
            return (int)Math.Floor(d);
        }

        public static int ManhDist(this Point p)
        {
            return p.X.Abs() + p.Y.Abs();
        }

        // 1 - string

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        // 2 - int

        public static T ToEnum<T>(this int value)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{type} is not an enum.");
            }

            if (!type.IsEnumDefined(value))
            {
                throw new ArgumentException($"{value} is not a valid ordinal of type {type}.");
            }

            return (T)Enum.ToObject(type, value);
        }

        public static IEnumerable<P> Around(this IEnumerable<P> points, P origin, Boolean diagonals = false)
        {
            return points.Where(e => origin.Around(diagonals).ToList().Contains(e)).ToList();
        }

        public static int NextMultipleAfter(this int n, int target)
        {
            return (n - (target % n)) + target;
        }

        public static int AbsDifference(this int n, int target)
        {
            return Math.Abs(target - n);
        }

        //from https://github.com/sjmulder/aoc/blob/master/2020/day01-cs-combine/Program.cs
        private static IEnumerable<IEnumerable<T>> Combine<T>(this T[] xs, int n)
        {
            if (n > xs.Length) yield break;
            var idxs = Enumerable.Range(0, n).ToArray();

            while (true)
            {
                yield return idxs.Select(i => xs[i]);
                int i = n - 1;
                while (++idxs[i] > xs.Length - n + i) if (--i < 0) yield break;
                while (++i < n) idxs[i] = idxs[i - 1] + 1;
            }
        }

        //from https://www.geeksforgeeks.org/lcm-of-given-array-elements/
        public static long GetLCM(this int[] element_array)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {
                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {
                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array[i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array[i] < 0)
                    {
                        element_array[i] = element_array[i] * (-1);
                    }
                    if (element_array[i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by devisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] = element_array[i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }

        public static long GetLCM(this long[] element_array)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {
                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {
                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array[i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array[i] < 0)
                    {
                        element_array[i] = element_array[i] * (-1);
                    }
                    if (element_array[i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by devisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] = element_array[i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }
    }
}