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



            if(_reset.Contains(currentMouse.Position)
                && _previousMouse.LeftButton == ButtonState.Pressed
                && currentMouse.LeftButton == ButtonState.Released)
            {
                _board.MakeNewBoard();
            }

            _previousMouse = currentMouse;
        }
    }
}
