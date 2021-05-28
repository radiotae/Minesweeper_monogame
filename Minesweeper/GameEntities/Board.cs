using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{
    class Board
    {
        public const int BOARD_WIDTH = 30;
        public const int BOARD_HEIGHT = 16;
        public const int TOTAL_BOMBS = 99;
        public const int TOTAL_SAFE_CELLS = BOARD_WIDTH * BOARD_HEIGHT - TOTAL_BOMBS;

        private ICell[,] CellList;
        private Random _random;

        private int _bombsFlagged;
        private int _revealed;
        public GameState GameState { get; private set; }

        public Board(SpriteFont font)
        {
            _random = new Random();

            MakeNewBoard();
            GameState = GameState.Running;
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
                    CellList[i, j].posX = i;
                    CellList[i, j].posY = j;

                }
            }

            int setBombs = 0;

            //Gets a random x and y value and checks to see if that position has a bomb. If it doesn't, add bomb.
            //Do this until there is 99 bombs.
            while (setBombs < TOTAL_BOMBS)
            {
                int x = _random.Next(0, BOARD_WIDTH );
                int y = _random.Next(0, BOARD_HEIGHT);

                if (CellList[x, y].Value != -1)
                {
                    CellList[x, y] = new BombCell();
                    CellList[x, y].posX = x;
                    CellList[x, y].posY = y;
                    List<ICell> surroundingCells = GetSurroundingCells(x, y);

                    foreach (ICell cell in surroundingCells)
                    {
                        if (cell.Value != -1)
                            cell.Value++;
                    }

                    setBombs++;
                }
            }

            _bombsFlagged = 0;
            _revealed = 0;
            GameState = GameState.Running;
        }

        private List<ICell> GetSurroundingCells(int x, int y)
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

        //A recursive call that calls the reveal function on the cell whose coordinates have been passed.
        //If a bomb is revealed (value is -1), return false.
        //If the value is 0, calls this function recursively on all the surrounding cells
        //Otherwise, calls the reveal function of the cell which simply sets the cellstate to revealed
        public void Reveal(int x, int y)
        {
            _revealed++;
            if (CellList[x, y].Value == 0)
            {
                CellList[x, y].Reveal();
                List<ICell> surroundingCells = GetSurroundingCells(x, y);

                foreach (SafeCell cell in surroundingCells)
                {
                    if (cell.State == CellState.Hidden)
                        Reveal(cell.posX, cell.posY);
                }
            }
            else if (!CellList[x, y].Reveal())
                GameState = GameState.GameOver;
        }

        public void SetCellState (int x, int y, CellState state)
        {
            CellList[x, y].State = state;
        }

        public CellState GetCellState(int x, int y)
        {
            return CellList[x, y].State;
        }

        public int GetCellValue(int x, int y)
        {
            return CellList[x, y].Value;
        }

        //Gets the 
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BOARD_HEIGHT; y++)
                {
                    CellList[x, y].Draw(spriteBatch, new Vector2(50 + x * 20, 65 + y * 20), font);
                }
            }
        }

        public void PressSurroundingCells(int x, int y)
        {
            List<ICell> surroundingCells = GetSurroundingCells(x, y);

            foreach (ICell cell in surroundingCells)
            {
                if (cell.State == CellState.Hidden)
                    cell.State = CellState.Pressed;
            }
        }

        public void FullCheck(int x, int y)
        {
            int bombCheck = 0;

            List<ICell> surroundingCells = GetSurroundingCells(x, y);

            foreach (ICell checkCell in surroundingCells)
            {
                if (checkCell.State == CellState.Flagged)
                {
                    bombCheck++;
                }
            }

            if (bombCheck == CellList[x, y].Value)
            {
                foreach (ICell checkCell in surroundingCells)
                {
                    if (checkCell.State == CellState.Hidden)
                        Reveal(checkCell.posX, checkCell.posY);

                }

            }
        }

        public void AddOrRemoveFlag(int x, int y)
        {
            if (CellList[x, y].State == CellState.Hidden)
            {
                CellList[x, y].State = CellState.Flagged;
                _bombsFlagged++;
            }
            else if (CellList[x, y].State == CellState.Flagged)
            {
                CellList[x, y].State = CellState.Hidden;
                _bombsFlagged--;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_revealed == TOTAL_SAFE_CELLS)
                GameState = GameState.Victory;

            //Unpressed all pressed HIDDEN cells
            foreach(ICell cell in CellList)
            {
                if (cell.State == CellState.Pressed)
                    cell.State = CellState.Hidden;
            }
            
        }
    }
}
