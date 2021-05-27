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

        private ICell[,] CellList;
        private Random _random;
        public GameState GameState;

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
            while (setBombs < 99)
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

            GameState = GameState.Running;
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

        public bool Reveal(int x, int y)
        {
            if (CellList[x,y].Value == 0)
            {
                CellList[x, y].Reveal();
                List<ICell> surroundingCells = GetSurroundingCells(x, y);

                foreach (SafeCell cell in surroundingCells)
                {
                    if (cell.State == CellState.Hidden)
                        Reveal(cell.posX, cell.posY);
                }
                return true;
            }
            else
            {
                return CellList[x, y].Reveal();
            }
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

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach(ICell cell in CellList)
            {
                if (cell.State == CellState.Hidden)
                    spriteBatch.DrawString(font, cell.Value.ToString(), new Vector2(50 + cell.posX * 20, 65 + cell.posY * 20), Color.Gray);
                else if (cell.State == CellState.Revealed)
                    spriteBatch.DrawString(font, cell.Value.ToString(), new Vector2(50 + cell.posX * 20, 65 + cell.posY * 20), Color.Black);
                else if (cell.State == CellState.Pressed)
                    spriteBatch.DrawString(font, cell.Value.ToString(), new Vector2(50 + cell.posX * 20, 65 + cell.posY * 20), Color.Green);

                else
                    spriteBatch.DrawString(font, cell.Value.ToString(), new Vector2(50 + cell.posX * 20, 65 + cell.posY * 20), Color.Red);
            }
        }

        public void Update(GameTime gameTime)
        {
            /*
            foreach(ICell in CellList)
            {

            }*/
        }
    }
}
