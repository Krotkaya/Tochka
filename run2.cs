
using System;
using System.Collections.Generic;
using System.Linq;


class Program
{
    // Константы для символов ключей и дверей
    static readonly char[] keys_char = Enumerable.Range('a', 26).Select(i => (char)i).ToArray();
    static readonly char[] doors_char = keys_char.Select(char.ToUpper).ToArray();
    
    // Метод для чтения входных данных
    static List<List<char>> GetInput()
    {
        var data = new List<List<char>>();
        string line;
        while ((line = Console.ReadLine()) != null && line != "")
        {
            data.Add(line.ToCharArray().ToList());
        }
        return data;
    }

    static int Solve(List<List<char>> data)
    {
    int height = data.Count;
    int width = data[0].Count;
    var characterPositions = new List<(int, int)>();
    int keys = 0;
    
    for (int columnIndex = 0; columnIndex < width; columnIndex++){
        for (int rowIndex = 0; rowIndex < height; rowIndex++)
        {

            var cell = data[rowIndex][columnIndex];
            if (cell == '@')
            {
                characterPositions.Add((rowIndex, columnIndex));
            }
            if (cell >= 'a' && cell <= 'z')
            {
                keys |= 1 << (cell - 'a');
                }
        }
    }

    var moveOptions = new List<(int, int)>
    {
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1)
    };

    var visited = new HashSet<string>();
    var queue = new Queue<(int, (int, int)[], int)>();
    var robotsArr = new (int, int)[4];
    Array.Copy(characterPositions.ToArray(), robotsArr, 4);

    visited.Add(GenerateStateKey(robotsArr, 0));
    queue.Enqueue((0, robotsArr, 0));

    while (queue.Count > 0)
    {
        var current= queue.Dequeue();
        int currentSteps = current.Item1;
        int currentKeys = current.Item3;

        if ((currentKeys | 0) == keys)
        {
            return currentSteps;
        }

        var currentPositions = current.Item2;

        int characterIndex = 0;

        while (characterIndex < 4)
        {
            var characterPosition = currentPositions[characterIndex];
            int dx = 0;
            while (dx < moveOptions.Count)
            {
                int newXCoordinate = characterPosition.Item1 + moveOptions[dx].Item1;
                int newYCoordinate = characterPosition.Item2 + moveOptions[dx].Item2;

                if (!(newXCoordinate < 0 || newYCoordinate < 0 || newXCoordinate >= height || newYCoordinate >= width))
                {
                    char cell = data[newXCoordinate][newYCoordinate];
                    if (!(cell == '#'))
                    {
                        if (!(char.IsUpper(cell) && ((currentKeys & (1 << (cell - 'A'))) == 0)))
                        {
                            int updatedKeys = currentKeys;
                            if (char.IsLower(cell))
                            {
                                updatedKeys = currentKeys | (1 << (cell - 'a'));
                            }

                            var updatedPositions = new (int, int)[4];
                            Array.Copy(currentPositions, updatedPositions, 4);

                            updatedPositions[characterIndex] = (newXCoordinate, newYCoordinate);

                            string nextState = GenerateStateKey(updatedPositions, updatedKeys);
                            if (!visited.Contains(nextState))
                            {
                                visited.Add(nextState);
                                queue.Enqueue((currentSteps + 1, updatedPositions, updatedKeys));
                            }
                        }
                    }
                }

                dx++;
            }

            characterIndex++;
        }
    }

    return -1;
}

static string GenerateStateKey((int, int)[] robotPositions, int keys)
{
    var sortable = new List<string>();
    for (int i = 0; i < robotPositions.Length; i++)
    {
        sortable.Add($"{robotPositions[i].Item1}:{robotPositions[i].Item2}");
    }
    sortable.Sort(); // Чтобы позиции в любом порядке были одинаковыми
    return string.Join("~", sortable) + "|" + keys.ToString();
}

static string StateKey((int, int)[] robots, int keys)
{
    var sorted = robots.OrderBy(p => p.Item1).ThenBy(p => p.Item2).ToArray();
    return string.Join(",", sorted.Select(p => $"{p.Item1}:{p.Item2}")) + $"|{keys}";
}

    
    static void Main()
    {
        var data = GetInput();
        int result = Solve(data);
        
        if (result == -1)
        {
            Console.WriteLine("No solution found");
        }
        else
        {
            Console.WriteLine(result);
        }
    }
}