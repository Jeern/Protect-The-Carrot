using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class Grass : Sprite
    {
        public static Color grassColor = Color.GreenYellow;

        public Grass(Game game, Vector2 startPos)
            : base(game, startPos)
        {
            Scale = 0.06F;
            
            //TheColor = Color.GreenYellow;
        }

        protected override GameState GetImage()
        {
            return Game.Content.Load<Texture2D>(@"Grass\Grass");
        }

        public override void Draw(GameTime gameTime)
        {
            base.BaseDraw(gameTime);
            ThisGame.CurrentSpriteBatch.Draw(Image,
               Position, // + Middle,
               null,
               grassColor,
               0F,
               Middle,
               new Vector2(Scale * 1.5F, Scale),
               SpriteEffects.None,
               Layer);

        }
    }
}
