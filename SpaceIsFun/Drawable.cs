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
    class Drawable : Object
    {
        #region fields
        private Texture2D spriteTexture;

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

        private Vector2 position2D;

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

        #endregion

        #region constructors / destructors

        public Drawable(Texture2D sprite, Vector2 position)
            : base()
        {
            spriteTexture = sprite;
            position2D = position;
        }

        #endregion

        #region methods

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            spriteBatch.Draw(spriteTexture, position2D, Color.White);
            //spriteBatch.
        }

        public void UnloadContent()
        {
        }

        public void MoveBy(Vector2 delta)
        {
            position2D.X += delta.X;
            position2D.Y += delta.Y;
        }


        

        #endregion

    }
}
