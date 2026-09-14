using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace inpsGE
{
    public class GameTile
    {
        Texture2D Image;
        float PositionX, PositionY;
        Rectangle Bounds;
        List<int> RoomID;
        bool IsTileSolid;

        public GameTile(Texture2D Image, float PositionX, float PositionY, bool IsTileSolid, List<int> RoomID)
        {
            this.Image = Image;
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            Bounds = new Rectangle((int)Math.Round(PositionX), (int)Math.Round(PositionY), Core.TILE_SIZE, Core.TILE_SIZE);
            this.IsTileSolid = IsTileSolid;
            this.RoomID = RoomID;
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

        public List<int> GetRoomID()
        {
            return RoomID;
        }

        public bool IsSolid()
        {
            return IsTileSolid;
        }
    }
}
