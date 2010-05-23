using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PTC.Input;
using Microsoft.Xna.Framework.Input;

namespace PTC.Utils
{
    /// <summary>
    /// Enables left mouse click in an area.
    /// </summary>
    public class ClickableArea : GameComponent
    {
        public ClickableArea(Game game)
            : base(game)
        {
        }

        public ClickableArea(Game game, Rectangle rectangle)
            : base(game)
        {
            SetArea(rectangle);
        }

        private Rectangle m_Rectangle;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if(MouseClickWithin(MouseExtended.Current.CurrentState))
            {
                if (MouseExtended.Current.WasSingleClick(MouseButton.Left))
                {
                    OnAreaClicked();
                }
            }
        }

        private bool MouseClickWithin(MouseState mouseState)
        {
            if (m_Rectangle.Left <= mouseState.X && mouseState.X <= m_Rectangle.Right
                && m_Rectangle.Top <= mouseState.Y && mouseState.Y <= m_Rectangle.Bottom)
            {
                return true;
            }
            return false;
        }

        private event EventHandler m_AreaClicked = delegate { };
        public event EventHandler AreaClicked
        {
            add { m_AreaClicked += value; }
            remove { m_AreaClicked += value; }
        }

        protected virtual void OnAreaClicked()
        {
            m_AreaClicked(this, new EventArgs());
        }

        public void SetArea(Rectangle rectangle)
        {
            m_Rectangle = rectangle;
        }

    }
}
