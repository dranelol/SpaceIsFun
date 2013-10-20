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
        #region game definitions
        // don't expect many of these to stay; they will most likely be abstracted out to their respective game states

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

        State startMenu, battle, pauseState;

        /// <summary>
        /// the GUI object
        /// </summary>
        Gui gui;

        Skin skin;
        SpriteFont font;

        /// <summary>
        /// the player's ship object in battle
        /// </summary>
        Ship playerShip;


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
        #endregion
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
        List<Crew> selectedCrewMembers;


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


            startMenu.Transitions.Add(battle.Name, battle);
            startMenu.Transitions.Add(pauseState.Name, pauseState);

            battle.Transitions.Add(startMenu.Name, startMenu);
            battle.Transitions.Add(pauseState.Name, pauseState);

            pauseState.Transitions.Add(startMenu.Name, startMenu);
            pauseState.Transitions.Add(battle.Name, battle);

            stateMachine.Start(startMenu);
            #endregion

            // set up any UI elements here

            #region ui setup


            #endregion

            // set up each game state
            setupStartMenu();
            setupBattle();
            setupPauseState();


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

            // make list of rooms

            Room room1 = new Room(roomHighlightSprite, roomHighlightSprite, 1, 1, Globals.roomShape.TwoXTwo, 2, 2);
            Room room2 = new Room(roomHighlightSprite, roomHighlightSprite, 3, 1, Globals.roomShape.TwoXTwo, 2, 2);
            Room room3 = new Room(roomHighlightSprite, roomHighlightSprite, 2, 3, Globals.roomShape.TwoXTwo, 2, 2);
            Room room4 = new Room(roomHighlightSprite, roomHighlightSprite, 4, 3, Globals.roomShape.TwoXTwo, 2, 2);
            Room room5 = new Room(roomHighlightSprite, roomHighlightSprite, 3, 5, Globals.roomShape.TwoXTwo, 2, 2);

            List<Room> roomList = new List<Room>();
            roomList.Add(room1);
            roomList.Add(room2);
            roomList.Add(room3);
            roomList.Add(room4);
            roomList.Add(room5);

            // initialize the player's ship

            playerShip = new Ship(shipTexture, gridSprite, gridHighlightSprite, new Vector2(50, 50), roomList);

            // load fonts

            font = Content.Load<SpriteFont>("Calibri");

            // load gui elements

            skin = new Skin(Content.Load<Texture2D>("uiskin"), System.IO.File.ReadAllText("Content/uiskinmap.txt"));

            gui = new Gui(this, skin, new Ruminate.GUI.Framework.Text(font, Color.White));

            // add all text the GUI may be using here

            gui.AddText("error", new Ruminate.GUI.Framework.Text(font, Color.Red));
            gui.AddText("password", new Ruminate.GUI.Framework.Text(font, Color.TransparentBlack));
            gui.AddText("empty", new Ruminate.GUI.Framework.Text(font, Color.LightSlateGray));


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



            System.Diagnostics.Debug.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds.ToString());

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
                playerShip.Draw(spriteBatch);

                //foreach (Crew man in crewMembers)
                //{
                //    man.Draw(spriteBatch);
                //}

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



    }




}