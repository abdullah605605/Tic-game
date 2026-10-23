
using System;
using System.Collections.Generic;

class Program
{
    static char[] cells = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    static Random random = new Random();

    static void Main()
    {
        char player = 'X';
        char bot = 'O';

        while (true)
        {
            Console.Clear();
            DisplayBoard();

            Console.Write("Choose a position (1-9): ");
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out int choice) ||
                choice < 1 || choice > 9 ||
                cells[choice - 1] == 'X' ||
                cells[choice - 1] == 'O')
            {
                Console.WriteLine("Invalid or occupied position!");
                Console.ReadKey();
                continue;
            }

            cells[choice - 1] = player;

            if (Winner(player))
            {
                Console.Clear();
                DisplayBoard();
                Console.WriteLine("Congratulations! You win!");
                break;
            }

            if (IsDraw())
            {
                Console.Clear();
                DisplayBoard();
                Console.WriteLine("It's a draw!");
                break;
            }

            Console.WriteLine("Computer is thinking...");
            System.Threading.Thread.Sleep(700);

            int botMove = FindBestMove(bot, player);
            cells[botMove] = bot;

            if (Winner(bot))
            {
                Console.Clear();
                DisplayBoard();
                Console.WriteLine("Computer wins! Try again.");
                break;
            }

            if (IsDraw())
            {
                Console.Clear();
                DisplayBoard();
                Console.WriteLine("It's a draw!");
                break;
            }
        }

        Console.WriteLine("\nPress Enter to exit.");
        Console.ReadLine();
    }

    static void DisplayBoard()
    {
        Console.WriteLine("=== TIC TAC TOE ===\n");
        Console.WriteLine($" {cells[0]} | {cells[1]} | {cells[2]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {cells[3]} | {cells[4]} | {cells[5]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {cells[6]} | {cells[7]} | {cells[8]} ");
        Console.WriteLine();
    }

    static int FindBestMove(char bot, char player)
    {
        // Win if possible
        int move = FindWinningMove(bot);
        if (move != -1)
            return move;

        // Block the player's winning move
        move = FindWinningMove(player);
        if (move != -1)
            return move;

        // Take the center
        if (cells[4] != 'X' && cells[4] != 'O')
            return 4;

        // Prefer a corner
        int[] corners = { 0, 2, 6, 8 };
        List<int> availableCorners = new List<int>();

        foreach (int corner in corners)
        {
            if (cells[corner] != 'X' && cells[corner] != 'O')
                availableCorners.Add(corner);
        }

        if (availableCorners.Count > 0)
            return availableCorners[random.Next(availableCorners.Count)];

        // Choose any remaining position
        List<int> available = new List<int>();

        for (int i = 0; i < 9; i++)
        {
            if (cells[i] != 'X' && cells[i] != 'O')
                available.Add(i);
        }

        return available[random.Next(available.Count)];
    }

    static int FindWinningMove(char symbol)
    {
        int[][] lines =
        {
            new int[] { 0, 1, 2 },
            new int[] { 3, 4, 5 },
            new int[] { 6, 7, 8 },
            new int[] { 0, 3, 6 },
            new int[] { 1, 4, 7 },
            new int[] { 2, 5, 8 },
            new int[] { 0, 4, 8 },
            new int[] { 2, 4, 6 }
        };

        foreach (int[] line in lines)
        {
            int count = 0;
            int empty = -1;

            foreach (int index in line)
            {
                if (cells[index] == symbol)
                    count++;
                else if (cells[index] != 'X' && cells[index] != 'O')
                    empty = index;
            }

            if (count == 2 && empty != -1)
                return empty;
        }

        return -1;
    }

    static bool Winner(char symbol)
    {
        return FindWinningMove(symbol) == -1 &&
            (
                (cells[0] == symbol && cells[1] == symbol && cells[2] == symbol) ||
                (cells[3] == symbol && cells[4] == symbol && cells[5] == symbol) ||
                (cells[6] == symbol && cells[7] == symbol && cells[8] == symbol) ||
                (cells[0] == symbol && cells[3] == symbol && cells[6] == symbol) ||
                (cells[1] == symbol && cells[4] == symbol && cells[7] == symbol) ||
                (cells[2] == symbol && cells[5] == symbol && cells[8] == symbol) ||
                (cells[0] == symbol && cells[4] == symbol && cells[8] == symbol) ||
                (cells[2] == symbol && cells[4] == symbol && cells[6] == symbol)
            );
    }

    static bool IsDraw()
    {
        foreach (char cell in cells)
        {
            if (cell != 'X' && cell != 'O')
                return false;
        }

        return true;
    }
}
