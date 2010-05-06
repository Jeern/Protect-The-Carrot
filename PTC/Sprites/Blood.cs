using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PTC.Utils;
using PTC.GraphicUtils;


namespace PTC.Sprites
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Blood : Sprite
    {
        public Blood(Game game, Vector2 startPos)
            : base(game, startPos)
        {
            Reset(startPos);


            // TODO: Construct any child components here
        }

        public override void Reset(Vector2 startPos)
        {
            base.Reset(startPos);
            Scale = 0.8f;
        }



        protected override GameState GetImage()
        {
            //barrelGunImage = Game.Content.Load<Texture2D>("Gun/barrel_fire_4");

            //gunState = new GameState(new List<GameImage>(){ 
            int temp_en = RealRandom.Create(0, 7).Next();

            switch (temp_en)
            {
                case 0:
                return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod1")));
                    
                    
                case 1:
                return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod2")));
                    

                case 2:
                return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod3")));
                    

                case 3:
                return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod4")));
                    

                case 4:
                return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod5")));
                    

                case 5:
                return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod6")));
                    
                    
                case 6:
                return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod7")));
                    default:
                    return new GameState(
                    new GameImage(ThisGame.Content.Load<Texture2D>("Blood/Blod7")));


                    
            }
        
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}