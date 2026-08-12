using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static inpsGE.Core;
using static inpsGE.UI;

namespace inpsGE
{
    public class TitleScreen : GameScenario
    {
        string Title;
        Panel Panel;
        Button PlayButton, QuitButton;

        public TitleScreen(string Title)
        {
            this.Title = Title;

            Panel = new Panel();
            Panel.SetGravity(Panel.Gravity.CENTER_CENTER);
            Panel.SetItemGap(8);

            PlayButton = new Button("Play", 0, 0, 120, 80);
            QuitButton = new Button("Quit", 0, 0, 120, 80);

            PlayButton.SetEvent(delegate
            {
                DirectoryInfo MapDirectoryInfo = new DirectoryInfo("Content\\Maps");
                DirectoryInfo ScenarioDirectoryInfo = new DirectoryInfo("Content\\Scenarios");

                foreach (FileInfo File in MapDirectoryInfo.GetFiles("*.igemap"))
                {
                    AddMap(new GameMap(Path.GetFileNameWithoutExtension(File.Name)));
                }

                foreach (FileInfo File in ScenarioDirectoryInfo.GetFiles("*.cs"))
                {
                    Assembly CompiledAssembly = Compiler.Run("Content\\Scenarios\\" + File.Name);
                    Type ScenarioType = CompiledAssembly.GetTypes().FirstOrDefault(Type => typeof(GameScenario).IsAssignableFrom(Type) && !Type.IsAbstract && Type.IsClass);

                    if (ScenarioType != null)
                    {
                        GameScenario Scenario = (GameScenario)Activator.CreateInstance(ScenarioType);
                        Scenario.SetName(Path.GetFileNameWithoutExtension(File.Name));
                        AddScenario(Scenario);
                    }
                }

                ChangeGameScenario("MainMenu");
            });
            QuitButton.SetEvent(static delegate
            {
                Engine.Stop();
            });

            Panel.Add(PlayButton);
            Panel.Add(QuitButton);
        }

        public override void Load()
        {
            AddToUIDrawList(Panel);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont_Title"), Title, new Vector2(GetScreenWidth() / 2 - Content.Load<SpriteFont>("DefaultFont_Title").MeasureString(Title).X / 2, GetScreenHeight() / 4), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
    }
}
