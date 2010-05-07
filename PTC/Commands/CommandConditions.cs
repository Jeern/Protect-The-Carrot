using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PTC.Input;

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
                (KeyboardExtended.Current.WasSingleClick(Keys.Enter) || 
                MouseExtended.Current.WasSingleClick(MouseButton.Left));
        }

        public static bool ChangeFromGameOverToWelcomeScene(GameTime time)
        {
            return KeyboardExtended.Current.CurrentState.GetPressedKeys().Count() > 0;
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


    }
}
