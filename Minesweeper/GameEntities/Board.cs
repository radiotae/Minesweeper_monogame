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
            CellList = new ICell[BOARD_WIDTH,BOARD_HEIGHT];
            _random = new Random();
        }

        public void MakeNewBoard()
        {
            foreach (ICell cell in CellList)
            {
                cell.Value = 0;
                cell.State = CellState.Hidden;
            }

            int setBombs = 0;

            while (setBombs < 99)
            {
                int x = _random.Next(1, BOARD_WIDTH + 1);
                int y = _random.Next(1, BOARD_HEIGHT + 1);

                if (CellList[x, y].Value != -1)
                {
                    CellList[x, y].Value = -1;

                    List<ICell> surroundingCells = GetSurroundingCells(x, y);

                    foreach (ICell cell in surroundingCells)
                    {
                        cell.Value++;
                    }

                    setBombs++;
                }
            }
        }

        public List<ICell> GetSurroundingCells(int x, int y)
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
