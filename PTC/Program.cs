using System;
using PTC.HighScoreProxy;
using PTC.Utils;

namespace PTC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var score = new HighScore();
            score.Score = 5;
            score.Name = "Hans";
            score.Country = "Ireland";
            var proxy = new HighScoreServiceClient();
            proxy.Save(score);

            var list = proxy.GetCurrentHighScores();

            using (PTCGame game = new PTCGame())
            {
                game.Run();
            }
        }
    }
}

