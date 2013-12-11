using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;


namespace SpaceIsFun
{
    public class EntityManager
    {
        protected Dictionary<int, Entity> objects = new Dictionary<int, Entity>();

        protected List<Entity> addList = new List<Entity>();

        protected List<Entity> deleteList = new List<Entity>();

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
        /// the last unique identifier assigned
        /// </summary>
        private int UIDcurrent = 0;

        /// <summary>
        /// initializes the object manager
        /// </summary>
        public void Initialize()
        {
            rng = new Random();
            objects.Clear();
            addList.Clear();
            deleteList.Clear();

        }

        /// <summary>
        /// update the objects in the entity manager
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public void Update(GameTime gameTime)
        {
            /*
            // add any objects that need to be added
            foreach (Entity entity in addList)
            {
                objects.Add(entity.UID, entity);
                entity.Initialize();
                entity.LoadContent();
            }

            addList.Clear();

            // delete any objects that need to be deleted
            foreach (Entity entity in deleteList)
            {
                // call GC

                // remove from objects list
                objects.Remove(entity.UID);
            }

            deleteList.Clear();
            */
            foreach(Entity entity in objects.Values)
            {
                entity.Update(gameTime);
            }
        }

        /// <summary>
        /// draw the objects in the entity manager
        /// </summary>
        /// <param name="spriteBatch">main spriteBatch object</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in objects.Values)
            {
                entity.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// adds an object to the object manager
        /// </summary>
        /// <param name="toAdd"></param>
        /// <returns>returns the UID of the object added</returns>
        public int AddEntity(Entity toAdd)
        {
            // for now, just increment the unique ID till there isnt a dictionary key collision
            while(objects.ContainsKey(UIDcurrent) == true)
            {
                UIDcurrent++;
            }

            toAdd.UID = UIDcurrent;
            //addList.Add(toAdd);

            objects.Add(UIDcurrent,toAdd);

            return UIDcurrent;
        }

        public void DeleteEntity(int uid)
        {
            //Entity toDelete = RetrieveEntity(uid);
            objects.Remove(uid);
            //deleteList.Add(toDelete);
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

        public Dictionary<int,Entity>.KeyCollection RetrieveKeys()
        {
            return objects.Keys;
        }


    }
}
