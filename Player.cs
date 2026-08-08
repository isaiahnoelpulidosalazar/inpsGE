using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace inpsGE
{
    public class Player : Entity
    {
        GameObject NearestGameObjectToPlayer;

        public void SetPosition(float PositionX, float PositionY)
        {
            this.PositionX = PositionX;
            this.PositionY = PositionY;
        }

        public void SetSpeed(float Speed)
        {
            this.Speed = Speed;
        }

        public void Update()
        {
            Up = Input.Up;
            Down = Input.Down;
            Left = Input.Left;
            Right = Input.Right;

            bool UL = Up && Left;
            bool UR = Up && Right;
            bool DL = Down && Left;
            bool DR = Down && Right;

            CheckTileCollision(this, Core.GetCurrentGameMap().GetTiles());
            CheckObjectCollision(this, Core.GetCurrentGameMap().GetObjects());
            NearestGameObjectToPlayer = NearestGameObject(this, Core.GetCurrentGameMap().GetObjects());

            if (Input.Interact)
            {
                if (NearestGameObjectToPlayer != null)
                {
                    NearestGameObjectToPlayer.DoEvent();
                }
            }

            if (UL || UR || DL || DR)
            {
                float NormalizedSpeed = Speed * 0.75f;

                if (Up)
                {
                    PositionY -= NormalizedSpeed;
                }
                if (Down)
                {
                    PositionY += NormalizedSpeed;
                }
                if (Left)
                {
                    PositionX -= NormalizedSpeed;
                }
                if (Right)
                {
                    PositionX += NormalizedSpeed;
                }
            }
            else
            {
                if (Up)
                {
                    PositionY -= Speed;
                }
                if (Down)
                {
                    PositionY += Speed;
                }
                if (Left)
                {
                    PositionX -= Speed;
                }
                if (Right)
                {
                    PositionX += Speed;
                }
            }

            if (!Up && !Down && !Left && !Right)
            {
                PositionX = (float)Math.Round(PositionX);
                PositionY = (float)Math.Round(PositionY);
            }

            Bounds = new Rectangle((int)PositionX + 8, (int)PositionY + 32, 32, 16);
            InteractionArea = new Rectangle((int)PositionX - 4, (int)PositionY - 4, Core.TILE_SIZE + 8, Core.TILE_SIZE + 8);
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(GetImage(), new Vector2(PositionX, PositionY), Color.White);

            if (NearestGameObject(this, Core.GetCurrentGameMap().GetObjects()) != null)
            {
                GameObject NearestObject = NearestGameObject(this, Core.GetCurrentGameMap().GetObjects());
                _spriteBatch.Draw(Core.GetMessageBackground(), new Rectangle((int)Math.Round(NearestObject.GetBounds().X + (Core.TILE_SIZE / 2) - (Content.Load<SpriteFont>("DefaultFont_Text").MeasureString("[E] to interact").X / 2)), NearestObject.GetBounds().Y - (Core.TILE_SIZE / 2), (int)Math.Round(Content.Load<SpriteFont>("DefaultFont_Text").MeasureString("[E] to interact").X) + 3, (int)Math.Round(Content.Load<SpriteFont>("DefaultFont_Text").MeasureString("[E] to interact").Y)), Color.White);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont_Text"), "[E] to interact", new Vector2(NearestObject.GetBounds().X + (Core.TILE_SIZE / 2) + 3 - (Content.Load<SpriteFont>("DefaultFont_Text").MeasureString("[E] to interact").X / 2), NearestObject.GetBounds().Y - (Core.TILE_SIZE / 2)), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
