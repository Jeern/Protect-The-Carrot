using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

namespace PTC.Scenes
{
    public class Scene : DrawableGameComponent
    {
        public Scene(Game game): base(game)
        {
            SceneExtensions.Disable(this);
            Initialize();
        }


        protected PTCGame ThisGame
        {
            get
            {
                return Game as PTCGame;
            }
        }

        private SpriteFont m_FontLarge;
        private SpriteFont m_FontMedium;
        private SpriteFont m_FontSmall;

        protected SpriteFont FontLarge
        {
            get { return m_FontLarge; }
        }

        protected SpriteFont FontMedium
        {
            get { return m_FontMedium; }
        }

        protected SpriteFont FontSmall
        {
            get { return m_FontSmall; }
        }

        private List<GameComponent> m_Components = new List<GameComponent>();
        public List<GameComponent> Components
        {
            get
            {
                return m_Components;
            }
        }

        protected void AddComponent(GameComponent component)
        {
            AddComponentNoUpdate(component);
            Game.Components.Add(component);
        }

        protected void AddComponent(DrawableGameComponent component)
        {
            AddComponentNoUpdate(component);
            Game.Components.Add(component);
        }

        protected void AddComponentNoUpdate(GameComponent component)
        {
            component.Enabled = Enabled;
            Components.Add(component);
        }

        protected void AddComponentNoUpdate(DrawableGameComponent component)
        {
            component.Enabled = Enabled;
            component.Visible = Visible;
            Components.Add(component);
        }

        protected void RemoveComponent(GameComponent component)
        {
            Components.Remove(component);
            Game.Components.Remove(component);
        }

        protected void RemoveComponentNoUpdate(GameComponent component)
        {
            Components.Remove(component);
        }

        protected override void LoadContent()
        {
            m_FontLarge = Game.Content.Load<SpriteFont>(@"Fonts\Large");
            m_FontMedium = Game.Content.Load<SpriteFont>(@"Fonts\Medium");
            m_FontSmall = Game.Content.Load<SpriteFont>(@"Fonts\Small");
            base.LoadContent();
        }

        public virtual void Reset()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}
