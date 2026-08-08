using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace inpsGE
{
    public class GameTile
    {
        Texture2D Image;
        float PositionX, PositionY;
        Rectangle Bounds;
        bool IsTileSolid;

        public GameTile(Texture2D Image, float PositionX, float PositionY, bool IsTileSolid)
        {
            this.Image = Image;
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            Bounds = new Rectangle((int)Math.Round(PositionX), (int)Math.Round(PositionY), Core.TILE_SIZE, Core.TILE_SIZE);
            this.IsTileSolid = IsTileSolid;
        }

        public Texture2D GetImage()
        {
            return Image;
        }

        public float GetPositionX()
        {
            return PositionX;
        }

        public float GetPositionY()
        {
            return PositionY;
        }

        public Rectangle GetBounds()
        {
            return Bounds;
        }

        public bool IsSolid()
        {
            return IsTileSolid;
        }
    }
}
