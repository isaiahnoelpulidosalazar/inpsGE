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
    public class GameScenarioManager
    {
        List<GameScenario> Scenarios = new List<GameScenario>();
        static GameScenario CurrentGameScenario;

        public void AddScenario(GameScenario Scenario)
        {
            Scenarios.Add(Scenario);
        }

        public void ChangeScenario(string Name)
        {
            CurrentGameScenario = Scenarios.Find(Scenario => Scenario.GetName() == Name);
        }

        public void Update(GameTime gameTime)
        {
            CurrentGameScenario.Update(gameTime);
        }

        public void Draw(ContentManager Content, SpriteBatch _spriteBatch)
        {
            CurrentGameScenario.Draw(Content, _spriteBatch);
        }
    }
}
