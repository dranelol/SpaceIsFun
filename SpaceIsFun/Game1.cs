#region Using Statements
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
#endregion

namespace SpaceIsFun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        StateMachine stateMachine;
        State startMenu, battle, pauseState;
        Gui gui = null;
        Skin skin;
        SpriteFont font;
        Widget panel1;

        Ship playerShip;

        Texture2D shipTexture;
        Texture2D energyBar;
        Texture2D healthBar;
        
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

        

        public Game1()
            : base()
        {
            Console.WriteLine("Happy Days!");
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            screenWidth = 1024;
            screenHeight = 768;
            Content.RootDirectory = "Content";
        }

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

            // TODO: Add your initialization logic here
            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            previousKeyState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

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


            
            
           

            #region ui setup

            

            //gui.AddText("error", new Text(font, Color.Red));
            //gui.AddText("password", new Text(font, Color.TransparentBlack));
            //gui.AddText("empty", new Text(font, Color.LightSlateGray));

            //panel1 = new Panel(200, 100, ScreenWidth / 2, ScreenHeight / 2);
            //Label subLabel1 = new Label(50, 25, "Test1");
            //Label subLabel2 = new Label(50, 75, "Test2");

            //panel1.AddWidget(subLabel1);
            //panel1.AddWidget(subLabel2);

            

            #endregion


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
            // TODO: use this.Content to load your game content here
            shipTexture = Content.Load<Texture2D>("ship1");
            energyBar = Content.Load<Texture2D>("energyBar");
            healthBar = Content.Load<Texture2D>("healthBar");
            playerShip = new Ship(shipTexture, new Vector2(50, 50));
            font = Content.Load<SpriteFont>("Calibri");

            
            skin = new Skin(Content.Load<Texture2D>("uiskin"), System.IO.File.ReadAllText("Content/uiskinmap.txt"));
            
            gui = new Gui(this, skin, new Text(font, Color.White));

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
            // TODO: Unload any non ContentManager content here

            Console.WriteLine("asdasd");
            Console.WriteLine("asdasd");
            Console.WriteLine("asdasd");
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

            previousKeyState = currentKeyState;
            previousMouseState = currentMouseState;

            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            gui.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.Escape))
                Exit();

            if (currentKeyState.IsKeyDown(Keys.Space) == true && previousKeyState.IsKeyUp(Keys.Space) == true)
            {
                if (stateMachine.CurrentState.Name == pauseState.Name)
                {
                    stateMachine.Transition(stateMachine.PreviousState.Name);
                }

                else
                {
                    stateMachine.Transition(pauseState.Name);
                }
            }

            // TODO: Add your update logic here

            stateMachine.Update(gameTime);

            if (stateMachine.CurrentState.Name == battle.Name
                || stateMachine.CurrentState.Name == pauseState.Name && stateMachine.PreviousState.Name == battle.Name)
            {
                
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            if (stateMachine.CurrentState.Name == battle.Name
                || stateMachine.CurrentState.Name == pauseState.Name && stateMachine.PreviousState.Name == battle.Name)
            {
                spriteBatch.Begin();
                playerShip.Draw(spriteBatch);
                spriteBatch.End();
                
            }

            gui.Draw();
            base.Draw(gameTime);
        }

        #endregion

        #region state methods

        void setupStartMenu()
        {
           
            Widget loginpanel = new Panel(100, 100, 300, 140);



        

            loginpanel.AddWidget(new Button(50, 100, "Play", 2, (Widget widget) =>
            {
                stateMachine.Transition(battle.Name);
            }));
            
            startMenu.enter += () =>
            {
                gui.AddWidget(loginpanel);
            };

            startMenu.update += (GameTime gameTime) =>
            {
                
                if (currentKeyState.IsKeyDown(Keys.B))
                {
                    stateMachine.Transition(battle.Name);
                }
                
               
                 
            };

            startMenu.leave += () =>
            {
                gui.RemoveWidget(loginpanel);
            };
        }

        void setupBattle()
        {

            Texture2D energyBarSprite = Content.Load<Texture2D>("energyBar");

            Panel energy1 = new Panel(4, screenHeight-128, 40, 128 - 8);
            Panel energy2 = new Panel(64+4, screenHeight-128, 40, 128 - 8);
            Panel energy3 = new Panel(128+4, screenHeight-128, 40, 128 - 8);
            Panel energy4 = new Panel(192+4, screenHeight-128, 40, 128 - 8);
            Panel energy5 = new Panel(256+4, screenHeight-128, 40, 128 - 8);
            Panel energy6 = new Panel(320+4, screenHeight-128, 40, 128 - 8);
            Panel energy7 = new Panel(384+4, screenHeight-128, 40, 128 - 8);


            Image energyBar1;




            List<Widget> energyBarTest = new List<Widget>();

            int shipStartEnergy = playerShip.Energy;
            
            battle.enter += () =>
            {
                gui.AddWidget(energy1);
                gui.AddWidget(energy2);
                gui.AddWidget(energy3);
                gui.AddWidget(energy4);
                gui.AddWidget(energy5);
                gui.AddWidget(energy6);
                gui.AddWidget(energy7);

                

                for (int i = 0; i < playerShip.Energy; i++)
                {
                    energy1.AddWidget(energyBar1 = new Image(0, (128 - 16 - 8 - 8) - i * 16, energyBarSprite));
                    energyBarTest.Add(energyBar1);
                }

                

                

                

            };

            battle.update += (GameTime gameTime) =>
            {
                


                
                System.Diagnostics.Debug.WriteLine(playerShip.Energy.ToString());

                

                if (currentKeyState.IsKeyDown(Keys.A))
                {
                    stateMachine.Transition(startMenu.Name);
                }

                if (currentKeyState.IsKeyDown(Keys.E) == true && previousKeyState.IsKeyUp(Keys.E) == true)
                {
                    System.Diagnostics.Debug.WriteLine("lost energy!");
                    if (playerShip.Energy >= 0)
                    {
                        playerShip.Energy = playerShip.Energy - 1;
                    }

                    for (int i = 0; i < shipStartEnergy; i++)
                    {
                        if (i >= playerShip.Energy)
                        {
                            energyBarTest[i].Visible = false;
                        }
                    }




                }
                if (currentKeyState.IsKeyDown(Keys.R) == true && previousKeyState.IsKeyUp(Keys.R) == true)
                {
                    System.Diagnostics.Debug.WriteLine("gained energy!");
                    if (playerShip.Energy <= 5)
                    {
                        playerShip.Energy = playerShip.Energy + 1;
                    }

                    for (int i = 0; i < shipStartEnergy; i++)
                    {
                        if (i < playerShip.Energy)
                        {
                            energyBarTest[i].Visible = true;
                        }
                    }
                }

                
            };

            battle.leave += () =>
            {
                

                gui.RemoveWidget(energy1);
                gui.RemoveWidget(energy2);
                gui.RemoveWidget(energy3);
                gui.RemoveWidget(energy4);
                gui.RemoveWidget(energy5);
                gui.RemoveWidget(energy6);
                gui.RemoveWidget(energy7);
            };
        }

        void setupPauseState()
        {
            pauseState.enter += () =>
            {
            };

            pauseState.update += (GameTime gameTime) =>
            {

            };

            pauseState.leave += () =>
            {
            };
        }

        #endregion
    }
}
