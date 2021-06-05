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
        public const int X_MARGIN = 25;
        public const int Y_MARGIN = 50;
        private const int CELL_SIZE = 20;
        private MouseState _previousMouse;
        private Board _board;

        private Rectangle _reset;

        private Rectangle _highScoreSwitch;

        private Rectangle _testButton;

        public InputManager(Board board)
        {
            _board = board;
            _reset = new Rectangle(325 - 30 / 2, 10, 30, 30);

            _testButton = new Rectangle(650 - 50, 0, 50, 50);

            _highScoreSwitch = new Rectangle(0, 0, 50, 50);
        }

        public void ProcessControls(GameTime gameTime)
        {
            MouseState currentMouse = Mouse.GetState();

            //Reset Button, using _reset values
            if (_previousMouse.LeftButton == ButtonState.Pressed
                && currentMouse.LeftButton == ButtonState.Released
                && _reset.Contains(currentMouse.Position))
                _board.MakeNewBoard();

            if (_previousMouse.LeftButton == ButtonState.Pressed
                && currentMouse.LeftButton == ButtonState.Released
                && _testButton.Contains(currentMouse.Position))
                _board.RevealAll();

            if (_reset.Contains(currentMouse.Position) && currentMouse.LeftButton == ButtonState.Pressed)
                _board.HoldReset();
            

            //Only accept inputs when GameState is set to Running
            if (_board.GameState == GameState.Running)
            {
                if (_previousMouse.LeftButton == ButtonState.Pressed
                && currentMouse.LeftButton == ButtonState.Released
                && _highScoreSwitch.Contains(currentMouse.Position))
                    _board.GoToHighScores();

                int[] cell = GetCellAtMousePos(currentMouse.Position);

                //Only applies inputs to cells if they are within the bounds of the cell grid
                if (cell != null)
                {
                    int x = cell[0];
                    int y = cell[1];

                    CellState cellState = _board.GetCellState(x, y);

                    if (currentMouse.LeftButton == ButtonState.Pressed) //if left button is pressed
                    {
                        //If the button is Hidden, places it into pressed state
                        if (cellState == CellState.Hidden)
                            _board.SetCellState(x, y, CellState.Pressed);

                        //if both buttons are pressed, press all relevant tiles
                        if (currentMouse.RightButton == ButtonState.Pressed)
                        {
                            _board.PressSurroundingCells(x, y);
                        }
                        //if right button is released while left button is pressed AND it is a revealed cell, do a surrounding check on that cell.
                        else if (_previousMouse.RightButton == ButtonState.Pressed
                                && currentMouse.RightButton == ButtonState.Released && cellState == CellState.Revealed)
                        {
                            _board.FullCheck(x, y);
                        }
                    }

                    //If the left button is pressed and released
                    if (_previousMouse.LeftButton == ButtonState.Pressed
                        && currentMouse.LeftButton == ButtonState.Released)
                    {
                        //Calls the reveal method
                        if (currentMouse.RightButton != ButtonState.Pressed && _previousMouse.RightButton != ButtonState.Pressed
                            && cellState == CellState.Hidden)
                            _board.Reveal(x, y);
                        //if the right button is held while left button is released, does a full check on the surrounding cells if it is a revealed cell
                        else if (_previousMouse.RightButton == ButtonState.Pressed && cellState == CellState.Revealed)
                        {
                            _board.FullCheck(x, y);
                        }

                    }

                    //if right button is pressed and released, will flag and unflag HIDDEN cells
                    if (_previousMouse.RightButton != ButtonState.Pressed
                        && currentMouse.RightButton == ButtonState.Pressed
                        && currentMouse.LeftButton != ButtonState.Pressed
                        && _previousMouse.LeftButton != ButtonState.Pressed)
                    {
                        _board.AddOrRemoveFlag(x, y);
                    }

                }


            }
            else if (_board.GameState == GameState.HighScores)
            {
                if (_previousMouse.LeftButton == ButtonState.Pressed
                && currentMouse.LeftButton == ButtonState.Released
                && _highScoreSwitch.Contains(currentMouse.Position))
                    _board.GoToBoard();
            }

            //stores the current mouse state for use in next update
            _previousMouse = currentMouse;
        }


        //Will return an array of 2 int values (x and y coordinate of a cell) IF the mouse position is within the boundaries of the cell grid.
        //Otherwise, it will return a null value.
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
    }
}
