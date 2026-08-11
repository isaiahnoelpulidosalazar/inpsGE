using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static inpsGE.Core;
using static inpsGE.UI;

namespace inpsGE
{
    public class TitleScreen : GameScenario
    {
        string Title;
        Panel Panel;

        public TitleScreen(string Title)
        {
            this.Title = Title;
            Panel = new Panel();
            //Button1.SetEvent(delegate
            //{
            //    ChangeGameScenario("MainMenu");
            //});
            Panel.SetGravity(Panel.Gravity.CENTER);
            for (int a = 0; a < 12; a++)
            {
                Panel.Add(new Button("Test", 0, 0, 120, 120));
            }
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
