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
                LEFT_TOP, LEFT_CENTER, LEFT_BOTTOM,
                CENTER_TOP, CENTER_CENTER, CENTER_BOTTOM,
                RIGHT_TOP, RIGHT_CENTER, RIGHT_BOTTOM
            }

            Gravity PanelGravity;
            int ItemGap;
            List<View> ElementList = new List<View>();

            public Panel(Gravity PanelGravity = Gravity.LEFT_TOP, int ItemGap = 0)
            {
                this.ItemGap = ItemGap;
                this.PanelGravity = PanelGravity;
            }

            public Panel(int ItemGap) : this(Gravity.LEFT_TOP, ItemGap) { }

            public void SetGravity(Gravity PanelGravity)
            {
                this.PanelGravity = PanelGravity;
                RecalculateLayout();
            }

            public void SetItemGap(int ItemGap)
            {
                this.ItemGap = ItemGap;
                RecalculateLayout();
            }

            public void Add(View view)
            {
                ElementList.Add(view);
                RecalculateLayout();
            }

            public void Remove(int Index)
            {
                ElementList.RemoveAt(Index);
                RecalculateLayout();
            }

            public void Remove(View View)
            {
                ElementList.Remove(View);
                RecalculateLayout();
            }

            public void Clear()
            {
                ElementList.Clear();
                RecalculateLayout();
            }

            public void RecalculateLayout()
            {
                if (ElementList.Count == 0)
                {
                    return;
                }

                List<List<View>> Rows = new List<List<View>>();
                List<View> CurrentRow = new List<View>();
                int CurrentRowWidth = 0;

                foreach (View View in ElementList)
                {
                    int ViewWidth = View.Bounds.Width;
                    if (CurrentRow.Count > 0 && CurrentRowWidth + ItemGap + ViewWidth > Core.GetScreenWidth())
                    {
                        Rows.Add(CurrentRow);
                        CurrentRow = new List<View>();
                        CurrentRowWidth = 0;
                    }

                    if (CurrentRow.Count > 0) CurrentRowWidth += ItemGap;
                    CurrentRow.Add(View);
                    CurrentRowWidth += ViewWidth;
                }
                if (CurrentRow.Count > 0) Rows.Add(CurrentRow);

                int TotalHeight = 0;
                List<int> RowWidths = new List<int>();
                List<int> RowHeights = new List<int>();

                for (int a = 0; a < Rows.Count; a++)
                {
                    int RowWidth = 0;
                    int RowHeight = 0;
                    for (int b = 0; b < Rows[a].Count; b++)
                    {
                        var View = Rows[a][b];
                        RowWidth += View.Bounds.Width;
                        if (b > 0)
                        {
                            RowWidth += ItemGap;
                        }
                        if (View.Bounds.Height > RowHeight)
                        {
                            RowHeight = View.Bounds.Height;
                        }
                    }
                    RowWidths.Add(RowWidth);
                    RowHeights.Add(RowHeight);

                    TotalHeight += RowHeight;
                    if (a > 0) TotalHeight += ItemGap;
                }

                int StartY = 0;
                if (PanelGravity == Gravity.LEFT_CENTER || PanelGravity == Gravity.CENTER_CENTER || PanelGravity == Gravity.RIGHT_CENTER)
                {
                    StartY = (Core.GetScreenHeight() - TotalHeight) / 2;
                }
                else if (PanelGravity == Gravity.LEFT_BOTTOM || PanelGravity == Gravity.CENTER_BOTTOM || PanelGravity == Gravity.RIGHT_BOTTOM)
                {
                    StartY = Core.GetScreenHeight() - TotalHeight;
                }

                int CurrentY = StartY;
                for (int a = 0; a < Rows.Count; a++)
                {
                    int RowWidth = RowWidths[a];
                    int RowHeight = RowHeights[a];

                    int CurrentX = 0;
                    if (PanelGravity == Gravity.CENTER_TOP || PanelGravity == Gravity.CENTER_CENTER || PanelGravity == Gravity.CENTER_BOTTOM)
                    {
                        CurrentX = (Core.GetScreenWidth() - RowWidth) / 2;
                    }
                    else if (PanelGravity == Gravity.RIGHT_TOP || PanelGravity == Gravity.RIGHT_CENTER || PanelGravity == Gravity.RIGHT_BOTTOM)
                    {
                        CurrentX = Core.GetScreenWidth() - RowWidth;
                    }

                    foreach (var View in Rows[a])
                    {
                        int ViewY = CurrentY + (RowHeight - View.Bounds.Height) / 2;
                        View.OverridePosition(CurrentX, ViewY);
                        CurrentX += View.Bounds.Width + ItemGap;
                    }

                    CurrentY += RowHeight + ItemGap;
                }
            }

            public override void UpdateUI()
            {
                for (int a = 0; a < ElementList.Count; a++)
                {
                    ElementList[a].UpdateUI();
                }
            }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
                for (int a = 0; a < ElementList.Count; a++)
                {
                    ElementList[a].DrawUI(Content, _spriteBatch);
                }
            }
        }

        public class Button : View
        {
            Texture2D BorderTexture, BlackOverlay;
            Rectangle BlackOverlayBounds;
            Color TextColor;
            string Title;
            Action Event;

            public Button(string Title)
            {
                BorderTexture = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BlackOverlay = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BorderTexture.SetData(new[] { Color.White });
                BlackOverlay.SetData(new[] { Color.Black });
                TextColor = Color.White;

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
                TextColor = Color.White;

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
                TextColor = Color.White;

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
                        TextColor = Color.Gray;
                    }
                    else
                    {
                        BorderTexture.SetData(new[] { Color.White });
                        TextColor = Color.White;
                    }
                    if (Input.MouseUp)
                    {
                        DoEvent();
                    }
                }
                else
                {
                    BorderTexture.SetData(new[] { Color.White });
                    TextColor = Color.White;
                }
            }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
                _spriteBatch.Draw(BorderTexture, Bounds, Color.White);
                _spriteBatch.Draw(BlackOverlay, BlackOverlayBounds, Color.White);
                _spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont_Text"), Title, new Vector2((Bounds.Width / 2) - (Content.Load<SpriteFont>("DefaultFont_Text").MeasureString(Title).X / 2) + PositionX, (Bounds.Height / 2) - (Content.Load<SpriteFont>("DefaultFont_Text").MeasureString(Title).Y / 2) + PositionY), TextColor, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }
        }

        public class ProgressBar : View
        {
            public int StepCounter = 1;
            public float Progress = 0f;

            Texture2D BorderTexture, BackgroundTexture, FillTexture;
            Rectangle BackgroundBounds, FillBounds;
            string Title = "";

            public ProgressBar(string Title = "")
            {
                InitTextures();
                this.Title = Title;
                Bounds = new Rectangle(PositionX, PositionY, Core.TILE_SIZE * 2, Core.TILE_SIZE / 2);
                UpdateLayout();
            }

            public ProgressBar(int PositionX, int PositionY, string Title = "")
            {
                InitTextures();
                this.PositionX = PositionX;
                this.PositionY = PositionY;
                this.Title = Title;
                Bounds = new Rectangle(PositionX, PositionY, Core.TILE_SIZE * 2, Core.TILE_SIZE / 2);
                UpdateLayout();
            }

            public ProgressBar(int PositionX, int PositionY, int Width, int Height, string Title = "")
            {
                InitTextures();
                this.PositionX = PositionX;
                this.PositionY = PositionY;
                this.Width = Width;
                this.Height = Height;
                this.Title = Title;
                Bounds = new Rectangle(PositionX, PositionY, Width, Height);
                UpdateLayout();
            }

            void InitTextures()
            {
                BorderTexture = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                BackgroundTexture = new Texture2D(Core.GetGraphicsDevice(), 1, 1);
                FillTexture = new Texture2D(Core.GetGraphicsDevice(), 1, 1);

                BorderTexture.SetData(new[] { Color.White });
                BackgroundTexture.SetData(new[] { Color.Black });
                FillTexture.SetData(new[] { Color.Green });
            }

            void UpdateLayout()
            {
                int w = Width == -1 ? Core.TILE_SIZE * 2 : Width;
                int h = Height == -1 ? Core.TILE_SIZE / 2 : Height;
                Bounds = new Rectangle(PositionX, PositionY, w, h);
                BackgroundBounds = new Rectangle(PositionX + 1, PositionY + 1, Math.Max(0, w - 2), Math.Max(0, h - 2));
                UpdateFillBounds();
            }

            void UpdateFillBounds()
            {
                int fillWidth = (int)(BackgroundBounds.Width * MathHelper.Clamp(Progress, 0f, 1f));
                FillBounds = new Rectangle(BackgroundBounds.X, BackgroundBounds.Y, fillWidth, BackgroundBounds.Height);
            }

            public void SetTitle(string Title)
            {
                this.Title = Title;
            }

            public void SetProgress(float Progress)
            {
                this.Progress = MathHelper.Clamp(Progress, 0f, 1f);
                UpdateFillBounds();
            }

            public void StepForward()
            {
                SetProgress(Progress + (StepCounter / 100f));
            }

            public void StepForward(int StepCounter)
            {
                this.StepCounter = StepCounter;
                StepForward();
            }

            public void StepBackward()
            {
                SetProgress(Progress - (StepCounter / 100f));
            }

            public void StepBackward(int StepCounter)
            {
                this.StepCounter = StepCounter;
                StepBackward();
            }

            public override void OverridePosition(int PositionX, int PositionY)
            {
                this.PositionX = PositionX;
                this.PositionY = PositionY;
                UpdateLayout();
            }

            public override void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
            {
                if (!string.IsNullOrEmpty(Title))
                {
                    SpriteFont font = Content.Load<SpriteFont>("DefaultFont_Text");
                    Vector2 textSize = font.MeasureString(Title);
                    _spriteBatch.DrawString(font, Title, new Vector2((Bounds.Width / 2) - (textSize.X / 2) + PositionX, PositionY - textSize.Y - 2), Color.White);
                }

                _spriteBatch.Draw(BorderTexture, Bounds, Color.White);
                _spriteBatch.Draw(BackgroundTexture, BackgroundBounds, Color.White);
                _spriteBatch.Draw(FillTexture, FillBounds, Color.White);
            }
        }

        public class Aim : UI
        {
            Texture2D AimArrowImage;
            Entity Entity;

            public Aim(Entity Entity)
            {
                AimArrowImage = Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Assets\\aim_arrow_x32.png");
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
                HintArrowImage = Texture2D.FromFile(Core.GetGraphicsDevice(), "Content\\Assets\\hint_arrow_x32.png");
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
