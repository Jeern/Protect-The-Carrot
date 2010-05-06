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
using PTC.Sequencer;
using PTC.Commands;
using PTC.GraphicUtils;


namespace PTC.Sprites
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BarrelGun : MovingSprite
    {
        Texture2D barrelGunImage;

        private MouseState m_CurrentMouseState, m_OldMouseState;

        GameState gunState;
        

        public BarrelGun(Game game, Vector2 startPos) : base (game, startPos)
        {
            Scale = 0.8f;
            //DrawOrder = 1000;
            
            //notShooting = new GameState(new GameImage(Game.Content.Load<Texture2D>(@"Gun\barrel_fire_0")));
            
        }


        protected override GameState GetImage()
        {
            barrelGunImage = Game.Content.Load<Texture2D>("Gun/barrel_fire_4");

            gunState = new GameState(new List<GameImage>(){ 

            Game.Content.Load<Texture2D>("Gun/barrel_fire_0"),
                new GameImage(
                    new SequencedIterator<Texture2D>( 
                        new ForwardingSequencer(0,5),
                        
                Game.Content.Load<Texture2D>("Gun/barrel_fire_0"),
                Game.Content.Load<Texture2D>("Gun/barrel_fire_0"),
                Game.Content.Load<Texture2D>("Gun/barrel_fire_1"),
                Game.Content.Load<Texture2D>("Gun/barrel_fire_2"),
                Game.Content.Load<Texture2D>("Gun/barrel_fire_3"),
                Game.Content.Load<Texture2D>("Gun/barrel_fire_4")), 50) });
                //Game.Content.Load<Texture2D>("Gun/barrel");
            return gunState;
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

        protected override void LoadContent()
        {
            Game.Content.Load<Texture2D>("Content/Gun/barrel");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            m_CurrentMouseState = Mouse.GetState();


            Position = new Vector2((float)m_CurrentMouseState.X, ThisGame.Height - (int)(Scale * 200));

            if (m_CurrentMouseState.LeftButton == ButtonState.Pressed &&
                m_OldMouseState.LeftButton == ButtonState.Released)
            {
                ShootAnimation();
            }

            if ((gunState.State == (int)GunState.Shooting) && gunState.Current.CurrentTexture == barrelGunImage)
            {
                gunState.State = (int)GunState.NotShooting;
            }

            m_OldMouseState = m_CurrentMouseState;
     
        }

        private void ShootAnimation()
        {
            gunState.State = (int)GunState.Shooting;
            gunState.Current.Reset();
        }



        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);
        }
    }
}