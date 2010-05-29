using System;
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

        const string SEP = "$$$$";

        public override string ToString()
        {
            return Score.ToString() + SEP + Name + SEP + Country;
        }

        public void FromString(string value)
        {
            string[] objContent = value.Split(new string[] { SEP }, StringSplitOptions.None);
            Score = int.Parse(objContent[0]);
            Name = objContent[1];
            Country = objContent[2];
        }
    }
}
