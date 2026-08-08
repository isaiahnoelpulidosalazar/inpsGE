using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace inpsGE
{
    public class GameObject
    {
        Texture2D Image;
        float PositionX, PositionY;
        Rectangle Bounds;
        bool IsObjectSolid;
        Action Event;
        int LightLevel = 0;
        bool IsPlayerNearMe = false;

        public GameObject(Texture2D Image, float PositionX, float PositionY, bool IsObjectSolid)
        {
            this.Image = Image;
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            Bounds = new Rectangle((int)Math.Round(PositionX), (int)Math.Round(PositionY), Core.TILE_SIZE, Core.TILE_SIZE);
            this.IsObjectSolid = IsObjectSolid;
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
            return IsObjectSolid;
        }

        public void SetEvent(Action Event)
        {
            this.Event = Event;
        }

        public void DoEvent()
        {
            Event?.Invoke();
        }

        public void SetLightLevel(int LightLevel)
        {
            this.LightLevel = LightLevel;
        }

        public int GetLightLevel()
        {
            return LightLevel;
        }

        public void ResetPlayerProximity()
        {
            IsPlayerNearMe = false;
        }

        public void PlayerIsNear()
        {
            IsPlayerNearMe = true;
        }

        public bool IsInteractionTooltipVisible()
        {
            return IsPlayerNearMe;
        }
    }
}
