
namespace PTC.Sequencer
{
    /// <summary>
    /// Alternates sequence again and again. Don't use foreach
    /// </summary>
    public class AlternatingSequencer : Sequencer
    {
        private bool m_GoingForward = true;

        public AlternatingSequencer(int maxValue) : this(0, maxValue) {}

        public AlternatingSequencer(int minValue, int maxValue)
            : base(minValue, maxValue)
        {
        }

        public override bool MoveNext()
        {
            if (Current == MaxValue && m_GoingForward)
            {
                m_GoingForward = false;
            }
            else if (Current == MinValue && !m_GoingForward)
            {
                m_GoingForward = true;
            }

            if (Current < MaxValue && m_GoingForward)
            {
                Current++;
            }
            else if (Current > MinValue && !m_GoingForward)
            {
                Current--;
            }
            return true;
        }
    }
}
