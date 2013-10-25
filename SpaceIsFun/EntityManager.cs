using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace SpaceIsFun
{
    public class EntityManager
    {
        protected Dictionary<int, Entity> objects = new Dictionary<int, Entity>();

        public Dictionary<int,Entity> Objects
        {
            get
            {
                return objects;
            }

            set
            {
                objects = value;
            }
        }

        //singleton implementation

        /*
        protected static EntityManager instance;

        public static EntityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityManager();
                }

                return instance;
            }
        }
        */

        /// <summary>
        /// random number generator for use in generating unique IDs
        /// </summary>
        private Random rng;

        /// <summary>
        /// initializes the object manager
        /// </summary>
        public void Initialize()
        {
            rng = new Random();

        }

        /// <summary>
        /// update the objects in the entity manager
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// draw the objects in the entity manager
        /// </summary>
        /// <param name="spriteBatch">main spriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }

        /// <summary>
        /// adds an entity to the entitymanager
        /// </summary>
        /// <param name="toAdd">entity to add to the entitymanager</param>
        public void AddEntity(Entity toAdd)
        {
            // create unique ID 

            
        }

        /// <summary>
        /// returns an entity object depending on the UID passed in
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public Entity RetrieveEntity(int UID)
        {
            return objects[UID];
        }


    }
}
