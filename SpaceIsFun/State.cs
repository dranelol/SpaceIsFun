using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceIsFun
{
    class State
    {
        private string name;
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

        public string cheese = "Parmesan";

        public delegate void Enter();
        public delegate void Leave();
        public delegate void Update(GameTime gameTime);

        public event Enter enter;
        public event Leave leave;
        public event Update update;

        internal void execEnter()
        {
            System.Diagnostics.Debug.WriteLine("entering: " + name);
            enter();
        }

        internal void execLeave()
        {
            System.Diagnostics.Debug.WriteLine("leaving: " + name);
            leave();
        }

        internal void execUpdate(GameTime gameTime)
        {
            update(gameTime);
        }

        Dictionary<string, State> transitions = new Dictionary<string, State>();

        public Dictionary<string, State> Transitions
        {
            get
            {
                return transitions;
            }
        }
    }
}
