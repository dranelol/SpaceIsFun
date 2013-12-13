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

        int narrativeState = 0;

        Panel introPanelOne = new Panel(300, 300, 800, 500);
        Ruminate.GUI.Content.
        TextBox Test = new TextBox(50, 200);

        Label introOne = new Label(0, 0, "Your first assignment is to take care of the ship attacking the neighboring planet Gymnopedies");
        Label introTwo = new Label(0, 0, "What? You already have? Well then. What do you even need me here for?");
        Label introThree = new Label(0, 0, "Wow, you're still alive? Well that's good... I mean good job on your success! \nYour next mission is ready for you. Come back here for your pay if you survive.\nI mean WHEN you survive. Right.\nThat one.");
        Label introFour = new Label(0, 0, "You failed. So completely, you failed. Absolutely. Completely. Failed. \nYou recieve no points, and deserve no mercy \nfor your approaching doom/next objective. \nHave Fun!");
        //      Label introOne = new Label(0, 0, "hwerl");
        void setupNarrative()
        {
            narrative.enter += () =>
            {
                // setup gui elements here
    
                introPanelOne.AddWidget(introOne);
                introPanelOne.AddWidget(introTwo);
                introPanelOne.AddWidget(introThree);
                introPanelOne.AddWidget(introFour);
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

                if (battle1Resolved == false && battle2Resolved == false)
                    introOne.Visible = true;

                if(battle1Resolved == true && battle2Resolved == false && introTwo.Visible == false)
                {
                    introOne.Visible = true;
                    narrativeState = 1;
                }

                if (battle1Resolved == true && battle2Resolved == true && battle1Result == true)
                    introThree.Visible = true;

                if (battle1Resolved == true && battle2Resolved == true && battle1Result == false)
                    introFour.Visible = true;
                    







                if (currentKeyState.IsKeyUp(Keys.Space) == true && previousKeyState.IsKeyDown(Keys.Space) == true)
                {
                    if (narrativeState == 1)
                    {
                        introOne.Visible = false;
                        introTwo.Visible = true;
                        narrativeState--;
                    }
                    else
                    {
                        introOne.Visible = false;
                        introTwo.Visible = false;
                        introThree.Visible = false;
                        introFour.Visible = false;
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
