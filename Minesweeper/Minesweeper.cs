using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.GameEntities;
using Minesweeper.System;

namespace Minesweeper
{
    public class Minesweeper : Game
    {
        
        public const int WINDOW_HEIGHT = 200;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;

        private Board _board;
        private InputManager _inputManager;

        private Texture2D _test;
        

        public Minesweeper()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            
            _graphics.PreferredBackBufferHeight = 450;
            _graphics.PreferredBackBufferWidth = 700;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Cells");

            _board = new Board(font);

            _inputManager = new InputManager(_board);

            _test = new Texture2D(GraphicsDevice, 1, 1);
            _test.SetData(new Color[] { Color.Blue });

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _inputManager.ProcessControls(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();


            _board.Draw(_spriteBatch, font);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
