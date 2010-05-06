using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace PTC.Input
{
    /// <summary>
    /// </summary>
    /// <typeparam name="S">The State (MouseState, KeyboardState, GamePadState)</typeparam>
    public class InputStateExtended<S> where S : struct
    {
        public InputStateExtended(GameTime gameTime, S state)
        {
            StateTime = gameTime.TotalRealTime;
            State = state;
        }
        public TimeSpan StateTime { get; set; }
        public S State { get; set; }
    }
}
