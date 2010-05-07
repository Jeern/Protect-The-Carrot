using System;

namespace PTC.Utils
{
    public class Resetable<X> where X : struct
    {
        private bool m_IsSet = false;

        public void Reset()
        {
            if (!m_IsSet)
                throw new ArgumentNullException("You can only reset the type if it was previously set");

            Value = m_OriginalValue;
        }

        public void Set()
        {
            if (!m_IsSet)
            {
                m_IsSet = true;
                m_OriginalValue = Value;
            }
        }

        private X m_OriginalValue = default(X);

        public X Value = default(X); 

        public static implicit operator X(Resetable<X> resetable)
        {
            return resetable.Value;
        }

        public static implicit operator Resetable<X>(X x)
        {
            var resetable = new Resetable<X>();
            resetable.Value = x;
            resetable.Set();
            return resetable;
        }

    }
}
