using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTC.Sprites;

namespace PTC.Utils
{
    public class DeadController<T>
    {
        public T DeadObject { get; set; }
        public TimeSpan TimeOfDeath { get; set; }
    }
}
