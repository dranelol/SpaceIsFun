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
        private List<int> targeted_room = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private enum weap_states { disabled, charging, ready };
        //unused for now

        //state machine and state declarations
        StateMachine weaponStateMachine;
        State ready, disabled, charging;

        /// <summary>
        /// This an enumeration of strings that represent the different weapon states
        /// </summary>

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
            disabled = new State { Name = "diabled" };
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
            weaponStateMachine.Start(disabled);
            int charge = 0;
            if (power >= requiredPower)
                enoughPower = true;


        }


        #endregion

        #region methods

        void set_disabled()
        {
            disabled.enter += () =>
            {
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
                if (charge == timeToCharge)
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

        }

        void start_charging()
        {
            if (enoughPower == true)
                is_charging = true;

        }

        void set_target(int targetIndex)
        {
            if (targetIndex > targeted_room.Count)
            {
                Console.WriteLine("You passed an invalid index here");
                return; //throw some exception here
            }

            aimedAtTarget = true;

            if (targetIndex != currentTarget)
                targeted_room[currentTarget] = 0;

            targeted_room[targetIndex] = 1;

        }

        void launch_weapon(int target)
        {
            if (weaponStateMachine.CurrentState == ready)
            {

                //write some code here to actually pass damage to another ship

            }
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
        private Grid[,] shipGrid;

        /// <summary>
        /// parameter for shipGrid
        /// </summary>
        public Grid[,] ShipGrid
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
        private List<Room> roomList;

        /// <summary>
        /// parameter for roomList
        /// </summary>
        public List<Room> RoomList
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
        /// relation between grids and their respective rooms
        /// </summary>
        private Dictionary<Grid, Room> roomGridDict;

        /// <summary>
        /// parameter for roomGridList
        /// </summary>
        public Dictionary<Grid, Room> RoomGridDict
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
        public Ship(Texture2D shipTexture, Texture2D gridTexture, Texture2D highlightTexture, Vector2 position, List<Room> rList)
            : base()
        {
            roomList = new List<Room>();
            roomGridDict = new Dictionary<Grid, Room>();
            roomList = rList;
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
            shipGrid = new Grid[gridWidth, gridHeight];

            // iterate over the ship sprite's width
            for (int i = 0; i < shipTexture.Bounds.Width/32; i++)
            {
                // in each column, iterate over the ship sprite's height
                for (int j = 0; j < shipTexture.Bounds.Height/32; j++)
                {
                    // create a new grid object for i,j
                    shipGrid[i, j] = new Grid(gridTexture, highlightTexture, new Vector2(i * 32 + position.X, j * 32 + position.Y), new Vector2(i, j));
                    
                }
            }

            //ShipGrid[0, 0].IsWalkable = false;
            //ShipGrid[1, 1].IsWalkable = false;
            //ShipGrid[2, 2].IsWalkable = false;
            //ShipGrid[3, 3].IsWalkable = false;
            //ShipGrid[4, 4].IsWalkable = false;
            

            // we need to move the rooms to align ontop of the ship; probably find a better way to do this in the future

            foreach (Room room in roomList)
            {
                room.Sprite.MoveBy(new Vector2(50, 50));
            }


            setRoomGridDictionary();
            setUnwalkableGrids();

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
            foreach (Grid shipgrid in shipGrid)
            {
                shipgrid.Draw(spriteBatch);
            }

            // for each room on the ship, draw
            foreach (Room shipRoom in roomList)
            {
                shipRoom.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// check whether or not the cursor is current hovering over this ship
        /// </summary>
        /// <param name="currentMouseState">current state of the mouse</param>
        /// <returns></returns>
        public bool checkShipHover(MouseState currentMouseState)
        {

            // if the cursor is between the sprite's topleft and bottomright corners
            if (((currentMouseState.X > sprite.Position2D.X)
                    && (currentMouseState.X < sprite.Position2D.X + sprite.Width)
                  && ((currentMouseState.Y > sprite.Position2D.Y)
                    && (currentMouseState.Y < sprite.Position2D.Y + sprite.Height))))
             {
                // our mouse cursor should be within the bounds of the ship
                //System.Diagnostics.Debug.WriteLine("Cursor on the ship!");
                return true;
             }

            else
            {
                return false;
            }

        }

        /// <summary>
        /// check which grid the cursor is currently hovering over; note: this only should get called if checkShipHover returns TRUE
        /// </summary>
        /// <param name="currentMouseState">current state of the mouse</param>
        /// <returns></returns>
        public Vector2 getGridHover(MouseState currentMouseState)
        {
            // we know the cursor is within bounds, this will only get called if checkShipHover returns true

            Vector2 ret = new Vector2();

            // x position relative to the ship
            float relativeXPos = currentMouseState.X - sprite.Position2D.X;
            // y position relative to the ship
            float relativeYPos = currentMouseState.Y - sprite.Position2D.Y;

            // grid x position relative to the ship
            ret.X = (int)relativeXPos / 32;

            // grid y position relative to the ship
            ret.Y = (int)relativeYPos / 32;

            return ret;
        
        }

        /// <summary>
        /// check whether or not the cursor is hovering over a room
        /// </summary>
        /// <param name="currentMouseState"></param>
        /// <returns></returns>
        public bool checkRoomHover(MouseState currentMouseState)
        {
            // find the grid we're hovering over

            Vector2 gridHover = getGridHover(currentMouseState);

            // convert this point to a grid object
            Grid gridToCheck = shipGrid[(int)gridHover.X, (int)gridHover.Y];

            // get the room out of the grid,room dict
            try
            {
                Room checkRoom = roomGridDict[gridToCheck];
            }

            catch (KeyNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine("grid not part of a room");
                //ret.RoomPosition = new Vector2(-1, -1);
                //return ret;
                return false;
            }


            return true;
        }

        /// <summary>
        /// check which room the cursor is currently hovering over, this only should get called if checkRoomHover returns TRUE
        /// </summary>
        /// <param name="gridToCheck"></param>
        /// <returns></returns>
        public Room getRoomHover(MouseState currentMouseState)
        {
            // find the grid we're hovering over

            Vector2 gridHover = getGridHover(currentMouseState);

            // convert this point to a grid object
            Grid gridToCheck = shipGrid[(int)gridHover.X, (int)gridHover.Y];

            // get the room out of the grid,room dict
            return roomGridDict[gridToCheck];
        }

        /// <summary>
        ///  initializes the relationship between grids and their rooms; this is called in the ship constructor ONCE, and only after all other initialization logic has occured
        /// </summary>
        private void setRoomGridDictionary()
        {
            foreach (Room rL in roomList)
            {
                switch (rL.RoomShape)
                {
                    case Globals.roomShape.TwoXTwo:
                        roomGridDict[ShipGrid[(int)rL.RoomPosition.X, (int)rL.RoomPosition.Y]] = rL;
                        roomGridDict[ShipGrid[(int)rL.RoomPosition.X + 1, (int)rL.RoomPosition.Y]] = rL;
                        roomGridDict[ShipGrid[(int)rL.RoomPosition.X, (int)rL.RoomPosition.Y + 1]] = rL;
                        roomGridDict[ShipGrid[(int)rL.RoomPosition.X + 1, (int)rL.RoomPosition.Y + 1]] = rL;
                        break;

                    // TODO: fill in other cases
                }
            }

            // TODO: possibly un-associate any un-wanted grids with rooms (weirdly-shaped rooms, for example)

        }

        /// <summary>
        /// sets every grid that doesnt belong to a room as unwalkable
        /// </summary>
        private void setUnwalkableGrids()
        {
            for (int i = 0; i < shipGrid.GetLength(0); i++)
            {
                for (int j = 0; j < shipGrid.GetLength(1); j++)
                {
                    // is this grid in the dictionary of grids  that have rooms?
                    // if not, make it unwalkable

                    if(!roomGridDict.Keys.Contains(shipGrid[i,j]))
                    {
                        //System.Diagnostics.Debug.WriteLine(shipGrid[i, j].GridPosition.ToString());
                        shipGrid[i, j].IsWalkable = false;
                        
                    }
                }
            }
        }


        #endregion 
    }
}
