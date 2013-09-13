using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceIsFun
{
    /// <summary>
    /// This contains all the info of a room on a ship 
    /// </summary>
    class Room : Entity
    {
        
        private Vector2 roomPosition;

        public Vector2 RoomPosition
        {
            get
            {
                return roomPosition;
            }

            set
            {
                RoomPosition = value;
            }
        }

        public Room()
        {
        }

        
    }


    class Ship : Entity
    {
        #region fields
        private int hp;
        private int shields;
        private int energy;



        public int HP
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
            }
        }

        public int Shields
        {
            get
            {
                return shields;
            }
            set
            {
                shields = value;
            }
        }

        public int Energy
        {
            get
            {
                return energy;
            }
            set
            {
                energy = value;
            }
        }

        private Drawable sprite;

        public Drawable Sprite
        {
            get
            {
                return sprite;
            }

            set
            {
                sprite = value;
            }
        }
        #endregion

        #region constructors / destructors

        public Ship(Texture2D spriteTexture, Vector2 position)
            : base()
        {
            hp = 10;
            energy = 5;
            shields = 2;

            sprite = new Drawable(spriteTexture, position);
        }

        #endregion

        #region methods

        override public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }


        #endregion 
    }
}
