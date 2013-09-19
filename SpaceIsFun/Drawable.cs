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
    ///  This is the object that holds all the info for drawing an entity
    /// </summary>
    class Drawable : Object
    {
        #region fields

        /// <summary>
        /// texture to draw
        /// </summary>
        private Texture2D spriteTexture;

        /// <summary>
        /// parameter for spriteTexture
        /// </summary>
        public Texture2D SpriteTexture
        {
            get
            {
                return spriteTexture;
            }
            set
            {
                spriteTexture = value;
            }
        }

        /// <summary>
        /// 2D position of the drawable in screen space
        /// </summary>
        private Vector2 position2D;

        /// <summary>
        /// parameter for position2D
        /// </summary>
        public Vector2 Position2D
        {
            get
            {
                return position2D;
            }
            set
            {
                position2D = value;
            }
        }

        /// <summary>
        /// width of the drawable, in pixels
        /// </summary>
        private int width;

        /// <summary>
        /// parameter for width
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        /// <summary>
        /// height of the drawable, in pixels
        /// </summary>
        private int height;

        /// <summary>
        /// parameter for height
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        #endregion

        #region constructors / destructors

        /// <summary>
        /// constructor for a drawable
        /// </summary>
        /// <param name="sprite">texture to draw</param>
        /// <param name="position">2d position of the drawable in screen space</param>
        public Drawable(Texture2D sprite, Vector2 position)
            : base()
        {
            spriteTexture = sprite;
            position2D = position;
            width = spriteTexture.Bounds.Width;
            height = spriteTexture.Bounds.Height;
        }

        #endregion

        #region methods

        /// <summary>
        /// init the drawable
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// load any content for the drawable
        /// </summary>
        public void LoadContent()
        {
            
        }

        /// <summary>
        /// update the drawable
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// draw the drawable
        /// </summary>
        /// <param name="spriteBatch">main spritebatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            spriteBatch.Draw(spriteTexture, position2D, Color.White);
            //spriteBatch.
        }

        /// <summary>
        /// unload any content for the drawable
        /// </summary>
        public void UnloadContent()
        {
        }

        /// <summary>
        /// move the drawable by (x,y) pixels in screen space
        /// </summary>
        /// <param name="delta">how much to move the drawable by</param>
        public void MoveBy(Vector2 delta)
        {
            position2D.X += delta.X;
            position2D.Y += delta.Y;
        }

        /// <summary>
        /// move the drawable to newPosition in screen space
        /// </summary>
        /// <param name="newPosition">position in screen space to which the drawable should be moved</param>
        public void MoveTo(Vector2 newPosition)
        {
            position2D = newPosition;
        }

        #endregion

    }
}
