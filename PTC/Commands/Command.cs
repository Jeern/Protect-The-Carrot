using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PTC.Commands
{
    public class Command : GameComponent
    {
        /// <summary>
        /// Constructs the Command
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <param name="condition">Condition which has to be true for the command to execute</param>
        /// <param name="commandAction">Action to be executed when conditon is true</param>
        /// <param name="game"></param>
        public Command(string name, Func<GameTime, bool> condition, 
            Action<GameTime> commandAction, Game game) : base(game)
        {
            m_Name = name;
            m_Condition = condition;
            m_CommandAction = commandAction;
            Executed = false;
        }

        private string m_Name;
        public string Name 
        {
            get { return m_Name; }
        }

        private Func<GameTime, bool> m_Condition = x => false;
        public Func<GameTime, bool> Condition
        {
            get { return m_Condition; }
        }

        private Action<GameTime> m_CommandAction = x => { };

        public void Execute(GameTime gameTime)
        {
            m_CommandAction(gameTime);
        }

        public bool Executed { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Condition(gameTime))
            {
                Execute(gameTime);
                Executed = true;
            }
            else
            {
                Executed = false;
            }
        }
    }
}
