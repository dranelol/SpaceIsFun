using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceIsFun
{
    class State
    {
        /// <summary>
        /// name of the state
        /// </summary>
        private string name;

        /// <summary>
        /// property for name
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// the method delegate for entering the state
        /// </summary>

        public delegate void Enter();

        /// <summary>
        /// the method delegate for leaving the state
        /// </summary>
        public delegate void Leave();

        /// <summary>
        /// the method delegate for updating the state
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public delegate void Update(GameTime gameTime);

        /// <summary>
        /// event called on state enter
        /// </summary>
        public event Enter enter;

        /// <summary>
        /// event called on state leave
        /// </summary>
        public event Leave leave;

        /// <summary>
        /// event called on state update
        /// </summary>
        public event Update update;


        /// <summary>
        /// the function executed on state enter
        /// </summary>
        internal void execEnter()
        {
            System.Diagnostics.Debug.WriteLine("entering: " + name);
            enter();
        }

        /// <summary>
        /// the function executed on state leave
        /// </summary>
        internal void execLeave()
        {
            System.Diagnostics.Debug.WriteLine("leaving: " + name);
            leave();
        }

        /// <summary>
        /// the function executed on state update
        /// </summary>
        /// <param name="gameTime">the current game time</param>
        internal void execUpdate(GameTime gameTime)
        {
            update(gameTime);
        }

        /// <summary>
        /// a mapping, from string to state, of state transitions
        /// </summary>
        Dictionary<string, State> transitions = new Dictionary<string, State>();

        /// <summary>
        /// property for transitions
        /// </summary>
        public Dictionary<string, State> Transitions
        {
            get
            {
                return transitions;

            }
        }
    }
}
