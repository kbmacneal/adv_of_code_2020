using adv_of_code_2020.Classes;
using MoreLinq;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace adv_of_code_2020
{
    public class Day20 : IDay
    {
        public string Part1Answer { get; set; } = "";
        public string Part2Answer { get; set; } = "";
        public Stopwatch sw { get; set; } = new Stopwatch();

        [DebuggerDisplay("{id}")]
        private class piece
        {
            public int id { get; set; }
            public string[] sides { get; set; } = new string[4];
            public string[] side_combinations { get; set; } = new string[8];
            public List<piece> matches { get; set; } = new List<piece>();
            public string[] raw { get; set; }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public async Task Run()
        {
            string[] input = (await IDay.ReadInputStringAsync(20)).Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            List<piece> pieces = parse(input);

            foreach (var piece in pieces)
            {
                foreach (var side in piece.side_combinations)
                {
                    foreach (var other in pieces.Where(e => e.id != piece.id))
                    {
                        if (other.side_combinations.Contains(side)) piece.matches.Add(other);
                    }
                }
            }

            Part1Answer = pieces.Where(e => e.matches.Count() == pieces.Select(f => f.matches.Count).Min()).Select(e => e.id).Product().ToString();

            Part2Answer = (await Part2()).ToString();
        }

        private List<piece> parse(string[] input)
        {
            List<piece> pieces = new List<piece>();

            foreach (var piece in input)
            {
                var split = piece.Split("\n");
                var id = Int32.Parse(split[0].Split(" ")[1].Replace(":", ""));

                var sides = new string[4];

                sides[0] = split[1];
                sides[3] = split.Last();
                sides[1] = String.Join("", split.Skip(1).Select(e => e.First()));
                sides[2] = String.Join("", split.Skip(1).Select(e => e.Last()));

                var sides_flipped = new string[8];
                sides.CopyTo(sides_flipped, 0);

                for (int i = 0; i < 4; i++)
                {
                    sides_flipped[i + 4] = String.Join("", sides[i].Reverse());
                }

                pieces.Add(new piece() { id = id, raw = split, sides = sides, side_combinations = sides_flipped });
            }

            return pieces;
        }

        public async Task<long> Part2(string input = null)
        {
            var lines = await File.ReadAllLinesAsync("inputs\\20.txt");
            var blocks = SplitExtension.Split(lines, "");
            var tiles = blocks.Select(b => new Tile(b.ToList())).ToList();

            var neighbours = tiles.Select(t => t.PotentialNeighbours(tiles));
            var edges = tiles.Where(t => t.PotentialNeighbours(tiles).ToList().Count(x => x == 0) >= 1);

            // edges.Count().Should().Be(44);

            var megaTile = new MegaTile(tiles);

            return IterateMegaTilePart2(megaTile);
        }

        private static long IterateMegaTilePart2(MegaTile megaTile)
        {
            var operationOrder = new[]
            {
                (Rotation.None, Flip2D.None),

                (Rotation.Right90, Flip2D.Vertical),

                (Rotation.Left90, Flip2D.None),
                (Rotation.Right180, Flip2D.None),
                (Rotation.Right90, Flip2D.None),
                (Rotation.None, Flip2D.Horizontal),
                (Rotation.Left90, Flip2D.Horizontal),
                (Rotation.Right180, Flip2D.Horizontal),
                (Rotation.Right90, Flip2D.Horizontal),
                (Rotation.None, Flip2D.Vertical),
                (Rotation.Left90, Flip2D.Vertical),
                (Rotation.Right180, Flip2D.Vertical),
            };

            foreach (var (rotate, flip) in operationOrder)
            {
                var toTest = megaTile.Rotate(rotate).Flip(flip);
                var monsters = toTest.NumberOfMonsters();
                if (monsters != 0)
                {
                    return toTest.CountOfNonMonsterTiles();
                }
            }

            return -1;
        }

        private class MegaTile
        {
            public List<List<bool>> Spaces;

            public int Size => Spaces.Count;

            public MegaTile(List<Tile> tiles)
            {
                // Assume all tiles same size! Known for this puzzle.
                var tileSize = tiles.First().Size;

                var sizeDouble = Math.Sqrt(tiles.Count);
                if (sizeDouble % 1 != 0)
                {
                    throw new Exception("Expecting a square number of pieces for this puzzle!");
                }

                var size = (int)sizeDouble;

                var grid = Enumerable.Range(0, size)
                    .Select(_ => Enumerable.Range(0, size).Select(_ => null as Tile).ToList()).ToList();

                // tiles.Count().Should().Be(144);
                var corners = tiles.Where(t => t.PotentialNeighbours(tiles).ToList().Count(x => x == 0) == 2).ToList();
                var edges = tiles.Where(t => t.PotentialNeighbours(tiles).ToList().Count(x => x == 0) == 1).ToList();

                var placedTileIds = new HashSet<long>();

                Tile GetTile(int x, int y)
                {
                    if (x < 0 || x >= size || y < 0 || y >= size)
                    {
                        return null;
                    }

                    return grid[y][x];
                }

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        Tile toPlace = null;

                        // first corner
                        if ((x, y) == (0, 0))
                        {
                            toPlace = corners.First();
                        }
                        else
                        {
                            var leftNeighbour = GetTile(x - 1, y);
                            var upNeighbour = GetTile(x, y - 1);
                            var isTop = y == 0;
                            var isBottom = y == size - 1;
                            var isLeft = x == 0;
                            var isRight = x == size - 1;

                            var isVerticalEdge = isLeft || isRight;
                            var isHorizontalEdge = isTop || isBottom;

                            var isEdge = isVerticalEdge || isHorizontalEdge;
                            var isCorner = isVerticalEdge && isHorizontalEdge;

                            if (leftNeighbour == null && upNeighbour == null)
                            {
                                throw new Exception(
                                    $"Found un-placeable position (no-placed-neighbours)! This should only be the case for (0,0). Actually: ({x}, {y})");
                            }

                            toPlace = tiles.First(t => !placedTileIds.Contains(t.Id)
                                                       && (leftNeighbour == null || t.IsAdjacentTo(tiles, leftNeighbour.Id))
                                                       && (upNeighbour == null || t.IsAdjacentTo(tiles, upNeighbour.Id))
                                                       && (!isEdge || t.NumberOfNeighbours(tiles) <= 3)
                                                       && (!isCorner || t.NumberOfNeighbours(tiles) == 2)
                            );
                        }

                        if (toPlace == null)
                        {
                            throw new Exception($"Could not find a piece to place! ({x}, {y})");
                        }

                        grid[y][x] = toPlace;
                        placedTileIds.Add(toPlace.Id);
                    }
                }

                Console.WriteLine("Placed all tiles.");
                // Placed all tiles, need to orient!

                bool IsOrientationCorrect(int x, int y, Tile tile = null)
                {
                    // Can only look left and up, as when building grid.

                    tile ??= GetTile(x, y);

                    var leftNeighbour = GetTile(x - 1, y);
                    var upNeighbour = GetTile(x, y - 1);

                    var leftNeighbourCorrect = leftNeighbour != null && tile.MatchesInSpecifiedOrientation(leftNeighbour, Ordinal.Left, Ordinal.Right);
                    var upNeighbourCorrect = upNeighbour != null && tile.MatchesInSpecifiedOrientation(upNeighbour, Ordinal.Up, Ordinal.Down);

                    var leftEdgeCorrect = leftNeighbour == null && tile.NumberOfNeighbours(tiles, Ordinal.Left) == 0;
                    var upEdgeCorrect = upNeighbour == null && tile.NumberOfNeighbours(tiles, Ordinal.Up) == 0;

                    return (leftNeighbourCorrect || leftEdgeCorrect) && (upNeighbourCorrect || upEdgeCorrect);
                }

                var operationOrder = new[]
                {
                (Rotation.Left90, Flip2D.None),
                (Rotation.None, Flip2D.Vertical),
                (Rotation.Right180, Flip2D.None),
                (Rotation.Right90, Flip2D.None),
                (Rotation.None, Flip2D.Horizontal),
                (Rotation.Left90, Flip2D.Horizontal),
                (Rotation.Right180, Flip2D.Horizontal),
                (Rotation.Right90, Flip2D.Horizontal),
                (Rotation.None, Flip2D.Vertical),
                (Rotation.Left90, Flip2D.Vertical),
                (Rotation.Right180, Flip2D.Vertical),
                (Rotation.Right90, Flip2D.Vertical),
            };

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        var tile = GetTile(x, y);
                        var toPlace = tile.Rotate(Rotation.None);
                        var clone = tile.Rotate(Rotation.None);
                        var operationId = 0;

                        while (!IsOrientationCorrect(x, y, toPlace))
                        {
                            if (operationId >= operationOrder.Length)
                            {
                                throw new Exception($"Ran out of possibilities to tweak orientation! ({x}, {y})");
                            }

                            var (rotate, flip) = operationOrder[operationId];
                            toPlace = clone.Rotate(rotate).Flip(flip);
                            operationId++;
                        }

                        grid[y][x] = toPlace;
                    }
                }

                Console.WriteLine("Oriented pieces.");

                var gridSize = (tileSize - 2) * size;
                var emptyGrid = Enumerable.Range(0, gridSize)
                    .Select(_ => Enumerable.Range(0, gridSize).Select(_ => false).ToList()).ToList();

                var trimmedGrid = grid.Select(row => row.Select(tile => tile.TrimEdges()).ToList()).ToList();
                var trimmedTileSize = trimmedGrid.First().First().Size;

                for (int outerX = 0; outerX < size; outerX++)
                {
                    for (int outerY = 0; outerY < size; outerY++)
                    {
                        var tile = trimmedGrid[outerY][outerX];

                        for (int innerX = 0; innerX < trimmedTileSize; innerX++)
                        {
                            for (int innerY = 0; innerY < trimmedTileSize; innerY++)
                            {
                                var cell = tile.GetCell(innerX, innerY);

                                var newX = innerX + (outerX * trimmedTileSize);
                                var newY = innerY + (outerY * trimmedTileSize);

                                emptyGrid[newY][newX] = cell;
                            }
                        }
                    }
                }

                Console.WriteLine("Stripped edges.");

                Spaces = emptyGrid;
            }

            public MegaTile(List<List<bool>> spaces)
            {
                Spaces = spaces;
            }

            public MegaTile(string debugInput)
            {
                var lines = debugInput.Split(Environment.NewLine);
                var grid = BuildEmptyGrid(lines.Length);

                for (int x = 0; x < lines.Length; x++)
                {
                    for (int y = 0; y < lines.Length; y++)
                    {
                        grid[y][x] = lines[y][x] == '#';
                    }
                }

                Spaces = grid;
            }

            // OOB = false
            private bool GetCell(int x, int y)
            {
                if (x < 0 || x >= Spaces.Count() || y < 0 || y >= Spaces.Count())
                {
                    return false;
                }

                return Spaces[y][x];
            }

            public bool IsMonsterBoundingBoxTopLeft(int x, int y)
            {
                var monsterDebug = @"
..................#.
#....##....##....###
.#..#..#..#..#..#...
";

                return GetCell(x + 18, y)
                       && GetCell(x, y + 1)
                       && GetCell(x + 5, y + 1)
                       && GetCell(x + 6, y + 1)
                       && GetCell(x + 11, y + 1)
                       && GetCell(x + 12, y + 1)
                       && GetCell(x + 17, y + 1)
                       && GetCell(x + 18, y + 1)
                       && GetCell(x + 19, y + 1)
                       && GetCell(x + 1, y + 2)
                       && GetCell(x + 4, y + 2)
                       && GetCell(x + 7, y + 2)
                       && GetCell(x + 10, y + 2)
                       && GetCell(x + 13, y + 2)
                       && GetCell(x + 16, y + 2);
            }

            public int NumberOfMonsters()
            {
                var monsters = 0;

                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        if (IsMonsterBoundingBoxTopLeft(x, y))
                        {
                            monsters++;
                        }
                    }
                }

                return monsters;
            }

            // Assume monsters don't overlap
            public int CountOfNonMonsterTiles()
            {
                return Spaces.Sum(row => row.Count(x => x)) - NumberOfMonsters() * 15;
            }

            public MegaTile Rotate(Rotation rotation)
            {
                var newGrid = rotation switch
                {
                    Rotation.None => Spaces,
                    Rotation.Right90 => Right90(Spaces),
                    Rotation.Right180 => Right90(Right90(Spaces)),
                    Rotation.Left90 => Right90(Right90(Right90(Spaces))),
                    _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
                };

                return new MegaTile(newGrid);
            }

            public MegaTile Flip(Flip2D flip2D)
            {
                var newGrid = flip2D switch
                {
                    Flip2D.None => Spaces,
                    Flip2D.Horizontal => FlipHorizontal(Spaces),
                    Flip2D.Vertical => FlipVertical(Spaces),
                    _ => throw new ArgumentOutOfRangeException(nameof(flip2D))
                };
                return new MegaTile(newGrid);
            }

            public List<List<bool>> Right90(List<List<bool>> original)
            {
                var rotated = BuildEmptyGrid(Size);

                for (var y = 0; y < Size; y++)
                {
                    for (var x = 0; x < Size; x++)
                    {
                        rotated[y][x] = original[Size - x - 1][y];
                    }
                }

                return rotated;
            }

            public List<List<bool>> FlipHorizontal(List<List<bool>> original)
            {
                var rotated = BuildEmptyGrid(Size);

                for (var y = 0; y < Size; y++)
                {
                    for (var x = 0; x < Size; x++)
                    {
                        rotated[y][x] = original[y][Size - x - 1];
                    }
                }

                return rotated;
            }

            public List<List<bool>> FlipVertical(List<List<bool>> original)
            {
                var rotated = BuildEmptyGrid(Size);
                for (var y = 0; y < Size; y++)
                {
                    for (var x = 0; x < Size; x++)
                    {
                        rotated[y][x] = original[Size - y - 1][x];
                    }
                }

                return rotated;
            }

            private static List<List<bool>> BuildEmptyGrid(int size)
            {
                return Enumerable.Range(0, size)
                    .Select(_ => Enumerable.Range(0, size).Select(_ => false).ToList()).ToList();
            }

            public string ToString()
            {
                var sb = new StringBuilder();
                foreach (var row in Spaces)
                {
                    foreach (var item in row)
                    {
                        sb.Append(item ? '#' : '.');
                    }

                    sb.Append(Environment.NewLine);
                }

                return sb.ToString().Trim();
            }
        }

        private class Tile
        {
            // hardcoded for ease, could generalise later...
            public int Size;

            public long Id { get; set; }

            public Rotation Rotation { get; set; } = Rotation.None;

            public List<List<bool>> Spaces;

            #region PreCalculatedForPerformance

            public Oriented<long> EdgesAsLongs;

            // options arise from flipping each side
            public Oriented<(long, long)> EdgeOptions;

            #endregion PreCalculatedForPerformance

            public Tile(IReadOnlyList<string> inputLines)
            {
                Id = long.Parse(inputLines[0].Split()[1].Replace(":", ""));
                InitBeforeTiles(inputLines.Last().Length);
                for (var y = 0; y < Size; y++)
                {
                    for (var x = 0; x < Size; x++)
                    {
                        Spaces[y][x] = inputLines[y + 1][x] == '#';
                    }
                }

                InitAfterTiles();
            }

            private Tile(long id, List<List<bool>> spaces)
            {
                Id = id;
                InitBeforeTiles(spaces.Count());
                Spaces = spaces;
                InitAfterTiles();
            }

            private void InitAfterTiles()
            {
                EdgesAsLongs = new Oriented<long>(ordinal => ordinal switch
                {
                    Ordinal.North => EdgeAsLong(Row(0)),
                    Ordinal.South => EdgeAsLong(Row(Size - 1)),
                    Ordinal.West => EdgeAsLong(Column(0)),
                    Ordinal.East => EdgeAsLong(Column(Size - 1)),
                    _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
                });

                EdgeOptions = new Oriented<(long, long)>(ordinal =>
                {
                    var selfLongs = EdgesAsLongs;
                    return ordinal switch
                    {
                        Ordinal.North => (selfLongs.North, Reverse(selfLongs.North)),
                        Ordinal.South => (selfLongs.South, Reverse(selfLongs.South)),
                        Ordinal.West => (selfLongs.West, Reverse(selfLongs.West)),
                        Ordinal.East => (selfLongs.East, Reverse(selfLongs.East)),
                        _ => throw new ArgumentOutOfRangeException(nameof(ordinal))
                    };
                });
            }

            public Tile Rotate(Rotation rotation)
            {
                var newGrid = rotation switch
                {
                    Rotation.None => Spaces,
                    Rotation.Right90 => Right90(Spaces),
                    Rotation.Right180 => Right90(Right90(Spaces)),
                    Rotation.Left90 => Right90(Right90(Right90(Spaces))),
                    _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
                };

                return new Tile(Id, newGrid);
            }

            public Tile Flip(Flip2D flip2D)
            {
                var newGrid = flip2D switch
                {
                    Flip2D.None => Spaces,
                    Flip2D.Horizontal => FlipHorizontal(Spaces),
                    Flip2D.Vertical => FlipVertical(Spaces),
                    _ => throw new ArgumentOutOfRangeException(nameof(flip2D))
                };
                return new Tile(Id, newGrid);
            }

            public List<List<bool>> Right90(List<List<bool>> original)
            {
                var rotated = BuildEmptyGrid(Size);

                for (var y = 0; y < Size; y++)
                {
                    for (var x = 0; x < Size; x++)
                    {
                        rotated[y][x] = original[Size - x - 1][y];
                    }
                }

                return rotated;
            }

            public List<List<bool>> FlipHorizontal(List<List<bool>> original)
            {
                var rotated = BuildEmptyGrid(Size);

                for (var y = 0; y < Size; y++)
                {
                    for (var x = 0; x < Size; x++)
                    {
                        rotated[y][x] = original[y][Size - x - 1];
                    }
                }

                return rotated;
            }

            public List<List<bool>> FlipVertical(List<List<bool>> original)
            {
                var rotated = BuildEmptyGrid(Size);
                for (var y = 0; y < Size; y++)
                {
                    for (var x = 0; x < Size; x++)
                    {
                        rotated[y][x] = original[Size - y - 1][x];
                    }
                }

                return rotated;
            }

            private void InitBeforeTiles(int size)
            {
                Size = size;
                Spaces = BuildEmptyGrid(size);
            }

            private static List<List<bool>> BuildEmptyGrid(int size)
            {
                return Enumerable.Range(0, size)
                    .Select(_ => Enumerable.Range(0, size).Select(_ => false).ToList()).ToList();
            }

            public int Count => Spaces.Sum(row => row.Count(c => c));

            public int CountExceptBorders()
            {
                return Spaces.Skip(1).Take(8).Sum(row => row.Skip(1).Take(8).Count(x => x));
            }

            public long EdgeAsLong(List<bool> edge)
            {
                var sRep = edge.Aggregate("", (current, c) => current + (c ? '1' : '0'));
                return Convert.ToInt64(sRep, 2);
            }

            public long Reverse(long edge)
            {
                var sRep = Convert.ToString(edge, 2);
                sRep = sRep.PadLeft(Size, '0');
                var reversed = string.Join("", sRep.Reverse());
                return Convert.ToInt64(reversed, 2);
            }

            public List<bool> Row(int i) => Spaces[i];

            public List<bool> Column(int i) => Spaces.Select(l => l[i]).ToList();

            public static bool AnyPairwiseMatch((long, long) one, (long, long) two) =>
                one.Item1 == two.Item1 || one.Item1 == two.Item2 || one.Item2 == two.Item1 || one.Item2 == two.Item2;

            public int MatchesInAnyOrientation(Tile other, Ordinal thisOrdinal)
            {
                var sides = EdgeOptions.Get(thisOrdinal);
                return other.EdgeOptions.Map((otherOptions, _) => AnyPairwiseMatch(sides, otherOptions) ? 1 : 0)
                    .ToList()
                    .Sum();
            }

            public bool MatchesInSpecifiedOrientation(Tile other, Ordinal thisOrdinal, Ordinal otherOrdinal)
            {
                var sides = EdgeOptions.Get(thisOrdinal);
                var otherSides = other.EdgeOptions.Get(otherOrdinal);
                return AnyPairwiseMatch(sides, otherSides);
            }

            /// <summary>
            /// North, South, West, East
            /// </summary>
            /// <param name="others"></param>
            /// <returns></returns>
            public Oriented<int> PotentialNeighbours(List<Tile> others)
            {
                others = others.Where(o => o.Id != Id).ToList();

                int CountMatches(Ordinal side)
                {
                    return others.Sum(o => MatchesInAnyOrientation(o, side));
                }

                return new Oriented<int>(CountMatches);
            }

            public int NumberOfNeighbours(List<Tile> others)
            {
                return PotentialNeighbours(others).ToList().Count(x => x != 0);
            }

            public int NumberOfNeighbours(List<Tile> others, Ordinal ordinal)
            {
                return PotentialNeighbours(others).Get(ordinal);
            }

            public Oriented<List<long>> IdsOfPotentialNeighbours(List<Tile> others)
            {
                others = others.Where(o => o.Id != Id).ToList();

                List<long> NeighbourIds(Ordinal side)
                {
                    return others.Where(o => MatchesInAnyOrientation(o, side) != 0).Select(o => o.Id).ToList();
                }

                return new Oriented<List<long>>(NeighbourIds);
            }

            public bool IsAdjacentTo(List<Tile> allOthers, long id)
            {
                return IdsOfPotentialNeighbours(allOthers)
                    .ToList()
                    .Any(potentialNeighbours => potentialNeighbours.Contains(id));
            }

            public string ToString()
            {
                var sb = new StringBuilder();
                foreach (var row in Spaces)
                {
                    foreach (var item in row)
                    {
                        sb.Append(item ? '#' : '.');
                    }

                    sb.Append(Environment.NewLine);
                }

                return sb.ToString().Trim();
            }

            public bool GetCell(int x, int y)
            {
                return Spaces[y][x];
            }

            public Tile TrimEdges()
            {
                var newSpaces = BuildEmptyGrid(Size - 2);

                for (int x = 1; x < Size - 1; x++)
                {
                    for (int y = 1; y < Size - 1; y++)
                    {
                        var cell = GetCell(x, y);
                        newSpaces[y - 1][x - 1] = cell;
                    }
                }
                return new Tile(Id, newSpaces);
            }
        }
    }
}