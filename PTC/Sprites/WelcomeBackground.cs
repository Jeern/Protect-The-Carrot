using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class WelcomeBackground : Sprite
    {
        public WelcomeBackground(Game game, Vector2 startPos)
            : base(game, startPos)
        {
        }

        protected override GameState GetImage()
        {
            return
                Game.Content.Load<Texture2D>("Backgrounds/Screen");
        }

        public override void Draw(GameTime gameTime)
        {
            base.BaseDraw(gameTime);
            ThisGame.CurrentSpriteBatch.Draw
               (
               Image,
               new Rectangle(0, 0, (int)ThisGame.Width, (int)ThisGame.Height),
               null,
               Color.White, 0F, Vector2.Zero, SpriteEffects.None, 1F);
        }

    }
}
