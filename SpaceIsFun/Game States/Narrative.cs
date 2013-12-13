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
