using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace PTC.Input
{
    public class KeyboardExtended : InputDeviceExtended<KeyboardState>
    {
        private bool m_HasPlayerIndex = false;
        private PlayerIndex m_PlayerIndex = PlayerIndex.One;

        public KeyboardExtended()
        {

        }

        public KeyboardExtended(PlayerIndex playerIndex) : this()
        {
            m_HasPlayerIndex = true;
            m_PlayerIndex = playerIndex;
        }

        private static KeyboardExtended m_Current;
        private static Dictionary<PlayerIndex, KeyboardExtended> m_CurrentDictionary;

        public static KeyboardExtended Current
        {
            get
            {
                if (m_Current == null)
                {
                    m_Current = new KeyboardExtended();
                }
                return m_Current;
            }
        }

        public static KeyboardExtended CurrentIndexed(PlayerIndex index)
        {
            if (m_CurrentDictionary == null)
            {
                m_CurrentDictionary = new Dictionary<PlayerIndex, KeyboardExtended>(4);
            }

            if (!m_CurrentDictionary.ContainsKey(index))
            {
                m_CurrentDictionary.Add(index, new KeyboardExtended(index));
            }
            return m_CurrentDictionary[index];
        }

        public KeyboardState GetState(GameTime currentTime)
        {
            DequeueOldStates(currentTime);
            KeyboardState state;
            if (m_HasPlayerIndex)
            {
                state = Keyboard.GetState(m_PlayerIndex);
            }
            else
            {
                state = Keyboard.GetState();
            }
            EnqueueNewState(currentTime, state);
            return state;
        }

        private bool ClickCount(Keys checkKey, int requiredCount)
        {
            bool keyWasDown = false;
            int count = 0;
            foreach (InputStateExtended<KeyboardState> stateExt in RecordedStates)
            {
                if (keyWasDown && stateExt.State.IsKeyUp(checkKey))
                {
                    count++;
                    if (count >= requiredCount)
                        return true;
                }
                keyWasDown = stateExt.State.IsKeyDown(checkKey);
            }
            return false;
        }

        public bool WasSingleClick(Keys checkKey)
        {
            return ClickCount(checkKey, 1);
        }

        public bool WasDoubleClick(Keys checkKey)
        {
            return ClickCount(checkKey, 2);
        }
    }
}
