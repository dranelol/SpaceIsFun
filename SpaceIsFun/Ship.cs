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

        #endregion

        #region constructors / destructors

        #endregion

        #region methods

        #endregion


    }

    /// <summary>
    /// A grid of the ship
    /// </summary>
    public class Grid : Entity
    {
        #region fields
        /// <summary>
        /// x,y position of the grid on the ship (ie, top left grid will be 1,1)
        /// </summary>
        private Vector2 gridPosition;

        public Vector2 GridPosition
        {
            get
            {
                return gridPosition;
            }

            set
            {
                gridPosition = value;
            }
        }

        private bool isWalkable = true;

        public bool IsWalkable
        {
            get
            {
                return isWalkable;
            }

            set
            {
                isWalkable = value;
            }
        }

        /// <summary>
        /// drawable for the grid (this is probably never gonna move)
        /// </summary>
        private Drawable sprite;

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
        /// texture for a grid object
        /// </summary>
        private Texture2D gridTexture;

        /// <summary>
        /// parameter for gridTexture
        /// </summary>
        public Texture2D GridTexture
        {
            get
            {
                return gridTexture;
            }

            set
            {
                gridTexture = value;
            }
        }

        private Texture2D highlightTexture;

        public Texture2D HighlightTexture
        {
            get
            {
                return highlightTexture;
            }

            set
            {
                highlightTexture = value;
            }
        }

        private bool highlighted;

        public bool Highlighted
        {
            get
            {
                return highlighted;
            }

            set
            {
                highlighted = value;
            }
        }



        #endregion

        #region constructors / destructors
        public Grid()
            : base()
        {
        }

        public Grid(Texture2D spriteTexture, Texture2D highlight, Vector2 position, Vector2 gPosition) 
            : base()
        {
            gridTexture = spriteTexture;
            sprite = new Drawable(gridTexture, position);
            gridPosition = gPosition;
            highlightTexture = highlight;
        }

        #endregion

        #region methods

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (highlighted == true)
            {
                Sprite.SpriteTexture = highlightTexture;
            }

            if (highlighted == false)
            {
                Sprite.SpriteTexture = gridTexture;
            }
            
            Sprite.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// highlight the grid if its unhighlighted, unhighlight it if its highlighted
        /// </summary>
        public void Highlight()
        {
            if (highlighted == true)
            {
                highlighted = false;
            }

            else
            {
                highlighted = true;
            }
        }
        #endregion




    }
    /// <summary>
    /// This contains all the info of a room on a ship 
    /// </summary>
    class Room : Entity
    {
        #region fields
        /// <summary>
        /// grid position of the room's top-left grid (if the room's top-left grid's grid position is (2,2), then this will be (2,2)
        /// </summary>
        private Vector2 roomPosition;

        /// <summary>
        /// parameter for roomPosition
        /// </summary>
        public Vector2 RoomPosition
        {
            get
            {
                return roomPosition;
            }

            set
            {
                RoomPosition = value;
            }
        }

        /// <summary>
        /// position of this room's 1,1 in screenspace
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// parameter for position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }
         

        /// <summary>
        /// drawable for the room (this is probably never gonna move)
        /// </summary>
        private Drawable sprite;

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
        /// texture for the room
        /// </summary>
        private Texture2D roomTexture;

        /// <summary>
        /// texture for the room while highlighted
        /// </summary>
        private Texture2D roomHighlightTexture;

        /// <summary>
        /// parameter for roomTexture
        /// </summary>
        public Texture2D RoomTexture
        {
            get
            {
                return roomTexture;
            }

            set
            {
                roomTexture = value;
            }
        }

        /// <summary>
        /// paramter for roomHighlightTexture
        /// </summary>
        public Texture2D RoomHighlightTexture
        {
            get
            {
                return roomHighlightTexture;
            }

            set
            {
                roomHighlightTexture = value;
            }
        }

        /// <summary>
        /// whether or not the room is highlighted
        /// </summary>
        private bool highlighted;

        /// <summary>
        /// parameter for highlighted
        /// </summary>
        public bool Highlighted
        {
            get
            {
                return highlighted;
            }

            set
            {
                highlighted = value;
            }
        }

         /// <summary>
        /// parameter for isMannable
        /// </summary>

        protected bool isMannable;

        public bool IsMannable
        {
            get
            {
                return isMannable;
            }

        }

        /// <summary>
        /// parameter for position
        /// </summary>

        protected int roomType;

        public int RoomType
        {
            get
            {
                return roomType;
            }

        }

        protected int status;

        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        #endregion

        #region constructors / destructors
        public Room() { }

        /// <summary>
        /// constructor for a room
        /// </summary>
        /// <param name="texture">texture for the room</param>
        /// <param name="highlightTexture">texture for the room when its highlighted</param>
        /// <param name="x">x-position of the top-left grid position</param>
        /// <param name="y">y-position of the top-left grid position</param>
        public Room(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            position = new Vector2(x * 32, y * 32);
            roomTexture = texture;
            roomHighlightTexture = highlightTexture;
            roomPosition = new Vector2(x, y);
            isMannable = new bool();
            isMannable = false;
            roomType = constants.EMPTY_ROOM;
        }

        #endregion
        #region methods

        /// <summary>
        /// update the room
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            updateStatus();


            Sprite.Update(gameTime);
            base.Update(gameTime);
        }

        virtual void updateStatus()
        {
            
        }

        /// <summary>
        /// draw the room
        /// </summary>
        /// <param name="spriteBatch">main spriteBatch object</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (highlighted == true)
            {
                Sprite.SpriteTexture = roomHighlightTexture;
            }

            if (highlighted == false)
            {
                Sprite.SpriteTexture = roomTexture;
            }

            Sprite.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// highlight the room if its unhighlighted, unhighlight it if its highlighted
        /// </summary>
        public void Highlight()
        {
            if (highlighted == true)
            {
                highlighted = false;
            }

            else
            {
                highlighted = true;
            }
        }

        #endregion


    }
    
    /// <summary>
    /// This contains info on rooms that can be manned
    /// </summary>
    #region mannedRooms

    class engineRoom : Room
    {
        #region fields
        
        #endregion

        #region constructors / destructors
        public engineRoom()
        {
        }

        public engineRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            roomType = constants.ENGINE_ROOM;
        }
        #endregion

        #region methods
        override protected void updateStatus()
        {
            switch (status)
            {
                case constants.STATUS_NORMAL:

                    break;

                case constants.STATUS_DAMAGED:
                    //
                    break;

                case constants.STATUS_INOPERABLE:
                    break;

                case constants.STATUS_DISABLED:
                    break;

                default:
                    //Normal status

                    break;
            }

        }
        #endregion
    }

    class pilotRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public pilotRoom()
        {
        }

        public pilotRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            roomType = constants.PILOT_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    class shieldRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public shieldRoom()
        {
        }

        public shieldRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            roomType = constants.SHIELD_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    class weaponRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public weaponRoom()
        {
        }

        public weaponRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            isMannable = new bool();
            isMannable = true;
            roomType = constants.WEAPONS_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }
    #endregion

    #region unmannedRooms

    class cloakRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public cloakRoom()
        {
        }

        public cloakRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            roomType = constants.CLOAK_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    class doorRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public doorRoom()
        {
        }

        public doorRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            roomType = constants.DOOR_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    class droneRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public droneRoom()
        {
        }
        public droneRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            roomType = constants.DRONE_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    class medbayRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public medbayRoom()
        {
        }
        public medbayRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            roomType = constants.MEDBAY_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    class O2Room : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public O2Room()
        {
        }
        public O2Room(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            roomType = constants.O2_ROOM;
        }
        #endregion

        #region methods
        #endregion
    }

    class sensorRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public sensorRoom()
        {
        }
        public sensorRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            roomType = constants.SENSORS_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }

    class teleporterRoom : Room
    {
        #region fields

        #endregion

        #region constructors / destructors
        public teleporterRoom()
        {
        }
        public teleporterRoom(Texture2D texture, Texture2D highlightTexture, int x, int y)
        {
            roomType = constants.TELEPORTER_ROOM;
        }
        #endregion

        #region methods

        #endregion
    }
    #endregion


    class Crew : Entity
    {
        #region fields
        #endregion

        #region constructors / destructors
        public Crew()
        {
        }
        #endregion

        #region methods
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
        /// relation between rooms and their grid objects
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
        public Ship(Texture2D shipTexture, Texture2D gridTexture, Texture2D highlightTexture, Vector2 position)
            : base()
        {
            // set some default values 
            maxHP = currentHP = 10;
            energy = 5;
            maxShields = currentShields = 2;

            // create the ship's drawable
            sprite = new Drawable(shipTexture, position);
            // create the ship's grid; each grid is 32-wide, so we get the amount of grids needed by dividing the ship's sprite up into 32x32 chunks
            int gridWidth = shipTexture.Bounds.Width / 32;
            int gridHeight = shipTexture.Bounds.Height / 32;
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

            ShipGrid[0, 0].IsWalkable = false;
            ShipGrid[1, 1].IsWalkable = false;
            ShipGrid[2, 2].IsWalkable = false;
            ShipGrid[3, 3].IsWalkable = false;
            ShipGrid[4, 4].IsWalkable = false;

        }

        #endregion

        #region methods

        /// <summary>
        /// update the ship object
        /// </summary>
        /// <param name="gameTime"></param>
        override public void Update(GameTime gameTime)
        {
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
        public Vector2 checkGridHover(MouseState currentMouseState)
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
        /// check which room the cursor is currently hovering over, this only should get called if checkShipHover returns TRUE
        /// </summary>
        /// <param name="gridToCheck"></param>
        /// <returns></returns>
        public Room checkRoomHover(MouseState currentMouseState)
        {
            Room ret = new Room();

            // find the grid we're hovering over

            Vector2 gridHover = checkGridHover(currentMouseState);

            // convert this point to a grid object
            Grid gridToCheck = shipGrid[(int)gridHover.X, (int)gridHover.Y];

            // get the room out of the grid,room dict

            ret = roomGridDict[gridToCheck];


            return ret;
        }

        #endregion 
    }
}
