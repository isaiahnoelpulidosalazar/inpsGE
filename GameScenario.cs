using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inpsGE
{
    public abstract class GameScenario
    {
        public string Name;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(ContentManager Content, SpriteBatch _spriteBatch);
        public string GetName()
        {
            return Name;
        }
    }
}
