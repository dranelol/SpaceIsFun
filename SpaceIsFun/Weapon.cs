using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;


namespace SpaceIsFun
{
    /// <summary>
    /// Pew pew, its a weapon!
    /// </summary>
    [Serializable] class Weapon : Entity, ISerializable
    {
        #region fields
        /// <summary>
        /// True if the weapon is primed to fire, false otherwise (not fully charged, not powered, etc)
        /// </summary>
        private bool readyToFire;

        /// <summary>
        /// parameter for readyToFire
        /// </summary>
        public bool ReadyToFire
        {
            get
            {
                return readyToFire;
            }

            set
            {
                readyToFire = value;
            }
        }

        /// <summary>
        /// True if the weapon has a target, false if not
        /// </summary>
        private bool aimedAtTarget;

        /// <summary>
        /// parameter for aimedAtTarget
        /// </summary>
        public bool AimedAtTarget
        {
            get
            {
                return aimedAtTarget;
            }

            set
            {
                aimedAtTarget = value;
            }
        }

        /// <summary>
        /// the time, in miliseconds, it takes to charge the weapon
        /// </summary>
        private int timeToCharge;

        /// <summary>
        /// parameter for timeToCharge
        /// </summary>
        public int TimeToCharge
        {
            get
            {
                return timeToCharge;
            }

            set
            {
                timeToCharge = value;
            }
        }

        private bool charged;
        /// if the weapon is charged
        /// end

        ///paramater for charged
        public bool Charged
        {
            get
            {
                return charged;
            }

            set
            {
                charged = value;
            }
        }

        private bool is_charging;
        /// if the weapon is charged
        /// end

        ///paramater for charged
        public bool Is_charging
        {
            get
            {
                return is_charging;
            }

            set
            {
                is_charging = value;
            }
        }

        //an int for weapon damage
        private int damage;

        //paramater for damage
        public int Damage
        {
            get
            {
                return damage;
            }

            set
            {
                damage = value;
            }
        }

        //an int for current charge, will be used with game time updated to track if a weapon is charged
        private int charge;

        //paramater for charge
        public int Charge
        {
            get
            {
                return charge;
            }

            set
            {
                charge = value;
            }
        }

        private int requiredPower;

        public int RequiredPower
        {
            get
            {
                return requiredPower;
            }

            set
            {
                requiredPower = value;
            }
        }

        private bool enoughPower;
        //used to see if there is enough power to use the weapon

        public bool EnoughPower
        {
            get
            {
                return enoughPower;
            }

            set
            {
                enoughPower = value;
            }

        }


        private int currentTarget;
        //these are used to see if which list index is the current target
        public int CurrentTarget
        {
            get
            {
                return currentTarget;
            }

            set
            {
                currentTarget = value;
            }
        }

        //a list of rooms that can be targeted, indexes are equivalent to the number of the room in constant.cs
        //list[index] is changed to 1 if the room is targeted


        private enum weap_states { disabled, charging, ready };
        //unused for now

        //state machine and state declarations
        public StateMachine weaponStateMachine;
        public State ready, disabled, charging, enroute;

        /// <summary>
        /// This an enumeration of strings that represent the different weapon states
        /// </summary>
        /// 
        List<Weapon> weapon_list = new List<Weapon>();

        #endregion

        #region constructors / destructors

        //a generic constructor
        public Weapon()
        {
        }
        //declaration of the weapon state machine

        public Weapon(Texture2D skin, int x, int y, int dmg, int time_to_charge, int power)
        {

            weaponStateMachine = new StateMachine();

            ///block for declaration of new states for the weapon
            disabled = new State { Name = "disabled" };
            charging = new State { Name = "charging" };
            ready = new State { Name = "ready" };
            enroute = new State { Name = "enroute" };

            //next blocks are transitions available for each state
            disabled.Transitions.Add(charging.Name, charging);
            disabled.Transitions.Add(ready.Name, ready);

            charging.Transitions.Add(disabled.Name, disabled);
            charging.Transitions.Add(ready.Name, ready);

            ready.Transitions.Add(disabled.Name, disabled);
            ready.Transitions.Add(charging.Name, charging);

            enroute.Transitions.Add(disabled.Name, disabled);
            enroute.Transitions.Add(ready.Name, ready);


            set_disabled();
            set_charging();
            set_ready();
            set_enroute();

            //int x will be x coordinate
            //int y will be y coordinate
            damage = dmg;
            timeToCharge = time_to_charge;
            is_charging = false;
            currentTarget = -1;
            weaponStateMachine.Start(disabled);
            charge = 0;

            if (power >= requiredPower)
            {
                enoughPower = true;
            }


        }

        //Constructor used when loading/deserializing objects
        public Weapon(SerializationInfo si, StreamingContext sc)
        {
            readyToFire = si.GetBoolean("readyToFire");
            aimedAtTarget = si.GetBoolean("aimedAtTarget");
            timeToCharge = si.GetInt32("timeToCharge");
            charged = si.GetBoolean("charged");
            is_charging = si.GetBoolean("is_charging");
            damage = si.GetInt32("damage");
            charge = si.GetInt32("charge");
            requiredPower = si.GetInt32("requiredPower");
            enoughPower = si.GetBoolean("enoughPower");
            currentTarget = si.GetInt32("currentTarget");

            try
            {
                weaponStateMachine = (StateMachine) si.GetValue("weaponStateMachine", typeof(StateMachine));
                ready = (State) si.GetValue("ready", typeof(State));
                disabled = (State) si.GetValue("disabled", typeof(State));
                charging = (State) si.GetValue("charging", typeof(State));
                enroute = (State) si.GetValue("enroute", typeof(State));
                weapon_list = (List<Weapon>) si.GetValue("weapon_list", typeof(List<Weapon>));
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem deserializing Weapon attributes of non-standard types");
                Console.WriteLine(e.ToString());
            }

        }

        #endregion

        #region methods

        void set_disabled()
        {
            disabled.enter += () =>
            {
                charge = 0;
                currentTarget = -1;
            };

            disabled.update += (GameTime gameTime) =>
            {
                if (readyToFire == false && is_charging == true)
                {
                    weaponStateMachine.Transition(charging.Name);
                }
            };

            disabled.leave += () =>
            {
            };

        }

        void set_charging()
        {
            charging.enter += () =>
            {
            };

            start_charging();

            charging.update += (GameTime gameTime) =>
            {
                charge += (int)gameTime.ElapsedGameTime.Milliseconds;
                System.Diagnostics.Debug.WriteLine("current charge: " + charge.ToString());
                System.Diagnostics.Debug.WriteLine("charge needed: " + timeToCharge.ToString());
                if (charge >= timeToCharge)
                {
                    System.Diagnostics.Debug.WriteLine("pew pew time");
                    readyToFire = true;
                }

                if (readyToFire == true)
                {
                    weaponStateMachine.Transition(ready.Name);
                }

                else if (readyToFire == false && is_charging == false)
                {
                    weaponStateMachine.Transition(disabled.Name);
                }
            };

            charging.leave += () =>
            {
            };
        }

        void set_ready()
        {
            ready.enter += () =>
            {
            };


            ready.update += (GameTime gameTime) =>
            {

                if (readyToFire == false && is_charging == true)
                {
                    weaponStateMachine.Transition(charging.Name);
                }

                else if (readyToFire == false && is_charging == false)
                {
                    weaponStateMachine.Transition(disabled.Name);
                }
            };

            ready.leave += () => { };

        }

        void set_enroute()
        {
            enroute.enter += () =>
            {

            };

            enroute.update += (GameTime gameTime) =>
            {

            };

            enroute.leave += () =>
            {

            };

        }

        override public void Update(GameTime gameTime)
        {
            weaponStateMachine.Update(gameTime);

            base.Update(gameTime);
        }

        public void start_charging()
        {
            if (enoughPower == true)
                is_charging = true;

        }

        public void set_target(int targetIndex)
        {

            currentTarget = targetIndex;

        }

        public void launch_weapon(int target)
        {
            if (weaponStateMachine.CurrentState == ready && currentTarget != -1)
            {

                //fire


            }


        }

        public void deactivate_weap()
        {
            ReadyToFire = false;
            Is_charging = false;
        }

        //Method used when saving/serializing objects
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("readyToFire", readyToFire);
            si.AddValue("aimedAtTarget", aimedAtTarget);
            si.AddValue("timeToCharge", timeToCharge);
            si.AddValue("charged", charged);
            si.AddValue("is_charging", is_charging);
            si.AddValue("damage", damage);
            si.AddValue("charge", charge);
            si.AddValue("requiredPower", requiredPower);
            si.AddValue("enoughPower", enoughPower);
            si.AddValue("currentTarget", currentTarget);
            si.AddValue("weaponStateMachine", weaponStateMachine, typeof(StateMachine));
            si.AddValue("ready", ready, typeof(State));
            si.AddValue("disabled", disabled, typeof(State));
            si.AddValue("charging", charging, typeof(State));
            si.AddValue("enroute", enroute, typeof(State));
            si.AddValue("weapon_list", weapon_list, typeof(List<Weapon>));
        }

        #endregion


    }
}
