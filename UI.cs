using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace inpsGE
{
    public abstract class UI
    {
        public abstract void UpdateUI();
        public abstract void DrawUI(ContentManager Content, SpriteBatch _spriteBatch);

        public abstract class View : UI
        {
            public int PositionX, PositionY, Width = -1, Height = -1;
            public Rectangle Bounds;

            public abstract void OverridePosition(int PositionX, int PositionY);
            public override void UpdateUI() { }
            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch) { }
        }

        public class Panel : UI
        {
            public enum Gravity
            {
                LEFT, RIGHT, CENTER
            }

            class LastElementInRow
            {
                public int Row, Index;

                public LastElementInRow(int Row, int Index)
                {
                    this.Row = Row;
                    this.Index = Index;
                }
            }

            Gravity PanelGravity;
            List<View> ElementList = new List<View>();
            int ElementX = 0, ElementY = 0, Row = 0;
            List<LastElementInRow> LastElements = new List<LastElementInRow>();

            public Panel()
            {
                SetGravity(Gravity.LEFT);
            }

            public void SetGravity(Gravity PanelGravity)
            {
                this.PanelGravity = PanelGravity;
                if (PanelGravity == Gravity.RIGHT)
                {
                    ElementX = Core.GetScreenWidth();
                }
                if (PanelGravity == Gravity.CENTER)
                {
                    ElementX = Core.GetScreenWidth() / 2;
                    ElementY = Core.GetScreenHeight() / 2;
                }
            }

            public void Add(View View)
            {
                if (PanelGravity == Gravity.LEFT)
                {
                    int CalculateX = ElementX + View.Bounds.Right;
                    int MaxX = Core.GetScreenWidth();

                    if (CalculateX < MaxX)
                    {
                        View.OverridePosition(ElementX, ElementY);
                        ElementX = View.Bounds.Right;
                    }
                    else
                    {
                        LastElements.Add(new LastElementInRow(Row, ElementList.Count - 1));
                        int LargestElementY = 0;
                        foreach (View ViewInElementList in ElementList)
                        {
                            if (ViewInElementList.Bounds.Bottom > LargestElementY)
                            {
                                LargestElementY = ViewInElementList.Bounds.Bottom;
                            }
                        }
                        ElementY = LargestElementY;
                        ElementX = 0;
                        View.OverridePosition(ElementX, ElementY);
                        ElementX = View.Bounds.Right;
                        Row++;
                    }
                }
                if (PanelGravity == Gravity.RIGHT)
                {
                    int CalculateX = ElementX - View.Bounds.Width;
                    int MaxX = 0;

                    if (CalculateX > MaxX)
                    {
                        ElementX -= View.Bounds.Width;
                        View.OverridePosition(ElementX, ElementY);
                    }
                    else
                    {
                        LastElements.Add(new LastElementInRow(Row, ElementList.Count - 1));
                        int LargestElementY = 0;
                        foreach (View ViewInElementList in ElementList)
                        {
                            if (ViewInElementList.Bounds.Bottom > LargestElementY)
                            {
                                LargestElementY = ViewInElementList.Bounds.Bottom;
                            }
                        }
                        ElementY = LargestElementY;
                        ElementX = Core.GetScreenWidth() - View.Bounds.Width;
                        View.OverridePosition(ElementX, ElementY);
                        Row++;
                    }
                }
                if (PanelGravity == Gravity.CENTER)
                {
                    int CalculateX = ElementX + View.Bounds.Right;
                    int MaxX = Core.GetScreenWidth();

                    if (ElementList.Count > 0)
                    {
                        View PreviousElementX = null;

                        for (int a = 0; a < ElementList.Count; a++)
                        {
                            if (a < ElementList.Count - 1)
                            {
                                View NextElementX = ElementList[a + 1];
                                ElementList[a].OverridePosition(ElementList[a].PositionX - (NextElementX.Width / 2), ElementY);
                            }
                            else
                            {
                                PreviousElementX = ElementList[a];
                                ElementList[a].OverridePosition(ElementList[a].PositionX - (View.Bounds.Width / 2), ElementY);
                            }
                        }
                        View LastElementInList = ElementList[ElementList.Count - 1];
                        CalculateX = LastElementInList.Bounds.Right + View.Bounds.Right;

                        if (CalculateX < MaxX)
                        {
                            LastElementInRow Last = null;
                            if (LastElements.Count > 0)
                            {
                                Last = LastElements[LastElements.Count - 1];
                                View.OverridePosition(PreviousElementX.Bounds.Right, ElementList[Last.Index].Bounds.Bottom);
                            }
                            else
                            {
                                View.OverridePosition(PreviousElementX.Bounds.Right, ElementY);
                            }
                        }
                        else
                        {
                            LastElements.Add(new LastElementInRow(Row, ElementList.Count - 1));
                            int LargestElementY = 0;
                            for (int a = 0; a < ElementList.Count; a++)
                            {
                                if (ElementList[a].Bounds.Height > LargestElementY)
                                {
                                    LargestElementY = ElementList[a].Bounds.Height;
                                }
                            }
                            ElementY -= LargestElementY / 2;
                            for (int a = 0; a < ElementList.Count; a++)
                            {
                                if (a < ElementList.Count - 1)
                                {
                                    View NextElementX = ElementList[a + 1];
                                    ElementList[a].OverridePosition(ElementList[a].PositionX + (NextElementX.Width / 2), ElementY);
                                }
                                else
                                {
                                    PreviousElementX = ElementList[a];
                                    ElementList[a].OverridePosition(ElementList[a].PositionX + (View.Bounds.Width / 2), ElementY);
                                }
                            }
                            View.OverridePosition(ElementX - (View.Bounds.Width / 2), ElementY);
                            Row++;
                        }
                    }
                    else
                    {
                        if (CalculateX < MaxX)
                        {
                            ElementY -= View.Bounds.Height / 2;
                            View.OverridePosition(ElementX - (View.Bounds.Width / 2), ElementY);
                        }
                    }
                }

                ElementList.Add(View);
            }

            public override void UpdateUI()
            {
                foreach (View View in ElementList)
                {
                    View.UpdateUI();
                }
            }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
                foreach (View View in ElementList)
                {
                    View.DrawUI(Content, _spriteBatch);
                }
            }
        }

        public class Button : View
        {
            Texture2D BorderTexture, BlackOverlay;
            Rectangle BlackOverlayBounds;
            string Title;
            Action Event;

            public Button(string Title)
            {
                BorderTexture = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BlackOverlay = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BorderTexture.SetData(new[] { Color.White });
                BlackOverlay.SetData(new[] { Color.Black });

                this.Title = Title;

                Bounds = new Rectangle(PositionX + 0, PositionY + 0, Core.TILE_SIZE, Core.TILE_SIZE);
                BlackOverlayBounds = new Rectangle(PositionX + 1, PositionY + 1, Core.TILE_SIZE - 2, Core.TILE_SIZE - 2);
            }

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

            public Button(string Title, int PositionX, int PositionY, int Width, int Height)
            {
                BorderTexture = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BlackOverlay = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BorderTexture.SetData(new[] { Color.White });
                BlackOverlay.SetData(new[] { Color.Black });

                this.Title = Title;
                this.PositionX = PositionX;
                this.PositionY = PositionY;
                this.Width = Width;
                this.Height = Height;

                Bounds = new Rectangle(PositionX + 0, PositionY + 0, Width, Height);
                BlackOverlayBounds = new Rectangle(PositionX + 1, PositionY + 1, Width - 2, Height - 2);
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

            public override void OverridePosition(int PositionX, int PositionY)
            {
                this.PositionX = PositionX;
                this.PositionY = PositionY;

                Bounds = new Rectangle(PositionX + 0, PositionY + 0, Width == -1 ? Core.TILE_SIZE : Width, Height == -1 ? Core.TILE_SIZE : Height);
                BlackOverlayBounds = new Rectangle(PositionX + 1, PositionY + 1, Width == -1 ? Core.TILE_SIZE - 2 : Width - 2, Height == -1 ? Core.TILE_SIZE - 2 : Height - 2);
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
