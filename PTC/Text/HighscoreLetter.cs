using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PTC.Utils;

namespace PTC.Text
{
    public class HighscoreLetter : TextUtil
    {
        private SpriteFont m_CurrentFont;

        public HighscoreLetter(PTCGame game, SpriteFont font, Vector2 offset)
            : base(game, font, Color.BlueViolet, Color.Black, offset,
            HorizontalAlignment.Left, VerticalAlignment.Top, string.Empty)
        {
            m_CurrentFont = font;
        }

        private string m_Value = string.Empty;
        
        public string Value 
        {
            get { return m_Value; }
            set 
            { 
                m_Value = value;
                SetText(m_Value);
            }
        }

        public float TouchDistance
        {
            get
            {
                return (m_CurrentFont.MeasureString(Value).Length() / 3);
            }
        }

        public Vector2 Position
        {
            get { return PositionTopLeft + (m_CurrentFont.MeasureString(Value) / 2); }
        }

    }
}
