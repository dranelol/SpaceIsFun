using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;

namespace SpaceIsFun
{
    public partial class Game1 : Game
    {

        /// <summary>
        /// sets up the game battle state
        /// </summary>
        /// 

        int roomUID;
        bool[] roomTypes;
        int weaponUID;
        Weapon selectedWeapon;
        bool selectedEnemy;
        Ship name;
        bool multiSelecting = false;
        Point selectRectStart = new Point();
        Point selectRectEnd = new Point();

        Ship enemyShip;

        List<int> currentEnemyShips = new List<int>();

        SoundEffectInstance ThisBattleSong;

        

        void setupBattle(int playerUID)
        {
            Vector2 target1 = new Vector2();

            Vector2 target2 = new Vector2();

            Vector2 target1Screen = new Vector2();
            Vector2 target2Screen = new Vector2();
            List<int> selectedCrewMembers = new List<int>();
            // mapping of gridID to true or false; whether or not this grid has a crew in it 
            Dictionary<int, bool> gridFilled = new Dictionary<int, bool>();

            bool target1Selected = false;
            bool target2Selected = false;

            

            
            Ship playerShip = (Ship)ShipManager.RetrieveEntity(playerUID);
            

            /*
            enemyShip = (Ship)ShipManager.RetrieveEntity(0);

            if (gameStateUID == 0)
            {
                enemyShip = (Ship)ShipManager.RetrieveEntity(enemyShipUID1);
                currentEnemyShips.Add(enemyShipUID1);
            }

            else if (gameStateUID == 2)
            {
                enemyShip = (Ship)ShipManager.RetrieveEntity(enemyShipUID2);
                currentEnemyShips.Add(enemyShipUID2);
            }
            */
            Pathfinder pather = new Pathfinder(playerShip.ShipGrid, playerShipStartPosition, GridManager);


            // sets up seven energy bars for the ship
            Panel energy1 = new Panel(4, screenHeight - 256, 40, 256 - 8);

            /*Lance's code*/
            Panel Health = new Panel(5, 5, (32 * playerShip.MaxHP) + 8, 72);
            Panel Shields = new Panel(5, 80, (32 * playerShip.MaxShields) + 8, 40);

            Panel enemyHealth = new Panel(screenWidth-((32 * playerShip.MaxHP) + 8), screenHeight - 77, (32 * playerShip.MaxHP) + 8, 72);

            /*END Lance's code*/
            
            Panel endBattle = new Panel((screenWidth / 2) - (250 / 2), (screenHeight / 2) - (85 / 2), 250, 85);
            Label endMessageGood = new Label(5,5, "You have defeated the enemy!");
            Label endMessagebad = new Label(5,5, "You have been defeated!");

            Panel wpnEnable = new Panel(500-(32*5), screenHeight - 128, 416 + 8, 75);

            Image energyBar1;
            Image HealthBar;
            Image sBubble;

            // this list will hold the individual bars within one energy bar
            List<Widget> energyBarTest = new List<Widget>();
            List<Widget> healthBarTest = new List<Widget>();
            List<Widget> enemyhealthBarTest = new List<Widget>();
            List<Widget> shieldTest = new List<Widget>();
            List<Button> weapons = new List<Button>();

            Button saveDataButton = new Button(1000, 50, 50, "Save Data", (Widget widget) =>
            {
                saveData("testytest");
                saveState("testytest");

            });

            int shipStartEnergy = playerShip.Energy;
            bool gameEnd = false;

            Point currentlySelectedPlayerGrid = new Point(-1, -1);
            Point currentlySelectedEnemyGrid = new Point(-1, -1);

            StateMachine cursorState = new StateMachine();

            State idleCursor = new State { Name = "idleCursor" };
            State hasSelectedCrew = new State { Name = "hasSelectedCrew" };
            State targetWeapon = new State { Name = "targetWeapon" };

            idleCursor.Transitions.Add(hasSelectedCrew.Name, hasSelectedCrew);
            idleCursor.Transitions.Add(targetWeapon.Name, targetWeapon);
            hasSelectedCrew.Transitions.Add(idleCursor.Name, idleCursor);
            hasSelectedCrew.Transitions.Add(hasSelectedCrew.Name, hasSelectedCrew);
            targetWeapon.Transitions.Add(idleCursor.Name, idleCursor);
            targetWeapon.Transitions.Add(hasSelectedCrew.Name, hasSelectedCrew);

            cursorState.Start(idleCursor);


            StateMachine enemyAI = new StateMachine();

            State enemyIdle = new State { Name = "enemyIdle" };
            State enemyFiring = new State { Name = "enemyFiring" };

            enemyIdle.Transitions.Add(enemyFiring.Name, enemyFiring);
            enemyFiring.Transitions.Add(enemyIdle.Name, enemyFiring);

            enemyAI.Start(enemyIdle);

            #region battle state methods
            // when entering the battle state
            #region battle state enter
            battle.enter += () =>
            {
                // add gui elements here

                // player ship HP, enemy ship HP


                // player ship shields, enemy ship shields


                // player ship total / current energy (we'll deal with segmented energy later)

                // player ship rooms: 
                // powered up or down, on fire, hull breach


                // weapons GUI: 
                // weapon slots 1-5 for each:
                // filled or not, enabled or not, charging or not
                #region music
                ThisBattleSong = BattleMusic.CreateInstance();
                ThisBattleSong.IsLooped = true;
                ThisBattleSong.Play();
                #endregion
                // Rebecca's Code

                // weapon enabling

                // this panel will hold all the login GUI objects

                //WidgetEvent wpn1click = new WidgetEvent();
                gui.AddWidget(saveDataButton);
                gui.AddWidget(wpnEnable);
                weapons.Clear();

                if(healthBarTest.Count > 0)
                {
                    healthBarTest.Clear();
                }

                if (energyBarTest.Count > 0)
                {
                    energyBarTest.Clear();
                }

                if (enemyhealthBarTest.Count > 0)
                {
                    enemyhealthBarTest.Clear();
                }

                

                /************************************************************************
                 * Weapon Menu in battle scene:                                         *
                 * Create a GUI that displayes button per weapon (5 buttons).           *
                 * 15 buttons in total (5 for Disabled, 5 for Enabled, 5 for Selected). *
                 ************************************************************************/
                // Create and add weapon disable buttons
                Button wpn1Disable = new Button(0, 0, 120, "WPN 1: Disabled");
                Button wpn2Disable = new Button(139, 0, 120, "WPN 2: Disabled");
                Button wpn3Disable = new Button(278, 0, 120, "WPN 3: Disabled");
                Button wpn4Disable = new Button(67, 35, 120, "WPN 4: Disabled");
                Button wpn5Disable = new Button(207, 35, 120, "WPN 5: Disabled");
                wpnEnable.AddWidget(wpn1Disable);
                weapons.Add(wpn1Disable);
                wpnEnable.AddWidget(wpn2Disable);
                weapons.Add(wpn2Disable);
                wpnEnable.AddWidget(wpn3Disable);
                weapons.Add(wpn3Disable);
                wpnEnable.AddWidget(wpn4Disable);
                weapons.Add(wpn4Disable);
                wpnEnable.AddWidget(wpn5Disable);
                weapons.Add(wpn5Disable);

                // Create and add weapon enable buttons
                Button wpn1Enable = new Button(0, 0, 120, "WPN 1: Enabled");
                Button wpn2Enable = new Button(140, 0, 120, "WPN 2: Enabled");
                Button wpn3Enable = new Button(280, 0, 120, "WPN 3: Enabled");
                Button wpn4Enable = new Button(70, 35, 120, "WPN 4: Enabled");
                Button wpn5Enable = new Button(210, 35, 120, "WPN 5: Enabled");
                wpnEnable.AddWidget(wpn1Enable);
                weapons.Add(wpn1Enable);
                wpnEnable.AddWidget(wpn2Enable);
                weapons.Add(wpn2Enable);
                wpnEnable.AddWidget(wpn3Enable);
                weapons.Add(wpn3Enable);
                wpnEnable.AddWidget(wpn4Enable);
                weapons.Add(wpn4Enable);
                wpnEnable.AddWidget(wpn5Enable);
                weapons.Add(wpn5Enable);

                // Create and add weapon select buttons
                Button wpn1Select = new Button(0, 0, 120, "WPN 1: Selected");
                Button wpn2Select = new Button(140, 0, 120, "WPN 2: Selected");
                Button wpn3Select = new Button(280, 0, 120, "WPN 3: Selected");
                Button wpn4Select = new Button(70, 35, 120, "WPN 4: Selected");
                Button wpn5Select = new Button(210, 35, 120, "WPN 5: Selected");
                wpnEnable.AddWidget(wpn1Select);
                weapons.Add(wpn1Select);
                wpnEnable.AddWidget(wpn2Select);
                weapons.Add(wpn2Select);
                wpnEnable.AddWidget(wpn3Select);
                weapons.Add(wpn3Select);
                wpnEnable.AddWidget(wpn4Select);
                weapons.Add(wpn4Select);
                wpnEnable.AddWidget(wpn5Select);
                weapons.Add(wpn5Select);

                // Hide Enabled and Selected buttons
                for (int i = 5; i < 15; i++)
                    weapons[i].Visible = false;

                //playerShip.WeaponSlots[1];
                //WeaponManager.


                // Rebecca's code end


                // adds all the energy bars to the gui
                gui.AddWidget(energy1);

                /*Lance's*/
                gui.AddWidget(Health);
                gui.AddWidget(Shields);

                /*create the health bar:
                 *first add the 3 red bars
                 *second add the 6 orange bars
                 *  (3 over the red bars)
                 *third add the 10 green bars
                 *  (6 over the orange)
                 *      (3 over the red as well)
                 *finally make any bars more over the starting
                 * health of the ship invisible
                 */
                for (int i = 0; i < 3; i++)
                {
                    // red healthbar
                    Health.AddWidget(HealthBar = new Image(32 * i, 0, healthBarLow));
                    healthBarTest.Add(HealthBar);
                }
                for (int i = 0; i < 6; i++)
                {
                    // orange healthbar
                    Health.AddWidget(HealthBar = new Image(32 * i, 0, healthBarMed));
                    healthBarTest.Add(HealthBar);
                }
                for (int i = 0; i < playerShip.MaxHP; i++)
                {
                    // green healthbar
                    Health.AddWidget(HealthBar = new Image(32 * i, 0, healthBarFull));
                    healthBarTest.Add(HealthBar);
                }

                int j = 0;
                for (int i = 0; i < 19; i++)
                {
                    if (playerShip.CurrentHP > 6)
                        j = 9;
                    else if (playerShip.CurrentHP > 3)
                        j = 3;
                    else
                        j = 0;

                    if (i >= playerShip.CurrentHP + j)
                    {
                        healthBarTest[i].Visible = false;
                    }
                }
                

                for (int i = 0; i < playerShip.CurrentShields; i++)
                {
                    Shields.AddWidget(sBubble = new Image(32 * i, 0, shieldBubble));
                    shieldTest.Add(sBubble);
                }
                /*END Lance's*/

                // add as many energy widgets as there is ship energy to one entire energy bar
                for (int i = 0; i < playerShip.Energy; i++)
                {
                    energy1.AddWidget(energyBar1 = new Image(0, (256 - 16 - 8 - 8) - i * 16, energyBarSprite));
                    energyBarTest.Add(energyBar1);
                }

                #region enemyship construction 1
                if (gameStateUID == 0)
                {
                    System.Diagnostics.Debug.WriteLine("in state 0");
                    gridUIDs = new List<int>();
                    roomUIDs = new List<int>();
                    weaponUIDs = new List<int>();
                    gridWidth = enemyShipTexture1.Bounds.Width / 32;
                    gridHeight = enemyShipTexture1.Bounds.Height / 32;
                    shipGrid = new int[gridWidth, gridHeight];
                    // grid creation for the player ship
                    for (int i = 0; i < enemyShipTexture1.Bounds.Width / 32; i++)
                    {
                        // in each column, iterate over the ship sprite's height
                        for (j = 0; j < enemyShipTexture1.Bounds.Height / 32; j++)
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

                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 3, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 5, 3, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 7, 2, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 7, 4, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 9, 2, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 9, 4, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 11, 1, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 11, 3, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 11, 5, enemyShip1StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
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


                    enemyShipUID1 = ShipManager.AddEntity(new Ship(enemyShipTexture1, gridSprite, gridHighlightSprite, enemyShip1StartPosition, roomUIDs, gridUIDs, weaponUIDs, roomTypes, shipGrid, 0));
                    enemyShip = (Ship)ShipManager.RetrieveEntity(enemyShipUID1);


                    #region enemy health bars


                    for (int i = 0; i < 3; i++)
                    {
                        // red healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarLow));
                        enemyhealthBarTest.Add(HealthBar);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        // orange healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarMed));
                        enemyhealthBarTest.Add(HealthBar);
                    }
                    for (int i = 0; i < playerShip.MaxHP; i++)
                    {
                        // green healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarFull));
                        enemyhealthBarTest.Add(HealthBar);
                    }

                    j = 0;
                    for (int i = 0; i < 19; i++)
                    {
                        if (enemyShip.CurrentHP > 6)
                            j = 9;
                        else if (enemyShip.CurrentHP > 3)
                            j = 3;
                        else
                            j = 0;

=======

                    #region enemy health bars


                    for (int i = 0; i < 3; i++)
                    {
                        // red healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarLow));
                        enemyhealthBarTest.Add(HealthBar);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        // orange healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarMed));
                        enemyhealthBarTest.Add(HealthBar);
                    }
                    for (int i = 0; i < playerShip.MaxHP; i++)
                    {
                        // green healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarFull));
                        enemyhealthBarTest.Add(HealthBar);
                    }

                    j = 0;
                    for (int i = 0; i < 19; i++)
                    {
                        if (enemyShip.CurrentHP > 6)
                            j = 9;
                        else if (enemyShip.CurrentHP > 3)
                            j = 3;
                        else
                            j = 0;

>>>>>>> 7088aa4524ecda21e16c2b4da509023775d904ef
                        if (i >= enemyShip.CurrentHP + j)
                        {
                            enemyhealthBarTest[i].Visible = false;
                        }
                    }

                    #endregion

                    foreach (var item in weaponUIDs)
                    {
                        WeaponToShip[item] = enemyShipUID1;
                    }


                    WeaponToShip[weaponUID] = enemyShipUID1;
                    setRoomToShipDictionary(enemyShipUID1, roomUIDs);
                    setRoomGridDictionary(enemyShipUID1);
                    setUnwalkableGrids(enemyShipUID1);
                }
                #endregion

                #region enemy ship construction 2

                if (gameStateUID == 2)
                {

                    System.Diagnostics.Debug.WriteLine("in state 2");
                    //enemyShipStartPosition = new Vector2(400, 50);
                    gridUIDs = new List<int>();
                    roomUIDs = new List<int>();
                    weaponUIDs = new List<int>();
                    gridWidth = enemyShipTexture2.Bounds.Width / 32;
                    gridHeight = enemyShipTexture2.Bounds.Height / 32;
                    shipGrid = new int[gridWidth, gridHeight];
                    // grid creation for the player ship
                    for (int i = 0; i < enemyShipTexture2.Bounds.Width / 32; i++)
                    {
                        // in each column, iterate over the ship sprite's height
                        for (j = 0; j < enemyShipTexture2.Bounds.Height / 32; j++)
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

                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 3, 3, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 5, 3, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 7, 2, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 7, 4, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 9, 2, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 9, 4, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 11, 1, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 11, 3, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
                    roomUIDs.Add(roomUID);
                    roomUID = RoomManager.AddEntity(new Room(roomHighlightSprite, roomHighlightSprite, 11, 5, enemyShip2StartPosition, Globals.roomShape.TwoXTwo, Globals.roomType.EMPTY_ROOM, 2, 2));
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


                    enemyShipUID2 = ShipManager.AddEntity(new Ship(enemyShipTexture2, gridSprite, gridHighlightSprite, enemyShip2StartPosition, roomUIDs, gridUIDs, weaponUIDs, roomTypes, shipGrid, 0));
                    enemyShip = (Ship)ShipManager.RetrieveEntity(enemyShipUID2);


                    #region enemy health bars


=======

                    #region enemy health bars


>>>>>>> 7088aa4524ecda21e16c2b4da509023775d904ef
                    for (int i = 0; i < 3; i++)
                    {
                        // red healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarLow));
                        enemyhealthBarTest.Add(HealthBar);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        // orange healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarMed));
                        enemyhealthBarTest.Add(HealthBar);
                    }
                    for (int i = 0; i < playerShip.MaxHP; i++)
                    {
                        // green healthbar
                        enemyHealth.AddWidget(HealthBar = new Image(32 * i, 0, healthBarFull));
                        enemyhealthBarTest.Add(HealthBar);
                    }

                    j = 0;
                    for (int i = 0; i < 19; i++)
                    {
                        if (enemyShip.CurrentHP > 6)
                            j = 9;
                        else if (enemyShip.CurrentHP > 3)
                            j = 3;
                        else
                            j = 0;

                        if (i >= enemyShip.CurrentHP + j)
                        {
                            enemyhealthBarTest[i].Visible = false;
                        }
                    }

                    #endregion


                    foreach (var item in weaponUIDs)
                    {
                        WeaponToShip[item] = enemyShipUID2;
                    }


                    WeaponToShip[weaponUID] = enemyShipUID2;
                    setRoomToShipDictionary(enemyShipUID2, roomUIDs);
                    setRoomGridDictionary(enemyShipUID2);
                    setUnwalkableGrids(enemyShipUID2);
                }
                #endregion 

                gui.AddWidget(enemyHealth);

                
            };

            #endregion
            // when updating the battle state
            #region battle state update
            battle.update += (GameTime gameTime) =>
            {

                #region check for enemy death

                if (enemyShip.CurrentHP <= 0 && gameEnd == false)
                {

                    gameEnd = true;
                    endBattle.AddWidget(endMessageGood);

                    endBattle.AddWidget(new Button((250/2)-35, 85-45, "Next>>", 2, (Widget widget) =>
                    {
                        // instead of going to a battle, go to an overworld instead
                        //stateMachine.Transition(overworld.Name);
                        stateMachine.Transition(overworld.Name);

                    }));

                    gui.AddWidget(endBattle);
                    battle1Result = true;
                    masterGameEnd = true;
                }

                #endregion

                #region weapon gui update

                if (weapon1Disabled == true)
                {
                    weapons[0].Visible = true;
                    weapons[5].Visible = false;
                    weapons[10].Visible = false;
                }
                else if (weapon1Enabled == true)
                {
                    weapons[0].Visible = false;
                    weapons[5].Visible = true;
                    weapons[10].Visible = false;
                }
                else if (weapon1Selected == true)
                {
                    weapons[0].Visible = false;
                    weapons[5].Visible = false;
                    weapons[10].Visible = true;
                }

                if (weapon2Disabled == true)
                {
                    weapons[1].Visible = true;
                    weapons[6].Visible = false;
                    weapons[11].Visible = false;
                }
                else if (weapon2Enabled == true)
                {
                    weapons[1].Visible = false;
                    weapons[6].Visible = true;
                    weapons[11].Visible = false;
                }
                else if (weapon2Selected == true)
                {
                    weapons[1].Visible = false;
                    weapons[6].Visible = false;
                    weapons[11].Visible = true;
                }

                if (weapon3Disabled == true)
                {
                    weapons[2].Visible = true;
                    weapons[7].Visible = false;
                    weapons[12].Visible = false;
                }
                else if (weapon3Enabled == true)
                {
                    weapons[2].Visible = false;
                    weapons[7].Visible = true;
                    weapons[12].Visible = false;
                }
                else if (weapon3Selected == true)
                {
                    weapons[2].Visible = false;
                    weapons[7].Visible = false;
                    weapons[12].Visible = true;
                }

                if (weapon4Disabled == true)
                {
                    weapons[3].Visible = true;
                    weapons[8].Visible = false;
                    weapons[13].Visible = false;
                }
                else if (weapon4Enabled == true)
                {
                    weapons[3].Visible = false;
                    weapons[8].Visible = true;
                    weapons[13].Visible = false;

                }
                else if (weapon4Selected == true)
                {
                    weapons[3].Visible = false;
                    weapons[8].Visible = false;
                    weapons[13].Visible = true;
                }

                if (weapon5Disabled == true)
                {
                    weapons[4].Visible = true;
                    weapons[9].Visible = false;
                    weapons[14].Visible = false;
                }
                else if (weapon5Enabled == true)
                {
                    weapons[4].Visible = false;
                    weapons[9].Visible = true;
                    weapons[14].Visible = false;
                }
                else if (weapon5Selected == true)
                {
                    weapons[4].Visible = false;
                    weapons[9].Visible = false;
                    weapons[14].Visible = true;
                }

                #endregion



                #region input handling

                #region keys


                /*
                // if the a key is pressed, transition back to the menu
                if (currentKeyState.IsKeyDown(Keys.A))
                {
                    stateMachine.Transition(startMenu.Name);
                }
                */
                /*if (currentKeyState.IsKeyDown(Keys.D1) && previousKeyState.IsKeyUp(Keys.D1))
                {
                    Weapon thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[0]);

                    // if weapon 0 is currently disabled
                    if (thisWeapon.weaponStateMachine.CurrentState.Name == "disabled")
                    {
                        // enable the weapon if possible

                        // start charging
                    }

                    // if weapon 0 is charging
                    else if (thisWeapon.weaponStateMachine.CurrentState.Name == "charging")
                    {
                        // go to target weapon state, try to assign target
                    }


                }

                if (currentKeyState.IsKeyDown(Keys.D2) && previousKeyState.IsKeyUp(Keys.D2))
                {
                    // if weapon 1 is not charging, try to assign energy, start charging, go to weapon target cursor state, and target enemy

                    // if weapon 1 is ready, go to weapon target cursor state, and target enemy
                }

                if (currentKeyState.IsKeyDown(Keys.D3) && previousKeyState.IsKeyUp(Keys.D3))
                {
                    // if weapon 2 is not charging, try to assign energy, start charging, go to weapon target cursor state, and target enemy

                    // if weapon 2 is ready, go to weapon target cursor state, and target enemy
                }

                if (currentKeyState.IsKeyDown(Keys.D4) && previousKeyState.IsKeyUp(Keys.D4))
                {
                    // if weapon 3 is not charging, try to assign energy, start charging, go to weapon target cursor state, and target enemy

                    // if weapon 3 is ready, go to weapon target cursor state, and target enemy
                }

                if (currentKeyState.IsKeyDown(Keys.D5) && previousKeyState.IsKeyUp(Keys.D5))
                {
                    // if weapon 4 is not charging, try to assign energy, start charging, go to weapon target cursor state, and target enemy

                    // if weapon 4 is ready, go to weapon target cursor state, and target enemy
                }*/



                #region keys.c
                /*
                // if the c key is tapped, query to see if the cursor is hovering over the ship
                if (currentKeyState.IsKeyDown(Keys.C) && previousKeyState.IsKeyUp(Keys.C))
                {
                    // returns whether or not the cursor is hovering over the player's ship
                    int shipUID = checkShipHover(currentMouseState);
                    


                    // if the cursor is hovering over the player's ship, print a message and figure out which room the cursor is in
                    if (shipUID == playerShipUID)
                    {
                        System.Diagnostics.Debug.WriteLine("Cursor on player ship!");
                        

                        // returns which grid (in ship grid coords) the cursor is hovering over
                        Vector2 gridHover = getGridHover(currentMouseState, shipUID);


                        // if gridHover isn't (-1,-1), which means the cursor ISNT on the grid, print messages, and highlight (or un-highlight) that grid 
                        if (gridHover.X != -1 && gridHover.Y != -1)
                        {
                            Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);

                            int gridUID = thisShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y];

                            Grid thisGrid = (Grid)GridManager.RetrieveEntity(gridUID);

                            System.Diagnostics.Debug.WriteLine("Cursor on grid: " + thisGrid.GridPosition.ToString());
                            
                            if (checkRoomHover(currentMouseState, shipUID) == true)
                            {
                                int roomUID = getRoomHover(currentMouseState, shipUID);

                                Room thisRoom = (Room)RoomManager.RetrieveEntity(roomUID);

                                System.Diagnostics.Debug.WriteLine("Cursor on room: " + thisRoom.RoomPosition.ToString());
                            }

                            //highlight the grid
                            if (target1Selected == false)
                            {
                                if (thisGrid.IsWalkable == true)
                                {
                                    target1Selected = true;
                                }

                            }

                            target1 = thisGrid.GridPosition;
                            target1Screen = new Vector2(currentMouseState.X, currentMouseState.Y);
                            

                        }
                    }


                }

                #endregion

                #region keys.v
                // second target for pathfinder checking 

                if (currentKeyState.IsKeyDown(Keys.V) && previousKeyState.IsKeyUp(Keys.V))
                {
                    // returns whether or not the cursor is hovering over the player's ship
                    int shipUID = checkShipHover(currentMouseState);



                    // if the cursor is hovering over the player's ship, print a message and figure out which room the cursor is in
                    if (shipUID == playerShipUID)
                    {
                        // returns which grid (in ship grid coords) the cursor is hovering over
                        Vector2 gridHover = getGridHover(currentMouseState, shipUID);

                        Ship thisShip = (Ship)ShipManager.RetrieveEntity(shipUID);

                        if (target2Selected == false && target1Selected == true)
                        {
                            Grid thisGrid = (Grid)GridManager.RetrieveEntity(thisShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y]);
                            if (thisGrid.IsWalkable == true)
                            {
                                
                                target2 = thisGrid.GridPosition;
                                target2Screen = new Vector2(currentMouseState.X, currentMouseState.Y);

                                System.Diagnostics.Debug.WriteLine(target1.ToString() + " " + target2.ToString());

                                pather = new Pathfinder(thisShip.ShipGrid, GridManager);

                                List<Vector2> path = pather.FindOptimalPath(target1, target2);

                                foreach (Vector2 item in path)
                                {
                                    Vector2 dumb = new Vector2((item.X / 32), (item.Y / 32));

                                    System.Diagnostics.Debug.WriteLine(dumb.ToString());
                                }

                                double c = Math.Sqrt(Math.Pow((double)(target2Screen.X - target1Screen.X), 2d) + Math.Pow((double)(target2Screen.Y - target1Screen.Y), 2d));

                                System.Diagnostics.Debug.WriteLine(c.ToString());

                                List<Vector2> pathList = new List<Vector2>();

                                pathList.Add(target2Screen);

                                target1Selected = false;

                                target1 = new Vector2();
                                target2 = new Vector2();
                            }
                        }
                    }
                }
                */
                #endregion

                #region keys.e
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

                #endregion

                #region keys.r
                // if the f key is tapped, try to gain energy if possibile
                if (currentKeyState.IsKeyDown(Keys.R) == true && previousKeyState.IsKeyUp(Keys.R) == true)
                {
                    if (playerShip.Energy < 10)
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

                #region keys.o
                if (currentKeyState.IsKeyDown(Keys.O) == true && previousKeyState.IsKeyUp(Keys.O) == true)
                {
                    if (playerShip.CurrentHP < 10)
                    {
                        System.Diagnostics.Debug.WriteLine("gained health!");
                        playerShip.CurrentHP = playerShip.CurrentHP + 1;
                        
                    }

                    /*if (playerShip.CurrentHP > 6)
                        healthBarFull = Content.Load<Texture2D>("healthBarFull");
                    else if (playerShip.CurrentHP > 3)
                        healthBarFull = Content.Load<Texture2D>("healthBarMed");
                    else
                        healthBarFull = Content.Load<Texture2D>("healthBarLow");

                    for (int i = 0; i < playerShip.MaxHP; i++)
                    {
                        if (i < playerShip.CurrentHP)
                        {
                            energyBarTest[i].Visible = true;
                        }
                    }*/

                    int j = 0;
                    /*
                     * there are 19 bars
                     * 0 - 3   : red
                     * 4 - 9   : orange
                     * 10 - 19 : green
                     */
                    for (int i = 0; i < 19; i++)
                    {
                        if (playerShip.CurrentHP > 6)
                            j = 9;
                            // we are still in green 
                            //don't worry about orange(6) and yellow(3)
                        else if (playerShip.CurrentHP > 3)
                            j = 3;
                            //we are in orange 
                            //don't worry about red(3)
                        else
                            j = 0;

                            //we are in red, hp and bars finally line up

                        if (i < playerShip.CurrentHP + j)
                        {
                            healthBarTest[i].Visible = true;
                        }
                    }
                }
                #endregion

                #region keys.p
                if (currentKeyState.IsKeyDown(Keys.P) == true && previousKeyState.IsKeyUp(Keys.P) == true)
                {
                    if (playerShip.CurrentHP > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("lost health!");
                        playerShip.CurrentHP = playerShip.CurrentHP - 1;
                        
                    }

                    int j = 0;
                    /*
                     * there are 19 bars
                     * 0 - 3   : red
                     * 4 - 9   : orange
                     * 10 - 19 : green
                     */
                    for (int i = 0; i < 19; i++)
                    {
                        if (playerShip.CurrentHP > 6)

                            j = 9; 
                            // we are still in green 
                            //don't worry about orange(6) and yellow(3)
                        else if (playerShip.CurrentHP > 3)
                            j = 3;
                            //we are in orange 
                            //don't worry about red(3) 
                        else
                            j = 0;
                            //we are in red, hp and bars finally line up

                        if (i >= playerShip.CurrentHP + j)
                        {
                            healthBarTest[i].Visible = false;
                        }
                    }
                }
                #endregion

                #region keys.z
                if (currentKeyState.IsKeyDown(Keys.Z) == true && previousKeyState.IsKeyUp(Keys.Z) == true)
                {
                    if (playerShip.CurrentShields > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("A shield has fallen!");
                        playerShip.CurrentShields = playerShip.CurrentShields - 1;
                    }
                    for (int i = 0; i < playerShip.MaxShields; i++)
                        if (i >= playerShip.CurrentShields)
                            shieldTest[i].Visible = false;
                }
                #endregion

                #region keys.x
                if (currentKeyState.IsKeyDown(Keys.X) == true && previousKeyState.IsKeyUp(Keys.X) == true)
                {
                    if (playerShip.CurrentShields < 4)
                    {
                        System.Diagnostics.Debug.WriteLine("A shield has recharged!");
                        playerShip.CurrentShields = playerShip.CurrentShields + 1;
                    }
                    for (int i = 0; i < playerShip.MaxShields; i++)
                        if (i < playerShip.CurrentShields)
                            shieldTest[i].Visible = true;
                }
                #endregion

                // Rebecca's code

                #region weapon keys
                // For each numeric key press (D1-D5), weapons are enabled and selected.
                // Initially, all weapons are Disabled.
                // When numeric key is pressed, the corresponding weapon is Enabled and starts charging
                // When numeric key is pressed again, the corresponding weapon is Selected and waits for a target to be selected
                // Every numeric key press after that switches back and forth between Enabled and Selected

                #region setting gui according to weapon states
<<<<<<< HEAD
                

                

=======
                

                

>>>>>>> 7088aa4524ecda21e16c2b4da509023775d904ef
                #endregion

                #region keys.1
                if (currentKeyState.IsKeyDown(Keys.D1) == true && previousKeyState.IsKeyUp(Keys.D1) == true)
                {
                    Weapon thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[0]);
                    
                    /*
                    int num = 0;
                    if (weapons[num].Visible || weapons[num + 10].Visible)
                    {
                        // enable weapon 1
                        weapons[num].Visible = false;//weapon disabled
                        weapons[num + 5].Visible = true;//weapon enabled
                        weapons[num + 10].Visible = false;//weapon selected
                    }
                    else
                    {
                        // select weapon 1
                        weapons[num].Visible = false;//weapon disabled
                        weapons[num + 5].Visible = false;//weapon enabled
                        weapons[num + 10].Visible = true;//weapon selected
                    }
                    */

                    //Weapon thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[0]);

                    // if weapon 1 is currently disabled
                    if (thisWeapon.weaponStateMachine.CurrentState.Name == "disabled")
                    {

                        weapon1Enabled = true;
                        weapon1Selected = false;
                        weapon1Disabled = false;

                        thisWeapon.start_charging();
                    }

                    // if weapon 1 is charging
                    else if (thisWeapon.weaponStateMachine.CurrentState.Name == "ready")
                    {
                        
                        
                        // go to target weapon state, try to assign target
                        if (cursorState.CurrentState.Name == "idleCursor")
                        {

                            weaponSlotsIndex = 0;
                            cursorState.Transition("targetWeapon");
                        }

                        
                    }
                }
                #endregion

                #region keys.2
                if (currentKeyState.IsKeyDown(Keys.D2) == true && previousKeyState.IsKeyUp(Keys.D2) == true)
                {
                    Weapon thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[1]);
                    
                    if (thisWeapon.weaponStateMachine.CurrentState.Name == "disabled")
                    {

                        weapon2Enabled = true;
                        weapon2Selected = false;
                        weapon2Disabled = false;

                        thisWeapon.start_charging();
                    }

                    // if weapon 1 is charging
                    else if (thisWeapon.weaponStateMachine.CurrentState.Name == "ready")
                    {


                        // go to target weapon state, try to assign target
                        if (cursorState.CurrentState.Name == "idleCursor")
                        {

                            weaponSlotsIndex = 1;
                            cursorState.Transition("targetWeapon");
                        }


                    }
                }
                #endregion

                #region keys.3
                if (currentKeyState.IsKeyDown(Keys.D3) == true && previousKeyState.IsKeyUp(Keys.D3) == true)
                {
                    Weapon thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[2]);
                    
                    if (thisWeapon.weaponStateMachine.CurrentState.Name == "disabled")
                    {

                        weapon3Enabled = true;
                        weapon3Selected = false;
                        weapon3Disabled = false;

                        thisWeapon.start_charging();
                    }

                    // if weapon 1 is charging
                    else if (thisWeapon.weaponStateMachine.CurrentState.Name == "ready")
                    {


                        // go to target weapon state, try to assign target
                        if (cursorState.CurrentState.Name == "idleCursor")
                        {

                            weaponSlotsIndex = 2;
                            cursorState.Transition("targetWeapon");
                        }


                    }
                }
                #endregion

                #region keys.4
                if (currentKeyState.IsKeyDown(Keys.D4) == true && previousKeyState.IsKeyUp(Keys.D4) == true)
                {
                    Weapon thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[3]);
                    
                    if (thisWeapon.weaponStateMachine.CurrentState.Name == "disabled")
                    {

                        weapon4Enabled = true;
                        weapon4Selected = false;
                        weapon4Disabled = false;

                        thisWeapon.start_charging();
                    }

                     // if weapon 1 is charging
                    else if (thisWeapon.weaponStateMachine.CurrentState.Name == "ready")
                    {


                        // go to target weapon state, try to assign target
                        if (cursorState.CurrentState.Name == "idleCursor")
                        {

                            weaponSlotsIndex = 3;
                            cursorState.Transition("targetWeapon");
                        }


                    }
                }
                #endregion

                #region keys.5
                if (currentKeyState.IsKeyDown(Keys.D5) == true && previousKeyState.IsKeyUp(Keys.D5) == true)
                {
                    Weapon thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[4]);
                    
                    if (thisWeapon.weaponStateMachine.CurrentState.Name == "disabled")
                    {

                        weapon4Enabled = true;
                        weapon4Selected = false;
                        weapon4Disabled = false;

                        thisWeapon.start_charging();
                    }

                     // if weapon 1 is charging
                    else if (thisWeapon.weaponStateMachine.CurrentState.Name == "ready")
                    {


                        // go to target weapon state, try to assign target
                        if (cursorState.CurrentState.Name == "idleCursor")
                        {

                            weaponSlotsIndex = 4;
                            cursorState.Transition("targetWeapon");
                        }


                    }
                }
                #endregion

                #endregion

                

                // end Rebecca's code


                #region weapons testing: keys.y, keys.u
                //a test to see if my states work -Peter
                if (currentKeyState.IsKeyDown(Keys.Y) == true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Weapon defaultWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[i]);

                        defaultWeapon.EnoughPower = true;
                        defaultWeapon.start_charging();
                        defaultWeapon.weaponStateMachine.Update(gameTime);
                        //System.Diagnostics.Debug.WriteLine(defaultWeapon.weaponStateMachine.CurrentState.Name);
                    }
                }

                //a test to see if my states work -Peter
                if (currentKeyState.IsKeyDown(Keys.U) == true)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Weapon defaultWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[0]);

                        defaultWeapon.deactivate_weap();
                        defaultWeapon.weaponStateMachine.Update(gameTime);
                    }
                }

                #endregion

                #endregion

                #region mouse




                #endregion
                #endregion
            

                ShipManager.Update(gameTime);
                GridManager.Update(gameTime);
                RoomManager.Update(gameTime);
                WeaponManager.Update(gameTime);
                CrewManager.Update(gameTime);

                cursorState.Update(gameTime);

            };
            #endregion

            // when leaving the battle state
            #region battle state leave
            battle.leave += () =>
            {
                //Get rid of the music
                ThisBattleSong.Stop();
                // tear down gui elements

                // remove the energy widgets from the gui
                

                gui.RemoveWidget(energy1);
                gui.RemoveWidget(Health);
                gui.RemoveWidget(wpnEnable);
                gui.RemoveWidget(Shields);
                gui.RemoveWidget(saveDataButton);
                gui.RemoveWidget(endBattle);
            };
            #endregion
            #endregion

            #region idle cursor state methods
            idleCursor.enter += () =>
            {
            };

            idleCursor.update += (GameTime gameTime) =>
            {
                #region input handling


                
                #region mouse

                #region left click

                // if we were previously holding the mouse button down, but now its released
                if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                {
                    // if there is a crew in the current cursor's grid, and we are not multiselecting, select that crew member, transition to hasSelectedCrew
                    if (multiSelecting == false)
                    {
                        //cursorState.Transition(hasSelectedCrew.Name);
                    }
                    // else if we are multiselecting: get (x1,y1;x2,y2), select all crew in that area, set multiselecting to false, transition to hasSelectedCrew if any crew were selected

                    // note: this should only happen if the two points are on the player's ship
                    else if (multiSelecting == true && checkShipHover(selectRectStart) == playerShipUID && checkShipHover(selectRectEnd) == playerShipUID)
                    {
                        int x1 = (selectRectStart.X - (int)playerShipStartPosition.X) / 32;
                        int y1 = (selectRectStart.Y - (int)playerShipStartPosition.Y) / 32;
                        int x2 = (selectRectEnd.X - (int)playerShipStartPosition.X) / 32;
                        int y2 = (selectRectEnd.Y - (int)playerShipStartPosition.Y) / 32;

                        // swap points if they need to be
                        int temp;

                        if (x2 < x1)
                        {
                            temp = x2;
                            x2 = x1;
                            x1 = temp;
                        }

                        if (y2 < y1)
                        {
                            temp = y2;
                            y2 = y1;
                            y1 = temp;
                        }

                        x1 = Math.Max(x1, 0);
                        y1 = Math.Max(y1, 0);

                        x2 = Math.Min(x2, playerShip.ShipGrid.GetLength(0) - 1);
                        y2 = Math.Min(y2, playerShip.ShipGrid.GetLength(1) - 1);



                        System.Diagnostics.Debug.WriteLine("x1, y1 {0},{1}", x1, y1);
                        System.Diagnostics.Debug.WriteLine("x2, y2 {0},{1}", x2, y2);



                        System.Diagnostics.Debug.WriteLine("Amount of rooms: "+RoomManager.RetrieveKeys().Count.ToString());   

                        for (int i = x1; i <= x2; i++)
                        {
                            for (int j = y1; j <= y2; j++)
                            {
                                System.Diagnostics.Debug.WriteLine("Selected Grid {0},{1}", i, j);

                                Vector2 blah = new Vector2(i, j);
                                foreach (int x in GridManager.RetrieveKeys())
                                {
                                    Grid grid = (Grid)GridManager.RetrieveEntity(x);

                                    if (grid.GridPosition == blah)
                                    {
                                        System.Diagnostics.Debug.WriteLine("Filled: " + FilledRooms[x].ToString());
                                        System.Diagnostics.Debug.WriteLine("Walkable: " + grid.IsWalkable.ToString());
                                        
                                        break;
                                    }
                                }

                                

                                /*for (i = 0; i < playerShip.ShipGrid.GetLength(0); i++)
                                {
                                    for (j = 0; j < playerShip.ShipGrid.GetLength(1); j++)
                                    {

                                        Grid walkCheck = (Grid)GridManager.RetrieveEntity(playerShip.ShipGrid[i, j]);

                                        //System.Diagnostics.Debug.WriteLine("Grid {0},{1}: {2}", i, j, walkCheck.IsWalkable.ToString());
                                    }

                                }*/




                                var crewMembers = CrewManager.RetrieveKeys();

                                foreach (int k in crewMembers)
                                {
                                    Crew thisguy = (Crew)CrewManager.RetrieveEntity(k);

                                    if (thisguy.Position.X == i && thisguy.Position.Y == j)
                                    {
                                        thisguy.Selected = true;

                                        selectedCrewMembers.Add(k);
                                    }
                                }
                            }
                        }

                        if (selectedCrewMembers.Count > 0)
                        {
                            cursorState.Transition(hasSelectedCrew.Name);
                        }
                    }

                    multiSelecting = false;

                    if (selectedCrewMembers.Count > 0)
                    {
                        foreach (var item in selectedCrewMembers)
                        {
                            System.Diagnostics.Debug.WriteLine(item);
                        }
                    }
                }

                // if we're holding the mouse button down
                if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    // if we arent multiselecting: set multiselecting to true, start point = previous cursor's position; end point = current cursor's position
                    if (multiSelecting == false)
                    {
                        multiSelecting = true;
                        selectRectStart.X = previousMouseState.X;
                        selectRectStart.Y = previousMouseState.Y;

                        selectRectEnd.X = currentMouseState.X;
                        selectRectEnd.Y = currentMouseState.Y;
                    }

                    else if (multiSelecting == true)
                    {
                        selectRectEnd.X = currentMouseState.X;
                        selectRectEnd.Y = currentMouseState.Y;
                    }
                    // else if we are multiselecting: end point = current cursor's position
                }

                #endregion


                #region right click
                if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                {
                    // if we've rightclicked

                    // gui interaction: 

                    // if we've right-clicked over a weapon gui element (the charge bar)
                    /*
                    if(rightclick position == weapon 0 space)
                    {
                        //disable weapon
                        thisWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponSlots[0]);
                        thisWeapon.WeaponStateMachine.Transition(disabled.Name);

                    }
                    */
                }

                #endregion

                #endregion

                #endregion
            };

            idleCursor.leave += () =>
            {
            };
            #endregion

            #region selected crew cursor state methods
            hasSelectedCrew.enter += () =>
            {
            };

            hasSelectedCrew.update += (GameTime gameTime) =>
            {
                #region input handling

                #region mouse

                if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    // if we've leftclicked

                    //deselect the crew, go to idlecursor



                    selectedCrewMembers.Clear();
                    cursorState.Transition(idleCursor.Name);
                }

                if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                {
                    // if we've rightclicked

                    // move the crew, we can assume the selected crew list has size of at least 1

                    if (selectedCrewMembers.Count == 1)
                    {
                        // we only have one man

                        // get the room we clicked, and if its on the ship, check if the room is empty of mans

                        // if it has room, move the man to an empty grid in that room

                        // transition to idle cursor on success

                        // todo: room-filling algorithm


                        if (checkShipHover(currentMouseState) == playerShipUID)
                        {
                            Vector2 targetGridVector = getGridHover(currentMouseState, playerShipUID);


                            //System.Diagnostics.Debug.WriteLine("target grid: "+targetGrid);

                            Grid targetGrid = (Grid)GridManager.RetrieveEntity(playerShip.ShipGrid[(int)targetGridVector.X, (int)targetGridVector.Y]);

                            //System.Diagnostics.Debug.WriteLine("target grid: " + targetGrid.UID);

                            int targetGridUID = targetGrid.UID;

                            Crew thisCrew = (Crew)CrewManager.RetrieveEntity(selectedCrewMembers[0]);
                            Grid thisGrid = (Grid)GridManager.RetrieveEntity(targetGridUID);

                            bool dothis = true;

                            foreach (var item in CrewManager.RetrieveKeys())
                            {
                                Crew testGuy = (Crew)CrewManager.RetrieveEntity(item);

                                if (testGuy.Position == thisGrid.GridPosition)
                                {
                                    dothis = false;
                                }

                            }


                            if (dothis == true)
                            {


                                int originGridUID = 0;
                                foreach (int x in GridManager.RetrieveKeys())
                                {
                                    Grid grid = (Grid)GridManager.RetrieveEntity(x);

                                    if (grid.GridPosition == thisCrew.Position)
                                    {
                                        originGridUID = x;
                                        break;
                                    }
                                }


                                List<Vector2> path = pather.FindOptimalPath(thisCrew.Position, thisGrid.GridPosition);
                                thisCrew.Move(path);
                                thisCrew.Position = thisGrid.GridPosition;
                                FilledRooms[originGridUID] = false;
                                FilledRooms[targetGridUID] = true;
                                CrewToRoom[thisCrew.UID] = GridToRoom[targetGridUID];

                            }


                        }



                    }

                    else
                    {
                        // we got more than one man

                        // did we click on a room on our ship?
                        if (checkShipHover(currentMouseState) == playerShipUID)
                        {
                            if (checkRoomHover(currentMouseState, playerShipUID) == true)
                            {
                                int thisRoomUID = getRoomHover(currentMouseState, playerShipUID);
                                Room thisRoom = (Room)RoomManager.RetrieveEntity(getRoomHover(currentMouseState, playerShipUID));
                                //check if the room has enough room to move the entire list of selected mans

                                // loop through CrewToRoom, count any hits in the values; if count is less than or equal to the room's size then continue
                                int count = 0;
                                foreach (var item in CrewToRoom.Keys)
                                {
                                    if (thisRoomUID == CrewToRoom[item] && selectedCrewMembers.Contains(item) == false)
                                    {
                                        count++;
                                    }
                                }

                                if (count <= thisRoom.RoomSize)
                                {
                                    // at this point, we can move the crew to the room
                                    // todo: room-filling algorithm
                                    // transition to idle cursor on success

                                    // get grids that exist in the room
                                    List<int> theGrids = new List<int>();
                                    foreach (var item in GridToRoom.Keys)
                                    {
                                        if (thisRoomUID == GridToRoom[item])
                                        {
                                            theGrids.Add(item);

                                        }

                                    }
                                    //add their positions to a list
                                    List<Tuple<int, int>> gridPositions = new List<Tuple<int, int>>();
                                    foreach (var item in theGrids)
                                    {
                                        Grid gridInRoom = (Grid)GridManager.RetrieveEntity(item);
                                        //System.Diagnostics.Debug.WriteLine(item);
                                        //System.Diagnostics.Debug.WriteLine(gridInRoom.GridPosition.ToString());
                                        gridPositions.Add(new Tuple<int, int>((int)gridInRoom.GridPosition.X, (int)gridInRoom.GridPosition.Y));

                                    }

                                    foreach (var item in gridPositions)
                                    {
                                        System.Diagnostics.Debug.WriteLine(item.Item1.ToString() + ", " + item.Item2.ToString());
                                    }

                                    // this is where we sort, if needed

                                    /*
                                    gridPositions.Sort();
                                    System.Diagnostics.Debug.WriteLine("sorted");
                                    foreach (var item in gridPositions)
                                    {
                                        System.Diagnostics.Debug.WriteLine(item.Item1.ToString() + ", " + item.Item2.ToString());
                                    }
                                    */
                                    // loop through crew members, for each one, find the next available grid and assign him that target
                                    for (int i = 0; i < selectedCrewMembers.Count; i++)
                                    {

                                        //if there are more selected crew than available room spaces, do nothing
                                        if (selectedCrewMembers.Count > (thisRoom.RoomSize - count))
                                        {
                                            //break;
                                        }

                                        // get the next available grid
                                        int targetGridUID = 0;
                                        foreach (int roomGridID in theGrids)
                                        {



                                            // if this grid is filled by a man

                                            //search the crew for their positions
                                            bool skipGrid = false;

                                            if (FilledRooms[roomGridID] == true)
                                            {
                                                skipGrid = true;
                                            }

                                            //if the grid was filled, skip this grid
                                            if (skipGrid == true)
                                            {
                                                System.Diagnostics.Debug.WriteLine("skipGrid: " + skipGrid.ToString());


                                                continue;
                                            }
                                            else
                                            {
                                                targetGridUID = roomGridID;
                                                break;
                                            }

                                        }


                                        System.Diagnostics.Debug.WriteLine("Target: " + targetGridUID.ToString());
                                        Crew thisCrew = (Crew)CrewManager.RetrieveEntity(selectedCrewMembers[i]);
                                        Grid thisGrid = (Grid)GridManager.RetrieveEntity(targetGridUID);


                                        int originGridUID = 0;
                                        foreach (int x in GridManager.RetrieveKeys())
                                        {
                                            Grid grid = (Grid)GridManager.RetrieveEntity(x);

                                            if (grid.GridPosition == thisCrew.Position)
                                            {
                                                originGridUID = x;
                                                break;
                                            }
                                        }

                                        if (CrewToRoom[thisCrew.UID] == thisRoomUID)
                                        {
                                            continue;
                                        }

                                        List<Vector2> path = pather.FindOptimalPath(thisCrew.Position, thisGrid.GridPosition);
                                        thisCrew.Move(path);
                                        thisCrew.Position = thisGrid.GridPosition;
                                        FilledRooms[originGridUID] = false;
                                        FilledRooms[targetGridUID] = true;
                                        CrewToRoom[thisCrew.UID] = GridToRoom[targetGridUID];

                                    }

                                }
                            }
                        }



                    }
                    selectedCrewMembers.Clear();
                    cursorState.Transition(idleCursor.Name);
                }

                #endregion
                #endregion
            };

            hasSelectedCrew.leave += () =>
            {
                
            };
            #endregion

            
            #region weapon target cursor state methods
            targetWeapon.enter += () =>
            {

                switch (weaponSlotsIndex)
                {
                    case 0:
                        selectedWeaponUID = playerShip.WeaponSlots[0];
                        weapon1Enabled = false;
                        weapon1Selected = true;
                        weapon1Disabled = false;

                        break;
                    case 1:
                        selectedWeaponUID = playerShip.WeaponSlots[1];
                        weapon2Enabled = false;
                        weapon2Selected = true;
                        weapon2Disabled = false;
                        break;

                    case 2:
                        selectedWeaponUID = playerShip.WeaponSlots[2];
                        weapon3Enabled = false;
                        weapon3Selected = true;
                        weapon3Disabled = false;
                        break;

                    case 3:
                        selectedWeaponUID = playerShip.WeaponSlots[3];
                        weapon4Enabled = false;
                        weapon4Selected = true;
                        weapon4Disabled = false;
                        break;

                    case 4:
                        selectedWeaponUID = playerShip.WeaponSlots[4];
                        weapon5Enabled = false;
                        weapon5Selected = true;
                        weapon5Disabled = false;
                        break;
                }

                
                
                
            };

            targetWeapon.update += (GameTime gameTime) =>
            {
                #region input handling

                #region mouse

                Weapon selectedWeapon = (Weapon)WeaponManager.RetrieveEntity(selectedWeaponUID);
                
                if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    // if we've leftclicked
                    System.Diagnostics.Debug.WriteLine("I'm here, clicking!");
                    name = (Ship)ShipManager.RetrieveEntity(checkShipHover(currentMouseState));
                    System.Diagnostics.Debug.WriteLine(name.UID);
                    if (name.UID != -1 && name.UID != playerShip.UID)
                    {
                        System.Diagnostics.Debug.WriteLine("Hey, it's an enemy!");
                        selectedEnemy = true;

                        dealDamage(name.UID, playerShip.WeaponUIDList[0]);
                                            
                        
                        if (enemyShip.CurrentHP >= 0)
                        {
                            int j = 0;
                            /*
                                * there are 19 bars
                                * 0 - 3   : red
                                * 4 - 9   : orange
                                * 10 - 19 : green
                                */
                            for (int i = 0; i < 19; i++)
                            {
                                if (enemyShip.CurrentHP > 6)

                                    j = 9;
                                // we are still in green 
                                //don't worry about orange(6) and yellow(3)
                                else if (enemyShip.CurrentHP > 3)
                                    j = 3;
                                //we are in orange 
                                //don't worry about red(3) 
                                else
                                    j = 0;
                                //we are in red, hp and bars finally line up

                                if (i >= enemyShip.CurrentHP + j)
                                {
                                    enemyhealthBarTest[i].Visible = false;
                                }                         

                            }
                        }
                        


                        System.Diagnostics.Debug.WriteLine("enemy health after damage: "+name.CurrentHP.ToString());
                        selectedWeapon.Charge = 0;
                        selectedWeapon.ReadyToFire = false;
                        selectedWeapon.weaponStateMachine.Transition("charging");
                        cursorState.Transition(idleCursor.Name);
                    }



                 

                }
                /*
                // if so, get the weapon we're currently selecting

                if (currentKeyState.IsKeyDown(Keys.L) && selectedEnemy == true )
                {
                    System.Diagnostics.Debug.WriteLine("I'm targeting a ship!");
                    //you want to activate weapon 1 to target the enemy
                    selectedWeapon = (Weapon)WeaponManager.RetrieveEntity(playerShip.WeaponUIDList[1]);
                    //get weapon 1
                    selectedWeapon.IsSelected = true;
                    selectedWeapon.start_charging();
                    //start its charging
                    selectedWeapon.CurrentTarget = enemyShip.UID;
                    //get the enemyID
                }

                */
                // set the enemy room as the weapon's target

                /*
                if (selectedWeapon.ReadyToFire)
                {
                    //the weapon is in ready stage
                    System.Diagnostics.Debug.WriteLine("I'm ready to pew pew!");
                    dealDamage(name.UID, selectedWeapon.UID);
                    //call weapon damage function in game1
                    selectedWeapon.ReadyToFire = false;
                    //the weapon is no longer ready to fire
                    cursorState.Transition("idleCursor");
                }
                */
                // transition to idle cursor on success
                    

                if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                {
                    // if we've rightclicked

                    selectedWeapon.IsSelected = false;

                    // transition to idle cursor

                    cursorState.Transition("idleCursor");
                }

                

                #endregion
                #endregion
            };

            targetWeapon.leave += () =>
            {
                switch (weaponSlotsIndex)
                {
                    case 0:
                        weapon1Enabled = true;
                        weapon1Selected = false;
                        weapon1Disabled = false;

                        break;
                    case 1:
                        weapon2Enabled = true;
                        weapon2Selected = false;
                        weapon2Disabled = false;
                        break;

                    case 2:
                        weapon3Enabled = true;
                        weapon3Selected = false;
                        weapon3Disabled = false;
                        break;

                    case 3:
                        weapon4Enabled = true;
                        weapon4Selected = false;
                        weapon4Disabled = false;
                        break;

                    case 4:
                        weapon5Enabled = true;
                        weapon5Selected = false;
                        weapon5Disabled = false;
                        break;
                }
                selectedWeaponUID = -1;


                

            };
            #endregion

            #region enemy AI state methods
            enemyIdle.enter += () =>
            {

            };

            enemyIdle.update += (GameTime gameTime) =>
            {
                #region input handling
                #endregion
            };

            enemyIdle.leave += () =>
            {
            };

            enemyFiring.enter += () =>
            {

            };

            enemyFiring.update += (GameTime gameTime) =>
            {
                #region input handling
                #endregion
            };

            enemyFiring.leave += () =>
            {
            };
            #endregion


        }

    }
}