using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    public class SudokuSolver
    {
        public const int SIDE = 3 * 3;

        public int NumBacktracks { get; private set; }
        public int[,] Board { get; private set; }

        public SudokuSolver(IEnumerable<int> digits)
        {
            Board = new int[SIDE, SIDE];
            int i = 0;

            foreach(var digit in digits)
            {
                int x = i % SIDE;
                int y = i++ / SIDE;

                Board[x, y] = digit;

                if (i > (SIDE * SIDE) - 1)
                    break;
            }
        }

        public SudokuSolver(int[,] board)
        {
            if (board.GetLength(0) != SIDE || board.GetLength(1) != SIDE)
                throw new ArgumentException("Board is not the correct size");

            Board = board;
        }

        public int[,] Solve()
        {
            NumBacktracks = 0;
            SolveImpl(Board, 0);

            return Board;
        }

        private bool SolveImpl(int[,] board, int index)
        {
            if (index > (SIDE * SIDE) - 1)
                return true;

            //Convert from linear to grid coordinates
            int x = index % SIDE;
            int y = index / SIDE;

            if (Board[x,y] != 0)
                return SolveImpl(board, index + 1);

            //Test each possible value
            for (int i = 1; i <= 9; i++)
            {
                if (IsValid(board, i, x, y))
                {
                    board[x,y] = i;

                    var result = SolveImpl(board, index + 1);
                    if (result)
                        return true;

                    //Erase out the flawed solution
                    board[x,y] = 0;
                    NumBacktracks++;
                }
            }

            return false;
        }

        private bool IsValid(int[,] board, int toTest, int x, int y)
        {
            //Find that quadrant start coordinates
            int qX = (x / 3) * 3;
            int qY = (y / 3) * 3;

            //Test local
            for (int lx = 0; lx < 3; lx++)
            {
                for (int ly = 0; ly < 3; ly++)
                {
                    if (board[qX + lx, qY + ly] == toTest)
                        return false;
                }
            }

            //Test global horizontal
            for (int gx = 0; gx < SIDE; gx++)
            {
                if (board[gx, y] == toTest)
                    return false;
            }

            //Test global vertical
            for (int gy = 0; gy < SIDE; gy++)
            {
                if (board[x, gy] == toTest)
                    return false;
            }

            return true;
        }
    }
}
