using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace inpsGE
{
    public class Entity
    {
        Texture2D Image;
        public float PositionX, PositionY, Speed;
        public Rectangle Bounds, InteractionArea;
        public bool Up, Down, Left, Right;
        public string CurrentRoomID;

        public void SetImage(Texture2D Image)
        {
            this.Image = Image;
        }

        public void SetImage(string FilePath)
        {
            Image = Texture2D.FromFile(Core.GetGraphicsDevice(), FilePath);
        }

        public Texture2D GetImage()
        {
            return Image;
        }

        public void CheckRoomID(Entity Entity, List<GameTile> Tiles)
        {
            foreach (GameTile Tile in Tiles)
            {
                if (Tile.GetBounds().Intersects(Entity.Bounds))
                {
                    CurrentRoomID = Tile.GetRoomID()[0];
                    break;
                }
            }
        }

        public void CheckTileCollision(Entity Entity, List<GameTile> Tiles)
        {
            foreach (GameTile Tile in Tiles)
            {
                Rectangle CalculateUp = new Rectangle(Entity.Bounds.X, (int)Math.Ceiling(Entity.Bounds.Y - Entity.Speed), Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateDown = new Rectangle(Entity.Bounds.X, (int)Math.Ceiling(Entity.Bounds.Y + Entity.Speed), Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateLeft = new Rectangle((int)Math.Ceiling(Entity.Bounds.X - Entity.Speed), Entity.Bounds.Y, Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateRight = new Rectangle((int)Math.Ceiling(Entity.Bounds.X + Entity.Speed), Entity.Bounds.Y, Entity.Bounds.Width, Entity.Bounds.Height);

                if (Tile.IsSolid())
                {
                    if (CalculateUp.Intersects(Tile.GetBounds()))
                    {
                        Up = false;
                    }
                    if (CalculateDown.Intersects(Tile.GetBounds()))
                    {
                        Down = false;
                    }
                    if (CalculateLeft.Intersects(Tile.GetBounds()))
                    {
                        Left = false;
                    }
                    if (CalculateRight.Intersects(Tile.GetBounds()))
                    {
                        Right = false;
                    }
                }
            }
        }

        public void CheckObjectCollision(Entity Entity, List<GameObject> Objects)
        {
            foreach (GameObject Object in Objects)
            {
                Rectangle CalculateUp = new Rectangle(Entity.Bounds.X, (int)Math.Ceiling(Entity.Bounds.Y - Entity.Speed), Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateDown = new Rectangle(Entity.Bounds.X, (int)Math.Ceiling(Entity.Bounds.Y + Entity.Speed), Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateLeft = new Rectangle((int)Math.Ceiling(Entity.Bounds.X - Entity.Speed), Entity.Bounds.Y, Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateRight = new Rectangle((int)Math.Ceiling(Entity.Bounds.X + Entity.Speed), Entity.Bounds.Y, Entity.Bounds.Width, Entity.Bounds.Height);

                if (Object.IsSolid())
                {
                    if (CalculateUp.Intersects(Object.GetBounds()))
                    {
                        Up = false;
                    }
                    if (CalculateDown.Intersects(Object.GetBounds()))
                    {
                        Down = false;
                    }
                    if (CalculateLeft.Intersects(Object.GetBounds()))
                    {
                        Left = false;
                    }
                    if (CalculateRight.Intersects(Object.GetBounds()))
                    {
                        Right = false;
                    }
                }
            }
        }

        public GameObject NearestGameObject(Entity Entity, List<GameObject> Objects)
        {
            foreach (GameObject Object in Objects)
            {
                Object.ResetPlayerProximity();
            }

            foreach (GameObject Object in Objects)
            {
                if (Entity.InteractionArea.Intersects(Object.GetBounds()))
                {
                    Object.PlayerIsNear();
                    return Object;
                }
            }
            return null;
        }
    }
}
