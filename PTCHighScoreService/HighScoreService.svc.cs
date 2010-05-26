using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PTC.Utils;
using PTC.Util;
using PTCHighScoreService.Encryption;

namespace PTCHighScoreService
{
    public class HighScoreService : IHighScoreService
    {
        public KeyInfo GetPublicKey()
        {
            var info = new KeyInfo();
            var enc = new Encrypter();
            info.Id = Guid.NewGuid();
            info.Key = enc.PublicKey;
            return info;
        }

        public void Submit(Guid Id, PTC.Utils.HighScore highScore)
        {
            throw new NotImplementedException();
        }

        public List<HighScore> GetCurrentHighScores()
        {
            throw new NotImplementedException();
        }
    }
}
