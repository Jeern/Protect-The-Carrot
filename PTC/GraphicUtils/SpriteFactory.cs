using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PTC.Sprites;

namespace PTC.GraphicUtils
{
    public static class SpriteFactory
    {
        /// <summary>
        /// Creates a new Rabbit or uses an old one if one exists
        /// </summary>
        /// <param name="game"></param>
        /// <param name="startPos"></param>
        /// <param name="sex"></param>
        /// <param name="kid"></param>
        /// <returns></returns>
        public static Rabbit CreateRabbit(Game game, Vector2 startPos, Sex sex, bool kid)
        {
            var spritesNotInUse = GetStack(typeof(Rabbit));
            Rabbit rabbit = null;
            if (m_SpritesNotInUse.ContainsKey(typeof(Rabbit)) && spritesNotInUse.Count > 0)
            {

                rabbit = spritesNotInUse.Pop() as Rabbit;
                rabbit.Reset(startPos, sex, kid);
            }
            else
            {
                rabbit = new Rabbit(game, startPos, sex, kid);
            }

            Add(rabbit);
            return rabbit;
        }

        public static Blood CreateBlood(Game game, Vector2 startPos)
        {
            var spritesNotInUse = GetStack(typeof(Blood));
            Blood blood = null;
            if (m_SpritesNotInUse.ContainsKey(typeof(Blood)) && spritesNotInUse.Count > 0)
            {

                blood = spritesNotInUse.Pop() as Blood;
                blood.Reset(startPos);
            }
            else
            {
                blood = new Blood(game, startPos);
            }

            Add(blood);
            return blood;
        }

        public static MatingRabbits CreateMatingRabbit(Game game, Vector2 startPos, TimeSpan startTime)
        {
            var spritesNotInUse = GetStack(typeof(MatingRabbits));
            MatingRabbits rabbit = null;
            if (m_SpritesNotInUse.ContainsKey(typeof(MatingRabbits)) && spritesNotInUse.Count > 0)
            {

                rabbit = spritesNotInUse.Pop() as MatingRabbits;
                rabbit.Reset(startPos);
            }
            else
            {
                rabbit = new MatingRabbits(game, startPos, startTime);
            }

            Add(rabbit);
            return rabbit;
        }

        //These two Containers are used to speed up the game and decrease amount of GC
        private static HashSet<Sprite> m_SpritesInUse = new HashSet<Sprite>();
        private static Dictionary<Type, Stack<Sprite>> m_SpritesNotInUse = new Dictionary<Type, Stack<Sprite>>();

        public static void Remove(Sprite sprite)
        {
            if (m_SpritesInUse.Contains(sprite))
            {
                m_SpritesInUse.Remove(sprite);
                Stack<Sprite> spritesNotInUse = GetStack(sprite.GetType());
                spritesNotInUse.Push(sprite);
            }
        }

        private static Stack<Sprite> GetStack(Type type)
        {
            if (m_SpritesNotInUse.ContainsKey(type))
                return m_SpritesNotInUse[type];

            Stack<Sprite> spritesNotInUse = new Stack<Sprite>();
            m_SpritesNotInUse.Add(type, spritesNotInUse);
            return spritesNotInUse;
        }

        private static void Add(Sprite sprite)
        {
            m_SpritesInUse.Add(sprite);
        }
        
    }
}
