using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.GraphicUtils;
using PTC.Utils;

namespace PTC.Sprites
{
    public class Hole : Sprite
    {
        public Hole(Game game, Vector2 startPos) : base(game, startPos)
        {
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
