using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTC.Utils
{
    public class Direction
    {
        public Direction(bool up, bool down, bool left, bool right)
        {
            m_Up = up;
            m_Down = down;
            m_Left = left;
            m_Right = right;

            if (m_Up && m_Down)
            {
                m_Up = false;
                m_Down = false;
            }
            if (m_Left && m_Right)
            {
                m_Left = false;
                m_Right = false;
            }
        }

        private bool m_Up;
        private bool m_Down;
        private bool m_Left;
        private bool m_Right;
        public bool Up { get { return m_Up; } }
        public bool Down { get { return m_Down; } }
        public bool Left { get { return m_Left; } }
        public bool Right { get { return m_Right; } }


    }
}
