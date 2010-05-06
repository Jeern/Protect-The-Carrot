using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PTC.GraphicUtils
{
    public class GameState : List<GameImage>
    {
        public GameState ()
	    {
            m_State = 0;
	    }

        public GameState(GameImage image)
        {

            Add(image);
            m_State = 0;
        }

        public GameState(IEnumerable<GameImage> image)
        {
            AddRange(image);
            m_State = 0;
        }


        private int m_State;
        public int State
        {
            get { return m_State; }
            set 
            { 
                if(value < 0 || value > Count)
                    throw new ArgumentOutOfRangeException("State", "State must be between 0 and " + Count.ToString());

                m_State = value;
            }
        }

        public GameImage Current
        {
            get { return this[m_State]; }
        }

        public static implicit operator GameState(Texture2D texture)
        {
            return new GameState(texture);
        }

        public static implicit operator Texture2D(GameState state)
        {
            return state.Current;
        }

        public void Update(GameTime time)
        {
            Current.Update(time);
        }

    }
}
