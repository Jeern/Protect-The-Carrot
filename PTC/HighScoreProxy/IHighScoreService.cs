using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PTC.Utils;

namespace PTC.Util
{
    [ServiceContract]
    public interface IHighScoreService
    {

        [OperationContract]
        KeyInfo GetPublicKey();

        [OperationContract]
        void Submit(Guid Id, HighScore highScore);

        [OperationContract]
        List<HighScore> GetCurrentHighScores();
    }
}
