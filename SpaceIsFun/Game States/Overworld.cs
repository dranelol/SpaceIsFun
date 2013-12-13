using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;

namespace SpaceIsFun
{
    public partial class Game1 : Game
    {
        //States for what the nodes can be
        enum NodeState { Narrative1, Narrative2, Battle1, Battle2 }

        List<Vector2> starNodes = new List<Vector2>();
        List<Drawable> starNodeDraws = new List<Drawable>();
        int starNodeSelectedIndex = 0;
        Vector2 starNodeSelected;
        Drawable overworldCursorDraw;
        Vector2 cursorCoords = new Vector2();          //Both the cursor coordinates and the selected node

        void setupOverworld()
        {
            
            overworld.enter += () =>
            {
                // setup gui elements here

                // for now, these probably will just be buttons arranged on a map to spawn certain battles / narrative events

                //

                setNodes();

                IntroMusic.Play();

                for (int i = 0; i < starNodes.Count; i++)
                {
                    if (i == (int)NodeState.Narrative1 || i == (int)NodeState.Battle1)
                    {
                        starNodeDraws.Add(new Drawable(starTexture, starNodes[i]));
                    }
                    else if (i == (int)NodeState.Battle2 || i == (int)NodeState.Narrative2)
                    {
                        starNodeDraws.Add(new Drawable(starGreyedTexture, starNodes[i]));
                    }
                }
                /*
                foreach (Vector2 item in starNodes) {
                    starNodeDraws.Add(new Drawable(starTexture, item));
                }
                 * */
            };

            overworld.update += (GameTime gameTime) =>
            {
                #region input handling

                if (currentKeyState.IsKeyUp(Keys.Up) && previousKeyState.IsKeyDown(Keys.Up))
                {
                    traverseStarsUp();
                }
                else if (currentKeyState.IsKeyUp(Keys.Down) && previousKeyState.IsKeyDown(Keys.Down))
                {
                    traverseStarsDown();
                }
                else if (currentKeyState.IsKeyUp(Keys.Left) && previousKeyState.IsKeyDown(Keys.Left))
                {
                    traverseStarsLeft();
                }
                else if (currentKeyState.IsKeyUp(Keys.Right) && previousKeyState.IsKeyDown(Keys.Right))
                {
                    traverseStarsRight();
                }

                if (currentKeyState.IsKeyUp(Keys.LeftControl) && previousKeyState.IsKeyDown(Keys.LeftControl))
                {
                    resetNodes();
                }

                if (battle1Resolved && narrative1Resolved)
                {
                    starNodeDraws[1].SpriteTexture = starTexture;
                }
                if (narrative2Resolved)
                {
                    starNodeDraws[3].SpriteTexture = starTexture;
                }
                if (battle1Resolved)
                {
                    starNodeDraws[2].SpriteTexture = starGreyedTexture;
                }
                if (narrative1Resolved)
                {
                    starNodeDraws[0].SpriteTexture = starGreyedTexture;
                }
                if (narrative2Resolved)
                {
                    starNodeDraws[1].SpriteTexture = starGreyedTexture;
                }
                if (battle2Resolved)
                {
                    starNodeDraws[3].SpriteTexture = starGreyedTexture;
                }

                if (narrative1Resolved && narrative2Resolved && battle1Resolved && battle2Resolved)
                {
                    //Generate you win message here
                }


                //Upon Enter press, check if the
                if (currentKeyState.IsKeyUp(Keys.Enter) && previousKeyState.IsKeyDown(Keys.Enter))
                {
                    if (starNodeSelectedIndex == (int)NodeState.Narrative1)
                    {
                        if (narrative1Resolved == false)
                        {
                            //Do some shit
                            System.Diagnostics.Debug.WriteLine("Narrative1");
                            narrative1Resolved = true;
                        }
                    }
                    else if (starNodeSelectedIndex == (int)NodeState.Narrative2)
                    {
                        if (narrative1Resolved && battle1Resolved)
                        {
                            //Do some more shit if narrative2
                            System.Diagnostics.Debug.WriteLine("Narrative2");
                            narrative2Resolved = true;
                        }
                    }
                    else if (starNodeSelectedIndex == (int)NodeState.Battle1)
                    {
                        if (battle1Resolved == false)
                        {
                            //Battle some shit
                            System.Diagnostics.Debug.WriteLine("Penises, Penises and lollipops.  Unicorn on top.");
                            battle1Resolved = true;
                        }
                    }
                    else if (starNodeSelectedIndex == (int)NodeState.Battle2)
                    {
                        if (narrative2Resolved)
                        {
                            //Battle some more shit
                            System.Diagnostics.Debug.WriteLine("Penises, Penises and lollipops.  Big floppy penises, just don't stop.");
                            battle2Resolved = true;
                        }
                    }
                }


                //If user hits down, traverseStarsLeft
                //If user hits right, traverseStarsRight

                #endregion
               
               //This is where it would draw if it weren't dumb

                cursorCoords = starNodes[starNodeSelectedIndex];
                overworldCursorDraw = new Drawable(overworldCursorTexture, cursorCoords);
               

            };

            overworld.leave += () =>
            {
                // remove gui elements here
            };
        }

        /// <summary>
        /// Create a list of vector2's.
        /// Randomly create coordinates in between 0 and screen width/height
        /// </summary>
        private void setNodes()
        {
            Random rand = new Random();
            
            
            starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));
            starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));
            starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));
            starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));
            
        }

        private void resetNodes()
        {
            starNodes = new List<Vector2>();
            starNodeDraws = new List<Drawable>();

            setNodes();

            narrative1Resolved = false;
            narrative2Resolved = false;
            battle1Resolved = false;
            battle2Resolved = false;

            for (int i = 0; i < starNodes.Count; i++)
            {
                if (i == (int)NodeState.Narrative1 || i == (int)NodeState.Battle1)
                {
                    starNodeDraws.Add(new Drawable(starTexture, starNodes[i]));
                }
                else if (i == (int)NodeState.Battle2 || i == (int)NodeState.Narrative2)
                {
                    starNodeDraws.Add(new Drawable(starGreyedTexture, starNodes[i]));
                }
            }
        }

        private void traverseStarsUp()
        {
            //Cut star map in vertical 'halves' 
            List<Vector2> mapHalf = new List<Vector2>();
            foreach (var item in starNodes)
            {
                if (item != cursorCoords)
                {
                    if (item.Y < cursorCoords.Y)
                    {
                        mapHalf.Add(item);
                    }
                }
            }
            //Calculate cartesian distance between selected node and every other node left
            List<double> distances = new List<double>();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
                distances.Add(distance);

            }

            //Whichever has the lowest cartesian distance is the one we reassign the select to
            //Recalc all distances, if the distance is equal to the min distance in distances, the set cursor coords to it.
            distances.Sort();
            Vector2 temp = new Vector2();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));

                if (distances[0] == distance)
                {
                    temp = item;
                }
            }

            for (int i = 0; i < starNodes.Count; i++)
            {
                if (temp == starNodes[i])
                {
                    starNodeSelectedIndex = i;
                }
            }
        }



        private void traverseStarsDown()
        {
            //Cut star map in vertical 'halves' 
            List<Vector2> mapHalf = new List<Vector2>();
            foreach (var item in starNodes)
            {
                if (item != cursorCoords)
                {
                    if (item.Y > cursorCoords.Y)
                    {
                        mapHalf.Add(item);
                    }
                }
            }
            //Calculate cartesian distance between selected node and every other node left
            List<double> distances = new List<double>();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
                distances.Add(distance);

            }

            //Whichever has the lowest cartesian distance is the one we reassign the select to
            //Recalc all distances, if the distance is equal to the min distance in distances, the set cursor coords to it.
            distances.Sort();
            Vector2 temp = new Vector2();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));

                if (distances[0] == distance)
                {
                    temp = item;
                }
            }

            for (int i = 0; i < starNodes.Count; i++)
            {
                if (temp == starNodes[i])
                {
                    starNodeSelectedIndex = i;
                }
            }
        }



        private void traverseStarsLeft()
        {
            //Cut star map in horizontal 'halves' 
            List<Vector2> mapHalf = new List<Vector2>();
            foreach (var item in starNodes)
            {
                if (item != cursorCoords)
                {
                    if (item.X < cursorCoords.X)
                    {
                        mapHalf.Add(item);
                    }
                }
            }
            //Calculate cartesian distance between selected node and every other node left
            List<double> distances = new List<double>();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
                distances.Add(distance);

            }

            //Whichever has the lowest cartesian distance is the one we reassign the select to
            //Recalc all distances, if the distance is equal to the min distance in distances, the set cursor coords to it.
            distances.Sort();
            Vector2 temp = new Vector2();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));

                if (distances[0] == distance)
                {
                    temp = item;
                }
            }

            for (int i = 0; i < starNodes.Count; i++)
            {
                if (temp == starNodes[i])
                {
                    starNodeSelectedIndex = i;
                }
            }
        }

        private void traverseStarsRight()
        {
            //Cut star map in vertical 'halves' 
            List<Vector2> mapHalf = new List<Vector2>();
            foreach (var item in starNodes)
            {
                if (item != cursorCoords)
                {
                    if (item.X > cursorCoords.X)
                    {
                        mapHalf.Add(item);
                    }
                }
            }
            //Calculate cartesian distance between selected node and every other node left
            List<double> distances = new List<double>();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
                distances.Add(distance);

            }

            //Whichever has the lowest cartesian distance is the one we reassign the select to
            //Recalc all distances, if the distance is equal to the min distance in distances, the set cursor coords to it.
            distances.Sort();
            Vector2 temp = new Vector2();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));

                if (distances[0] == distance)
                {
                    temp = item;
                }
            }

            for (int i = 0; i < starNodes.Count; i++)
            {
                if (temp == starNodes[i])
                {
                    starNodeSelectedIndex = i;
                }
            }
        }
    }
}
