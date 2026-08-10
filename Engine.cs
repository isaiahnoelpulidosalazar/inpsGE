using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace inpsGE
{
    public class Engine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Core.Initialize(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DirectoryInfo MapDirectoryInfo = new DirectoryInfo("Content\\Maps");
            DirectoryInfo ScenarioDirectoryInfo = new DirectoryInfo("Content\\Scenarios");

            foreach (FileInfo File in MapDirectoryInfo.GetFiles("*.igemap"))
            {
                Core.AddMap(new GameMap(Path.GetFileNameWithoutExtension(File.Name)));
            }

            GameScenario TitleScreen = new TitleScreen("Sample Title");
            TitleScreen.SetName(Path.GetFileNameWithoutExtension("TitleScreen"));
            Core.AddScenario(TitleScreen);

            foreach (FileInfo File in ScenarioDirectoryInfo.GetFiles("*.cs"))
            {
                Assembly CompiledAssembly = Compiler.Run("Content\\Scenarios\\" + File.Name);
                Type ScenarioType = CompiledAssembly.GetTypes().FirstOrDefault(Type => typeof(GameScenario).IsAssignableFrom(Type) && !Type.IsAbstract && Type.IsClass);

                if (ScenarioType != null)
                {
                    GameScenario Scenario = (GameScenario)Activator.CreateInstance(ScenarioType);
                    Scenario.SetName(Path.GetFileNameWithoutExtension(File.Name));
                    Core.AddScenario(Scenario);
                }
            }

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
    }
}
