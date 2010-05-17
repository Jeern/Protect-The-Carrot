using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.Sprites;
using PTC.Text;
using PTC.Utils;

namespace PTC.Scenes
{
    public class WelcomeScene : Scene
    {
        private WelcomeBackground m_BackGround;
        private TextUtil m_NameOfGame;
        private TextUtil m_WhatToDo;
        private TextUtil m_Details;

        public WelcomeScene(Game game)
            : base(game)
        {
        }
         
        public override void Draw(GameTime gameTime)
        {
            m_NameOfGame.Draw(gameTime);
            m_WhatToDo.Draw(gameTime);
            m_Details.Draw(gameTime);
        }


        protected override void LoadContent()
        {
            base.LoadContent();
            m_BackGround = new WelcomeBackground(ThisGame, Vector2.Zero);
            AddComponent(m_BackGround);
            m_NameOfGame = new TextUtil(ThisGame, Vector2.Zero, FontLarge, Color.DarkBlue, Color.Black, new Vector2(0, -90),
                HorizontalAlignment.Center,
                VerticalAlignment.Center);
            m_NameOfGame.SetText("Protect The Carrot");
            m_WhatToDo = new TextUtil(ThisGame, Vector2.Zero, FontMedium, Color.Red, Color.Black, Vector2.Zero,
                HorizontalAlignment.Center,
                VerticalAlignment.Center);
            m_WhatToDo.SetText("Press mouse to play");
            m_Details = new TextUtil(ThisGame, Vector2.Zero, FontMedium, Color.Gray, Color.Black, new Vector2(0, 135),
                HorizontalAlignment.Center,
                VerticalAlignment.Center);
            m_Details.SetText("For God's sake protect the carrot", "Disintegrate the dead rabbits", "for extra points");
            AddComponent(m_NameOfGame);
            AddComponent(m_WhatToDo);
            AddComponent(m_Details);
        }
    }
}
