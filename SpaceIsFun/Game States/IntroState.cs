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
            
            introState.enter += () =>
            {
                // setup gui elements here

                // for now, these probably will just be buttons arranged on a map to spawn certain battles / narrative events

                //
            };

            introState.update += (GameTime gameTime) =>
            {
                #region input handling

                #endregion



            };

           introState.leave += () =>
            {
                // remove gui elements here
            };
        }
    }
}
