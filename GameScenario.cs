using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace inpsGE
{
    public abstract class GameScenario
    {
        string Name;
        List<UI> UIs = new List<UI>();

        public abstract void Load();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(ContentManager Content, SpriteBatch _spriteBatch);

        public void UpdateUI()
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine))
            {
                throw new InvalidOperationException("The GameScenario.UpdateUI() method can only be called from the Engine class.");
            }

            foreach (UI UI in UIs)
            {
                UI.UpdateUI();
            }
        }

        public void DrawUI(ContentManager Content, SpriteBatch _spriteBatch)
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine))
            {
                throw new InvalidOperationException("The GameScenario.DrawUI() method can only be called from the Engine class.");
            }

            UI CheckForAimUI = null;

            foreach (UI UI in UIs)
            {
                if (UI.GetType() == typeof(UI.Aim))
                {
                    CheckForAimUI = UI;
                }
                else
                {
                    _spriteBatch.Begin();

                    UI.DrawUI(Content, _spriteBatch);

                    _spriteBatch.End();
                }
            }

            if (CheckForAimUI != null)
            {
                _spriteBatch.Begin();

                CheckForAimUI.DrawUI(Content, _spriteBatch);

                _spriteBatch.End();
            }
        }

        public void AddToUIDrawList(UI UI)
        {
            UIs.Add(UI);
        }

        public void RemoveFromUIDrawList(UI UI)
        {
            UIs.Remove(UI);
        }

        public void SetName(string Name)
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine) && EngineCheck?.DeclaringType != typeof(TitleScreen))
            {
                throw new InvalidOperationException("The GameScenario.SetName() method can only be called from the Engine and TitleScreen class.");
            }

            this.Name = Name;
        }

        public string GetName()
        {
            return Name;
        }
    }
}
