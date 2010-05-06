using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PTC.Utils;
using PTC.Commands;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class Crosshair : Sprite
    {
        private MouseState m_CurrentMouseState, m_OldMouseState;

        public Crosshair(Game game, Vector2 startPos) : base(game, startPos)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_CurrentMouseState = Mouse.GetState();
            Position = new Vector2((float)m_CurrentMouseState.X, (float)m_CurrentMouseState.Y);

            if (m_CurrentMouseState.LeftButton == ButtonState.Pressed &&
                m_OldMouseState.LeftButton == ButtonState.Released)
            {
                OnGunFired(gameTime);
            }

            m_OldMouseState = m_CurrentMouseState;
        }

        public override void Draw(GameTime gameTime)
        {
            base.BaseDraw(gameTime);
            ThisGame.CurrentSpriteBatch.Draw(Image,
               Position, // + Middle,
               null,
               TheColor,
               0F,
               Middle,
               m_Scale,
               SpriteEffects.None,
               1F);

            
        }

        protected override GameState GetImage()
        {
            return Game.Content.Load<Texture2D>(@"Crosshair\Crosshair");
        }

        private event EventHandler<EventArgs<GameTime>> m_GunFired = delegate { };

        public event EventHandler<EventArgs<GameTime>> GunFired
        {
            add { m_GunFired += value; }
            remove { m_GunFired -= value; }
        }

        public virtual void OnGunFired(GameTime time)
        {
            m_GunFired(this, new EventArgs<GameTime>(time));
        }

    }
}
