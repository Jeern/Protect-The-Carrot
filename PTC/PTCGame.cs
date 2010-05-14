using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PTC.Commands;
using PTC.Input;
using PTC.Scenes;
using PTC.Utils;

namespace PTC
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PTCGame : Game
    {
        GraphicsDeviceManager m_Graphics;
        SpriteBatch m_SpriteBatch;

        public SpriteBatch CurrentSpriteBatch
        {
            get
            {
                return m_SpriteBatch;
            }
        }

        public GraphicsDeviceManager GraphicsManager
        {
            get
            {
                return m_Graphics;
            }
        }


        public PTCGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_Graphics.PreferredBackBufferHeight = 768;
            m_Graphics.PreferredBackBufferWidth = 1024;
#if DEBUG
            m_Graphics.IsFullScreen = false;
#else
            m_Graphics.IsFullScreen = true;
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            CommandActions.Game = this;
            CommandConditions.Game = this;
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            var scheduler = new SceneScheduler(this);
            var mainScene = new MainScene(this);
            var gameOverScene = new GameOverScene(this);
            var welcomeScene = new WelcomeScene(this);
            var highScoreScene = new HighScoreScene(this);

            scheduler.AddSceneChange(new SceneChange(welcomeScene, mainScene, CommandConditions.ChangeFromWelcomeToMainScene));
            scheduler.AddSceneChange(new SceneChange(mainScene, highScoreScene, time => CommandConditions.GameOverCheatCode(time) || (mainScene.IsGameOver() && Highscores.IsNewHighscore(CurrentPoints))));
            scheduler.AddSceneChange(new SceneChange(mainScene, gameOverScene, time => mainScene.IsGameOver() && !Highscores.IsNewHighscore(CurrentPoints)));
            scheduler.AddSceneChange(new SceneChange(highScoreScene, gameOverScene, highScoreScene.Finished));
            scheduler.AddSceneChange(new SceneChange(gameOverScene, welcomeScene, CommandConditions.ChangeFromGameOverToWelcomeScene));    

            Components.Add(mainScene);
            Components.Add(gameOverScene);
            Components.Add(highScoreScene);
            Components.Add(welcomeScene);
            Components.Add(scheduler);
            Components.Add(new Command(CommandNames.Exit, CommandConditions.Exit, CommandActions.Exit, this));
            Components.Add(new Command(CommandNames.ToggleFullScreen, CommandConditions.ToggleFullScreen, CommandActions.ToggleFullScreen, this));
            Highscores = HighscoreList.Load();
            base.LoadContent();
            XACT.StartSoundScape();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Nødvendigt at kalde konstant i update.
            XACT.StartAmbientSound();

            //GetState kaldes her en gang for alle.
            KeyboardExtended.Current.GetState(gameTime);
            MouseExtended.Current.GetState(gameTime);

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //m_SpriteBatch.GraphicsDevice.ResolveBackBuffer =
            m_SpriteBatch.Begin(); //SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            base.Draw(gameTime);
            m_SpriteBatch.End();



        }

        public float Width
        {
            get
            {
                return (float)GraphicsDevice.Viewport.Width;
            }
        }

        public float Height
        {
            get
            {
                return (float)GraphicsDevice.Viewport.Height;
            }
        }

        public int CurrentPoints
        {
            get;
            set;
        }

        private HighscoreList m_Highscores = new HighscoreList();
        public HighscoreList Highscores
        {
            get { return m_Highscores; }
            set { m_Highscores = value; }
        }

    }
}
