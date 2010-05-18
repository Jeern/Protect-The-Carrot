using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PTC.Input;
using PTC.Utils;

namespace PTC.Commands
{
    public static class CommandConditions
    {
        public static PTCGame Game { get; set; }

        public static bool Exit(GameTime time)
        {
            return KeyboardExtended.Current.WasSingleClick(Keys.Escape);
        }

        public static bool ChangeFromWelcomeToMainScene(GameTime time)
        {
            return
                CountryChosen() &&
                (KeyboardExtended.Current.WasSingleClick(Keys.Enter) || 
                MouseExtended.Current.WasSingleClick(MouseButton.Left));
        }

        public static bool ChangeFromWelcomeToCountryChoiceScene(GameTime time)
        {
            return
                !CountryChosen() &&
                (KeyboardExtended.Current.WasSingleClick(Keys.Enter) ||
                MouseExtended.Current.WasSingleClick(MouseButton.Left));
        }

        public static bool ChangeFromGameOverToWelcomeScene(GameTime time)
        {
            return (KeyboardExtended.Current.WasSingleClick(Keys.Enter) ||
                MouseExtended.Current.WasSingleClick(MouseButton.Left));
        }

        public static bool ToggleFullScreen(GameTime time)
        {
            return KeyboardExtended.Current.WasSingleClick(Keys.F4);
        }

        public static bool Fire(GameTime time)
        {
            return
                (KeyboardExtended.Current.WasSingleClick(Keys.Space) ||
                MouseExtended.Current.WasSingleClick(MouseButton.Left));
        }

        public static bool GameOverCheatCode(GameTime time)
        {
            return KeyboardExtended.Current.WasDoubleClick(Keys.F7);
        }

        private static bool CountryChosen()
        {
            return Game.Highscores.Country != Environment.NoCountryName;
        }


    }
}
