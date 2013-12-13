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

        //Let github see my comments
        String comments;

        List<Vector2> starNodes = new List<Vector2>(); //A list of star nodes
        List<Drawable> starNodeDraws = new List<Drawable>(); //Drawable objects to correlate to the star nodes
        int starNodeSelectedIndex = 0; //The index in the list of nodes for the star that is selected
        Vector2 starNodeSelected; //Vector that correlates with the selected star
        //Drawable overworldCursorDraw; //Drawable for the cursor
        Vector2 cursorCoords = new Vector2();          //Both the cursor coordinates and the selected node
        SoundEffectInstance ThisIntroSong;
        
        void setupOverworld()
        {
            setNodes();


            overworld.enter += () =>
            {
                // setup gui elements here

                // for now, these probably will just be buttons arranged on a map to spawn certain battles / narrative events

                //

                starNodeDraws.Clear();

                System.Diagnostics.Debug.WriteLine("Narr Bool 1: " + narrative1Resolved.ToString());
                System.Diagnostics.Debug.WriteLine("Narr Bool 2: " + narrative2Resolved.ToString());
                System.Diagnostics.Debug.WriteLine("Battle Bool 1: " + battle1Resolved.ToString());
                System.Diagnostics.Debug.WriteLine("Battle Bool 2: " + battle2Resolved.ToString());


                //Traverse through starNodes, and assign gray or regular textures based on which node it is

                #region music
                ThisIntroSong = IntroMusic.CreateInstance();
                ThisIntroSong.IsLooped = true;
                ThisIntroSong.Play();
                #endregion
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
                //Reset the sprite textures of the stars based on the current state of the narrative and battle resolutions
                if (battle1Resolved == true)
                {
                    starNodeDraws[2].SpriteTexture = starGreyedTexture;
                }
                if (narrative1Resolved == true)
                {
                    starNodeDraws[0].SpriteTexture = starGreyedTexture;
                }
                if (battle1Resolved == true && narrative1Resolved == true)
                {
                    starNodeDraws[1].SpriteTexture = starTexture;
                }
                if (narrative2Resolved == true)
                {
                    starNodeDraws[1].SpriteTexture = starGreyedTexture;
                    starNodeDraws[3].SpriteTexture = starTexture;
                }
            };

            overworld.update += (GameTime gameTime) =>
            {
                #region input handling

                //Go to the next star node based on which key is pressed
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

                //Completely reset all of the stars when the user presses control. 
                if (currentKeyState.IsKeyUp(Keys.LeftControl) && previousKeyState.IsKeyDown(Keys.LeftControl))
                {
                    resetNodes();
                }
                
                

                /*
                if (battle1Resolved && narrative1Resolved)
                {
                    
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
                 */

                //Win condition
                if (narrative1Resolved && narrative2Resolved && battle1Resolved && battle2Resolved)
                {
                    //Generate you win message here
                }


                //Upon pressing Enter, transition to the necessary state based on which node is selected and whether or not they meet the criteria
                if (currentKeyState.IsKeyUp(Keys.Enter) && previousKeyState.IsKeyDown(Keys.Enter))
                {
                    if (starNodeSelectedIndex == (int)NodeState.Narrative1)
                    {
                        if (narrative1Resolved == false)
                        {
                            //Do some 
                            //System.Diagnostics.Debug.WriteLine("Narrative1");
                    
                            stateMachine.Transition(narrative.Name);

                            
                        }
                    }
                    else if (starNodeSelectedIndex == (int)NodeState.Narrative2)
                    {
                        if (narrative1Resolved && battle1Resolved)
                        {
                            //Do some more  if narrative2
                            //System.Diagnostics.Debug.WriteLine("Narrative2");
                            
                           
                            stateMachine.Transition(narrative.Name);
                        }
                    }
                    else if (starNodeSelectedIndex == (int)NodeState.Battle1)
                    {
                        if (battle1Resolved == false)
                        {
                            //Battle some 
                            //System.Diagnostics.Debug.WriteLine(",  and lollipops.  Unicorn on top.");

                           
                            battle1Resolved = true;
                            stateMachine.Transition(battle.Name);
                            
                        }
                    }
                    else if (starNodeSelectedIndex == (int)NodeState.Battle2)
                    {
                        if (narrative2Resolved)
                        {
                            //Battle some more
                            //System.Diagnostics.Debug.WriteLine(",  and lollipops.   just don't stop.");
                            gameStateUID = 2;
                            battle2Resolved = true;
                            stateMachine.Transition(battle.Name);
                            
                        }
                    }
                }


                #endregion
               

                cursorCoords = starNodes[starNodeSelectedIndex];
                overworldCursorDraw.MoveTo(cursorCoords);
               

            };

            overworld.leave += () =>
            {
                //Stop the music on exit
                ThisIntroSong.Stop();
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
            
            //Add four stars to the list of star nodes at random points on the screen
            //starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));
            //starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));
            //starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));
            //starNodes.Add(new Vector2(rand.Next(50, graphics.PreferredBackBufferWidth), rand.Next(100, graphics.PreferredBackBufferHeight - 100)));

            starNodes.Add(new Vector2(50, 50));
            starNodes.Add(new Vector2(200, 200));
            starNodes.Add(new Vector2(500, 500));
            starNodes.Add(new Vector2(150, 400));
            
        }

        private void resetNodes()
        {
            //Nullify the old star node lists and draw lists so we can reset them
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
            //Cut star map in vertical 'halves'  by finding the stars with a y coordinate lower than the selected node.
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
            //Put the distances in another list
            List<double> distances = new List<double>();
            foreach (var item in mapHalf)
            {
                double distance;
                float xDistance = item.X - cursorCoords.X;
                float yDistance = item.Y - cursorCoords.Y;
                distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
                distances.Add(distance);

            }
            
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

            //Whichever has the lowest cartesian distance is the one we reassign the select to
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
