
using System.Diagnostics;
namespace PTC.Sequencer
{
    public class ForwardingSequencer : Sequencer
    {
        public ForwardingSequencer(int maxValue) : this(0, maxValue) {}

        public ForwardingSequencer(int minValue, int maxValue) : base(minValue, maxValue)
        {
        }

        public override bool MoveNext()
        {
            if (Current < MaxValue)
            {

                Current++;
                return true;
            }
            return false;
        }
    }
}
