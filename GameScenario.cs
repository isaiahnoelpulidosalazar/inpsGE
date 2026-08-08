using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace inpsGE
{
    public abstract class GameScenario
    {
        string Name;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(ContentManager Content, SpriteBatch _spriteBatch);
        public void SetName(string Name)
        {
            var EngineCheck = new StackFrame(1).GetMethod()?.DeclaringType;

            if (EngineCheck != typeof(Engine))
            {
                throw new InvalidOperationException("The GameScenario.SetName() method can only be called from the Engine class.");
            }

            this.Name = Name;
        }
        public string GetName()
        {
            return Name;
        }
    }
}
