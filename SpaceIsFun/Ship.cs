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

    class Crew : Entity
    {
        #region fields

        /// <summary>
        /// position of the crewman on grid space
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Parameter for position
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
        /// Max HP of the crewman
        /// </summary>
        private int maxHP;

        /// <summary>
        /// Parameter for max HP
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
        /// Current HP of the crewman
        /// </summary>
        private int currentHP;

        /// <summary>
        /// Parameter for current HP
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
        /// texture for the crewman
        /// </summary>
        private Texture2D crewTexture;

        /// <summary>
        /// texture for the crewman while selected
        /// </summary>
        private Texture2D crewSelectedTexture;

        /// <summary>
        /// parameter for crewTexture
        /// </summary>
        public Texture2D CrewTexture
        {
            get
            {
                return crewTexture;
            }

            set
            {
                crewTexture = value;
            }
        }

        /// <summary>
        /// paramter for crewSelectedTexture
        /// </summary>
        public Texture2D CrewSelectedTexture
        {
            get
            {
                return crewSelectedTexture;
            }

            set
            {
                crewSelectedTexture = value;
            }
        }

        /// <summary>
        /// whether or not the crewman is selected
        /// </summary>
        private bool selected;

        /// <summary>
        /// parameter for selected
        /// </summary>
        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
            }
        }

        #endregion

        #region constructors / destructors
        /// <summary>
        /// construtor of crewman
        /// </summary>
        /// <param name="position">starting position of the crewman</param>
        /// <param name="crewTexture">texture of the crewman when not selected</param>
        /// <param name="crewSelectedTexture">texture of the crewman when selected</param>
        public Crew(Vector2 position, Texture2D crewTexture, Texture2D crewSelectedTexture) : base()
        {
            maxHP = currentHP = 100;
            this.position = position;

            this.crewTexture = crewTexture;
            this.crewSelectedTexture = crewSelectedTexture;
            this.selected = false;

            sprite = new Drawable(crewTexture, position);

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
            if (selected == true)
            {
                Sprite.SpriteTexture = crewSelectedTexture;
            }

            if (selected == false)
            {
                Sprite.SpriteTexture = crewTexture;
            }

            Sprite.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }


        /// <summary>
        /// highlight the grid if its unhighlighted, unhighlight it if its highlighted
        /// </summary>
        public void Select()
        {
            if (selected == true)
            {
                selected = false;
            }

            else
            {
                selected = true;
            }
        }

        
        public void Move(List<Vector2> path)
        {
            //I have a dream that one day this function will exist, that it will tell the sprite where to move, and the sprite will move there as decreed by the mighty A* algorithm given to us by Peter Hart, Nils Nilsson and Bertram Raphael of the hallowed Stanford Research Instituteendregion
            //sprite.setPath(path);
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
