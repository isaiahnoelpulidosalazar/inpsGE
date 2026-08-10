using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace inpsGE
{
    public abstract class UI
    {
        public abstract void UpdateUI();
        public abstract void DrawUI(ContentManager Content, SpriteBatch _spriteBatch);

        public class Panel : UI
        {
            public Panel()
            {
            }

            public override void UpdateUI() { }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
            }
        }

        public class Button : UI
        {
            Texture2D BorderTexture, BlackOverlay;
            Rectangle Bounds, BlackOverlayBounds;
            string Title;
            int PositionX, PositionY;
            Action Event;

            public Button(string Title, int PositionX, int PositionY)
            {
                BorderTexture = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BlackOverlay = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BorderTexture.SetData(new[] { Color.White });
                BlackOverlay.SetData(new[] { Color.Black });

                this.Title = Title;
                this.PositionX = PositionX;
                this.PositionY = PositionY;

                Bounds = new Rectangle(PositionX + 0, PositionY + 0, Core.TILE_SIZE, Core.TILE_SIZE);
                BlackOverlayBounds = new Rectangle(PositionX + 1, PositionY + 1, Core.TILE_SIZE - 2, Core.TILE_SIZE - 2);
            }

            bool MouseIsInside()
            {
                if (Input.MouseX > Bounds.Left
                    && Input.MouseX < Bounds.Right
                    && Input.MouseY > Bounds.Top
                    && Input.MouseY < Bounds.Bottom)
                {
                    return true;
                }
                return false;
            }

            public void SetEvent(Action Event)
            {
                this.Event = Event;
            }

            public void DoEvent()
            {
                Event?.Invoke();
            }

            public override void UpdateUI()
            {
                if (MouseIsInside())
                {
                    if (Input.MouseDown)
                    {
                        BorderTexture.SetData(new[] { Color.Gray });
                    }
                    else
                    {
                        BorderTexture.SetData(new[] { Color.White });
                    }
                    if (Input.MouseUp)
                    {
                        DoEvent();
                    }
                }
                else
                {
                    BorderTexture.SetData(new[] { Color.White });
                }
            }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
                _spriteBatch.Draw(BorderTexture, Bounds, Color.White);
                _spriteBatch.Draw(BlackOverlay, BlackOverlayBounds, Color.White);
            }
        }

        public class Aim : UI
        {
            Texture2D AimArrowImage;
            Entity Entity;

            public Aim(Entity Entity)
            {
                AimArrowImage = Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Assets\\aim_arrow.png");
                this.Entity = Entity;
            }

            public override void UpdateUI() { }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
                _spriteBatch.Draw(AimArrowImage, new Vector2(Entity.PositionX + Camera.GetCameraPosition().X + (Core.TILE_SIZE / 2), Entity.PositionY + Camera.GetCameraPosition().Y + (Core.TILE_SIZE / 2)), null, Color.White, Input.Theta, new Vector2(Core.TILE_SIZE / 2, Core.TILE_SIZE + (Core.TILE_SIZE / 2)), 1f, SpriteEffects.None, 0f);
            }
        }

        public class Hint : UI
        {
            Texture2D HintArrowImage;
            Entity Entity;
            GameObject Object;

            public Hint(Entity Entity, GameObject Object)
            {
                HintArrowImage = Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Assets\\hint_arrow.png");
                this.Entity = Entity;
                this.Object = Object;
            }

            public override void UpdateUI() { }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
                _spriteBatch.Draw(HintArrowImage, new Vector2(Entity.PositionX + Camera.GetCameraPosition().X + (Core.TILE_SIZE / 2), Entity.PositionY + Camera.GetCameraPosition().Y + (Core.TILE_SIZE / 2)), null, Color.White, ((float)Math.Atan2(Object.GetBounds().Y - Entity.PositionY, Object.GetBounds().X - Entity.PositionX) + MathHelper.PiOver2), new Vector2(Core.TILE_SIZE / 2, Core.TILE_SIZE + (Core.TILE_SIZE / 2)), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
