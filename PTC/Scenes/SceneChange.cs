using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace PTC.Scenes
{
    public class SceneChange
    {
        public SceneChange(Scene from, Scene to, Func<GameTime, bool> changeNow)
        {
            m_From = from;
            m_To = to;
            m_ChangeNow =  changeNow;
        }

        private Scene m_From;
        public Scene From
        {
            get { return m_From; }
        }
        
        private Scene m_To;
        public Scene To
        {
            get { return m_To; }
        }
        
        private Func<GameTime, bool> m_ChangeNow;
        public Func<GameTime, bool> ChangeNow
        {
            get { return m_ChangeNow; }
        }
    }
}
