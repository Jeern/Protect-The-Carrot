using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTC.Utils
{
    [Serializable]
    public class HighScore
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
