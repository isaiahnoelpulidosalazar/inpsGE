using Microsoft.Xna.Framework;
using System;

namespace inpsGE
{
    public class GameTimer
    {
        float Duration, TimeRemaining;
        Action Event;
        public bool IsActive { get; private set; }
        public bool AutoReset { get; set; }

        public GameTimer(float Duration, bool AutoReset = false)
        {
            this.Duration = Duration;
            TimeRemaining = Duration;
            this.AutoReset = AutoReset;
            IsActive = true;
        }

        public void SetEvent(Action Event)
        {
            this.Event = Event;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            float Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeRemaining -= Delta;

            if (TimeRemaining <= 0)
            {
                if (AutoReset)
                {
                    TimeRemaining += Duration;
                }
                else
                {
                    TimeRemaining = 0;
                    IsActive = false;
                }
                if (Event != null)
                {
                    Event?.Invoke();
                }
            }
        }

        public void Restart()
        {
            TimeRemaining = Duration;
            IsActive = true;
        }

        public void Stop()
        {
            IsActive = false;
        }
    }
}
