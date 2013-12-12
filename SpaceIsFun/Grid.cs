using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;


namespace SpaceIsFun
{

    /// <summary>
    /// A grid of the ship
    /// </summary>
    [Serializable] public class Grid : Entity, ISerializable
    {
        #region fields
        /// <summary>
        /// x,y position of the grid on the ship (ie, top left grid will be 1,1)
        /// </summary>
        private Vector2 gridPosition;

        public Vector2 GridPosition
        {
            get
            {
                return gridPosition;
            }

            set
            {
                gridPosition = value;
            }
        }

        private bool isWalkable = true;

        public bool IsWalkable
        {
            get
            {
                return isWalkable;
            }

            set
            {
                isWalkable = value;
            }
        }

        /// <summary>
        /// drawable for the grid (this is probably never gonna move)
        /// </summary>
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



        /// <summary>
        /// texture for a grid object
        /// </summary>
        private Texture2D gridTexture;

        /// <summary>
        /// parameter for gridTexture
        /// </summary>
        public Texture2D GridTexture
        {
            get
            {
                return gridTexture;
            }

            set
            {
                gridTexture = value;
            }
        }

        private Texture2D highlightTexture;

        public Texture2D HighlightTexture
        {
            get
            {
                return highlightTexture;
            }

            set
            {
                highlightTexture = value;
            }
        }

        private bool highlighted;

        public bool Highlighted
        {
            get
            {
                return highlighted;
            }

            set
            {
                highlighted = value;
            }
        }

        //Whether or not the individual grid position is on fire.  Will affect room health and oxygen levels.
        private bool aflame;

        public bool Aflame
        {
            get
            {
                return aflame;
            }

            set
            {
                aflame = value;
            }
        }

        //Whether or not the individual grid position is breached.  Will affect room oxygen levels.
        private bool hullBreach;

        public bool HullBreach
        {
            get
            {
                return hullBreach;
            }

            set
            {
                hullBreach = value;
            }
        }


        #endregion

        #region constructors / destructors
        public Grid()
            : base()
        {
        }

        public Grid(Texture2D spriteTexture, Texture2D highlight, Vector2 position, Vector2 gPosition)
            : base()
        {
            gridTexture = spriteTexture;
            sprite = new Drawable(gridTexture, position);
            gridPosition = gPosition;
            highlightTexture = highlight;
            hullBreach = false;
            aflame = false;
        }
        public Grid(SerializationInfo si, StreamingContext sc)
            : base()
        {
            gridPosition = (Vector2)si.GetValue("gridPosition", typeof(Vector2));
            isWalkable = si.GetBoolean("isWalkable");
            sprite = (Drawable)si.GetValue("sprite", typeof(Drawable));
            gridTexture = (Texture2D)si.GetValue("gridTexture", typeof(Texture2D));
            highlightTexture = (Texture2D)si.GetValue("highlightTexture", typeof(Texture2D));
            highlighted = si.GetBoolean("highlighted");
            aflame = si.GetBoolean("aflame");
            hullBreach = si.GetBoolean("hullBreach");
        }

        #endregion

        #region methods

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isWalkable == false)
            {
                Sprite.SpriteTexture = highlightTexture;
            }

            if (isWalkable == true)
            {
                Sprite.SpriteTexture = gridTexture;
            }

            Sprite.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        /// <summary>
        /// highlight the grid if its unhighlighted, unhighlight it if its highlighted
        /// </summary>
        public void Highlight()
        {
            if (highlighted == true)
            {
                highlighted = false;
            }

            else
            {
                highlighted = true;
            }
        }

        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("gridPosition", gridPosition, typeof(Vector2));
            si.AddValue("isWalkable", isWalkable);
            si.AddValue("sprite", sprite, typeof(Drawable));
            si.AddValue("gridTexture", gridTexture, typeof(Texture2D));
            si.AddValue("highlightTexture", highlightTexture, typeof(Texture2D));
            si.AddValue("highlighted", highlighted);
            si.AddValue("aflame", aflame);
            si.AddValue("hullBreach", hullBreach);
        }


        #endregion


    }
}
