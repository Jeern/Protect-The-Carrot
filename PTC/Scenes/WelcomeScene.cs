using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PTC.Sprites;
using PTC.Utils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PTC.Text;

namespace PTC.Scenes
{
    public class WelcomeScene : Scene
    {
        private WelcomeBackground m_BackGround;
        private TextUtil m_TextUtil1;
        private TextUtil m_TextUtil2;
        private TextUtil m_TextUtil3;
        //private TextUtil m_TextUtilScrolling;

        public WelcomeScene(Game game)
            : base(game)
        {
        }
         
        public override void Draw(GameTime gameTime)
        {
            m_TextUtil1.Draw(gameTime);
            m_TextUtil2.Draw(gameTime);
            m_TextUtil3.Draw(gameTime);
//            m_TextUtilScrolling.Draw(gameTime);
        }


        protected override void LoadContent()
        {
            base.LoadContent();
            m_BackGround = new WelcomeBackground(ThisGame, Vector2.Zero);
            AddComponent(m_BackGround);
            m_TextUtil1 = new TextUtil(ThisGame, FontLarge, Color.DarkBlue, Color.Black, new Vector2(0, -90),
                HorizontalAlignment.Center,
                VerticalAlignment.Center,
                "Protect The Carrot");
            m_TextUtil2 = new TextUtil(ThisGame, FontMedium, Color.Red, Color.Black, Vector2.Zero,
                HorizontalAlignment.Center,
                VerticalAlignment.Center,
                "Press mouse to play");
            m_TextUtil3 = new TextUtil(ThisGame, FontMedium, Color.Gray, Color.Black, new Vector2(0, 135),
                HorizontalAlignment.Center,
                VerticalAlignment.Center,
                "For God's sake protect your carrot", "Disintegrate the dead rabbits", "for extra points");
            //m_TextUtilScrolling = new TextUtil(ThisGame, new Rectangle(50, 50, 250, 280), new Vector2(-1F, 0.0F), true,
            //    FontMedium, Color.Black, Vector2.Zero, HorizontalAlignment.Left, VerticalAlignment.Top, "Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo Bastardo ", "Bastardo", "Bastardo", "bastardo", "Bastardo");
            AddComponent(m_TextUtil1);
            AddComponent(m_TextUtil2);
            AddComponent(m_TextUtil3);
            //AddComponent(m_TextUtilScrolling);
        }

    }
}
