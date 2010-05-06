using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class Fence : Sprite
    {
        private const float FenceY = 550;

        public Fence(Game game, float xPos)
            : base(game, new Vector2(xPos, FenceY))
        {
            Scale = 1F;
            Position = new Vector2(xPos, ThisGame.Height - Middle.Y / 2); // - Image.Current.CurrentTexture.Height);
        }

        protected override GameState GetImage()
        {
            return Game.Content.Load<Texture2D>(@"Fence\Stakit");
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
               Scale,
               SpriteEffects.None,
               1F);

        }



    }
}
