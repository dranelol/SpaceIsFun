﻿using System;
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
    public partial class Game1 : Game
    {

        /// <summary>
        /// sets up the game battle state
        /// </summary>
        void setupBattle()
        {
            Vector2 target1 = new Vector2();

            Vector2 target2 = new Vector2();

            bool target1Selected = false;
            bool target2Selected = false;

            Pathfinder pather = new Pathfinder(playerShip.ShipGrid);

            // sets up seven energy bars for the ship
            Panel energy1 = new Panel(4, screenHeight - 128, 40, 128 - 8);
            Panel energy2 = new Panel(64 + 4, screenHeight - 128, 40, 128 - 8);
            Panel energy3 = new Panel(128 + 4, screenHeight - 128, 40, 128 - 8);
            Panel energy4 = new Panel(192 + 4, screenHeight - 128, 40, 128 - 8);
            Panel energy5 = new Panel(256 + 4, screenHeight - 128, 40, 128 - 8);
            Panel energy6 = new Panel(320 + 4, screenHeight - 128, 40, 128 - 8);
            Panel energy7 = new Panel(384 + 4, screenHeight - 128, 40, 128 - 8);

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
                            if (target1Selected == false)
                            {
                                target1Selected = true;
                            }

                            target1 = playerShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y].GridPosition;
                            

                        }
                    }


                }

                // second target for pathfinder checking 

                if (currentKeyState.IsKeyDown(Keys.V) && previousKeyState.IsKeyUp(Keys.V))
                {
                    Vector2 gridHover = playerShip.checkGridHover(currentMouseState);
                    System.Diagnostics.Debug.WriteLine("Cursor on grid: " + playerShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y].GridPosition.ToString());

                    if (target2Selected == false && target1Selected == true)
                    {
                        target2 = playerShip.ShipGrid[(int)gridHover.X, (int)gridHover.Y].GridPosition;

                        System.Diagnostics.Debug.WriteLine(target1.ToString() + " " + target2.ToString());

                        pather = new Pathfinder(playerShip.ShipGrid);

                        List<Vector2> path = pather.FindOptimalPath(target1, target2);

                        foreach (Vector2 item in path)
                        {
                            System.Diagnostics.Debug.WriteLine(item.ToString());
                        }
                        

                        target1Selected = false;

                        target1 = new Vector2();
                        target2 = new Vector2();
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

    }
}