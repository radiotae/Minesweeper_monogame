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

        public const int CELL_SIZE = 20;
        private const int WIDTH_MARGIN = 25;
        private const int HEIGHT_MARGIN = 50;
        private const int RESET_BUTTON_HEIGH_MARGIN = 10;
        private ICell[,] _cellList;
        private Random _random;
        private ResetButton _reset;

        private double _realTime;
        private int _displayTime => (int) Math.Ceiling(_realTime);
        private bool _realHasStarted;

        //private bool _firstClick;

        private int _bombsFlagged;
        private int _revealed;
        public GameState GameState { get; private set; }

        public Board(SpriteFont font)
        {
            _random = new Random();

            MakeNewBoard();

            _reset = new ResetButton();
        }

        public void MakeNewBoard()
        {
            _realTime = 0;
            _realHasStarted = false;
            //_firstClick = false;
            //First sets all cell values to 0 and to hidden
            _cellList = new ICell[BOARD_WIDTH, BOARD_HEIGHT];

            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    _cellList[i, j] = new SafeCell();
                    _cellList[i, j].posX = i;
                    _cellList[i, j].posY = j;

                }
            }

            int setBombs = 0;

            //Gets a random x and y value and checks to see if that position has a bomb. If it doesn't, add bomb.
            //Increments the value of surrounding SAFE cells by 1
            //Do this until there is 99 bombs.
            while (setBombs < TOTAL_BOMBS)
            {
                int x = _random.Next(0, BOARD_WIDTH );
                int y = _random.Next(0, BOARD_HEIGHT);

                if (_cellList[x, y].Value != -1)
                {
                    _cellList[x, y] = new BombCell();
                    _cellList[x, y].posX = x;
                    _cellList[x, y].posY = y;
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

        //Returns a list of cells that are the surrounding cells of the given coordinate
        private List<ICell> GetSurroundingCells(int x, int y)
        {
            List<ICell> cellList = new List<ICell>();

            if (IsValidCell(x - 1, y - 1))
                cellList.Add(_cellList[x - 1, y - 1]);
            if (IsValidCell(x, y - 1))
                cellList.Add(_cellList[x, y - 1]);
            if (IsValidCell(x + 1, y - 1))
                cellList.Add(_cellList[x + 1, y - 1]);
            if (IsValidCell(x - 1, y))
                cellList.Add(_cellList[x - 1, y]);
            if (IsValidCell(x + 1, y))
                cellList.Add(_cellList[x + 1, y]);
            if (IsValidCell(x - 1, y + 1))
                cellList.Add(_cellList[x - 1, y + 1]);
            if (IsValidCell(x, y + 1))
                cellList.Add(_cellList[x, y + 1]);
            if (IsValidCell(x + 1, y + 1))
                cellList.Add(_cellList[x + 1, y + 1]);

            return cellList;

        }

        //Checks to see if the given coordinates are valid indexes
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
            if(_revealed == 0)
            {
                
                if (_cellList[x, y].Value == -1)
                {
                    do
                    {
                        MakeNewBoard();
                    } while (_cellList[x, y].Value == -1);
                }

                _realHasStarted = true;
            }

            _revealed++; //NEED TO TEST IF THIS WORKS ==================================================================================================================================================
            if (_cellList[x, y].Value == 0)
            {
                _cellList[x, y].Reveal();
                List<ICell> surroundingCells = GetSurroundingCells(x, y);

                foreach (SafeCell cell in surroundingCells)
                {
                    if (cell.State == CellState.Hidden)
                        Reveal(cell.posX, cell.posY);
                }
            }
            else if (!_cellList[x, y].Reveal())
                GameState = GameState.GameOver;
        }

        //Changes the given cell's state
        //MIGHT REMOVE THIS AND HAVE SEPARATE FUNCTIONS FOR EACH CELL STATE CHANGE
        public void SetCellState (int x, int y, CellState state)
        {
            _cellList[x, y].State = state;
        }

        //Returns the cell state given the coordinates
        public CellState GetCellState(int x, int y)
        {
            return _cellList[x, y].State;
        }

        //Will get the cell value given the coordinates
        //THIS METHOD IS NEVER CALLED BUT I'M KEEPING IT IN JUST IN CASE I USE IT FOR LATER
        //POSSIBLY FOR A BOARD SOLVER
        public int GetCellValue(int x, int y)
        {
            return _cellList[x, y].Value;
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            _reset.Draw(spriteBatch, new Rectangle(350 - 30 / 2, RESET_BUTTON_HEIGH_MARGIN, 30, 30), spriteSheet, GameState);

            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BOARD_HEIGHT; y++)
                {
                    //_cellList[x, y].Draw(spriteBatch, new Vector2(WIDTH_MARGIN + x * CELL_SIZE, HEIGHT_MARGIN + y * CELL_SIZE), spriteSheet);
                    _cellList[x, y].Draw(spriteBatch, new Rectangle(WIDTH_MARGIN + x * CELL_SIZE, HEIGHT_MARGIN + y * CELL_SIZE, 20, 20), spriteSheet);
                }
            }

            int[] time = SplitDigits(_displayTime);
            spriteBatch.Draw(spriteSheet,
                new Rectangle(650 - 3 * 18 - 5, 5, 18, 33),
                new Rectangle(128, 253 - (22 + 1) * time[0], 12, 22),
                Color.White);

            spriteBatch.Draw(spriteSheet,
                new Rectangle(650 - 2 * 18 - 5, 5, 18, 33),
                new Rectangle(128, 253 - (22 + 1) * time[1], 12, 22),
                Color.White);

            spriteBatch.Draw(spriteSheet,
                new Rectangle(650 - 1 * 18 - 5, 5, 18, 33),
                new Rectangle(128 , 253 - (22 + 1) * time[2], 12, 22),
                Color.White);
        }

        //Given the input cell coordinates, will press the surrounding cells IF they are hidden
        public void PressSurroundingCells(int x, int y)
        {
            List<ICell> surroundingCells = GetSurroundingCells(x, y);

            foreach (ICell cell in surroundingCells)
            {
                if (cell.State == CellState.Hidden)
                    cell.State = CellState.Pressed;
            }
        }

        private int[] SplitDigits(int input)
        {
            if (input > 999)
                input = 999;

            string inputStr = input.ToString().PadLeft(3, '0');
            int[] result = new int[inputStr.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = (int)char.GetNumericValue(inputStr[i]);
            }

            return result;
        }

        //Will check the surrounding cells of the input coordinate (if revealed) and check the number
        //of bombs flagged. If the bombs flagged is equal to the value of the center cell, will attempt
        //to reveal all surrounding hidden cells (possibly causing a Game Over)
        public void FullCheck(int x, int y)
        {
            if (_cellList[x, y].State == CellState.Revealed)
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

                if (bombCheck == _cellList[x, y].Value)
                {
                    foreach (ICell checkCell in surroundingCells)
                    {
                        if (checkCell.State == CellState.Hidden)
                            Reveal(checkCell.posX, checkCell.posY);

                    }

                }
            }
        }

        //Adds or Removes flag depending on the state of the cell.
        //If the cell is neither hidden nor flagged, will do nothing.
        public void AddOrRemoveFlag(int x, int y)
        {
            if (_cellList[x, y].State == CellState.Hidden)
            {
                _cellList[x, y].State = CellState.Flagged;
                _bombsFlagged++;
            }
            else if (_cellList[x, y].State == CellState.Flagged)
            {
                _cellList[x, y].State = CellState.Hidden;
                _bombsFlagged--;
            }
        }

        public void HoldReset()
        {
            _reset.Press();
        }


        //When the Board updates, it will check to see if the revealed cells is equal to the total number of safe cells in the board.
        //If so, change the state of the game to Victory AND flag any bombs that are still hidden
        //Update will ALSO unpress every button, which is ok because inputmanager updates afterwards so buttons can still be pressed
        public void Update(GameTime gameTime)
        {
            if (GameState == GameState.Running && _realHasStarted)
                _realTime += gameTime.ElapsedGameTime.TotalSeconds;

            _reset.Release();

            if (_revealed == TOTAL_SAFE_CELLS && GameState == GameState.Running)
            {
                GameState = GameState.Victory;

                foreach(ICell cell in _cellList)
                {
                    if (cell.State == CellState.Hidden && cell.Value == -1)
                        cell.State = CellState.Flagged;
                }
            }

            if (GameState == GameState.GameOver)
            {
                foreach (ICell cell in _cellList)
                {
                    if (cell.Value == -1 && cell.State == CellState.Hidden)
                        cell.State = CellState.MissingBomb;
                }
            }



            //Unpressed all pressed HIDDEN cells
            foreach(ICell cell in _cellList)
            {
                if (cell.State == CellState.Pressed)
                    cell.State = CellState.Hidden;
            }
            
        }
    }
}
