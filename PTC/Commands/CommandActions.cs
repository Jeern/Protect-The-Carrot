using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PTC.Commands
{
    public static class CommandActions
    {
        public static PTCGame Game { get; set; }

        public static void Exit(GameTime time)
        {
            Game.Exit();
        }

        public static void ToggleFullScreen(GameTime time)
        {
            Game.GraphicsManager.ToggleFullScreen();
        }
    }
}
