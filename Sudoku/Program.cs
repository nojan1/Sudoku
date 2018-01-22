using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Sudoku
{
    class SolverDataCombo
    {
        public SudokuSolver Solver { get; set; }
        public string Solution { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var digits = File.ReadAllText("Testfiles/real.txt").Select(x => Convert.ToInt32(x.ToString()));
            //var sudokuSolver = new SudokuSolver(digits);

            //var board = sudokuSolver.Solve();

            var tasks = File.ReadAllLines("Testfiles/a-lot-of-sudokus.txt")
                .Select(line =>{
                    var solution = line.Substring(0, line.IndexOf(';'));
                    var problem = line.Substring(line.IndexOf(';') + 1);

                    return new SolverDataCombo
                    {
                        Solver = new SudokuSolver(problem.Select(x => Convert.ToInt32(x.ToString()))),
                        Solution = solution
                    };
                })
                .ToList();


            var stopWatch = new Stopwatch();
            int numSolved = 0;

            stopWatch.Start();

            foreach(var task in tasks)
            {
                task.Solver.Solve();

                numSolved++;

                if (numSolved % 50 == 0)
                    PrintStat(numSolved, stopWatch);
            }

            stopWatch.Stop();
            PrintStat(numSolved, stopWatch);

            Console.ReadKey();

            for(int i = 0; i < tasks.Count; i++)
            {
                var calculatedSolution = ReduceToString(tasks[i].Solver.Board);
                if(calculatedSolution != tasks[i].Solution)
                {
                    Console.WriteLine("Missmatch detected! At line {i+1}");
                    Console.WriteLine($"Got: {calculatedSolution}");
                    Console.WriteLine($"Expected: {tasks[i].Solution}");
                    Console.WriteLine("=================================");
                }
            }

            Console.ReadKey();
        }

        static string ReduceToString(int[,] board)
        {
            var sb = new StringBuilder();

            for(int y = 0; y < board.GetLength(1); y++)
            {
                for(int x = 0; x < board.GetLength(0); x++)
                {
                    sb.Append(board[x, y]);
                }
            }

            return sb.ToString();
        }

        static void PrintStat(int numSolved, Stopwatch stopWatch)
        {
            double rate = (double)numSolved / stopWatch.Elapsed.TotalSeconds;

            Console.Clear();
            Console.WriteLine($"Sudokus solved: {numSolved}");
            Console.WriteLine($"Time elapsed: {stopWatch.Elapsed.TotalSeconds} seconds");
            Console.WriteLine($"Rate: {rate} sudoku/sec");

            Console.WriteLine(stopWatch.IsRunning ? "Running...." : "Done");
        }

        static void Print(int[,] board, SudokuSolver sudokuSolver)
        {
            for (int y = 0; y < SudokuSolver.SIDE; y++)
            {
                Console.Write("|");
                for (int x = 0; x < SudokuSolver.SIDE; x++)
                {
                    if (y % 3 == 0)
                        Console.Write("====");
                    else
                        Console.Write("----");
                }
                Console.WriteLine("\b|");

                for (int x = 0; x < SudokuSolver.SIDE; x++)
                {
                    if (x % 3 == 0)
                        Console.Write("| ");
                    else
                        Console.Write(": ");

                    Console.Write($"{board[x, y]} ");
                }
                Console.WriteLine("|");
            }

            Console.Write("|");
            for (int x = 0; x < SudokuSolver.SIDE; x++)
                Console.Write("====");

            Console.WriteLine("\b|");


            Console.Write($"Num backtracks: {sudokuSolver.NumBacktracks}");
        }

    }
}
