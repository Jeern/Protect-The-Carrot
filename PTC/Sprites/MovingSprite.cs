using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using PTC.Utils;

namespace PTC.Sprites
{
    public abstract class MovingSprite : Sprite
    {
        public Vector2 CurrentSpeed
        {
            get;
            set;
        }

        public MovingSprite(Game game, Vector2 startPos) : base(game, startPos)   { }

        public override void Reset(Vector2 startPos)
        {
            base.Reset(startPos);
            CurrentSpeed = Vector2.Zero;
            m_Direction = Vector2.Zero;
        }

        private Vector2 m_Direction;

        public void PointTowards(Vector2 target)
        {
            m_Direction = target - this.Position;
            m_Direction.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsPaused)
            {
                CurrentSpeed = Vector2.Zero;
            }
            else
            {
                CurrentSpeed = m_Direction;
            }
            m_Position = m_Position + CurrentSpeed * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 17F);
        }

        public virtual bool IsPaused
        {
            get
            {
                return false;
            }
        }

      }
}
