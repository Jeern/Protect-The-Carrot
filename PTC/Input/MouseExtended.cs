using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace PTC.Input
{
    public class MouseExtended : InputDeviceExtended<MouseState>
    {
        private static MouseExtended m_Current;
        public static MouseExtended Current
        {
            get
            {
                if (m_Current == null)
                {
                    m_Current = new MouseExtended();
                }
                return m_Current;
            }
        }

        public MouseState GetState(GameTime currentTime)
        {
            DequeueOldStates(currentTime);
            MouseState state = Mouse.GetState();
            EnqueueNewState(currentTime, state);
            return state;
        }

        private bool ClickCount(MouseButton checkButton, int requiredCount)
        {
            ButtonState found = ButtonState.Released;
            int count = 0;
            foreach (InputStateExtended<MouseState> stateExt in RecordedStates)
            {
                if (found == ButtonState.Pressed && 
                    ButtonStateToCheck(stateExt.State, checkButton) == ButtonState.Released)
                {
                    count++;
                    if (count >= requiredCount)
                        return true;
                }
                found = ButtonStateToCheck(stateExt.State, checkButton);
            }
            return false;
        }

        private ButtonState ButtonStateToCheck(MouseState state, MouseButton checkButton)
        {
            switch (checkButton)
            {
                case MouseButton.Left:
                    return state.LeftButton;
                case MouseButton.Middle:
                    return state.MiddleButton;
                case MouseButton.Right:
                    return state.RightButton;
                case MouseButton.XButton1:
                    return state.XButton1;
                case MouseButton.XButton2:
                    return state.XButton2;
                default:
                    return state.LeftButton;
            }
        }

        public bool WasSingleClick(MouseButton checkButton)
        {
            bool clicked = ClickCount(checkButton, 1);
            if (clicked)
            {
                FlushAllStates();
            }
            return clicked;
        }

        public bool WasDoubleClick(MouseButton checkButton)
        {
            bool clicked = ClickCount(checkButton, 2);
            if (clicked)
            {
                FlushAllStates();
            }
            return clicked;
        }

    }
}
