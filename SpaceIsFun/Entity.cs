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
    class Entity : Object
    {
        #region fields

        #endregion

        #region constructors / destructors
        public Entity()
            : base()
        {
        }
        #endregion

        #region methods
        virtual public void Update(GameTime gameTime)
        {
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
        }

        #endregion
    }
}
