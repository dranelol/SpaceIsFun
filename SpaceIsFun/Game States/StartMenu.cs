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
                // instead of going to a battle, go to an overworld instead
                //stateMachine.Transition(overworld.Name);
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

    }
}
