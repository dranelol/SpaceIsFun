using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monopulse.GameEngine.Entities
{
	public class Animation : ICloneable
	{
		#region fields
        // first frame of animation
        private Rectangle initialFrame;

        // number of frames in the animation
        private int frameCount = 1;

        // current frame
        private int currentFrame = 0;

        // amount of time, in seconds, to display each frame
        private float frameLength = 0.5f;

        // amount of time that has passed since the last frame change
        private float frameTimer = 0.0f;

        // how many times this animation has been played
        private int playedCount = 0;

        // texture string for the animation to be played next
        private string nextAnimation = null;
        #endregion

        #region getters and setters

        /// <summary>
        /// Number of frames in the animation
        /// </summary>
        public int FrameCount
        {
            get
            {
                return frameCount;
            }

            set
            {
                frameCount = value;
            }
        }

        /// <summary>
        /// The time, in seconds, each frame lasts
        /// </summary>
        public float FrameLength
        {
            get
            {
                return frameLength;
            }

            set
            {
                frameLength = value;
            }
        }

        /// <summary>
        /// Width of the frame
        /// </summary>
        public int FrameWidth
        {
            get
            {
                return initialFrame.Width;
            }
        }

        /// <summary>
        /// Height of the frame
        /// </summary>
        public int FrameHeight
        {
            get
            {
                return initialFrame.Height;
            }
        }

        /// <summary>
        /// Current frame being drawn
        /// </summary>
        public int CurrentFrame
        {
            get
            {
                return currentFrame;
            }

            set
            {
                // restricts the current frame to be between 0 and framecount -1
                // don't want negative frames, now do we?
                currentFrame = (int)MathHelper.Clamp(value, 0, frameCount - 1);
            }
        }

        /// <summary>
        /// Rectangle of the animation frame
        /// </summary>
        public Rectangle Frame
        {
            get
            {
                return new Rectangle
                    (
                        initialFrame.X + (initialFrame.Width * currentFrame),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height
                    );
            }
        }

        /// <summary>
        /// How many times this animation has been played
        /// </summary>
        public int PlayedCount
        {
            get
            {
                return playedCount;
            }

            set
            {
                playedCount = value;
            }
        }

        /// <summary>
        /// Next texture string in sequence (what frame will be played next)
        /// </summary>
        public string NextAnimation
        {
            get
            {
                return nextAnimation;
            }

            set
            {
                nextAnimation = value;
            }
        }

        #endregion

		#region constructors
		/// <summary>
		/// when you know the initial rect and the number of frames
		/// </summary>
		/// <param name="first"></param>
		/// <param name="numFrames"></param>
        public Animation(Rectangle first, int numFrames)
        {
            initialFrame = first;
            frameCount = numFrames;
        }

		/// <summary>
		/// when you want to create the initial rect, and you know the number of frames
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="numFrames"></param>
        public Animation(int x, int y, int width, int height, int numFrames)
        {
            initialFrame = new Rectangle(x, y, width, height);
            frameCount = numFrames;
        }

		/// <summary>
		/// when you want to create the initial rect, you know the number of frames, and the length of the frames
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="numFrames"></param>
		/// <param name="length"></param>
        public Animation(int x, int y, int width, int height, int numFrames, float length)
        {
            initialFrame = new Rectangle(x, y, width, height);
            frameCount = numFrames;
            frameLength = length;
        }

		/// <summary>
		/// when you want to create the initial rect, you know the number of frames, the length of the frames, and the next texture to use
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="numFrames"></param>
		/// <param name="length"></param>
		/// <param name="next"></param>
        public Animation(int x, int y, int width, int height, int numFrames, float length, string next)
        {
            initialFrame = new Rectangle(x, y, width, height);
            frameCount = numFrames;
            frameLength = length;
            nextAnimation = next;
        }

        #endregion

		#region methods

        /// <summary>
        /// update
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // set the frame timer to elapsed time 
            frameTimer = frameTimer + (float)gameTime.ElapsedGameTime.TotalSeconds;

            // if the frametime is greater than the frame time length, update that sucker!
            if (frameTimer > frameLength)
            {
                // reset frametimer
                frameTimer = 0.0f;

                // set current frame to the next frame; if we're looping then mod it with frame count
                currentFrame = (currentFrame + 1) % frameCount;

                // if current frame equals 0, we looped; so, increment played count
                if (currentFrame == 0)
                {
                    playedCount = playedCount + 1;
                }
            }
        }

        /// <summary>
        /// clones the animation
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return new Animation
                (
                    this.initialFrame.X,
                    this.initialFrame.Y,
                    this.initialFrame.Width,
                    this.initialFrame.Height,
                    this.frameCount,
                    this.frameLength,
                    nextAnimation
                );
        }

        #endregion
	}

	public class Drawable : IDisposable
	{
		#region fields
		Texture2D currentTexture;
		bool animating = true;
		Color tint = Color.White;
		Vector2 position = new Vector2(0, 0);
		Vector2 lastPosition = new Vector2(0, 0);
		Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
		string currentAnimation = null;
		Vector2 center;
		int width;
		int height;
		Vector2 drawOffset;
		float drawDepth;
		bool active = true; // MAY REMOVE THIS
		bool visible = true;
		#endregion

		#region getters/setters

		/// <summary>
		/// offset, pair x,y from the entity's position from where the drawable will be drawn
		/// </summary>
		public Vector2 DrawOffset
		{
			get
			{
				return drawOffset;
			}

			set
			{
				drawOffset = value;
			}
		}

		/// <summary>
		/// depth at which the drawable is drawn
		/// 0:
		/// 1:
		/// </summary>
		public float DrawDepth
		{
			get
			{
				return drawDepth;
			}

			set
			{
				drawDepth = value;
			}
		}

		/// <summary>
		/// position of the upper left corner (in pixels)
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return position;
			}

			set
			{
				lastPosition = position;
				position = value;
			}
		}

		/// <summary>
		/// last position of the drawable
		/// </summary>
		public Vector2 LastPosition
		{
			get
			{
				return lastPosition;
			}

			set
			{
				lastPosition = value;
			}
		}

		/// <summary>
		/// width of the sprite frame (in pixels)
		/// </summary>
		public int Width
		{
			get
			{
				return width;
			}
		}

		/// <summary>
		/// height of the sprite frame (in pixels)
		/// </summary>
		public int Height
		{
			get
			{
				return height;
			}
		}

		/// <summary>
		/// coordinates, on the screen, of the bounding box of the sprite
		/// </summary>
		public Rectangle boundBox
		{
			get
			{
				return new Rectangle((int)position.X, (int)position.Y, width, height);
			}

		}

		/// <summary>
		/// the texture of the sprite
		/// </summary>
		public Texture2D texture
		{
			get
			{
				return currentTexture;
			}
		}

		/// <summary>
		/// color to tint the sprite with when drawing
		/// </summary>
		public Color Tint
		{
			get
			{
				return tint;
			}

			set
			{
				tint = value;
			}
		}

		/// <summary>
		/// returns true of the sprite is, or should be, animating
		/// if false, this sprite will not be drawn
		/// </summary>
		public bool isAnimating
		{
			get
			{
				return animating;
			}

			set
			{
				animating = value;
			}

		}

		/// <summary>
		/// current frame of the animation
		/// </summary>
		public Animation CurrentFrame
		{
			get
			{
				if (string.IsNullOrEmpty(currentAnimation) == false)
				{
					return animations[currentAnimation];
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// name of the currently playing animation.
		/// setting the animation (giving it a new string) resets currentFrame and playedCount
		/// NOTE: when this class is edited to account for jumping animations, or idle animations,
		/// or sword swings or whatever, we will NEED to change this, and be able to start/stop animations
		/// wherever we wish, or implement multiple spritesheets for one sprite (have a different set of animations
		/// on each sheet)
		/// </summary>
		public string CurrentAnimation
		{
			get
			{
				return currentAnimation;
			}

			set
			{
				if (animations.ContainsKey(value))
				{
					currentAnimation = value;
					animations[currentAnimation].CurrentFrame = 0;
					animations[currentAnimation].PlayedCount = 0;
				}
			}
		}

		public bool IsVisible
		{
			get
			{
				return visible;
			}

			set
			{
				visible = value;
			}
		}

		public bool IsActive
		{
			get
			{
				return active;
			}

			set
			{
				active = value;
			}
		}




		#endregion

		#region constructors / destructors

        /// <summary>
        /// construct the animation, giving it a texture
        /// </summary>
        /// <param name="texture"></param>
        public Drawable(Texture2D texture)
        {
            currentTexture = texture;
            drawOffset = Vector2.Zero;
            drawDepth = 0.0f;
        }

        

        

        #endregion

		#region methods

		/// <summary>
		/// may need to force collection on a drawable, depending on the texture / animation
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// usually use this to add an animation
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="frames"></param>
		/// <param name="frameLength"></param>
		public void AddAnimation(string name, int x, int y, int w, int h, int frames, float frameLength)
		{
			animations.Add(name, new Animation(x, y, w, h, frames, frameLength));
			width = w;
			height = h;
			center = new Vector2((int)(width / 2), (int)(height / 2));
		}

		/// <summary>
		/// use this if you know the next frame in animation
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="frames"></param>
		/// <param name="frameLength"></param>
		/// <param name="nextAnimation"></param>
		public void AddAnimation(string name, int x, int y, int w, int h,
			int frames, float frameLength, string nextAnimation)
		{
			animations.Add(name, new Animation(x, y, w, h, frames, frameLength, nextAnimation));
			width = w;
			height = h;
			center = new Vector2((int)(width / 2), (int)(height / 2));
		}


		/// <summary>
		/// gets the name of the current animation
		/// if there is none, it doesnt exist in the dictionary of animations, and returns null string
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Animation GetAnimation(string name)
		{
			if (animations.ContainsKey(name))
			{
				return animations[name];
			}

			else
			{
				return null;
			}

		}
		
		/// <summary>
		/// move this bro, passing in a delta x and delta y
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void MoveBy(int x, int y)
		{
			lastPosition = position;
			position.X = position.X + x;
			position.Y = position.Y + y;
		}

		/// <summary>
		/// move this bro to a new position x and y
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void MoveTo(int x, int y)
		{
			lastPosition = position;
			position.X = x;
			position.Y = y;
		}

        

		/// <summary>
		/// update this
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			// dont do anything if its not animating
			if (animating == true)
			{
				// if there is not an active animation
				if (CurrentFrame == null)
				{
					// make sure there's an animation 
					if (animations.Count > 0)
					{
						// set the active animation to the first animation
						string[] keys = new string[animations.Count];
						animations.Keys.CopyTo(keys, 0);
						currentAnimation = keys[0];
					}

					else
					{
						return;
					}
				}

				// run the animation's update
				CurrentFrame.Update(gameTime);

				// check to see if there is an animation following this one
				if (String.IsNullOrEmpty(CurrentFrame.NextAnimation) == false)
				{
					// if there is a next animation
					// check if the animation has completed a full cycle
					if (CurrentFrame.PlayedCount > 0)
					{
						currentAnimation = CurrentFrame.NextAnimation;
					}
				}
			}
		}


		/// <summary>
		/// draw this
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="xOffset"></param>
		/// <param name="yOffset"></param>
		public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset)
		{
			if (visible == true) // animating == true
			{
				spriteBatch.Draw
					(
						currentTexture,
						position + new Vector2(xOffset, yOffset) + center + DrawOffset,
						CurrentFrame.Frame,
						tint,
						0.0f,
						center,
						1f,
						SpriteEffects.None,
						DrawDepth
					);
			}
		}
		#endregion
	}
}
