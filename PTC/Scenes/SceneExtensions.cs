using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PTC.Scenes
{
    public static class SceneExtensions
    {
        public static void Disable(this GameComponent component)
        {
            if (component == null)
                return;
            component.Enabled = false;

            var drawable = component as DrawableGameComponent;
            if(drawable == null)
                return;
            drawable.Visible = false;

            var scene = component as Scene;
            if (scene == null)
                return;

            foreach (GameComponent comp in scene.Components)
            {
                comp.Disable();
            }
            scene.Enabled = false;
            scene.Visible = false;
        }

        public static void Enable(this GameComponent component)
        {
            if (component == null)
                return;
            component.Enabled = true;

            var drawable = component as DrawableGameComponent;
            if (drawable == null)
                return;
            drawable.Visible = true;

            var scene = component as Scene;
            if (scene == null)
                return;

            foreach (GameComponent comp in scene.Components)
            {
                comp.Enable();
            }
            scene.Enabled = true;
            scene.Visible = true;
        }
    }
}
