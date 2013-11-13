using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;
//using NUnit.Framework;
//using Rhino.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SpaceIsFun 
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public partial class Game1 : Game
    {
        #region fields

        /// <summary>
        /// manager for the graphics device
        /// </summary>
        GraphicsDeviceManager graphics;

        /// <summary>
        /// the main spritebatch, use this for drawing
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// the state of the keyboard on the current frame
        /// </summary>
        KeyboardState currentKeyState;

        /// <summary>
        /// the state of the keyboard on the previous frame
        /// </summary>
        KeyboardState previousKeyState;

        /// <summary>
        /// the state of the mouse on the current frame
        /// </summary>
        MouseState currentMouseState;

        /// <summary>
        /// the state of the mouse on the previous frame
        /// </summary>
        MouseState previousMouseState;

        /// <summary>
        /// the state machine handling main game states and their transitions
        /// </summary>
        StateMachine stateMachine;

        State startMenu, battle, overworld, narrative, pauseState;

        /// <summary>
        /// the GUI object
        /// </summary>
        Gui gui;

        Skin skin;
        SpriteFont font;

        /// <summary>
        /// the player's ship object in battle
        /// </summary>
        //Ship playerShip;
        int playerShipUID;

        int enemyShipUID;

        // game object managers
        #region object management
        EntityManager RoomManager = new EntityManager();
        EntityManager GridManager = new EntityManager();
        EntityManager CrewManager = new EntityManager();
        EntityManager WeaponManager = new EntityManager();
        EntityManager ShipManager = new EntityManager();

        Dictionary<int, int> GridToRoom = new Dictionary<int, int>();
        Dictionary<int, int> RoomToShip = new Dictionary<int, int>();
        Dictionary<int, int> WeaponToShip = new Dictionary<int, int>();
        Dictionary<int, int> CrewToShip = new Dictionary<int, int>();
        #endregion

        // definitions for all the textures go here
        #region textures
        Texture2D shipTexture;
        Texture2D energyBar;
        Texture2D healthBar;
        Texture2D gridSprite;
        Texture2D gridHighlightSprite;
        Texture2D energyBarSprite;
        Texture2D roomSprite;
        Texture2D roomHighlightSprite;
        Texture2D pixel;
        Texture2D crewNoAnimate;

        Drawable testDrawable;
        #endregion

        // 0: cursor over no ship
        // 1: cursor over player ship
        // 2: cursor over enemy ship
        int shipCursorFocus;

        /// <summary>
        /// width of the current screen, in pixels
        /// </summary>
        private int screenWidth;
        public int ScreenWidth
        {
            get
            {
                return GraphicsDevice.DisplayMode.Width;
            }
            set
            {
                screenWidth = value;
            }
        }

        /// <summary>
        /// height of the current screen, in pixels
        /// </summary>
        private int screenHeight;
        public int ScreenHeight
        {
            get
            {
                return GraphicsDevice.DisplayMode.Height;
            }
            set
            {
                screenHeight = value;
            }
        }

        List<Crew> crewMembers;
        


        #endregion

        #region constructors / destructors
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            screenWidth = 1024;
            screenHeight = 768;
            Content.RootDirectory = "Content";
        }

        #endregion

        #region game loop methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 

        protected override void Initialize()
        {
            base.Initialize();

            // init managers

            // ALL OF THIS STUFF HAS BEEN MOVED TO LOAD CONTENT

            // maybe move it back when we can... 
            

            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //gui.AddWidget(panel1);

            // load all needed textures here

            #region textures
            shipTexture = Content.Load<Texture2D>("ship1");
            energyBar = Content.Load<Texture2D>("energyBar");
            healthBar = Content.Load<Texture2D>("healthBar");
            gridSprite = Content.Load<Texture2D>("Grid");
            gridHighlightSprite = Content.Load<Texture2D>("GridNotWalkable");
            energyBarSprite = Content.Load<Texture2D>("energyBar");
            roomSprite = Content.Load<Texture2D>("Room2x2");
            roomHighlightSprite = Content.Load<Texture2D>("Room2x2highlight");
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.Green });
            crewNoAnimate = Content.Load<Texture2D>("crewNoAnimate");
            


            #endregion

            
            #region player ship construction

            Vector2 playerShipStartPosition = new Vector2(50,50);
            List<int> gridUIDs = new List<int>();
            List<int> roomUIDs = new List<int>();
            List<int> weaponUIDs = new List<int>();
            int gridWidth = shipTexture.Bounds.Width / 32;
            int gridHeight = shipTexture.Bounds.Height / 32;
            int [,] shipGrid = new int[gridWidth, gridHeight];
            // initialize the player's ship

            // TODO: initialize all objects for a ship outside of the ship itself
            // pass in the UIDs of the grids, rooms, and weapons attributed with this ship

            // grid creation for the player ship
            for (int i = 0; i < shipTexture.Bounds.Width / 32; i++)
            {
                // in each column, iterate over the ship sprite's height
                for (int j = 0; j < shipTexture.Bounds.Height / 32; j++)
                {
                    // create a new grid object for i,j
                    //shipGrid[i, j] = new Grid(gridTexture, highlightTexture, new Vector2(i * 32 + position.X, j * 32 + position.Y), new Vector2(i, j));
                    Grid toAdd = new Grid(gridSprite, gridHighlightSprite,
                               new Vector2(i * 32 + playerShipStartPosition.X, j * 32 + playerShipStartPosition.Y),
                               new Vector2(i, j));

                    int UID = GridManager.AddEntity(toAdd);
                    gridUIDs.Add(UID);
                    shipGrid[i, j] = UID;
                }
            }

            // create rooms, add them to the manager, pass their UIDs to the ship
            int roomUID = RoomManager.AddEntity( new Room( roomHighlightSprite, roomHighlightSprite, 1, 1, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2,2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 2, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);

            bool[] roomTypes = new bool[11];

            for (int i = 0; i < 11; i++)
            {
                roomTypes[i] = false;
            }

            int weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));

            weaponUIDs.Add(weaponUID);

            playerShipUID = ShipManager.AddEntity(new Ship(shipTexture, gridSprite, gridHighlightSprite, playerShipStartPosition, roomUIDs, gridUIDs, weaponUIDs, roomTypes, shipGrid, 0));

            WeaponToShip[weaponUID] = playerShipUID;

            setRoomGridDictionary(playerShipUID);
            setUnwalkableGrids(playerShipUID);

            //playerShip = new Ship(shipTexture, gridSprite, gridHighlightSprite, new Vector2(50, 50), roomUIDs, gridUIDs, weaponUIDs, roomTypes);

            #endregion

            #region enemy ship construction
            Vector2 enemyShipStartPosition = new Vector2(400,50);
            gridUIDs = new List<int>();
            roomUIDs = new List<int>();
            weaponUIDs = new List<int>();
            gridWidth = shipTexture.Bounds.Width / 32;
            gridHeight = shipTexture.Bounds.Height / 32;
            shipGrid = new int[gridWidth, gridHeight];
            // grid creation for the player ship
            for (int i = 0; i < shipTexture.Bounds.Width / 32; i++)
            {
                // in each column, iterate over the ship sprite's height
                for (int j = 0; j < shipTexture.Bounds.Height / 32; j++)
                {
                    // create a new grid object for i,j
                    //shipGrid[i, j] = new Grid(gridTexture, highlightTexture, new Vector2(i * 32 + position.X, j * 32 + position.Y), new Vector2(i, j));
                    Grid toAdd = new Grid(gridSprite, gridHighlightSprite,
                               new Vector2(i * 32 + enemyShipStartPosition.X, j * 32 + enemyShipStartPosition.Y),
                               new Vector2(i, j));

                    int UID = GridManager.AddEntity(toAdd);
                    gridUIDs.Add(UID);
                    shipGrid[i, j] = UID;
                }
            }

            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 1, enemyShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 4, enemyShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);

            roomTypes = new bool[11];

            for (int i = 0; i < 11; i++)
            {
                roomTypes[i] = false;
            }

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 10000, 3));

            weaponUIDs.Add(weaponUID);

            enemyShipUID = ShipManager.AddEntity(new Ship(shipTexture, gridSprite, gridHighlightSprite, enemyShipStartPosition, roomUIDs, gridUIDs, weaponUIDs, roomTypes, shipGrid, 0));
            WeaponToShip[weaponUID] = enemyShipUID;
            setRoomGridDictionary(enemyShipUID);
            setUnwalkableGrids(enemyShipUID);

            #endregion
            // load fonts

            font = Content.Load<SpriteFont>("Calibri");

            // load gui elements

            skin = new Skin(Content.Load<Texture2D>("uiskin"), System.IO.File.ReadAllText("Content/uiskinmap.txt"));

            gui = new Gui(this, skin, new Ruminate.GUI.Framework.Text(font, Color.White));

            // add all text the GUI we may be using here

            gui.AddText("error", new Ruminate.GUI.Framework.Text(font, Color.Red));
            gui.AddText("password", new Ruminate.GUI.Framework.Text(font, Color.TransparentBlack));
            gui.AddText("empty", new Ruminate.GUI.Framework.Text(font, Color.LightSlateGray));


            #region stuff from initialize

            // initialize the state of all input managers
            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            previousKeyState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            // initialize the game state machine and states

            #region state machine setup
            stateMachine = new StateMachine();

            startMenu = new State { Name = "startMenu" };
            battle = new State { Name = "battle" };
            pauseState = new State { Name = "pauseState" };
            overworld = new State { Name = "overworld" };
            narrative = new State { Name = "narrative" };


            startMenu.Transitions.Add(battle.Name, battle);
            startMenu.Transitions.Add(overworld.Name, overworld);
            startMenu.Transitions.Add(pauseState.Name, pauseState);


            battle.Transitions.Add(startMenu.Name, startMenu);
            battle.Transitions.Add(overworld.Name, overworld);
            battle.Transitions.Add(pauseState.Name, pauseState);

            pauseState.Transitions.Add(startMenu.Name, startMenu);
            pauseState.Transitions.Add(battle.Name, battle);

            overworld.Transitions.Add(battle.Name, battle);
            overworld.Transitions.Add(narrative.Name, narrative);
            overworld.Transitions.Add(pauseState.Name, pauseState);

            narrative.Transitions.Add(overworld.Name, overworld);
            narrative.Transitions.Add(pauseState.Name, pauseState);

            

            stateMachine.Start(startMenu);
            #endregion

            // set up any UI elements here

            #region ui setup


            #endregion

            // set up game objects

            crewMembers = new List<Crew>();

            // set up each game state
            setupStartMenu();
            setupBattle(playerShipUID);
            setupPauseState();

            #endregion

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {


        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //screenHeight = GraphicsDevice.DisplayMode.Height;
            //screenWidth = GraphicsDevice.DisplayMode.Width;

            //System.Diagnostics.Debug.WriteLine("ScreenHeight: " + screenHeight.ToString() + "\nScreenWidth: " + screenWidth.ToString());

            // update input managers

            //System.Diagnostics.Debug.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds.ToString());

            

            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;

            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            // if we need to exit, do it now
            if (currentKeyState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // update the GUI

            gui.Update();

            // update the game state machine

            stateMachine.Update(gameTime);

            #region input handling

            // initial pass on game pausing logic

            // if space is pressed
            if (currentKeyState.IsKeyDown(Keys.Space) == true && previousKeyState.IsKeyUp(Keys.Space) == true)
            {
                // if we're in the pause state, transition to the previous state
                if (stateMachine.CurrentState.Name == pauseState.Name)
                {
                    stateMachine.Transition(stateMachine.PreviousState.Name);
                }

                // else, we're not in the pause state, so transition there
                else
                {
                    stateMachine.Transition(pauseState.Name);
                }
            }


            #endregion

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // to start, clear the graphics device of the last frame
            GraphicsDevice.Clear(Color.Black);

            // draw the GUI
            gui.Draw();

            // if we're in the battle state, or paused in the battle state
            if (stateMachine.CurrentState.Name == battle.Name
                || stateMachine.CurrentState.Name == pauseState.Name && stateMachine.PreviousState.Name == battle.Name)
            {
                spriteBatch.Begin();
                ShipManager.Draw(spriteBatch);
                GridManager.Draw(spriteBatch);
                RoomManager.Draw(spriteBatch);



                if (multiSelecting == true)
                {
                    //System.Diagnostics.Debug.WriteLine("multiselecting");
                    // create a rectangle out of the points

                    Point startPoint = new Point();

                    // for both x and y if the end point is "less" than the start point, use the end point's coord as the starting point for the rect

                    // otherwise, use the startpoint's coord

                    if (selectRectStart.X > selectRectEnd.X)
                    {
                        startPoint.X = selectRectEnd.X;
                    }

                    else
                    {
                        startPoint.X = selectRectStart.X;
                    }
 
                    if (selectRectStart.Y > selectRectEnd.Y)
                    {
                        startPoint.Y = selectRectEnd.Y;
                    }

                    else
                    {
                        startPoint.Y = selectRectStart.Y;
                    }


                    Rectangle drawRect = new Rectangle(startPoint.X, startPoint.Y, Math.Abs(selectRectEnd.X - selectRectStart.X), Math.Abs(selectRectEnd.Y - selectRectStart.Y));
                    
                    // draw the rectangle using pixel lines
                    // top, bottom, left, right
                    spriteBatch.Draw(pixel, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, 5), Color.Green);
                    spriteBatch.Draw(pixel, new Rectangle(drawRect.X, drawRect.Y + drawRect.Height - 5, drawRect.Width, 5), Color.Green);
                    spriteBatch.Draw(pixel, new Rectangle(drawRect.X, drawRect.Y, 5, drawRect.Height), Color.Green);
                    spriteBatch.Draw(pixel, new Rectangle((drawRect.X + drawRect.Width - 5), drawRect.Y, 5, drawRect.Height), Color.Green);
                }


                spriteBatch.End();

            }



            
            base.Draw(gameTime);
        }

        #endregion

        #region state methods

        /// <summary>
        /// sets up the game pause state
        /// </summary>
        void setupPauseState()
        {
            // when entering the pause state
            pauseState.enter += () =>
            {
            };

            


            // when updating the pause state
            pauseState.update += (GameTime gameTime) =>
            {

            };

            // when leaving the pause state
            pauseState.leave += () =>
            {
            };
        }

        #endregion

        /// <summary>
        ///  initializes the relationship between grids and their rooms; this is called once before ship construction
        /// </summary>
        public void setRoomGridDictionary(int shipUID)
        {
            Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);

            Dictionary<int, int> roomGridDict = new Dictionary<int, int>();

            foreach (int key in thisShip.RoomUIDList)
            {
                Room room = (Room)RoomManager.RetrieveEntity(key);
                switch (room.RoomShape)
                {
                    // Case for a 2 by 2 room.
                    // x x
                    // x x
                    case Globals.roomShape.TwoXTwo:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 1]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y + 1]] = key;
                        break;

                    // Case for a 3 by 3 room
                    // x x x
                    // x x x
                    // x x x
                    case Globals.roomShape.ThreeXThree:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 2, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 1]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 2]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y + 1]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y + 2]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 2, (int)room.RoomPosition.Y + 1]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 2, (int)room.RoomPosition.Y + 2]] = key;
                        break;

                    // Case for a 1 by 3 room.
                    // x x x
                    case Globals.roomShape.OneXThree:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 2, (int)room.RoomPosition.Y]] = key;
                        break;

                    // Case for a 1 by 2 room
                    // x x
                    case Globals.roomShape.OneXTwo:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y]] = key;
                        break;

                    // Case for a 3 by 1 room
                    // x
                    // x
                    // x
                    case Globals.roomShape.ThreeXOne:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 1]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 2]] = key;
                        break;

                    // Case for a 2 by 1 room
                    // x
                    // x
                    case Globals.roomShape.TwoXOne:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 1]] = key;
                        break;

                    // Case for a J-shaped room
                    //   x
                    // x x
                    case Globals.roomShape.JRoom:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 1]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y + 1]] = key;
                        break;

                    // Case for an R-shaped room
                    // x x
                    // x
                    case Globals.roomShape.RRoom:
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X + 1, (int)room.RoomPosition.Y]] = key;
                        GridToRoom[thisShip.ShipGrid[(int)room.RoomPosition.X, (int)room.RoomPosition.Y + 1]] = key;
                        break;

                }
            }

            //thisShip.RoomGridDict = roomGridDict;

            //return roomGridDict;
            // TODO: possibly un-associate any un-wanted grids with rooms (weirdly-shaped rooms, for example)

        }

        /// <summary>
        /// check whether or not the cursor is current hovering over a ship
        /// </summary>
        /// <param name="currentMouseState">current state of the mouse</param>
        /// <returns>-1 if the cursor isnt hovering over a ship, else returns the UID of the ship its hovering over</returns>
        public int checkShipHover(MouseState currentMouseState)
        {
            var keys = ShipManager.RetrieveKeys();
            
            foreach(var key in keys)
            {
                Ship ship = (Ship)ShipManager.RetrieveEntity(key);

                if (((currentMouseState.X > ship.Sprite.Position2D.X)
                    && (currentMouseState.X < ship.Sprite.Position2D.X + ship.Sprite.Width)
                  && ((currentMouseState.Y > ship.Sprite.Position2D.Y)
                    && (currentMouseState.Y < ship.Sprite.Position2D.Y + ship.Sprite.Height))))
                 {
                    // our mouse cursor should be within the bounds of the ship
                    //System.Diagnostics.Debug.WriteLine("Cursor on the ship!");
                    return key;
                 }


            }
            // if the cursor is between the sprite's topleft and bottomright corners

            return -1;

        }

        /// <summary>
        /// check whether or not a point resides over a ship
        /// </summary>
        /// <param name="point">the point we're checking</param>
        /// <returns>-1 if the cursor isnt hovering over a ship, else returns the UID of the ship its hovering over</returns>
        public int checkShipHover(Point point)
        {
            var keys = ShipManager.RetrieveKeys();

            foreach (var key in keys)
            {
                Ship ship = (Ship)ShipManager.RetrieveEntity(key);

                if (((point.X > ship.Sprite.Position2D.X)
                    && (point.X < ship.Sprite.Position2D.X + ship.Sprite.Width)
                  && ((point.Y > ship.Sprite.Position2D.Y)
                    && (point.Y < ship.Sprite.Position2D.Y + ship.Sprite.Height))))
                {
                    // our mouse cursor should be within the bounds of the ship
                    //System.Diagnostics.Debug.WriteLine("Cursor on the ship!");
                    return key;
                }


            }
            // if the cursor is between the sprite's topleft and bottomright corners

            return -1;

        }

        /// <summary>
        /// check whether or not the cursor is hovering over a room
        /// </summary>
        /// <param name="currentMouseState"></param>
        /// <returns></returns>
        public bool checkRoomHover(MouseState currentMouseState, int shipUID)
        {
            // get the ship

            Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);
            // find the grid we're hovering over

            Vector2 gridHover = getGridHover(currentMouseState, shipUID);

            int gridUID = thisShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y];

            // convert this point to a grid object
            //Grid gridToCheck = (Grid)GridManager.RetrieveEntity(thisShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y]);

            // get the room out of the grid,room dict
            try
            {
                Room roomToCheck = (Room)RoomManager.RetrieveEntity(GridToRoom[gridUID]);
                
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
        public int getRoomHover(MouseState currentMouseState, int shipUID)
        {
            // get the ship
            Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);

            // find the grid we're hovering over

            Vector2 gridHover = getGridHover(currentMouseState, shipUID);

            int gridUID = thisShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y];

            int roomUID = GridToRoom[gridUID];

            // get the room out of the grid,room dict
            return roomUID;
        }


        /// <summary>
        /// sets every grid that doesnt belong to a room as unwalkable
        /// </summary>
        public void setUnwalkableGrids(int shipUID)
        {
            Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);
            for (int i = 0; i < thisShip.ShipGrid.GetLength(0); i++)
            {
                for (int j = 0; j < thisShip.ShipGrid.GetLength(1); j++)
                {
                    // is this grid in the dictionary of grids  that have rooms?
                    // if not, make it unwalkable

                    

                    if (!GridToRoom.ContainsKey(thisShip.ShipGrid[i, j]))
                    {
                        //System.Diagnostics.Debug.WriteLine(shipGrid[i, j].GridPosition.ToString());
                        Grid thisGrid = (Grid)GridManager.RetrieveEntity(thisShip.ShipGrid[i, j]);
                        thisGrid.IsWalkable = false;

                    }
                }
            }
        }

        /// <summary>
        /// check which grid the cursor is currently hovering over; note: this only should get called if checkShipHover returns TRUE
        /// </summary>
        /// <param name="currentMouseState">current state of the mouse</param>
        /// <returns></returns>
        public Vector2 getGridHover(MouseState currentMouseState, int shipUID)
        {
            Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);
            // we know the cursor is within bounds, this will only get called if checkShipHover returns true

            Vector2 ret = new Vector2();

            // x position relative to the ship
            float relativeXPos = currentMouseState.X - thisShip.Sprite.Position2D.X;
            // y position relative to the ship
            float relativeYPos = currentMouseState.Y - thisShip.Sprite.Position2D.Y;

            // grid x position relative to the ship
            ret.X = (int)relativeXPos / 32;

            // grid y position relative to the ship
            ret.Y = (int)relativeYPos / 32;

            return ret;

        }
    }
}