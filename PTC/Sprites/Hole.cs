using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.Utils;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class Hole : Sprite
    {
        public Hole(Game game, Vector2 startPos) : base(game, startPos)
        {
            //Scale = 0.13F;
            //TheColor = Color.GreenYellow;
        }
        protected override GameState GetImage()
        {
            switch (RealRandom.Create(0,3).Next())
            {
                case 0 :
                    return Game.Content.Load<Texture2D>(@"Hole\Hole");
                case 1:
                    return Game.Content.Load<Texture2D>(@"Hole\Hole1");
                default:
                    return Game.Content.Load<Texture2D>(@"Hole\Hole2");
            }
            
        }
    }
}
