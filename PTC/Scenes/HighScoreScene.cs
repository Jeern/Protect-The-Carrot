using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.Sprites;
using PTC.Text;
using PTC.Utils;

namespace PTC.Scenes
{
    public class HighScoreScene : Scene
    {
        private WelcomeBackground m_BackGround;
        private TextUtil m_Title;
        private TextUtil m_ChosenText;
        private const string m_ChosenTextStart =  "________________________";
        private string m_CurrentText = string.Empty;

        private Crosshair m_Crosshair;

        private bool m_Finished = false;

        private List<HighscoreLetter> m_Letters = new List<HighscoreLetter>(30);

        public HighScoreScene(Game game)
            : base(game)
        {

        }

        public override void OnEnter()
        {
            m_Title.SetText("New Highscore", string.Format("You got {0} points", ThisGame.CurrentPoints.ToString()));
            m_CurrentText = ThisGame.Highscores.LatestHighscoreHolder;
        }

        public override void OnExit()
        {
            ThisGame.Highscores.Add(new HighScore() { Score = ThisGame.CurrentPoints, Name = m_CurrentText });
            ThisGame.Highscores.Save();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_BackGround = new WelcomeBackground(ThisGame, Vector2.Zero);
            AddComponent(m_BackGround);
            m_Title = new TextUtil(ThisGame, Vector2.Zero, FontLarge, Color.Violet, Color.Black, new Vector2(0, -200),
                HorizontalAlignment.Center, VerticalAlignment.Center);
            m_Title.SetText("New Highscore", string.Format("You got {0} points", ThisGame.CurrentPoints.ToString()));
            AddComponent(m_Title);
            m_ChosenText = new TextUtil(ThisGame, Vector2.Zero, FontMedium, Color.Black, Color.Blue, new Vector2(0, -70),
                HorizontalAlignment.Center, VerticalAlignment.Center);
            AddComponent(m_ChosenText);
            MakeLetterMesh(new Vector2(200, 350));
            m_Crosshair = new Crosshair(ThisGame, new Vector2(50, 50));
            AddComponent(m_Crosshair);
            m_Crosshair.GunFired += CrosshairGunFired;

        }

        private void MakeLetterMesh(Vector2 offset)
        {
            const float XSpacing = 60F;
            const float YSpacing = 70F;
            SpriteFont font = FontMedium;

            //Vector2 letterSize = font.MeasureString("X");

            Dictionary<string, Vector2> letters = new Dictionary<string, Vector2>();
            letters.Add("A", new Vector2(0,0));
            letters.Add("B", new Vector2(1,0));
            letters.Add("C", new Vector2(2,0));
            letters.Add("D", new Vector2(3,0));
            letters.Add("E", new Vector2(4,0));
            letters.Add("F", new Vector2(5,0));
            letters.Add("G", new Vector2(6,0));
            letters.Add("H", new Vector2(7,0));
            letters.Add("I", new Vector2(8,0));
            letters.Add("J", new Vector2(9,0));
            letters.Add("K", new Vector2(0,1));
            letters.Add("L", new Vector2(1,1));
            letters.Add("M", new Vector2(2,1));
            letters.Add("N", new Vector2(3,1));
            letters.Add("O", new Vector2(4,1));
            letters.Add("P", new Vector2(5,1));
            letters.Add("Q", new Vector2(6,1));
            letters.Add("R", new Vector2(7,1));
            letters.Add("S", new Vector2(8,1));
            letters.Add("T", new Vector2(9,1));
            letters.Add("U", new Vector2(0,2));
            letters.Add("V", new Vector2(1,2));
            letters.Add("W", new Vector2(2,2));
            letters.Add("X", new Vector2(3,2));
            letters.Add("Y", new Vector2(4,2));
            letters.Add("Z", new Vector2(5,2));
            letters.Add("SPACE", new Vector2(6,2));
            letters.Add("DEL", new Vector2(0,3));
            letters.Add("CLEAR", new Vector2(2,3));
            letters.Add("ENTER", new Vector2(5, 3));

            //int x = 0;


            foreach(var letter in letters)
            {
                Vector2 currentVector = offset + letter.Value * new Vector2(XSpacing, YSpacing);
                var highscoreLetter = new HighscoreLetter(ThisGame, font, currentVector);
                highscoreLetter.Value = letter.Key;
                m_Letters.Add(highscoreLetter);
            }
        }



        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_BackGround.Draw(gameTime);
            m_Title.Draw(gameTime);
            m_ChosenText.Draw(gameTime);
            foreach (var letter in m_Letters)
            {
                letter.Draw(gameTime);
            }
            DrawCrossHair(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_ChosenText.SetText(ChosenText);
        }

        public void DrawCrossHair(GameTime time)
        {
            if (m_Crosshair.Visible)
            {
                m_Crosshair.Draw(time);
            }
        }

        private void CrosshairGunFired(object sender, EventArgs<GameTime> e)
        {
            foreach (HighscoreLetter letter in m_Letters)
            {
                if ((m_Crosshair.Position - letter.Position).Length() < (letter.TouchDistance + m_Crosshair.TouchDistance))
                {
                    //Aha a letter is hit.
                    ChangeText(letter);
                    return;
                }
            }
        }

        public bool Finished(GameTime time)
        {
            return m_Finished;
        }

        private void ChangeText(HighscoreLetter letter)
        {
            switch (letter.Value)
            {
                case "DEL":
                    if (m_CurrentText.Length > 0)
                    {
                        m_CurrentText = m_CurrentText.Substring(0, m_CurrentText.Length - 1);
                    }
                    break;
                case "CLEAR":
                    m_CurrentText = string.Empty;
                    break;
                case "ENTER":
                    m_Finished = true;
                    break;
                case "SPACE":
                    m_CurrentText += " ";
                    break;
                default:
                    m_CurrentText += letter.Value;
                    break;
            }
        }

        private string ChosenText
        {
            get { return (m_CurrentText + m_ChosenTextStart).Substring(0, m_ChosenTextStart.Length); }
        }

        public override void Reset()
        {
            m_Finished = false;
        }
    }
}
