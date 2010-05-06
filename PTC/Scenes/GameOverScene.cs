using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.Scenes;
using PTC.Sprites;
using PTC.Utils;
using PTC.Text;

namespace PTC.Scenes
{
    public class GameOverScene : Scene
    {
        private WelcomeBackground m_BackGround;
        private TextUtil m_TextUtil;
        private TextUtil m_TextUtilCredits;
        //private TextUtil m_TextUtilHighScores;

        public GameOverScene(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_BackGround.Draw(gameTime);
            m_TextUtil.Draw(gameTime);
            m_TextUtilCredits.Draw(gameTime);
            //m_TextUtilHighScores.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_BackGround = new WelcomeBackground(ThisGame, Vector2.Zero);
            AddComponent(m_BackGround);
            m_TextUtil = new TextUtil(ThisGame,FontLarge, Color.Violet, Color.Black, new Vector2(0, -200),
                HorizontalAlignment.Center, VerticalAlignment.Center,
                "Game Over", string.Format("You got {0} points", ThisGame.CurrentPoints.ToString()));
            AddComponent(m_TextUtil);
            m_TextUtilCredits = new TextUtil(ThisGame, 14, 30, new Vector2(0, -0.6F), false, true,
                FontMedium, Color.Gray, Color.Black, new Vector2(0, 150),
                HorizontalAlignment.Center,
                VerticalAlignment.Center, "Credits",
                "Mathias Flohr - Graphics", "Jakob Krarup - Code", "Ulrik Müller - Audio Design",
                "Jesper Niedermann - Code", "Lars Nysom - Producer", "Jakob Randa - Code");
            AddComponent(m_TextUtilCredits);
            //m_TextUtilHighScores = new TextUtil(ThisGame, 10, 16, new Vector2(0, -1F), true, false,
            //    FontLarge, Color.Violet, Color.Black, Vector2.Zero,
            //    HorizontalAlignment.Center,
            //    VerticalAlignment.Bottom,
            //    m_Highscores.GetScores().ToArray());
            //AddComponent(m_TextUtilHighScores);
        }

        public override void OnEnter()
        {
            //m_Highscores.Load();
            //m_Highscores.Add(new HighScore() { Score = ThisGame.CurrentPoints, Name = "XXX" });
            //m_Highscores.Save();
            m_TextUtil.SetText("Game Over", string.Format("You got {0} points", ThisGame.CurrentPoints.ToString()));
            m_TextUtilCredits.SetText(GetCredits().ToArray());
            //m_TextUtilHighScores.SetText(m_Highscores.GetScores().ToArray());
        }

        private List<string> GetCredits()
        {
            var credits = new List<string>();
            credits.Add("Highscores");
            credits.AddRange(ThisGame.Highscores.GetScores());
            credits.Add(string.Empty);
            credits.Add("Credits");
            credits.Add("Mathias Flohr - Graphics");
            credits.Add("Jakob Krarup - Code");
            credits.Add("Ulrik Müller - Audio Design");
            credits.Add("Jesper Niedermann - Code");
            credits.Add("Lars Nysom - Producer");
            credits.Add("Jakob Randa - Code");
            return credits;
        }

        public override void Reset()
        {
            m_TextUtilCredits.Reset();   
        }
        

        //public override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);
        //    m_TextUtil.SetText("Game Over", string.Format("You got {0} points", ThisGame.CurrentPoints.ToString()));
        //}

    }
}
