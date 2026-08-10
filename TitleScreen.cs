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
        Button Button1;

        public TitleScreen(string Title)
        {
            this.Title = Title;
            Button1 = new Button("Test", (GetScreenWidth() / 2) - (TILE_SIZE / 2), (GetScreenHeight() / 2) - (TILE_SIZE / 2));
            Button1.SetEvent(delegate
            {
                ChangeGameScenario("MainMenu");
            });
        }

        public override void Load()
        {
            AddToUIDrawList(Button1);
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
