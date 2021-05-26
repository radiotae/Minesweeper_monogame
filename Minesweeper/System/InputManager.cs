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
        private MouseState _previousMouse;
        private Board _board;

        private Rectangle _reset;

        public InputManager(Board board)
        {
            _board = board;
            _reset = new Rectangle(0, 0, 50, 50);
        }

        public void ProcessControls(GameTime gameTime)
        {
            MouseState currentMouse = Mouse.GetState();
            int[] cell = GetCellAtMousePos(currentMouse.Position);

            if (currentMouse.LeftButton == ButtonState.Pressed)
            {
                

                for (int i = 0; i<Board.BOARD_WIDTH; i++)
                {
                    for (int j = 0; j < Board.BOARD_HEIGHT; j++)
                    {
                        if (_board.GetCellState(i, j) == CellState.Pressed &&
                            (cell == null || i != cell[0] || j != cell[1]))
                            _board.SetCellState(i, j, CellState.Hidden);
                        if (cell != null &&_board.GetCellState(i, j) == CellState.Hidden &&
                            i == cell[0] && j == cell[1])
                            _board.SetCellState(i, j, CellState.Pressed);

                        
                    }
                }
                
            }


            if(_previousMouse.LeftButton == ButtonState.Pressed
                && currentMouse.LeftButton == ButtonState.Released)
            {
                if (_reset.Contains(currentMouse.Position))
                    _board.MakeNewBoard();



                if (cell != null)
                {
                    _board.Reveal(cell[0], cell[1]);
                }

            }

            if(_previousMouse.RightButton == ButtonState.Pressed
                && currentMouse.RightButton == ButtonState.Released
                && currentMouse.LeftButton != ButtonState.Pressed
                && cell != null)
            {
                if (_board.GetCellState(cell[0], cell[1]) == CellState.Hidden)
                    _board.SetCellState(cell[0], cell[1], CellState.Flagged);
                else if(_board.GetCellState(cell[0], cell[1]) == CellState.Flagged)
                    _board.SetCellState(cell[0], cell[1], CellState.Hidden);
            }

            _previousMouse = currentMouse;
        }

        public int[] GetCellAtMousePos(Point mouse)
        {
            if (mouse.X < 50 || mouse.X > 50 + 20 * 30
                || mouse.Y < 65 || mouse.Y > 65 + 20 * 16)
                return null;

            int[] point = new int[2];
            point[0] = (int)((mouse.X - 50) / 20);
            point[1] = (int)((mouse.Y - 65) / 20);
            

            return point;
        }
    }
}
