using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PTC.Sequencer;
using Microsoft.Xna.Framework.Graphics;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class MatingRabbits : MovingSprite
    {
        public MatingRabbits(Game game, Vector2 startPos, TimeSpan startTime)
            : base(game, startPos)
        {
            Scale = 2.5F;
            m_StartTime = startTime;
        }

        private MinMaxIterator m_DelayIterator = new MinMaxIterator(new RandomSequencer(0, 300), 100, 400);

        protected override GameState GetImage()
        {
            return 
                new GameState(
                    new GameImage(
                        new SequencedIterator<Texture2D>(new RepeatingSequencer(0, 1),
                       new List<Texture2D>()
                    {
                        Game.Content.Load<Texture2D>(@"MatingRabbits\Mating1"),
                        Game.Content.Load<Texture2D>(@"MatingRabbits\Mating2")
                      }), 
                      m_DelayIterator));
        }

        private TimeSpan m_StartTime;

        public TimeSpan StartTime
        {
            get { return m_StartTime; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_StartTime < gameTime.TotalGameTime.Subtract(new TimeSpan(0, 0, 0, 0, 3000)))
            {
                OnMatingOver();
                m_StartTime = TimeSpan.MaxValue;
            }
        }

        private event EventHandler<EventArgs> m_MatingOver = delegate { };

        public event EventHandler<EventArgs> MatingOver
        {
            add { m_MatingOver += value; }
            remove { m_MatingOver += value; }
        }

        private void OnMatingOver()
        {
            m_MatingOver(this, EventArgs.Empty);
        }


    }
}
