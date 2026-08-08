using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace inpsGE
{
    public abstract class UI
    {
        public abstract void DrawUI(SpriteBatch _spriteBatch);

        public class Aim : UI
        {
            Texture2D AimArrowImage;
            Entity Entity;

            public Aim(Entity Entity)
            {
                AimArrowImage = Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Assets\\aim_arrow.png");
                this.Entity = Entity;
            }

            public override void DrawUI(SpriteBatch _spriteBatch)
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

            public override void DrawUI(SpriteBatch _spriteBatch)
            {
                _spriteBatch.Draw(HintArrowImage, new Vector2(Entity.PositionX + Camera.GetCameraPosition().X + (Core.TILE_SIZE / 2), Entity.PositionY + Camera.GetCameraPosition().Y + (Core.TILE_SIZE / 2)), null, Color.White, ((float)Math.Atan2(Object.GetBounds().Y - Entity.PositionY, Object.GetBounds().X - Entity.PositionX) + MathHelper.PiOver2), new Vector2(Core.TILE_SIZE / 2, Core.TILE_SIZE + (Core.TILE_SIZE / 2)), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
