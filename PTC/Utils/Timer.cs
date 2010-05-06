using System;
using Microsoft.Xna.Framework;
using PTC.Utils;

namespace PTC.Utils
{
    public class Timer : GameComponent
    {
        private event EventHandler<EventArgs<GameTime>> m_TimesUp = delegate { };

        public bool Repeat { get; set; }

        public float DelayInMilliseconds { get; set; }

        public event EventHandler<EventArgs<GameTime>> TimesUp
        {
            add { m_TimesUp += value; }
            remove { m_TimesUp -= value; }
        }

        private float m_millisecondsLeft;


        public Timer(Game game, int delay)
            : base(game)
        {
            DelayInMilliseconds = delay;
            m_millisecondsLeft = DelayInMilliseconds;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_millisecondsLeft -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_millisecondsLeft <= 0)
            {
                OnTimesUp(gameTime);
            }
        }

        public void Reset()
        {
            m_millisecondsLeft = DelayInMilliseconds;
        }

        protected void OnTimesUp(GameTime time)
        {
            m_TimesUp(this, new EventArgs<GameTime>(time));

            if (Repeat)
            {
                this.Enabled = true;
                Reset();
            }
        }
    }
}
