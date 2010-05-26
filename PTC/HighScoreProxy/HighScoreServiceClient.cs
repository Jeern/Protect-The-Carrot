using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTC.Util;
using System.ServiceModel;
using PTC.Utils;
using System.Diagnostics;

namespace PTC.HighScoreProxy
{
    public class HighScoreServiceClient : ClientBase<IHighScoreService>, IHighScoreService
    {
#if DEBUG
        public HighScoreServiceClient() : base(new BasicHttpBinding(BasicHttpSecurityMode.None), new EndpointAddress("http://localhost:1701/HighScoreService.svc")) {}
#else
        public HighScoreServiceClient() : base(new BasicHttpBinding(BasicHttpSecurityMode.None), new EndpointAddress("http://www.niedermann.dk/PTC/HighScoreService.svc")) { }
#endif

        public KeyInfo GetPublicKey()
        {
            return Channel.GetPublicKey();
        }

        public void Submit(Guid Id, HighScore highScore)
        {
            Channel.Submit(Id, highScore);
        }

        public List<HighScore> GetCurrentHighScores()
        {
            return Channel.GetCurrentHighScores();
        }
    }
}
