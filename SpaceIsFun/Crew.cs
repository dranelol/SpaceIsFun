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
    [Serializable] class Crew : Entity, ISerializable
    {
        #region fields

        /// <summary>
        /// position of the crewman on grid space
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Parameter for position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Max HP of the crewman
        /// </summary>
        private int maxHP;

        /// <summary>
        /// Parameter for max HP
        /// </summary>
        public int MaxHP
        {
            get
            {
                return maxHP;
            }

            set
            {
                maxHP = value;
            }
        }

        /// <summary>
        /// Current HP of the crewman
        /// </summary>
        private int currentHP;

        /// <summary>
        /// Parameter for current HP
        /// </summary>
        public int CurrentHP
        {
            get
            {
                return currentHP;
            }

            set
            {
                currentHP = value;
            }
        }

        /// <summary>
        /// the ship's Drawable object
        /// </summary>
        private Drawable sprite;

        /// <summary>
        /// parameter for sprite
        /// </summary>
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
        /// texture for the crewman
        /// </summary>
        private Texture2D crewTexture;

        /// <summary>
        /// texture for the crewman while selected
        /// </summary>
        private Texture2D crewSelectedTexture;

        /// <summary>
        /// parameter for crewTexture
        /// </summary>
        public Texture2D CrewTexture
        {
            get
            {
                return crewTexture;
            }

            set
            {
                crewTexture = value;
            }
        }

        /// <summary>
        /// paramter for crewSelectedTexture
        /// </summary>
        public Texture2D CrewSelectedTexture
        {
            get
            {
                return crewSelectedTexture;
            }

            set
            {
                crewSelectedTexture = value;
            }
        }

        /// <summary>
        /// whether or not the crewman is selected
        /// </summary>
        private bool selected;

        /// <summary>
        /// parameter for selected
        /// </summary>
        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
            }
        }

        #endregion

        #region constructors / destructors
        /// <summary>
        /// construtor of crewman
        /// </summary>
        /// <param name="position">starting position of the crewman</param>
        /// <param name="crewTexture">texture of the crewman when not selected</param>
        /// <param name="crewSelectedTexture">texture of the crewman when selected</param>
        public Crew(Vector2 position, Vector2 gPosition ,Texture2D crewTexture, Texture2D crewSelectedTexture)
            : base()
        {
            maxHP = currentHP = 100;
            this.position = gPosition;

            this.crewTexture = crewTexture;
            this.crewSelectedTexture = crewSelectedTexture;
            this.selected = false;

            sprite = new Drawable(crewTexture, position);



        }
        //Constructor used when deserializing object
        public Crew(SerializationInfo si, StreamingContext sc) : base()
        {
            position = (Vector2) si.GetValue("position", typeof(Vector2));
            maxHP = si.GetInt32("maxHP");
            currentHP = si.GetInt32("currentHP");
            sprite = (Drawable)si.GetValue("sprite", typeof(Drawable));
            crewTexture = (Texture2D)si.GetValue("crewTexture", typeof(Texture2D));
            crewSelectedTexture = (Texture2D)si.GetValue("crewSelectedTexture", typeof(Texture2D));
            selected = si.GetBoolean("selected");

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
            
            
            if (selected == true)
            {
                Sprite.SpriteTexture = crewSelectedTexture;
            }

            if (selected == false)
            {
                Sprite.SpriteTexture = crewTexture;
            }

            Sprite.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }


        /// <summary>
        /// highlight the grid if its unhighlighted, unhighlight it if its highlighted
        /// </summary>
        public void Select()
        {
            if (selected == true)
            {
                selected = false;
            }

            else
            {
                selected = true;
            }
        }


        public void Move(List<Vector2> path)
        {
            //I have a dream that one day this function will exist, that it will tell the sprite where to move, and the sprite will move there as decreed by the mighty A* algorithm given to us by Peter Hart, Nils Nilsson and Bertram Raphael of the hallowed Stanford Research Instituteendregion
            System.Diagnostics.Debug.WriteLine("Positions Before Move: "+sprite.Position2D.ToString());
            sprite.setPath(path);
            System.Diagnostics.Debug.WriteLine("Positions After Move: " + sprite.Position2D.ToString());
        }

        //Function used when serializing an object
        public virtual void GetObjectData(SerializationInfo si, StreamingContext sc)
        {
            si.AddValue("position", position, typeof(Vector2));
            si.AddValue("maxHP", maxHP);
            si.AddValue("currentHP", currentHP);
            si.AddValue("sprite", sprite, typeof(Drawable));
            si.AddValue("crewTexture", crewTexture, typeof(Texture2D));
            si.AddValue("crewSelectedTexture", crewSelectedTexture, typeof(Texture2D));
            si.AddValue("selected", selected);

        }

        #endregion

    }
}
