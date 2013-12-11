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
    public abstract class Entity : Object
    {
        #region fields
        /// <summary>
        /// unique ID of this entity, set by the entity manager
        /// </summary>
        public int UID;

        /// <summary>
        /// whether or not this entity should be updated
        /// </summary>
        public bool updatable;

        /// <summary>
        /// whether or not we should skip this entity's next update
        /// </summary>
        public bool skipNextUpdate;
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
        /// inits the entity
        /// </summary>
        virtual public void Initialize()
        {
        }

        /// <summary>
        /// loads any content the entity may need
        /// </summary>
        virtual public void LoadContent()
        {
        }

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
