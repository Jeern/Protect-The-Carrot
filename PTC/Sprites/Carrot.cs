using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.Sequencer;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class Carrot : Sprite
    {
        public Carrot(Game game, Vector2 startPos) : base(game, startPos)
        {
            Scale = 0.26F;
        }

        protected override GameState GetImage()
        {
            return
                new GameState(
                    new GameImage(
                        new SequencedIterator<Texture2D>(new ForwardingSequencer(0, 4),
                           new List<Texture2D>()
                            {
                                Game.Content.Load<Texture2D>(@"Carrot\Carrot_Growing_1"),
                                Game.Content.Load<Texture2D>(@"Carrot\Carrot_Growing_1"),
                                Game.Content.Load<Texture2D>(@"Carrot\Carrot_Growing_2"),
                                Game.Content.Load<Texture2D>(@"Carrot\Carrot_Growing_3"),
                                Game.Content.Load<Texture2D>(@"Carrot\Carrot_Growing_4")
                                  }), 5000));

        }
    }
}
