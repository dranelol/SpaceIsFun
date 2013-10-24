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
    public class Drawable : Object
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

        /// <summary>
        /// which frame of animation we're on. if this isn't an animated drawable, this remains 0
        /// </summary>
        private int frame;

        /// <summary>
        /// parameter for frame
        /// </summary>
        public int Frame
        {
            get
            {
                return frame;
            }

            set
            {
                frame = value;
            }
        }

        /// <summary>
        /// speed of the drawable; how much it moves (in screen space) per frame
        /// </summary>
        private float speed;

        /// <summary>
        /// parameter for speed
        /// </summary>
        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        /// <summary>
        /// current target of moving drawable
        /// </summary>
        private Vector2 target;

        /// <summary>
        /// parameter for target
        /// </summary>
        public Vector2 Target
        {
            get
            {
                return target;
            }

            set
            {
                target = value;
            }
        }

        #endregion

        /// <summary>
        /// whether or not the drawable is moving
        /// </summary>
        private bool moving;

        /// <summary>
        /// paramter for moving
        /// </summary>
        public bool Moving
        {
            get
            {
                return moving;
            }

            set
            {
                moving = value;
            }
        }

        /// <summary>
        /// whether or not the drawable is moving along a path
        /// </summary>
        private bool pathing;

        /// <summary>
        /// parameter for pathing
        /// </summary>
        public bool Pathing
        {
            get
            {
                return pathing;
            }

            set
            {
                pathing = value;
            }
        }

        /// <summary>
        /// path of the drawable
        /// </summary>
        private List<Vector2> path;

        /// <summary>
        /// parameter for path
        /// </summary>
        public List<Vector2> Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
            }
        }

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
            path = new List<Vector2>();
            frame = 0;
            speed = 2;
            
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
            if(pathing == true)
            {
                // find the distance to target
                //double c = Math.Sqrt(Math.Pow((double)(target.X - position2D.X), 2d) + Math.Pow((double)(target.Y - position2D.Y), 2d));
                Vector2 delta = new Vector2(target.X - position2D.X, target.Y - position2D.Y);
                //System.Diagnostics.Debug.WriteLine("target: " + target.ToString());

                // if distance to target is less than speed, we're at our destination
                if (delta.Length() <= (double)speed)
                {
                    // so, move to destination
                    MoveTo(new Vector2(target.X, target.Y));
                    // if path is not empty, pop path and update new target
                    if (path.Count != 0)
                    {
                        target = path[0];
                        path.RemoveAt(0);
                    }

                    // if path is empty, stop moving, stop pathing, set target to null
                    else
                    {
                        pathing = false;
                        moving = false;
                        target = new Vector2();
                    }
                }

                // else, we need to move along the delta
                else
                {
                    // get the movement delta
                    #region old movement code
                    /*
                    double alpha = Math.Asin((target.Y - position2D.Y) / c);

                    // alpha is the angle between c and (x2-x1)

                    double Y = speed * Math.Sin(alpha);

                    double theta = 90d - alpha;

                    double X = speed * Math.Sin(theta);

                    MoveBy(new Vector2((float)X, (float)Y));
                     */
                    #endregion

                    delta.Normalize();
                    delta = delta * speed;
                    MoveBy(delta);
                }

            }






            
            


            // if we have a current target, figure out how much to move by and move

            
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

        /// <summary>
        /// sets the drawable's path
        /// </summary>
        /// <param name="path"></param>
        public void setPath(List<Vector2> path)
        {
            this.path = path;
            pathing = true;
            moving = true;
            target = path[0];
            path.RemoveAt(0);
        }

        

        #endregion

    }
}
