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

        private SpriteFont _font;
        private Texture2D _spriteSheet;

        private Board _board;
        private InputManager _inputManager;
        private HighScoreManager _highScoreManager;

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
            
            _graphics.PreferredBackBufferHeight = 390;
            _graphics.PreferredBackBufferWidth = 650;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _font = Content.Load<SpriteFont>("Cells");
            _spriteSheet = Content.Load<Texture2D>("mines");

            _board = new Board(_font);

            _inputManager = new InputManager(_board);

            _highScoreManager = new HighScoreManager();

            //_board.TakeScore(_highScoreManager.Load());

            var loadScore = _highScoreManager.Load();

            _test = new Texture2D(GraphicsDevice, 1, 1);
            _test.SetData(new Color[] { Color.Blue });

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                && _board.SaveTasks.Count == 0)
                Exit();

            _board.Update(gameTime);

            if(IsActive)
                _inputManager.ProcessControls(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();


            _board.Draw(_spriteBatch, _spriteSheet);

            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
