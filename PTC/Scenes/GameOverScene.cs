using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.Sprites;
using PTC.Text;
using PTC.Utils;

namespace PTC.Scenes
{
    public class GameOverScene : Scene
    {
        private WelcomeBackground m_BackGround;
        private TextUtil m_Title;
        private TextUtil m_Credits;

        public GameOverScene(Game game)
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_BackGround.Draw(gameTime);
            m_Title.Draw(gameTime);
            m_Credits.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_BackGround = new WelcomeBackground(ThisGame, Vector2.Zero);
            AddComponent(m_BackGround);
            m_Title = new TextUtil(ThisGame, Vector2.Zero, FontLarge, Color.Violet, Color.Black, new Vector2(0, -200),
                HorizontalAlignment.Center, VerticalAlignment.Center);
            m_Title.SetText("Game Over", string.Format("You got {0} points", ThisGame.CurrentPoints.ToString()));
            AddComponent(m_Title);
            //TODO: Viewbox
            m_Credits = new TextUtil(ThisGame, new Rectangle(0, 290, 1024, 460), new Vector2(0, -0.6F), FontMedium, 
                Color.Gray, Color.Black, new Vector2(0, 780), HorizontalAlignment.Center, VerticalAlignment.Center);
            AddComponent(m_Credits);
        }

        public override void OnEnter()
        {
            m_Title.SetText("Game Over", string.Format("You got {0} points", ThisGame.CurrentPoints.ToString()));
            m_Credits.SetText(GetCredits().ToArray());
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
            m_Credits.Reset();   
        }
    }
}
