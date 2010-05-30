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
            using (PTCGame game = new PTCGame())
            {
                game.Run();
            }
        }
    }
}

