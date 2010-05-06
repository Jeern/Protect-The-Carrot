using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using PTC.Scenes;

namespace PTC.Utils
{
    public static class XACT 
    {
        private static AudioEngine ulriksEngine;
        private static SoundBank gameSound;
        public static SoundBank GameSound { get { return gameSound; } }

        static Cue ambient1;
        static Cue ambient2;

        static bool onlyOnceStartSoundScape;

        private static WaveBank GameWave;

        static XACT()
        {   
        ulriksEngine = new AudioEngine("Content\\XACT\\PTC.xgs");
        gameSound = new SoundBank(ulriksEngine, "Content\\XACT\\PTCSound.xsb");
        GameWave = new WaveBank(ulriksEngine, "Content\\XACT\\PTCWave.xwb");
        
        ambient1 = gameSound.GetCue("ambientInsects");
        ambient2 = gameSound.GetCue("ambientBirds");
    
         }

        public static void StartAmbientSound()
        {
            if (ambient1.IsStopped || ambient1.IsPrepared)
            {
                ambient1 = gameSound.GetCue("ambientInsects");
                ambient1.Play();
            }

            if (ambient2.IsStopped || ambient2.IsPrepared)
            {
                ambient2 = gameSound.GetCue("ambientBirds");
                ambient2.Play();
            }
        }

        public static void StartSoundScape()
        {
            if (!onlyOnceStartSoundScape)
            {
                GameSound.PlayCue("summerLoop");
                onlyOnceStartSoundScape = true;
            }
        }

        public static void PlayerHit()
        {
            GameSound.PlayCue("playerHit");
        }

        public static void PlayerMiss()
        {
            GameSound.PlayCue("playerMiss");
        }

        public static void KneppeLyd()
        {
            GameSound.PlayCue("bunnyMating");
        }

        public static void RabbitLands()
        {
            GameSound.PlayCue("maleJump");
        }

        /// <summary>
        /// Når drengen slår kaninen med et baseball-bat.
        /// </summary>
        public static void KidHit()
        {
            GameSound.PlayCue("KidHit");
        }

        /// <summary>
        /// Når kaninen går i børnefælden.
        /// </summary>
        public static void BearTrap()
        {
            gameSound.PlayCue("bearTrap");
        }
    }
}
