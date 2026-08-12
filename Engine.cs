using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace inpsGE
{
    public class Engine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        static Game Game;

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Game = this;
        }

        protected override void Initialize()
        {
            Core.Initialize(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameScenario TitleScreen = new TitleScreen("Sample Title");
            TitleScreen.SetName(Path.GetFileNameWithoutExtension("TitleScreen"));
            Core.AddScenario(TitleScreen);

            Core.ChangeGameScenario("TitleScreen");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            Input.Update();

            Core.GetCurrentGameScenario().Update(gameTime);
            Core.GetCurrentGameScenario().UpdateUI();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, Color.Black, 0, 0);

            _spriteBatch.Begin();

            Core.GetCurrentGameScenario().Draw(Content, _spriteBatch);

            if (LightingSystem.IsLightingSystemEnabled())
            {
                _spriteBatch.End();
                _spriteBatch.Begin();
                _spriteBatch.Draw(Core.GetLightMask(), Vector2.Zero, Color.White);
            }

            _spriteBatch.End();

            Core.GetCurrentGameScenario().DrawUI(Content, _spriteBatch);

            base.Draw(gameTime);
        }

        public static void Stop()
        {
            Game.Exit();
        }
    }
}
