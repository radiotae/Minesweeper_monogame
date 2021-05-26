using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{
    class Board
    {
        public const int BOARD_WIDTH = 30;
        public const int BOARD_HEIGHT = 16;

        private ICell[,] CellList;
        private Random _random;

        public Board()
        {
            _random = new Random();

            MakeNewBoard();
        }

        public void MakeNewBoard()
        {
            //First sets all cell values to 0 and to hidden
            CellList = new ICell[BOARD_WIDTH, BOARD_HEIGHT];

            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    CellList[i, j] = new SafeCell();
                }
            }

            int setBombs = 0;

            //Gets a random x and y value and checks to see if that position has a bomb. If it doesn't, add bomb.
            //Do this until there is 99 bombs.
            while (setBombs < 99)
            {
                int x = _random.Next(1, BOARD_WIDTH + 1);
                int y = _random.Next(1, BOARD_HEIGHT + 1);

                if (CellList[x, y].Value != -1)
                {
                    CellList[x, y] = new BombCell();
                    List<ICell> surroundingCells = GetSurroundingCells(x, y);

                    foreach (SafeCell cell in surroundingCells)
                    {
                        cell.Value++;
                    }

                    setBombs++;
                }
            }
        }

        public List<ICell> GetSurroundingCells(int x, int y, bool key = true)
        {
            List<ICell> cellList = new List<ICell>();

            if (IsValidCell(x - 1, y - 1))
                cellList.Add(CellList[x - 1, y - 1]);
            if (IsValidCell(x, y - 1))
                cellList.Add(CellList[x, y - 1]);
            if (IsValidCell(x + 1, y - 1))
                cellList.Add(CellList[x + 1, y - 1]);
            if (IsValidCell(x - 1, y))
                cellList.Add(CellList[x - 1, y]);
            if (IsValidCell(x + 1, y))
                cellList.Add(CellList[x + 1, y]);
            if (IsValidCell(x - 1, y + 1))
                cellList.Add(CellList[x - 1, y + 1]);
            if (IsValidCell(x, y + 1))
                cellList.Add(CellList[x, y + 1]);
            if (IsValidCell(x + 1, y + 1))
                cellList.Add(CellList[x + 1, y + 1]);

            return cellList;

        }

        public bool IsValidCell(int x, int y)
        {
            return x >= 0
                && x < BOARD_WIDTH
                && y >= 0
                && y < BOARD_HEIGHT;
        }
    }
}
