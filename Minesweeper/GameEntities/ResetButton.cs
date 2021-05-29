using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.GameEntities
{

    class ResetButton
    {
        private bool _isPressed;
        public ResetButton()
        {
            _isPressed = false;
        }

        public void Press()
        {
            _isPressed = true;
        }

        public void Release()
        {
            _isPressed = false;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rect, Texture2D spriteSheet, GameState State)
        {
            if (_isPressed == true)
                spriteBatch.Draw(spriteSheet, rect, new Rectangle(232, 0, 23, 23), Color.White);
            else
            {
                if (State == GameState.Running)
                    spriteBatch.Draw(spriteSheet, rect, new Rectangle(232, 96, 23, 23), Color.White);
                else if (State == GameState.GameOver)
                    spriteBatch.Draw(spriteSheet, rect, new Rectangle(232 + 24, 96, 23, 23), Color.White);
                else if (State == GameState.Victory)
                    spriteBatch.Draw(spriteSheet, rect, new Rectangle(232, 24, 23, 23), Color.White);
            }
                
        }
    }
}
