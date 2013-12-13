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


        /// <summary>
        /// the current state in execution
        /// </summary>
        private State currentState = null;
        
        /// <summary>
        /// property for currentState
        /// </summary>
        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }

        /// <summary>
        /// the previous state in execution
        /// </summary>
        private State previousState = null;

        /// <summary>
        /// property for previousState
        /// </summary>
        public State PreviousState
        {
            get
            {
                return previousState;
            }
        }

        /// <summary>
        /// is the state machine started?
        /// </summary>
        private bool started = false;

        public bool Started
        {
            get
            {
                return started;
            }

            set
            {
                started = value;
            }
        }

        #endregion

        #region constructors / destructors
        public StateMachine()
        {
        }

        #endregion

        
        #region methods

        /// <summary>
        /// start the state machine
        /// </summary>
        /// <param name="startState">the state to start with</param>
        public void Start(State startState)
        {
            if (currentState == null)
            {
                currentState = startState;
            }
        }

        /// <summary>
        /// init the state machine, possible state setup
        /// </summary>
        /// <param name="startState">the state to start with</param>
        public void Initialize(State startState)
        {
        }

        /// <summary>
        /// update the state machine
        /// </summary>
        /// <param name="gameTime">the current game time</param>
        public void Update(GameTime gameTime)
        {
            // if there's no current state running, and we've somehow tried to update the state machine, throw an exception
            if (currentState == null)
            {
                throw new NullReferenceException("no start state");
            }

            // if we havent started yet, start!
            if (started == false)
            {
                currentState.execEnter();
                started = true;
            }

            //System.Diagnostics.Debug.WriteLine("current state: " + currentState.Name);

            // update the current state
            currentState.execUpdate(gameTime);
        }

        /// <summary>
        /// transition to the next state in execution
        /// </summary>
        /// <param name="nextState">the name of the state to transition to</param>
        public void Transition(string nextState)
        {
            System.Diagnostics.Debug.WriteLine(nextState);
            // if there's no current state running, and we've somehow tried to  transition, throw an exception
            if(currentState == null)
            {
                throw new NullReferenceException("no current state");
            }

            // get the next state from the current state's list of transitions
            State next = currentState.Transitions[nextState];

            // if no state was returned, then the current state cannot transition to that state
            if(nextState == null)
            {
                throw new NullReferenceException("no state to transition to");
            }

            // leave the current state
            currentState.execLeave();
            // set the state objects correctly
            previousState = currentState;
            currentState = next;
            // enter the (new) current state
            currentState.execEnter();
        }

        #endregion

    }
}


