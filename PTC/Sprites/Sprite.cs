using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public abstract class Sprite : DrawableGameComponent
    {
        private GameState m_Image;

        public Sprite(Game game, Vector2 startPos) : base(game)
        {
            Reset(startPos);
        }

        public virtual void Reset(Vector2 startPos)
        {
            Scale = 1F;
            if (m_Image == null)
            {
                m_Image = GetImage();
            }
            m_Position = startPos;
            TheColor = Color.White;
        }

        public GameState Image
        {
            get
            {
                return m_Image;
            }
        }

        protected abstract GameState GetImage();

        public Color TheColor 
        { get; set; }

        public override void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ThisGame.CurrentSpriteBatch.Draw(Image,
               Position, // + Middle,
               null,
               TheColor,
               0F,
               Middle,
               Scale,
               SpriteEffects.None,
               Layer); //Layer
        }

        public void BaseDraw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected PTCGame ThisGame
        {
            get
            {
                return Game as PTCGame;
            }
        }

        protected Vector2 m_Position;
        public virtual Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public float TouchDistance
        {
            get
            {
                return (new Vector2(Width / 3, Height / 3).Length()) * Scale;
            }
        }



        protected float m_Scale;
        public float Scale
        {
            get { return m_Scale * ZScale; }
            set { m_Scale = value; }
        }


        public Vector2 Middle
        {
            get { return new Vector2((float)Width / 2F, (float)Height / 2F); }
        }

        public float ZPosition
        {
            get { return m_Position.Y + (float)Height; } 
        }

        public float ZScale
        {
            get 
            {

                return ((1F - PTC.Utils.Environment.BackScale) / (ThisGame.Height + (float)Height)) * ZPosition + PTC.Utils.Environment.BackScale;
            }
        }

        public virtual float Layer
        {
            get { return 0; } // return Math.Min(ZScale, 1F); }
        }

        public float Height
        {
            get { return Image.Current.CurrentTexture.Height; }
        }

        public float Width
        {
            get { return Image.Current.CurrentTexture.Width; }
        }
    }
}
