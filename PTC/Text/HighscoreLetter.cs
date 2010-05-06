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
        private string m_VisibleValue = string.Empty;
        
        public string Value 
        {
            get { return m_Value; }
            set 
            { 
                m_Value = value;
                if (VisibleValue == string.Empty)
                {
                    VisibleValue = value;
                }
            }
        }

        public string VisibleValue
        {
            get { return m_VisibleValue; }
            set 
            { 
                m_VisibleValue = value;
                SetText(value);
            }
        }

        public float TouchDistance
        {
            get
            {
                return (m_CurrentFont.MeasureString(VisibleValue).Length() / 3);
            }
        }

    }
}
