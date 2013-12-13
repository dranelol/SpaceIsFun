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
    public partial class Game1 : Game
    {
        int narrativeUID;

        bool narrativeState = false;
        bool outState = false;
        bool twoHappened = false;

        Panel introPanelOne;

        int currentNar = 1;

        Label introOne = new Label(0, 0, "Your first assignment is to take care of the ship attacking the neighboring planet Gymnopedies");
        Label introTwo = new Label(0, 0, "What? You already have? Well then. What do you even need me here for?");
        Label introThree = new Label(0, 0, "Wow, you're still alive? Well that's good... I mean good job on your success!");
        Label introFour = new Label(0, 0, "You failed. So completely, you failed. Absolutely. Completely. Failed. \nYou recieve no points, and deserve no mercy \nfor your approaching doom/next objective. \nHave Fun!");

        
        //      Label introOne = new Label(0, 0, "hwerl");
        void setupNarrative()
        {


             
            
            
            
            narrative.enter += () =>
            {

                introPanelOne = new Panel(300, 300, 800, 500);

                introPanelOne.AddWidget(introOne);
                introPanelOne.AddWidget(introTwo);
                introPanelOne.AddWidget(introThree);
                introPanelOne.AddWidget(introFour);

                if (masterGameEnd == false)
                {


                    introPanelOne.AddWidget(new Button(410, 420, "Continue", 2, (Widget widget) =>
                    {
                        // instead of going to a battle, go to an overworld instead
                        //stateMachine.Transition(overworld.Name);


                        if (currentNar == 1)
                        {

                            if (battle1Resolved == true && battle1Result == true && narrative1Resolved == false)
                            {
                                narrative1Resolved = true;
                                currentNar = 2;
                            }
                            else
                            {
                                narrative1Resolved = true;
                                currentNar = 3;
                                stateMachine.Transition(overworld.Name);
                            }

                        }
                        else if (currentNar == 2)
                        {
                            currentNar = 3;
                            stateMachine.Transition(overworld.Name);
                        }
                        else if (currentNar == 3)
                        {
                            narrative2Resolved = true;
                            stateMachine.Transition(overworld.Name);
                        }



                    }));
                }
                else if (masterGameEnd == true)
                {
                    introPanelOne.AddWidget(new Button(410, 420, "End Game", 2, (Widget widget) =>
                    {
                        // instead of going to a battle, go to an overworld instead
                        //stateMachine.Transition(overworld.Name);


                        Exit();



                        /*
                        if (currentNar == 1)
                        {

                            if (battle1Resolved == true && battle1Result == true && narrative1Resolved == false)
                            {
                                narrative1Resolved = true;
                                currentNar = 2;
                            }
                            else
                            {
                                narrative1Resolved = true;
                                currentNar = 3;
                                stateMachine.Transition(overworld.Name);
                            }

                        }
                        else if (currentNar == 2)
                        {
                            currentNar = 3;
                            stateMachine.Transition(overworld.Name);
                        }
                        else if (currentNar == 3)
                        {
                            narrative2Resolved = true;
                            stateMachine.Transition(overworld.Name);
                        }

                        */

                    }));

                }
                
                
                // setup gui elements here
    
                
                gui.AddWidget(introPanelOne);
         //       gui.AddWidget(introPanelOne);
                introPanelOne.Visible = true;
                introOne.Visible = false;
                introTwo.Visible = false;
                introThree.Visible = false;
                introFour.Visible = false;

                // to do

                // info screen with "PLOT"

                // back / next buttons

                // end button to resolve narrative and go to overworld
            };
                                                             

            narrative.update += (GameTime gameTime) =>
            {
                #region input handling

                #endregion


                switch (currentNar)
                {
                    case 1:
                        introOne.Visible = true;
                        introTwo.Visible = false;
                        introThree.Visible = false;
                        introFour.Visible = false;
                        break;

                    case 2:
                        introOne.Visible = false;
                        introTwo.Visible = true;
                        introThree.Visible = false;
                        introFour.Visible = false;
                        break;

                    case 3:
                        introOne.Visible = false;
                        introTwo.Visible = false;
                        introThree.Visible = true;
                        introFour.Visible = false;
                        break;

                    case 4:
                        introOne.Visible = false;
                        introTwo.Visible = false;
                        introThree.Visible = false;
                        introFour.Visible = true;
                        break;

                }


                /*
                if (battle1Resolved == false && battle2Resolved == false && narrative1Resolved == false)
                {
                    
                    
                    
                    
                    
                    introTwo.Visible = false;
                    introThree.Visible = false;
                    introFour.Visible = false;
                    
                    introOne.Visible = true;
                    narrative1Resolved = true;
                }

                else if (battle1Resolved == true && battle2Resolved == false && narrative2Resolved == false && narrative1Resolved == false && twoHappened == false && outState == false)
                {
                    
                    introTwo.Visible = false;
                    introThree.Visible = false;
                    introFour.Visible = false;


                    introOne.Visible = true;
                    narrative1Resolved = true;
                    narrativeState = true;
                    
                  
                }

                else if (battle1Resolved == true && battle2Resolved == false && battle1Result == true && narrative1Resolved==true && outState == false && narrativeState == false)
                {
                    introOne.Visible = false;
                    introTwo.Visible = false;
                    introFour.Visible = false;
                    
                    introThree.Visible = true;
                    narrative2Resolved = true;
                }

                else if (battle1Resolved == true && battle2Resolved == false && battle1Result == false && narrative1Resolved==true && outState == false)
                {
                    introOne.Visible = false;
                    introTwo.Visible = false;
                    introThree.Visible = false;
                    
                    introFour.Visible = true;
                    narrative2Resolved = true;
                }
                    

                */





                if (currentKeyState.IsKeyUp(Keys.Space) == true && previousKeyState.IsKeyDown(Keys.Space) == true)
                {
                    if (narrativeState == true && outState == false)
                    {
                        introOne.Visible = false;
                        introTwo.Visible = true;
                        narrativeState = false;
                        outState = true;
                        twoHappened = true;
                    }
                    else
                    {
                        introOne.Visible = false;
                        introTwo.Visible = false;
                        introThree.Visible = false;
                        introFour.Visible = false;
                        outState = false;
                        stateMachine.Transition(overworld.Name);
                    }
                }                
            };

            narrative.leave += () =>
            {
                // remove gui elements here
                gui.RemoveWidget(introPanelOne);
            };
        }

    }
}
