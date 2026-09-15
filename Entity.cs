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
        public bool Up, Down, Left, Right, OnStairs;
        public string CurrentRoomID;
        public int CurrentFloor;
        public GameObject StairsOn = null;

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

        public void CheckRoomAndFloor(Entity Entity, List<GameTile> Tiles, List<GameObject> Objects)
        {
            foreach (GameTile Tile in Tiles)
            {
                if (Entity.CurrentFloor == Tile.GetFloor() && Entity.Bounds.Intersects(Tile.GetBounds()))
                {
                    CurrentRoomID = Tile.GetRoomID()[0];
                    break;
                }
            }

            foreach (GameObject Object in Objects)
            {
                if (Entity.CurrentFloor == Object.GetFloor() && Object.GetBounds().Contains(Entity.Bounds))
                {
                    if (Object.GetName().Contains("stairs") && Object.GetName().Contains("up"))
                    {
                        if (!OnStairs)
                        {
                            OnStairs = true;
                            CurrentFloor = Object.GetFloor() + 1;
                            StairsOn = Object;
                        }
                    }
                    if (Object.GetName().Contains("stairs") && Object.GetName().Contains("down"))
                    {
                        if (!OnStairs)
                        {
                            OnStairs = true;
                            CurrentFloor = Object.GetFloor() - 1;
                            StairsOn = Object;
                        }
                    }
                }
            }

            if (StairsOn != null)
            {
                if (!StairsOn.GetBounds().Contains(Entity.Bounds))
                {
                    OnStairs = false;
                    StairsOn = null;
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

                if (Entity.CurrentFloor == Tile.GetFloor() && Tile.GetRoomID().Contains(Entity.CurrentRoomID))
                {
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
        }

        public void CheckObjectCollision(Entity Entity, List<GameObject> Objects)
        {
            foreach (GameObject Object in Objects)
            {
                Rectangle CalculateUp = new Rectangle(Entity.Bounds.X, (int)Math.Ceiling(Entity.Bounds.Y - Entity.Speed), Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateDown = new Rectangle(Entity.Bounds.X, (int)Math.Ceiling(Entity.Bounds.Y + Entity.Speed), Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateLeft = new Rectangle((int)Math.Ceiling(Entity.Bounds.X - Entity.Speed), Entity.Bounds.Y, Entity.Bounds.Width, Entity.Bounds.Height);
                Rectangle CalculateRight = new Rectangle((int)Math.Ceiling(Entity.Bounds.X + Entity.Speed), Entity.Bounds.Y, Entity.Bounds.Width, Entity.Bounds.Height);

                if (Entity.CurrentFloor == Object.GetFloor() && Object.GetRoomID().Contains(Entity.CurrentRoomID))
                {
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
        }

        public GameObject NearestGameObject(Entity Entity, List<GameObject> Objects)
        {
            foreach (GameObject Object in Objects)
            {
                Object.ResetPlayerProximity();
            }

            foreach (GameObject Object in Objects)
            {
                if (Entity.CurrentFloor == Object.GetFloor() && Entity.InteractionArea.Intersects(Object.GetBounds()))
                {
                    Object.PlayerIsNear();
                    return Object;
                }
            }
            return null;
        }
    }
}
