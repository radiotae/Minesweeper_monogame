using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.GameEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.System
{
    class InputManager
    {
        public const int X_MARGIN = 50;
        public const int Y_MARGIN = 65;
        private const int CELL_SIZE = 20;
        private MouseState _previousMouse;
        private Board _board;

        private Rectangle _reset;

        public InputManager(Board board)
        {
            _board = board;
            _reset = new Rectangle(0, 0, X_MARGIN, X_MARGIN);
        }

        public void ProcessControls(GameTime gameTime)
        {
            MouseState currentMouse = Mouse.GetState();
            int[] cell = GetCellAtMousePos(currentMouse.Position);


            //Reset Button, using _reset values
            if (_previousMouse.LeftButton == ButtonState.Pressed
                && currentMouse.LeftButton == ButtonState.Released
                && _reset.Contains(currentMouse.Position))
                _board.MakeNewBoard();

            //Only accept inputs when GameState is set to Running
            if (_board.GameState == GameState.Running)
            {

                //Goes through entire board to stop pressing on buttons that are Hidden
                for (int i = 0; i < Board.BOARD_WIDTH; i++)
                {
                    for (int j = 0; j < Board.BOARD_HEIGHT; j++)
                    {
                        if (_board.GetCellState(i, j) == CellState.Pressed /*&&
                            (cell == null || i != cell[0] || j != cell[1])*/)
                            _board.SetCellState(i, j, CellState.Hidden);
                    }
                }

                //Only applies inputs to cells if they are within the bounds of the cell grid
                if (cell != null)
                {
                    CellState cellState = _board.GetCellState(cell[0], cell[1]);
                    if (currentMouse.LeftButton == ButtonState.Pressed) //if left button is pressed
                    {
                        if (cellState == CellState.Hidden)
                            _board.SetCellState(cell[0], cell[1], CellState.Pressed);

                        if (currentMouse.RightButton == ButtonState.Pressed)
                        {
                            List<ICell> surroundingCells = _board.GetSurroundingCells(cell[0], cell[1]);

                            foreach (ICell pushCell in surroundingCells)
                            {
                                if(pushCell.State == CellState.Hidden)
                                    pushCell.State = CellState.Pressed;
                            }
                        }
                        else if (_previousMouse.RightButton == ButtonState.Pressed
                                && currentMouse.RightButton == ButtonState.Released && cellState == CellState.Revealed)
                        {
                            FullCheck(cell);
                        }
                    }


                    if (_previousMouse.LeftButton == ButtonState.Pressed
                        && currentMouse.LeftButton == ButtonState.Released)
                    {
                        if (currentMouse.RightButton != ButtonState.Pressed
                            && cellState == CellState.Hidden && !_board.Reveal(cell[0], cell[1]))
                            _board.GameState = GameState.GameOver;
                        else if(currentMouse.RightButton == ButtonState.Pressed && cellState == CellState.Revealed)
                        {
                            FullCheck(cell);
                        }

                    }

                    if (_previousMouse.RightButton == ButtonState.Pressed
                        && currentMouse.RightButton == ButtonState.Released)
                    {
                        if (currentMouse.LeftButton != ButtonState.Pressed)
                        {
                            if (cellState == CellState.Hidden)
                                _board.SetCellState(cell[0], cell[1], CellState.Flagged);
                            else if (cellState == CellState.Flagged)
                                _board.SetCellState(cell[0], cell[1], CellState.Hidden);
                        }

                    }
                }


            }

            _previousMouse = currentMouse;
        }

        private int[] GetCellAtMousePos(Point mouse)
        {
            if (mouse.X < X_MARGIN || mouse.X >= X_MARGIN + CELL_SIZE * Board.BOARD_WIDTH
                || mouse.Y < Y_MARGIN || mouse.Y >= Y_MARGIN + CELL_SIZE * Board.BOARD_HEIGHT)
                return null;

            int[] point = new int[2];
            point[0] = (int)((mouse.X - X_MARGIN) / CELL_SIZE);
            point[1] = (int)((mouse.Y - Y_MARGIN) / CELL_SIZE);
            

            return point;
        }

        private void FullCheck(int[] values)
        {
            int bombCheck = 0;
            //bool SafeCheck = true;
            int x = values[0];
            int y = values[1];

            List<ICell> surroundingCells = _board.GetSurroundingCells(x,y);

            foreach (ICell checkCell in surroundingCells)
            {
                if (checkCell.State == CellState.Flagged)
                {
                    bombCheck++;
                } 
                /*
                else if (checkCell.Value != -1 && checkCell.State == CellState.Flagged)
                {
                    SafeCheck = false;
                }*/
            }

            if(bombCheck == _board.GetCellValue(x, y))
            {
                foreach (ICell checkCell in surroundingCells)
                {
                    if(checkCell.State == CellState.Hidden)
                    {
                        if (!_board.Reveal(checkCell.posX, checkCell.posY))
                            _board.GameState = GameState.GameOver;
                    }
                        
                }
                    
            }
        }
    }
}
