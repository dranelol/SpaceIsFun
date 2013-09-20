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

namespace SpaceIsFun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
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

        Texture2D shipTexture;
        Texture2D energyBar;
        Texture2D healthBar;
        Texture2D gridSprite;
        Texture2D gridHighlightSprite;
        Texture2D energyBarSprite;
        Texture2D roomSprite;
        Texture2D roomHighlightSprite;

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
            stateMachine = new StateMachine(this);

            startMenu = new State { Name = "startMenu" };
            battle = new State { Name = "battle" };
            pauseState = new State { Name = "pauseState" };

            stateMachine.Start(startMenu);

            startMenu.Transitions.Add(battle.Name, battle);
            startMenu.Transitions.Add(pauseState.Name, pauseState);

            battle.Transitions.Add(startMenu.Name, startMenu);
            battle.Transitions.Add(pauseState.Name, pauseState);

            pauseState.Transitions.Add(startMenu.Name, startMenu);
            pauseState.Transitions.Add(battle.Name, battle);

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
            gridHighlightSprite = Content.Load<Texture2D>("GridHighlight");
            energyBarSprite = Content.Load<Texture2D>("energyBar");
            roomSprite = Content.Load<Texture2D>("Room2x2");
            roomHighlightSprite = Content.Load<Texture2D>("Room2x2highlight");

            #endregion

            // initialize the player's ship

            playerShip = new Ship(shipTexture, gridSprite, gridHighlightSprite, new Vector2(50, 50));

            // load fonts

            font = Content.Load<SpriteFont>("Calibri");

            // load gui elements

            skin = new Skin(Content.Load<Texture2D>("uiskin"), System.IO.File.ReadAllText("Content/uiskinmap.txt"));
            
            gui = new Gui(this, skin, new Text(font, Color.White));

            // add all text the GUI may be using here

            gui.AddText("error", new Text(font, Color.Red));
            gui.AddText("password", new Text(font, Color.TransparentBlack));
            gui.AddText("empty", new Text(font, Color.LightSlateGray));

            
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

            // if we're in the battle state, or paused in the battle state, draw the ship
            if (stateMachine.CurrentState.Name == battle.Name
                || stateMachine.CurrentState.Name == pauseState.Name && stateMachine.PreviousState.Name == battle.Name)
            {
                spriteBatch.Begin();
                playerShip.Draw(spriteBatch);
                spriteBatch.End();
                
            }

            
            base.Draw(gameTime);
        }

        #endregion

        #region state methods
        /// <summary>
        /// sets up the game menu state
        /// </summary>
        void setupStartMenu()
        {
            // this panel will hold all the login GUI objects
            Widget loginpanel = new Panel(100, 100, 300, 140);

            // this adds a button labelled "Play" that will transition to the battle state when clicked
            loginpanel.AddWidget(new Button(50, 100, "Play", 2, (Widget widget) =>
            {
                stateMachine.Transition(battle.Name);
            }));
            
            // when entering start menu state
            startMenu.enter += () =>
            {
                // add the login widget to the gui
                gui.AddWidget(loginpanel);
            };

            // when updating start menu state
            startMenu.update += (GameTime gameTime) =>
            {
                #region input handling
                // if b is pressed, transition to the battle state
                if (currentKeyState.IsKeyDown(Keys.B))
                {
                    stateMachine.Transition(battle.Name);
                }
                #endregion

            };

            // when leaving the start menu state
            startMenu.leave += () =>
            {
                // remove the login widget from the gui
                gui.RemoveWidget(loginpanel);
            };
        }

        /// <summary>
        /// sets up the game battle state
        /// </summary>
        void setupBattle()
        {
            // sets up seven energy bars for the ship
            Panel energy1 = new Panel(4, screenHeight-128, 40, 128 - 8);
            Panel energy2 = new Panel(64+4, screenHeight-128, 40, 128 - 8);
            Panel energy3 = new Panel(128+4, screenHeight-128, 40, 128 - 8);
            Panel energy4 = new Panel(192+4, screenHeight-128, 40, 128 - 8);
            Panel energy5 = new Panel(256+4, screenHeight-128, 40, 128 - 8);
            Panel energy6 = new Panel(320+4, screenHeight-128, 40, 128 - 8);
            Panel energy7 = new Panel(384+4, screenHeight-128, 40, 128 - 8);

            Image energyBar1;

            // this list will hold the individual bars within one energy bar
            List<Widget> energyBarTest = new List<Widget>();

            int shipStartEnergy = playerShip.Energy;

            Point currentlySelectedPlayerGrid = new Point(-1, -1);
            Point currentlySelectedEnemyGrid = new Point(-1, -1);
            
            // when entering the battle state
            battle.enter += () =>
            {
                // adds all the energy bars to the gui
                gui.AddWidget(energy1);
                gui.AddWidget(energy2);
                gui.AddWidget(energy3);
                gui.AddWidget(energy4);
                gui.AddWidget(energy5);
                gui.AddWidget(energy6);
                gui.AddWidget(energy7);

                // add as many energy widgets as there is ship energy to one entire energy bar
                for (int i = 0; i < playerShip.Energy; i++)
                {
                    energy1.AddWidget(energyBar1 = new Image(0, (128 - 16 - 8 - 8) - i * 16, energyBarSprite));
                    energyBarTest.Add(energyBar1);
                }
            };

            // when updating the battle state
            battle.update += (GameTime gameTime) =>
            {
                #region input handling

                // if the a key is pressed, transition back to the menu
                if (currentKeyState.IsKeyDown(Keys.A))
                {
                    stateMachine.Transition(startMenu.Name);
                }

                // if the c key is tapped, query to see if the cursor is hovering over the ship
                if (currentKeyState.IsKeyDown(Keys.C) && previousKeyState.IsKeyUp(Keys.C))
                {
                    // returns whether or not the cursor is hovering over the player's ship
                    bool shipHover = playerShip.checkShipHover(currentMouseState);
                    
                    // if the cursor is hovering over the player's ship, print a message and figure out which room the cursor is in
                    if (shipHover == true)
                    {
                        System.Diagnostics.Debug.WriteLine("Cursor on ship!");

                        // returns which grid (in ship grid coords) the cursor is hovering over
                        Vector2 gridHover = playerShip.checkGridHover(currentMouseState);

                        // if gridHover isn't (-1,-1), which means the cursor ISNT on the grid, print messages, and highlight (or un-highlight) that grid 
                        if (gridHover.X != -1 && gridHover.Y != -1)
                        {
                            System.Diagnostics.Debug.WriteLine("Cursor on grid: " + playerShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y].GridPosition.ToString());
                            // highlight that grid
                            //playerShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y].Highlight();

                            //System.Diagnostics.Debug.WriteLine("Highighted?: " + playerShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y].Highlighted.ToString());

                        }
                    }
                }

                // if the e key is tapped, try to lose energy if possible
                if (currentKeyState.IsKeyDown(Keys.E) == true && previousKeyState.IsKeyUp(Keys.E) == true)
                {
                    if (playerShip.Energy > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("lost energy!");
                        playerShip.Energy = playerShip.Energy - 1;
                    }

                    // iterate over the energy widgets for the first energy bar, and make the current level invisible
                    for (int i = 0; i < shipStartEnergy; i++)
                    {
                        if (i >= playerShip.Energy)
                        {
                            energyBarTest[i].Visible = false;
                        }
                    }

                }

                // if the f key is tapped, try to gain energy if possibile
                if (currentKeyState.IsKeyDown(Keys.R) == true && previousKeyState.IsKeyUp(Keys.R) == true)
                {
                    if (playerShip.Energy < 5)
                    {
                        System.Diagnostics.Debug.WriteLine("gained energy!");
                        playerShip.Energy = playerShip.Energy + 1;
                    }

                    // iterate over the energy widgets for the first energy bar, and make the one above the current level visible again
                    for (int i = 0; i < shipStartEnergy; i++)
                    {
                        if (i < playerShip.Energy)
                        {
                            energyBarTest[i].Visible = true;
                        }
                    }
                }
                #endregion
            


            };

            // when leaving the battle state
            battle.leave += () =>
            {
                
                // remove the energy widgets from the gui
                gui.RemoveWidget(energy1);
                gui.RemoveWidget(energy2);
                gui.RemoveWidget(energy3);
                gui.RemoveWidget(energy4);
                gui.RemoveWidget(energy5);
                gui.RemoveWidget(energy6);
                gui.RemoveWidget(energy7);
            };
        }

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
