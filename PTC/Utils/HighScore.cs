﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PTC.Utils
{
    [Serializable]
    [DataContract]
    public class HighScore
    {
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Country { get; set; }
    }
}
