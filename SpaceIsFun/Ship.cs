using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceIsFun
{
    /// <summary>
    /// Pew pew, its a weapon!
    /// </summary>
    class Weapon : Entity
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
       public State ready, disabled, charging;

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

            //next blocks are transitions available for each state
            disabled.Transitions.Add(charging.Name, charging);
            disabled.Transitions.Add(ready.Name, ready);

            charging.Transitions.Add(disabled.Name, disabled);
            charging.Transitions.Add(ready.Name, ready);

            ready.Transitions.Add(disabled.Name, disabled);
            ready.Transitions.Add(charging.Name, charging);


            set_disabled();
            set_charging();
            set_ready();

            //int x will be x coordinate
            //int y will be y coordinate
            damage = dmg;
            timeToCharge = time_to_charge;
            is_charging = false;
            currentTarget = -1;
            weaponStateMachine.Start(disabled);
            charge = 0;
            if (power >= requiredPower)
                enoughPower = true;


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
                //not sure if this will actually work.
                charge += (int)gameTime.ElapsedGameTime.Milliseconds;
                if (charge >= timeToCharge)
                    readyToFire = true;
                if (readyToFire == true)
                {
                    weaponStateMachine.Transition(ready.Name);
                }

                else if (readyToFire == false && is_charging == false)
                    weaponStateMachine.Transition(disabled.Name);
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
            if (weaponStateMachine.CurrentState == ready && currentTarget!=-1)
            {

                //fire
                

            }

           
        }

        public void deactivate_weap()
        {
            ReadyToFire = false;
            Is_charging = false;
        }


        #endregion


    }




    /// <summary>
    /// Its a ship!
    /// </summary>
    class Ship : Entity
    {
        #region fields
        /// <summary>
        /// Max hp of the ship
        /// </summary>
        private int maxHP;

        /// <summary>
        /// parameter for maxHP
        /// </summary>
        public int MaxHP
        {
            get
            {
                return maxHP;
            }

            set
            {
                maxHP = value;
            }
        }

        /// <summary>
        /// current HP of the ship
        /// </summary>
        private int currentHP;

        /// <summary>
        /// parameter for currentHP
        /// </summary>
        public int CurrentHP
        {
            get
            {
                return currentHP;
            }

            set
            {
                currentHP = value;
            }
        }

        /// <summary>
        /// the current shield level of the ship
        /// </summary>
        private int currentShields;

        /// <summary>
        /// parameter for currentShields
        /// </summary>
        public int CurrentShields
        {
            get
            {
                return currentShields;
            }
            set
            {
                currentShields = value;
            }
        }

        /// <summary>
        /// Maximum shield level of the ship
        /// </summary>
        private int maxShields;

        /// <summary>
        /// parameter for maxShields
        /// </summary>
        public int MaxShields
        {
            get
            {
                return maxShields;
            }
            set
            {
                maxShields = value;
            }
        }

        /// <summary>
        /// energy pool of the ship
        /// </summary>
        private int energy;

        /// <summary>
        /// parameter for energy
        /// </summary>
        public int Energy
        {
            get
            {
                return energy;
            }
            set
            {
                energy = value;
            }
        }

        private int shipO2;

        public int ShipO2
        {
            get
            {
                return shipO2;
            }
            set
            {
                shipO2 = value;
            }
        }

        

        /// <summary>
        /// the ship's Drawable object
        /// </summary>
        private Drawable sprite;

        /// <summary>
        /// parameter for sprite
        /// </summary>
        public Drawable Sprite
        {
            get
            {
                return sprite;
            }

            set
            {
                sprite = value;
            }
        }

        /// <summary>
        /// the 2D Grid array holding the grid objects attributed to the ship
        /// </summary>
        private int[,] shipGrid;

        /// <summary>
        /// parameter for shipGrid
        /// </summary>
        public int[,] ShipGrid
        {
            get
            {
                return shipGrid;
            }

            set
            {
                shipGrid = value;
            }
        }

        /// <summary>
        /// the list of rooms for a ship
        /// </summary>
        private bool[] roomList;

        /// <summary>
        /// parameter for roomList
        /// </summary>
        public bool[] RoomList
        {
            get
            {
                return roomList;
            }

            set
            {
                roomList = value;
            }
        }

        /// <summary>
        /// relation between grids and their respective rooms: key gridUID, value roomUID
        /// </summary>
        private Dictionary<int, int> roomGridDict;

        /// <summary>
        /// parameter for roomGridList
        /// </summary>
        public Dictionary<int, int> RoomGridDict
        {
            get
            {
                return roomGridDict;
            }

            set
            {
                roomGridDict = value;
            }
        }


        /// <summary>
        /// how wide the grid list is
        /// </summary>
        private int gridWidth;

        /// <summary>
        /// parameter for gridWidth
        /// </summary>
        public int GridWidth
        {
            get
            {
                return gridWidth;
            }

            set
            {
                gridWidth = value;
            }
        }

        private Weapon default_weap;

        public Weapon Default_weap
        {
            get
            {
                return default_weap;
            }

            set
            {
                default_weap = value;
            }
        }

        /// <summary>
        /// how high the grid list is
        /// </summary>
        private int gridHeight;

        /// <summary>
        /// parameter for gridWidth
        /// </summary>
        public int GridHeight
        {
            get
            {
                return gridHeight;
            }

            set
            {
                gridHeight = value;
            }
        }

        private List<int> roomUIDList;

        public List<int> RoomUIDList
        {
            get
            {
                return roomUIDList;
            }

            set
            {
                roomUIDList = value;
            }
        }

        private List<int> gridUIDList;

        public List<int> GridUIDList
        {
            get
            {
                return gridUIDList;
            }

            set
            {
                gridUIDList = value;
            }
        }

        private List<int> weaponUIDList;

        public List<int> WeaponUIDList
        {
            get
            {
                return weaponUIDList;
            }

            set
            {
                weaponUIDList = value;
            }
        }

        /// <summary>
        /// faction owner of this ship; for now 0=player, 1=enemy
        /// </summary>
        private int owner;

        public int Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
            }
        }

        #endregion

        #region constructors / destructors

        public Ship()
        {
        }
        /// <summary>
        /// constructor for a ship object
        /// </summary>
        /// <param name="shipTexture">texture used to draw the ship's sprite</param>
        /// <param name="gridTexture">texture used to draw the ship's grid</param>
        /// <param name="highlightTexture">texture used to draw the ship's grid when a grid is selected</param>
        /// <param name="position">initial position of the ship's sprite</param>
        public Ship(Texture2D shipTexture, 
                    Texture2D gridTexture, 
                    Texture2D highlightTexture, 
                    Vector2 position,
                    List<int> roomUIDs,
                    List<int> gridUIDs,
                    List<int> weaponUIDs,
                    bool[] roomTypes,
                    int[,] shipGrid,
                    int owner)


            : base()
        {
            roomList = new bool[11];
            roomGridDict = new Dictionary<int,int>();
            System.Diagnostics.Debug.WriteLine("initting ship");
            // set some default values 
            maxHP = currentHP = 10;
            energy = 5;
            maxShields = currentShields = 2;

            // create the ship's drawable
            sprite = new Drawable(shipTexture, position);
            // create the ship's grid; each grid is 32-wide, so we get the amount of grids needed by dividing the ship's sprite up into 32x32 chunks
            gridWidth = shipTexture.Bounds.Width / 32;
            gridHeight = shipTexture.Bounds.Height / 32;
            //this.shipGrid = new int[gridWidth, gridHeight];
            this.shipGrid = shipGrid;
            Default_weap = new Weapon(gridTexture, 0, 0, 2, 10, 3);

            this.owner = owner;

            roomUIDList = roomUIDs;
            gridUIDList = gridUIDs;
            weaponUIDList = weaponUIDs;
            

            // we need to move the rooms to align ontop of the ship; probably find a better way to do this in the future
            /*
            foreach (Room room in roomList)
            {
                room.Sprite.MoveBy(new Vector2(50, 50));
            }
            */

            //setRoomGridDictionary();
            //setUnwalkableGrids();

        }

        #endregion

        #region methods

        /// <summary>
        /// update the ship object
        /// </summary>
        /// <param name="gameTime"></param>
        override public void Update(GameTime gameTime)
        {
            //update the shield
            /*if (roomList[0].getStatus() != "inoperable" || roomList[0].getStatus() != "disabled")
            {

            }
            //update the O2
            if (roomList[1].getStatus() != "inoperable" || roomList[1].getStatus() != "disabled")
            {

            }*/
            base.Update(gameTime);
        }

        /// <summary>
        /// draw the ship object
        /// </summary>
        /// <param name="spriteBatch"></param>
        override public void Draw(SpriteBatch spriteBatch)
        {
            // draw the ship's sprite
            sprite.Draw(spriteBatch);

            // for each grid on the ship, draw its sprite
            

            // for each room on the ship, draw
            

            base.Draw(spriteBatch);
        }

        

        

        

        


        #endregion 
    }
}
