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
        void setupIntro()
        {

            Label introLabel = new Label(0, 0, "Oh, so you're off to explore space are you?\nGood for you, though you might be forewarned that the\nDemonstrably Erratic Mostly-flying Object spacecraft that\nyou've been assigned might be a bit, err, unique.\n\nLet me see... let me see... What do you need to get going here?\nYou have a ship, possibly some crew if I can manage to get\nthem on \"that\" thing again.....OH!\nYou need a star chart to get you going. Here's a\ncomplimentary one from the bureau.\n\nWell now that you've got your map, you've got two choices\nto choose from because whoever made the map is\nJust That Lazy.");

            Panel introPanel = new Panel(50,50,500, 500);

            introPanel.AddWidget(new Button(410, 420, "Next>>", 2, (Widget widget) =>
            {
                // instead of going to a battle, go to an overworld instead
                //stateMachine.Transition(overworld.Name);
                stateMachine.Transition(overworld.Name);

            }));


            introState.enter += () =>
            {
                // setup gui elements here

                // for now, these probably will just be buttons arranged on a map to spawn certain battles / narrative events

                //

                introPanel.AddWidget(introLabel);
                gui.AddWidget(introPanel);
            };

            introState.update += (GameTime gameTime) =>
            {
                #region input handling

                #endregion

                gui.Update();
                gui.Draw();


            };

           introState.leave += () =>
            {
                // remove gui elements here
                gui.RemoveWidget(introPanel);
            };
        }
    }
}
