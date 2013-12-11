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
        void setupNarrative()
        {
            narrative.enter += () =>
            {
                // setup gui elements here

                // to do

                // info screen with "PLOT"

                // back / next buttons

                // end button to resolve narrative and go to overworld
            };

            narrative.update += (GameTime gameTime) =>
            {
                #region input handling

                #endregion

               

            };

            narrative.leave += () =>
            {
                // remove gui elements here
            };
        }
    }
}
