using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using NUnit.Framework;
//using Rhino.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


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

        State startMenu, battle, overworld, narrative, pauseState, introState;

        Boolean battleStatus;

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

        int enemyShipUID1;
        int enemyShipUID2;
        

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
        Dictionary<int, int> CrewToRoom = new Dictionary<int, int>();
        Dictionary<int, bool> FilledRooms = new Dictionary<int, bool>();

        #endregion

        // definitions for all the textures go here
        #region textures
        Texture2D shipTexture;
        Texture2D enemyShipTexture1;
        Texture2D enemyShipTexture2;
        Texture2D energyBar;
        Texture2D healthBarFull;
        Texture2D healthBarMed;
        Texture2D healthBarLow;
        Texture2D shieldBubble;
        Texture2D gridSprite;
        Texture2D gridHighlightSprite;
        Texture2D energyBarSprite;
        Texture2D roomSprite;
        Texture2D roomHighlightSprite;
        Texture2D pixel;
        Texture2D crewNoAnimate;
        Texture2D starTexture;
        Texture2D overworldCursorTexture;
        Texture2D starGreyedTexture;

        Drawable testDrawable;
        #endregion
        
        // definitions for all the sounds go here!
        #region sounds

        SoundEffect weaponsSelected;
        SoundEffect weaponsDeselected;
        SoundEffect menuClick;
        SoundEffect IntroMusic;
        SoundEffect BattleMusic;

        #endregion

        // 0: cursor over no ship
        // 1: cursor over player ship
        // 2: cursor over enemy ship
        int shipCursorFocus;

        //UID Lists
        List<int> gridUIDs = new List<int>();
        List<int> roomUIDs = new List<int>();
        List<int> weaponUIDs = new List<int>();
        List<int> filledRoomUIDs = new List<int>();

        //Grid Size Settings
        int gridWidth;
        int gridHeight;
        int[,] shipGrid;


        // ship start offset
        Vector2 playerShipStartPosition = new Vector2(25, 175);
        Vector2 enemyShip1StartPosition = new Vector2(660, 490);
        Vector2 enemyShip2StartPosition = new Vector2(660, 490);

        public bool battle1Resolved = false;
        public bool battle2Resolved = false;
        public bool battle1Result = false;
        public bool narrative1Resolved = false;
        public bool narrative2Resolved = false;

        public int weaponSlotsIndex;
        public int selectedWeaponUID;

        public bool weapon1Enabled = false;
        public bool weapon1Selected = false;
        public bool weapon1Disabled = true;

        public bool weapon2Enabled = false;
        public bool weapon2Selected = false;
        public bool weapon2Disabled = true;

        public bool weapon3Enabled = false;
        public bool weapon3Selected = false;
        public bool weapon3Disabled = true;

        public bool weapon4Enabled = false;
        public bool weapon4Selected = false;
        public bool weapon4Disabled = true;

        public bool weapon5Enabled = false;
        public bool weapon5Selected = false;
        public bool weapon5Disabled = true;

        Drawable overworldCursorDraw;

        public bool masterGameEnd = false;

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


        int gameStateUID;
        

        #endregion

        #region constructors / destructors
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight =1024;
            screenWidth = 1200;
            screenHeight = 1024;
            Content.RootDirectory = "Content";
			Console.WriteLine("Muahahaha!");
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
            shipTexture = Content.Load<Texture2D>("ship01");
            enemyShipTexture1 = Content.Load<Texture2D>("ship02Flipped");
            enemyShipTexture2 = Content.Load<Texture2D>("ship03Flipped");
            energyBar = Content.Load<Texture2D>("energyBar");
            healthBarFull = Content.Load<Texture2D>("healthBarFull");
            healthBarMed = Content.Load<Texture2D>("healthBarMed");
            healthBarLow = Content.Load<Texture2D>("healthBarLow");
            shieldBubble = Content.Load<Texture2D>("shieldBubble");
            gridSprite = Content.Load<Texture2D>("Grid");
            gridHighlightSprite = Content.Load<Texture2D>("GridNotWalkable");
            energyBarSprite = Content.Load<Texture2D>("energyBar");
            roomSprite = Content.Load<Texture2D>("Room2x2");
            roomHighlightSprite = Content.Load<Texture2D>("Room2x2");
            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.Green });
            crewNoAnimate = Content.Load<Texture2D>("crewBlue");
            starTexture = Content.Load<Texture2D>("starNode");
            overworldCursorTexture = Content.Load<Texture2D>("overworldCursor");
            starGreyedTexture = Content.Load<Texture2D>("starNodeGreyed");
            #endregion


            #region sounds

            weaponsSelected = Content.Load<SoundEffect>("WeaponsSelected");
            weaponsDeselected = Content.Load<SoundEffect>("WeaponsDeselected");
            menuClick = Content.Load<SoundEffect>("MenuClick");
            IntroMusic = Content.Load<SoundEffect>("SpaceIsFunIntro");
            BattleMusic = Content.Load<SoundEffect>("SpaceIsFunBatlle");

            #endregion

            #region player ship construction

            //Vector2 playerShipStartPosition = new Vector2(50,50);
            
            gridWidth = shipTexture.Bounds.Width / 32;
            gridHeight = shipTexture.Bounds.Height / 32;
            shipGrid = new int[gridWidth, gridHeight];
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
            int roomUID = RoomManager.AddEntity( new Room( roomHighlightSprite, roomHighlightSprite, 3, 1, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2,2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 3, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 5, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 5, 2, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 5, 4, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 7, 2, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 7, 4, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 9, 3, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 11, 3, playerShipStartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);

            bool[] roomTypes = new bool[11];

            for (int i = 0; i < 11; i++)
            {
                roomTypes[i] = false;
            }



            int weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 2, 500, 3));
            weaponUIDs.Add(weaponUID);
            

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 2, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 2, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 2, 500, 3));
            weaponUIDs.Add(weaponUID);


            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 2, 500, 3));
            weaponUIDs.Add(weaponUID);

            System.Diagnostics.Debug.WriteLine(weaponUIDs.Count);
                

            playerShipUID = ShipManager.AddEntity(new Ship(shipTexture, gridSprite, gridHighlightSprite, playerShipStartPosition, roomUIDs, gridUIDs, weaponUIDs, roomTypes, shipGrid, 0));

            
            foreach (var item in weaponUIDs)
            {
                WeaponToShip[item] = playerShipUID;
            }
          

            setRoomGridDictionary(playerShipUID);
            setRoomToShipDictionary(playerShipUID, roomUIDs);
            setUnwalkableGrids(playerShipUID);
            filledRoomUIDs = setCrewDictionary(playerShipUID);
            setFilledDict(playerShipUID, filledRoomUIDs);


            //playerShip = new Ship(shipTexture, gridSprite, gridHighlightSprite, new Vector2(50, 50), roomUIDs, gridUIDs, weaponUIDs, roomTypes);




            #endregion

            

            #region enemy ship construction 1

            /*
            //Vector2 enemyShipStartPosition = new Vector2(400,50);

            Vector2 enemyShipStartPosition;
            enemyShipStartPosition = new Vector2(400, 50);

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
                               new Vector2(i * 32 + enemyShip1StartPosition.X, j * 32 + enemyShip1StartPosition.Y),
                               new Vector2(i, j));

                    int UID = GridManager.AddEntity(toAdd);
                    gridUIDs.Add(UID);
                    shipGrid[i, j] = UID;
                }
            }

            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 1, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 4, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);

            roomTypes = new bool[11];

            for (int i = 0; i < 11; i++)
            {
                roomTypes[i] = false;
            }

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            System.Diagnostics.Debug.WriteLine(weaponUIDs.Count);


            enemyShipUID1 = ShipManager.AddEntity(new Ship(shipTexture, gridSprite, gridHighlightSprite, playerShipStartPosition, roomUIDs, gridUIDs, weaponUIDs, roomTypes, shipGrid, 0));


            foreach (var item in weaponUIDs)
            {
                WeaponToShip[item] = enemyShipUID1;
            }
            
            
            WeaponToShip[weaponUID] = enemyShipUID1;
            setRoomGridDictionary(enemyShipUID1);
            setUnwalkableGrids(enemyShipUID1);
            */
            #endregion

            #region enemy ship construction 2
            /*
            //enemyShipStartPosition = new Vector2(400, 50);
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
                               new Vector2(i * 32 + enemyShip2StartPosition.X, j * 32 + enemyShip2StartPosition.Y),
                               new Vector2(i, j));

                    int UID = GridManager.AddEntity(toAdd);
                    gridUIDs.Add(UID);
                    shipGrid[i, j] = UID;
                }
            }

            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 1, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);
            roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 4, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
            roomUIDs.Add(roomUID);

            roomTypes = new bool[11];

            for (int i = 0; i < 11; i++)
            {
                roomTypes[i] = false;
            }

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            weaponUID = WeaponManager.AddEntity(new Weapon(gridSprite, 0, 0, 10, 500, 3));
            weaponUIDs.Add(weaponUID);

            System.Diagnostics.Debug.WriteLine(weaponUIDs.Count);


            enemyShipUID2 = ShipManager.AddEntity(new Ship(shipTexture, gridSprite, gridHighlightSprite, playerShipStartPosition, roomUIDs, gridUIDs, weaponUIDs, roomTypes, shipGrid, 0));


            foreach (var item in weaponUIDs)
            {
                WeaponToShip[item] = enemyShipUID2;
            }


            WeaponToShip[weaponUID] = enemyShipUID2;
            setRoomGridDictionary(enemyShipUID2);
            setUnwalkableGrids(enemyShipUID2);
            */
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
            gui.AddText("Oh, so you’re off to explore space are you? Good for you, though you might be forewarned that the Demonstrably Erratic Mostly-flying Object spacecraft that you’ve been assigned might be a bit, err, unique",
                new Ruminate.GUI.Framework.Text(font, Color.White));


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
            introState = new State { Name = "introState" };


            startMenu.Transitions.Add(battle.Name, battle);
            startMenu.Transitions.Add(overworld.Name, overworld);
            startMenu.Transitions.Add(pauseState.Name, pauseState);
            startMenu.Transitions.Add(introState.Name, introState);


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
            narrative.Transitions.Add(battle.Name, battle);

            introState.Transitions.Add(overworld.Name, overworld);

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
            setupOverworld();

            setupIntro();


            setupNarrative();
            overworldCursorDraw = new Drawable(overworldCursorTexture, cursorCoords);

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
                /*
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
                 * */


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

            if (stateMachine.CurrentState.Name == overworld.Name)
            {
                spriteBatch.Begin();
                foreach (var item in starNodeDraws)
                {
                    item.Update(gameTime);

                    item.Draw(spriteBatch);
                }
                
                overworldCursorDraw.Draw(spriteBatch);
                spriteBatch.End();
            }

            // if we're in the battle state, or paused in the battle state
            if (stateMachine.CurrentState.Name == battle.Name
                || stateMachine.CurrentState.Name == pauseState.Name && stateMachine.PreviousState.Name == battle.Name)
            {
                spriteBatch.Begin();

                
                Ship playerShip = (Ship)ShipManager.RetrieveEntity(playerShipUID);
                playerShip.Draw(spriteBatch);

                // draw different stuff based on the current gamestate
                
                switch (gameStateUID)
                {
                    case 0:
                        //this is battle one
                        Ship enemyShip1 = (Ship)ShipManager.RetrieveEntity(enemyShipUID1);
                        enemyShip1.Draw(spriteBatch);
                        break;
                    case 1:
                        //this is narrative one

                        break;
                    case 2:
                        //this is battle two
                        Ship enemyShip2 = (Ship)ShipManager.RetrieveEntity(enemyShipUID2);
                        //System.Diagnostics.Debug.WriteLine("enemy ship id1:" + playerShipUID);
                        //System.Diagnostics.Debug.WriteLine("enemy ship id1:"+enemyShipUID1);
                        //System.Diagnostics.Debug.WriteLine("enemy ship id2:" + enemyShipUID2);


                        

                        

                        enemyShip2.Draw(spriteBatch);
                        break;
                    case 3:
                        //this is narrative two

                        break;
                    default:
                        
                        break;
                        
                }



                GridManager.Draw(spriteBatch);
                RoomManager.Draw(spriteBatch);

                //System.Diagnostics.Debug.WriteLine("before crew draw");
                CrewManager.Draw(spriteBatch);
                //System.Diagnostics.Debug.WriteLine("after crew draw");
                WeaponManager.Draw(spriteBatch);



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
                
                // Rebecca's room grids
                // each room is built with one grid at a time within the grid map
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


        public List<int> setCrewDictionary(int shipUID)
        {
            Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);

            

            // roomShipKeys: current room UIDs in current ship
            List<int> roomShipKeys = new List<int>();

            // gridRoomKeys: current grid UIDs in rooms on current ship
            List<int> gridRoomKeys = new List<int>();


            // This is looping over every room that exists.
            //System.Diagnostics.Debug.WriteLine("RoomToShip: " + RoomToShip.Keys.Count.ToString());    
            foreach (int i in RoomToShip.Keys)
            {
                if (RoomToShip[i] == shipUID)
                {
                    //System.Diagnostics.Debug.WriteLine("This is getting the rooms: "+i.ToString());                    
                    roomShipKeys.Add(i);
                }
            }

            // This is looping over Grids in every room that exists.
            foreach (int i in GridToRoom.Keys)
            {
                if (roomShipKeys.Contains(GridToRoom[i]))
                {
                    //System.Diagnostics.Debug.WriteLine("This is getting the grids: " + i.ToString());
                    gridRoomKeys.Add(i);
                }
            }


            int mans = 0;

            List<int> filledRoomUIDs = new List<int>();

            foreach (int i in gridRoomKeys)
            {
                if (mans == 3)
                {
                    break;
                }

                Grid thisGrid = (Grid)GridManager.RetrieveEntity(i);

                Crew newguy = new Crew(thisGrid.Sprite.Position2D, thisGrid.GridPosition, crewNoAnimate, crewNoAnimate);

                int crewUID = CrewManager.AddEntity(newguy);

                CrewToShip[crewUID] = shipUID;
                CrewToRoom[crewUID] = GridToRoom[i];

                filledRoomUIDs.Add(i);


                mans++;
            }



            return filledRoomUIDs;

        }

        public void setRoomToShipDictionary(int shipUID, List<int> roomUIDs)
        {
            foreach (int i in roomUIDs)
            {
                RoomToShip[i] = shipUID;
            }

        }


        //For now, just appends current state to save file
        public void saveState(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fileName, FileMode.Append);
            formatter.Serialize(stream, stateMachine.CurrentState);
            stream.Close();
        }

        public void saveData(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fileName, FileMode.Create);
            stream.Close();  //This is a bit odd but
            //Each function appends to the file.  Opening and closing here
            //just erases the previous file if there was one, and creates a new one.
            //Closes file so there aren't multiple streams to the same file open.

            saveState(fileName);

            //These dump the dictionary<int, Entity> objects from EntityManager
            RoomManager.dumpObjects(fileName);
            GridManager.dumpObjects(fileName);
            CrewManager.dumpObjects(fileName);
            WeaponManager.dumpObjects(fileName);
            ShipManager.dumpObjects(fileName);

            FileStream stream2 = new FileStream(fileName, FileMode.Append);
            formatter.Serialize(stream2, GridToRoom);
            formatter.Serialize(stream2, RoomToShip);
            formatter.Serialize(stream2, WeaponToShip);
            formatter.Serialize(stream2, CrewToShip);
            formatter.Serialize(stream2, CrewToRoom);
            formatter.Serialize(stream2, FilledRooms);

            stream2.Close();


        }

        public void loadData(string fileName)
        {
            //Load and set currentState in stateMachine
            //May need to do more

            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fileName, FileMode.Open);
            State new_CurrentState = (State)formatter.Deserialize(stream);
            //TODO transition state machine to this state
            //
            //

            //Loads "objects" dictionary from save to Entity managers. Keys and Values will be transferred from save file.
            RoomManager.setObjects((Dictionary<int, Entity>)formatter.Deserialize(stream));
            GridManager.setObjects((Dictionary<int, Entity>)formatter.Deserialize(stream));
            CrewManager.setObjects((Dictionary<int, Entity>)formatter.Deserialize(stream));
            WeaponManager.setObjects((Dictionary<int, Entity>)formatter.Deserialize(stream));
            ShipManager.setObjects((Dictionary<int, Entity>)formatter.Deserialize(stream));

            Dictionary<int, int> temp;
            Dictionary<int, bool> temp2;
            temp = (Dictionary<int, int>)formatter.Deserialize(stream);
            GridToRoom = new Dictionary<int, int>(temp);
            temp = (Dictionary<int, int>)formatter.Deserialize(stream);
            RoomToShip = new Dictionary<int, int>(temp);
            temp = (Dictionary<int, int>)formatter.Deserialize(stream);
            WeaponToShip = new Dictionary<int, int>(temp);
            temp = (Dictionary<int, int>)formatter.Deserialize(stream);
            CrewToShip = new Dictionary<int, int>(temp);
            temp = (Dictionary<int, int>)formatter.Deserialize(stream);
            CrewToRoom = new Dictionary<int, int>(temp);
            temp2 = (Dictionary<int, bool>)formatter.Deserialize(stream);
            FilledRooms = new Dictionary<int, bool>(temp2);
        }


        public void setFilledDict(int shipUID, List<int> filledRoomUIDs)
        {

            var grids = GridManager.RetrieveKeys();
            
            foreach (int i in grids)
            {
                if (filledRoomUIDs.Contains(i))
                {
                    FilledRooms[i] = true;                    
                }
                else
                {
                    FilledRooms[i] = false;
                }

            }
        }
        
        public void dealDamage(int shipUID, int weaponUID)
        {
            Ship targetShip = (Ship)ShipManager.RetrieveEntity(shipUID);
            Weapon targetWeap = (Weapon)WeaponManager.RetrieveEntity(weaponUID);

            //figure out wait time and drawable nonsense

            targetShip.TakeDamage(targetWeap.Damage);
            System.Diagnostics.Debug.WriteLine("hp after damage: " + targetShip.CurrentHP.ToString());
        }

    }
}
