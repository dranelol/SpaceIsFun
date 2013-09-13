using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//using Ruminate.GUI.Framework;
//using Ruminate.GUI.Content;

namespace SpaceIsFun
{
    public abstract class Screen
    {
        #region fields
        private Color backgroundColor;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
            }
        }
        /*
        private Gui gui;

        public Gui Gui
        {
            get
            {
                return gui;
            }

            set
            {
                gui = value;
            }
        }
        */
        #endregion

        #region methods
        public abstract void Initialize(Game1 game);
        public abstract void OnScreenResize();
        public abstract void Update(GameTime gametime);
        public abstract void Draw();

        #endregion
    }
}
