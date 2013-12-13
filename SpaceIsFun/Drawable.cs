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
    /// Different ways an animation can run 
    /// </summary>		
    public enum AnimationType
    {
        Single,		/**< Runs through animation one time */
        Oscillate,	/**< Runs through animation back and forth */
        Loop,		/**< Runs through animation indefinitely */
        None		/**< Doesn't Animate */
    };

    /// <summary>
    ///  This is the object that holds all the info for drawing and animating an entity
    /// </summary>
    public class Drawable : Object
    {
        #region fields

        /// <summary>
        /// Spritesheet for the drawable
        /// </summary>
        private Texture2D spriteTexture;

        /// <summary>
        /// parameter for spriteTexture
        /// </summary>
        public Texture2D SpriteTexture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }

        /// <summary>
        /// 2D position of the drawable in screen space
        /// </summary>
        private Vector2 position2D;

        /// <summary>
        /// Parameter for position2D
        /// </summary>
        public Vector2 Position2D
        {
            get { return position2D; }
            set { position2D = value; }
        }

        /// <summary>
        /// Width of the drawable's sprite in pixels
        /// </summary>
        private int width;

        /// <summary>
        /// Parameter for width
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Height of the drawable's sprite in pixels
        /// </summary>
        private int height;

        /// <summary>
        /// Parameter for height
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Which frame of animation we're on. If this isn't an animated drawable, this remains 0
        /// </summary>
        private int frame;

        /// <summary>
        /// Parameter for frame
        /// </summary>
        public int Frame
        {
            get { return frame; }
            set { frame = value; }
        }

        /// <summary>
        /// Speed of the drawable; how much it moves (in screen space) per frame
        /// </summary>
        private float speed;

        /// <summary>
        /// Parameter for speed
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// Target (X,Y) grid coordinates the drawable is moving towards
        /// </summary>
        private Vector2 target;

        /// <summary>
        /// Parameter for target
        /// </summary>
        public Vector2 Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// Whether the drawable is currently moving
        /// </summary>
        private bool moving;

        /// <summary>
        /// Paramter for moving
        /// </summary>
        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        /// <summary>
        /// Whether the drawable is currently moving along a path
        /// </summary>
        private bool pathing;

        /// <summary>
        /// Parameter for pathing
        /// </summary>
        public bool Pathing
        {
            get { return pathing; }
            set { pathing = value; }
        }

        /// <summary>
        /// List of Vector2 (X,Y) grid cells that make up the drawable's path
        /// </summary>
        private List<Vector2> path;

        /// <summary>
        /// Parameter for path list
        /// </summary>
        public List<Vector2> Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// Vector2 (X,Y) coordinates of the drawable's current grid position
        /// </summary>
        private Vector2 positionGrid;

        /// <summary>
        /// Parameter for grid position
        /// </summary>
        public Vector2 PositionGrid
        {
            get { return positionGrid; }
            set { positionGrid = value; }
        }

        //////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Number of columns in spritesheet
        /// </summary>
        private int numColumns;

        /// <summary>
        /// Parameter for Number of columns in spritesheet
        /// </summary>
        public int NumColumns
        {
            get { return numColumns; }
            set { numColumns = value; }
        }

        /// <summary>
        /// Number of rows in spritesheet
        /// </summary>
        private int numRows;

        /// <summary>
        /// Parameter for Number of rows in spritesheet
        /// </summary>
        public int NumRows
        {
            get { return numRows; }
            set { numRows = value; }
        }

        /// <summary>
        /// The type of animation
        /// </summary>
        private AnimationType animType;

        /// <summary>
        /// Parameter for Animation Type
        /// </summary>
        public AnimationType AnimType
        {
            get { return animType; }
            set { animType = value; }
        }

        /// <summary>
        /// The frame to start the animation on
        /// </summary>
        private int startFrame;

        /// <summary>
        /// Parameter for Start Frame
        /// </summary>
        public int StartFrame
        {
            get { return startFrame; }
            set { startFrame = (value <= numRows * numColumns) ? value : 1; }
        }

        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        private int animLength;

        /// <summary>
        /// Parameter for animation length
        /// </summary>
        public int AnimLength
        {
            get { return animLength; }
            set { animLength = value; }
        }

        /// Functional Variables
        /// Variables that are only used and modified by the methods of this class
        private float lastFrame;	//!< Time the frame was last changed
        private Rectangle src;      //!< Bounding box of individual sprite
        //private Vector2 offset;   //!< X,Y relative to the texture for the current frame
        private bool playing;       //!< bool to indicate whether the animation is currently playing or not
        private bool recoil;        //!< indicates direction of oscillation; true indicates left

        #endregion

        #region constructors / destructors

        /// <summary>
        /// Simple constructor for a drawable (non-Animated)
        /// </summary>
        /// <param name="sprite"> Spritesheet for the drawable</param>
        /// <param name="position"> 2D position of the drawable in screen space</param>
        public Drawable(Texture2D sprite, Vector2 position)
            : base()
        {
            spriteTexture = sprite;
            position2D = position;
            width = spriteTexture.Bounds.Width;
            height = spriteTexture.Bounds.Height;
            frame = 0;
            speed = 1;
            animLength = 1;
            animType = AnimationType.None;
            numColumns = 1;
            numRows = 1;
            Initialize();
        }

        /// <summary>
        /// Full constructor for an animated drawable
        /// </summary>
        /// <param name="sprite"> Spritesheet for the drawable</param>
        /// <param name="position"> 2D position of the drawable in screen space</param>
        /// <param name="w"> Width of individual sprite</param>
        /// <param name="h"> Height of individual sprite</param>
        /// <param name="spd"> Frame rate of the animation in 1/100 of a sec</param>
        /// <param name="nCols"> Number of columns in the spritesheet</param>
        /// <param name="nRows"> Number of rows in the spritesheet</param>
        /// <param name="aType"> How the frames are animated</param>
        /// <param name="start"> The initial frame of the animation</param>
        /// <param name="length"> The number of frames in the animation</param>
        public Drawable(Texture2D sprite, Vector2 position, int w, int h, float spd,
                        int nCols, int nRows, AnimationType aType, int start, int length)
            : base()
        {
            // Parameter's passed in:
            spriteTexture = sprite;
            position2D = position;
            width = w;
            height = h;
            speed = spd;
            numColumns = nCols;
            numRows = nRows;
            animType = aType;
            startFrame = start;
            animLength = length;
            Initialize();
        }

        /// <summary>
        /// Catch-All Constructor
        /// </summary>
        /// <param name="sprite"> Texture to draw</param>
        /// <param name="position"> 2D position of the drawable in screen space</param>
        /// <param name="gPosition"> Position of the drawable in the grid space</param>
        public Drawable(Texture2D sprite, Vector2 position, Vector2 gPosition, 
                        int nCols, int nRows, AnimationType aType, int start, int length)
            : base()
        {
            spriteTexture = sprite;
            position2D = position;
            positionGrid = gPosition;
            width = (int)(spriteTexture.Bounds.Width/nCols);
            height = (int)(spriteTexture.Bounds.Height/nRows);
            startFrame = start;
            speed = 1;
            animLength = length;
            animType = AnimationType.Loop;
            numColumns = nCols;
            numRows = nRows;
            Initialize();
        }


        #endregion

        #region methods

        /// <summary>
        /// Initialize the drawable
        /// </summary>
        public void Initialize()
        {
            lastFrame = 0.0f;
            src = new Rectangle(0, 0, width, height);
            src.Y = (int)(frame / numColumns) * height;
            src.X = (frame % numColumns) * width;
            playing = false;
            recoil = false;
            path = new List<Vector2>();
            frame = startFrame;
        }

        /// <summary>
        /// Load any content for the drawable
        /// </summary>
        public void LoadContent()
        {

        }

        /// <summary>
        /// Update the drawable
        /// </summary>
        /// <param name="gameTime"> Current game time</param>
        public void Update(GameTime gameTime)
        {
            //[DEBUG]/ System.Diagnostics.Debug.WriteLine("position: "+position2D.ToString());

            //[DEBUG]/ System.Diagnostics.Debug.WriteLine("updating");

            //[UPDATE ANIMATION FRAME]
            //1. If this drawable is animated && the time since the last frame change is greater than animation speed
            //   > Based on the animation type determine the next frame
            //   > Set the src Vector2 to that coordinates of that frame on the Texture
            if ((animType != AnimationType.None) && (playing))
            {
                lastFrame += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (lastFrame >= speed * 0.1)
                {
                    //[DETERMINE NEXT FRAME]
                    switch (animType)
                    {
                        case AnimationType.Loop:
                            frame = (frame == (startFrame - 1 + animLength)) ? startFrame : frame + 1;
                            break;
                        case AnimationType.Oscillate:
                            if (!recoil) // If frame animation is forward...
                            {
                                if (frame == (startFrame - 1 + animLength))
                                {
                                    recoil = true;
                                    frame--;
                                }
                                else frame++;
                            }
                            else        // Else if frame animation is reversed
                            {
                                if (frame == startFrame)
                                {
                                    recoil = false;
                                    frame++;
                                }
                                else frame--;
                            }
                            break;
                        case AnimationType.Single:
                            if (frame == (startFrame - 1 + animLength)) playing = false;
                            else frame++;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Animation Issue");
                            break;
                    }

                    //[SET NEXT FRAME]
                    src.Y = (int)(frame / numColumns) * height;
                    src.X = (frame % numColumns) * width;
                }
            }

            if (pathing == true)
            {
                //[DEBUG]/ System.Diagnostics.Debug.WriteLine("updating");

                // Find the distance to target
                /*
				double c = Math.Sqrt(Math.Pow((double)(target.X - position2D.X), 2d) +
					Math.Pow((double)(target.Y - position2D.Y), 2d));
                
				//[DEBUG]/ System.Diagnostics.Debug.WriteLine("target: " + target.ToString());
                */
                Vector2 delta = new Vector2(target.X - position2D.X, target.Y - position2D.Y);

                //[DEBUG]/ System.Diagnostics.Debug.WriteLine("target: " + target.ToString());

                // If distance to target is less than speed, we're at our destination
                if (delta.Length() <= (double)speed)
                {
                    // so, move to destination
                    MoveTo(new Vector2(target.X, target.Y));
                    // if path is not empty, pop path and update new target
                    if (path.Count != 0)
                    {
                        this.positionGrid = target;
                        //[DEBUG]/ System.Diagnostics.Debug.WriteLine("Draw Position: "+positionGrid.ToString());
                        target = path[0];
                        path.RemoveAt(0);
                    }

                    // Else if path is empty, stop moving, stop pathing, set target to null
                    else
                    {
                        pathing = false;
                        moving = false;
                        target = new Vector2();
                        playing = false;
                    }
                }

                // Else, we need to move along the delta
                else
                {
                    // Get the movement delta
                    #region old movement code
                    /*
                    double alpha = Math.Asin((target.Y - position2D.Y) / c);

                    // Alpha is the angle between c and (x2-x1)

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
            spriteBatch.Draw(spriteTexture, position2D, src, Color.White);
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
        /// Move the drawable to newPosition in screen space
        /// </summary>
        /// <param name="newPosition"> Position in screen space to which the drawable should be moved</param>
        public void MoveTo(Vector2 newPosition)
        {
            position2D = newPosition;
        }

        /// <summary>
        /// Sets the drawable's path
        /// </summary>
        /// <param name="path">List of grid X,Y coordinates of the drawable's path </param>
        public void setPath(List<Vector2> path)
        {
            this.path = path;
            pathing = true;
            moving = true;
            playing = true;
            target = path[0];
            path.RemoveAt(0);
        }



        #endregion

    }
}
