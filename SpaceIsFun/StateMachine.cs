using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceIsFun
{
    class StateMachine
    {
        #region fields
        private Game1 Game;
        private State currentState = null;
        
        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }

        private State previousState = null;

        public State PreviousState
        {
            get
            {
                return previousState;
            }
        }

        private bool started = false;

        #endregion

        #region constructors / destructors

        public StateMachine(Game1 thisGame)
        {
            Game = thisGame;
        }

        #endregion

        

        #region methods

        public void Start(State startState)
        {
            if (currentState == null)
            {
                currentState = startState;
            }
        }

        public void Initialize(State startState)
        {
            

            #region transitions setup

            

            #endregion

            //Start(startState);

            

        }

        public void Update(GameTime gameTime)
        {
            if (currentState == null)
            {
                throw new NullReferenceException("no start state");
            }

            if (started == false)
            {
                currentState.execEnter();
                started = true;
            }

            //System.Diagnostics.Debug.WriteLine("current state: " + currentState.Name);
            currentState.execUpdate(gameTime);
        }

        public void Transition(string nextState)
        {
            if(currentState == null)
            {
                throw new NullReferenceException("no current state");
            }

            State next = currentState.Transitions[nextState];

            if(nextState == null)
            {
                throw new NullReferenceException("no state to transition to");
            }

            currentState.execLeave();
            previousState = currentState;
            currentState = next;
            currentState.execEnter();
        }

        #endregion

    }
}


