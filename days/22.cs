using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day22 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string[] input = (await IDay.ReadInputStringAsync(22)).Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            var decks = generate_decks(input);

            var result = play_nonrecursive(decks.player1, decks.player2);

            Part1Answer = result.player1.Count > 0 ? calculate_score(result.player1).ToString() : calculate_score(result.player2).ToString();

            decks = generate_decks(input);

            result = play_recursive(decks.player1, decks.player2);

            Part2Answer = result.player1.Count > 0 ? calculate_score(result.player1).ToString() : calculate_score(result.player2).ToString();
        }

        private (Queue<int> player1, Queue<int> player2) generate_decks(string[] input)
        {
            Queue<int> player1 = new Queue<int>(input[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(Int32.Parse));
            Queue<int> player2 = new Queue<int>(input[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(Int32.Parse));

            return (player1, player2);
        }

        private (Queue<int> player1, Queue<int> player2) play_nonrecursive(Queue<int> player1, Queue<int> player2)
        {
            while (player1.Count > 0 && player2.Count > 0)
            {
                var player1_round = player1.Dequeue();
                var player2_round = player2.Dequeue();

                if (player1_round > player2_round)
                {
                    player1.Enqueue(player1_round);
                    player1.Enqueue(player2_round);
                }
                else if (player2_round > player1_round)
                {
                    player2.Enqueue(player2_round);
                    player2.Enqueue(player1_round);
                }
                else
                {
                    throw new NotImplementedException("Ties are not implemented");
                }
            }

            return (player1, player2);
        }

        private (Queue<int> player1, Queue<int> player2) play_recursive(Queue<int> deck1, Queue<int> deck2)
        {
            var previousRoundsP1 = new HashSet<string>();
            var previousRoundsP2 = new HashSet<string>();

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                // Prevent an endless game
                var s1 = String.Join("|", deck1.Select(e => e.ToString()));
                var s2 = String.Join("|", deck2.Select(e => e.ToString()));

                if (previousRoundsP1.Contains(s1) || previousRoundsP2.Contains(s2))
                {
                    return (deck1, new Queue<int>());
                }
                else
                {
                    previousRoundsP1.Add(s1);
                    previousRoundsP2.Add(s2);
                }

                var card1 = deck1.Dequeue();
                var card2 = deck2.Dequeue();

                var winner = -1;

                if (deck1.Count >= card1 && deck2.Count >= card2)
                {
                    var sub1 = new Queue<int>(deck1.Take(card1));
                    var sub2 = new Queue<int>(deck2.Take(card2));

                    var result = play_recursive(sub1, sub2);

                    winner = result.player1.Count > result.player2.Count ? 1 : 2;
                }
                else
                {
                    winner = card1 > card2 ? 1 : 2;
                }

                if (winner == 1)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }

            return (deck1, deck2);
        }

        private int calculate_score(Queue<int> deck)
        {
            var deck_arr = deck.Reverse().ToArray();
            var score = 0;

            for (int i = 0; i < deck_arr.Length; i++)
            {
                score += (deck_arr[i] * (i + 1));
            }

            return score;
        }
    }
}