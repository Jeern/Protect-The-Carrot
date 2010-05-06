using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PTC.Scenes
{
    public class SceneScheduler : GameComponent
    {
        public SceneScheduler(Game game) : base(game)
        {

        }

        private Dictionary<Scene, List<SceneChange>> m_SceneChanges = new Dictionary<Scene,List<SceneChange>>();

        public void AddSceneChange(SceneChange sceneChange)
        {
            if (CurrentScene == null)
            {
                CurrentScene = sceneChange.From;
            }
            if (m_SceneChanges.ContainsKey(sceneChange.From))
            {
                m_SceneChanges[sceneChange.From].Add(sceneChange);
            }
            else
            {
                m_SceneChanges.Add(sceneChange.From, new List<SceneChange>() { sceneChange });
            }
        }

        private Scene m_CurrentScene;
        public Scene CurrentScene
        {
            get
            {
                return m_CurrentScene;
            }
            set
            {
                m_CurrentScene.Disable();
                m_CurrentScene = value;
                m_CurrentScene.Enable();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_SceneChanges.ContainsKey(CurrentScene))
            {
                foreach (SceneChange sc in m_SceneChanges[CurrentScene])
                {
                    if (sc.ChangeNow(gameTime))
                    {
                        if (CurrentScene != null)
                        {
                            CurrentScene.OnExit();
                        }
                        CurrentScene = sc.To;
                        CurrentScene.Reset();
                        CurrentScene.OnEnter();
                    }
                }
            }
        }
    }
}
