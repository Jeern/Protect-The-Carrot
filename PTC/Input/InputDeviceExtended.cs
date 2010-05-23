using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Diagnostics;

namespace PTC.Input
{
    public class InputDeviceExtended<S> where S : struct
    {
        private Queue<InputStateExtended<S>> m_RecordedStates = new Queue<InputStateExtended<S>>();

        public Queue<InputStateExtended<S>> RecordedStates
        {
            get { return m_RecordedStates; }
        }

        private Stack<InputStateExtended<S>> m_StatesForReuse = new Stack<InputStateExtended<S>>();

        protected void EnqueueNewState(GameTime time, S state)
        {
            if (!state.Equals(m_CurrentState))
            {
                m_CurrentState = state;
                m_RecordedStates.Enqueue(CreateState(time, state));
            }
        }

        private S m_CurrentState;
        public S CurrentState
        {
            get { return m_CurrentState; }
        }

        protected void DequeueOldStates(GameTime currentTime)
        {
            InputStateExtended<S> state = null;
            if (m_RecordedStates.Count > 0)
            {
                state = m_RecordedStates.Peek();
            }
            if (state != null && state.StateTime < currentTime.TotalRealTime.Subtract(new TimeSpan(0, 0, 0, 0, InputDeviceConstants.ClickCountTimeMS)))
            {
                m_StatesForReuse.Push(m_RecordedStates.Dequeue());
                DequeueOldStates(currentTime);
            }
        }

        private InputStateExtended<S> CreateState(GameTime time, S state)
        {
            if (m_StatesForReuse.Count > 0)
            {
                //Reuses the object to fight of the GC
                InputStateExtended<S> stateExt = m_StatesForReuse.Pop();
                stateExt.StateTime = time.TotalRealTime;
                stateExt.State = state;
                return stateExt;
            }
            else
            {
                return new InputStateExtended<S>(time, state);
            }
        }

        /// <summary>
        /// Deletes all the states in the queue and adds them to the reuse stack.
        /// Used when a Click event or similar succeededs to stop another click event to occur immediately after.
        /// </summary>
        private void FlushAllStates()
        {
            while (m_RecordedStates.Count > 0)
            {
                m_StatesForReuse.Push(m_RecordedStates.Dequeue());
            }
        }

        public void Reset()
        {
            FlushAllStates();
        }
    }
}
