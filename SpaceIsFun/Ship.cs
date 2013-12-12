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
    /// Its a ship!
    /// </summary>
    [Serializable] class Ship : Entity, ISerializable
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
        /// the 2D int array holding the grid UIDs attributed to the ship
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
        /// the list of rooms on a ship; this is always gonna be size 11, and just tells whether or not the ship has a room of that type
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

        /// <summary>
        /// holds the room UIDs assigned to this ship
        /// </summary>
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

        /// <summary>
        ///  holds the grid UIDs assigned to this ship
        /// </summary>
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
        
        /// <summary>
        /// holds the UIDs of weapons assigned to this ship
        /// </summary>
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
        /// holds the UID for a weapon assigned to a specific slot; -1 if nothing is assigned to that slot
        /// </summary>
        private int[] weaponSlots;

        public int[] WeaponSlots
        {
            get
            {
                return weaponSlots;
            }

            set
            {
                weaponSlots = value;
            }
        }

        /// <summary>
        /// an array of screenspace x,y points where each weapon slots is mounted on the ship
        /// weaponMountPoints[0] corresponds to weaponSlots[0], [1] to [1], etc
        /// </summary>
        private Point[] weaponMountPoints;

        public Point[] WeaponMountPoints
        {
            get
            {
                return weaponMountPoints;
            }

            set
            {
                value = weaponMountPoints;
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



            

            this.owner = owner;

            roomUIDList = roomUIDs;
            gridUIDList = gridUIDs;
            weaponUIDList = weaponUIDs;

            weaponSlots = new int[4];

            weaponSlots[0] = weaponUIDList[0];
            weaponSlots[1] = -1;
            weaponSlots[2] = -1;
            weaponSlots[3] = -1;

            //Default_weap = new Weapon(gridTexture, 0, 0, 2, 10, 3);
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

        public Ship(SerializationInfo si, StreamingContext sc)
            : base()
        {
            maxHP = si.GetInt32("maxHP");
            currentHP = si.GetInt32("currentHP");
            currentShields = si.GetInt32("currentShields");
            maxShields = si.GetInt32("maxShields");
            energy = si.GetInt32("energy");
            shipO2 = si.GetInt32("shipO2");
            sprite = (Drawable)si.GetValue("sprite", typeof(Drawable));
            shipGrid = (int[,])si.GetValue("shipGrid", typeof(int[,]));
            roomList = (bool[])si.GetValue("roomList", typeof(bool[]));
            roomGridDict = (Dictionary<int, int>)si.GetValue("roomGridDict", typeof(Dictionary<int, int>));
            gridWidth = si.GetInt32("gridWidth");
            gridHeight = si.GetInt32("gridHeight");
            default_weap = (Weapon) si.GetValue("default_weap", typeof(Weapon));
            roomUIDList = (List<int>)si.GetValue("roomUIDList", typeof(List<int>));
            gridUIDList = (List<int>)si.GetValue("gridUIDList", typeof(List<int>));
            weaponUIDList = (List<int>)si.GetValue("weaponUIDList", typeof(List<int>));
            weaponSlots = (int[])si.GetValue("weaponSlots", typeof(int[]));
            weaponMountPoints = (Point[])si.GetValue("weaponMountPoints", typeof(Point[]));
            owner = si.GetInt32("owner");
        }

        #endregion

        #region methods

        /// <summary>
        /// update the ship object
        /// </summary>
        /// <param name="gameTime"></param>
        override public void Update(GameTime gameTime)
        {
            if (currentHP == 0)
            {
                System.Diagnostics.Debug.WriteLine("dead ship");
            }
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

        /// <summary>
        /// do damage to the ship
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>1 if this shot has killed the ship, 0 if this shot has not killed the ship</returns>
        public int TakeDamage(int amount)
        {
            if (currentHP < amount)
            {
                currentHP = 0;
                return 1;
            }

            else
            {
                currentHP = currentHP - amount;
                return 0;
            }
        }


        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("maxHP", maxHP);
            si.AddValue("currentHP", currentHP);
            si.AddValue("currentShields", currentShields);
            si.AddValue("maxShields", maxShields);
            si.AddValue("energy", energy);
            si.AddValue("shipO2", shipO2);
            si.AddValue("sprite", sprite, typeof(Drawable));
            si.AddValue("shipGrid", shipGrid, typeof(int[,]));
            si.AddValue("roomList", roomList, typeof(bool[]));
            si.AddValue("roomGridDict", roomGridDict, typeof(Dictionary<int, int>));
            si.AddValue("gridWidth", gridWidth);
            si.AddValue("default_weap", default_weap, typeof(Weapon));
            si.AddValue("gridHeight", gridHeight);
            si.AddValue("roomUIDList", roomUIDList, typeof(List<int>));
            si.AddValue("gridUIDList", gridUIDList, typeof(List<int>));
            si.AddValue("weaponUIDList", weaponUIDList, typeof(List<int>));
            si.AddValue("weaponSlots", weaponSlots, typeof(int[]));
            si.AddValue("weaponMountPoints", weaponMountPoints, typeof(Point[]));
            si.AddValue("owner", owner);


        }

        

        


        #endregion 
    }
}
