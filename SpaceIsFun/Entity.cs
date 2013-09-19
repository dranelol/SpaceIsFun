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
    /// <summary>
    /// This holds all the info for an entity in our game. Generally, entities are just thought of as "special" objects; they may have a drawable, they may be moved, etc.
    /// </summary>
    class Entity : Object
    {
        #region fields

        #endregion

        #region constructors / destructors

        /// <summary>
        /// constructor for an entity
        /// </summary>
        public Entity()
            : base()
        {
        }
        #endregion

        #region methods

        /// <summary>
        /// update the entity
        /// </summary>
        /// <param name="gameTime">current game time</param>
        virtual public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// draw the entity
        /// </summary>
        /// <param name="spriteBatch">main spriteBatch object</param>
        virtual public void Draw(SpriteBatch spriteBatch)
        {
        }

        #endregion
    }
}
