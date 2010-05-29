using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace PTC.Utils
{
    [DataContract]
    public class KeyInfo
    {
        [DataMember]
        public string Key { get; set; }
    }
}
