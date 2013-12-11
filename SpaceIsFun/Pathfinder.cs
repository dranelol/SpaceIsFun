using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Ruminate.GUI.Framework;
using Ruminate.GUI.Content;


namespace SpaceIsFun
{
    /// <summary>
    /// Reresents one node in the search space
    /// </summary>
    public class SearchNode
    {

        /// <summary
        /// Position on the map
        /// </summary>
        /// 
        
        public Vector2 Position
        {
            get;
            set;
        }
        
        /// <summary>
        /// If true, this position is passable by anyone
        /// </summary

        public bool Walkable
        {
            get;
            set;
        }

        /// <summary>
        /// This contains the four nodes surrounding the tile (North, South, East, West)
        /// </summary>
        /// 
        public SearchNode[] Neighbors;

        /// <summary>
        /// A reference to the node that transferred this node to the open list. This will be used to trace back from the target
        /// node to the start
        /// </summary>
        public SearchNode Parent;

        /// <summary>
        /// Checks whether or not this node is in the open list
        /// </summary>
        public bool InOpenList;

        /// <summary>
        /// Checks whether or not this node is in the closed list
        /// </summary>
        public bool InClosedList;

        /// <summary>
        /// Approximate distance from the start to the goal
        /// </summary>
        public float DistanceToGoal;

        /// <summary>
        /// Current total distance traveled
        /// </summary>
        public float DistanceTraveled;
        

    }

    public class Pathfinder
    {
        // Array of the walkable nodes

        private SearchNode[,] searchNodes;

        // Width of the map

        private int mapWidth;

        // Height of the map
        private int mapHeight;

        // size of one grid square
        private int sizeofOneGrid = 32;

        /// <summary>
        /// Constructor
        /// </summary>
        public Pathfinder(int[,] map, EntityManager manager)
        {
            mapWidth = map.GetLength(0);
            mapHeight = map.GetLength(1);

            System.Diagnostics.Debug.WriteLine(mapWidth.ToString() + ", " + mapHeight.ToString());
            
            InitSearchNodes(map, manager);

            // instead of units, i want to path with source grid and target grid

            

        }

        /// <summary>
        /// This function initializes the map's search tiles
        /// Precondition: 
        /// </summary>
        private void InitSearchNodes(int[,] map, EntityManager manager)
        {
            searchNodes = new SearchNode[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    SearchNode node = new SearchNode();

                    node.Position = new Vector2(x, y);

                    // heuristic for "is this node walkable?"

                    Grid grid = (Grid)manager.RetrieveEntity(map[x, y]);

                    if (grid.Highlighted == false)  
                    { 
                        node.Walkable = true;
                    }

                    else
                    {
                        node.Walkable = false;
                    }

                    //node.Walkable = true;
                    
                    
                    if (node.Walkable == true)
                    {
                        node.Neighbors = new SearchNode[4];


                        searchNodes[x, y] = node;
                    }

                }

            }

        
            // Now, connect each search node to its neighbor

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];


                    // Only look at the nodes that are walkable

                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    Vector2[] neighbors = new Vector2[]
                    {
                        new Vector2(x, y - 1), // The node above the current node
                        new Vector2(x, y + 1), // The node below the current node
                        new Vector2(x - 1, y), // The node to the left of the current node
                        new Vector2(x + 1, y), // The node to the right of the current node
                    };

                    // Loop through all the neighbors

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Vector2 position = neighbors[i];

                        // Make sure this neighbor is part of the level
                        if (position.X < 0
                            || position.X > mapWidth - 1
                            || position.Y < 0
                            || position.Y > mapHeight - 1)
                        {
                            continue;
                        }


                        SearchNode neighbor = searchNodes[(int)position.X, (int)position.Y];

                        // Again, only care about the nodes that can be walked on

                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }


                        // And finally, store a reference to the neighbor itself

                        node.Neighbors[i] = neighbor;

                    }
                }
            }
        }

        /// <summary>
        /// Holds the nodes available to search
        /// </summary>
        private List<SearchNode> openList = new List<SearchNode>();

        /// <summary>
        /// Holds the nodes that have already been searched
        /// </summary>
        private List<SearchNode> closedList = new List<SearchNode>();

        /// <summary>
        /// Heuristic for figuring out the distance between two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        private float GetManhattanDistance(Vector2 point1, Vector2 point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);
        }

        /// <summary>
        /// Clears all node lists, resets state of each node
        /// </summary>
        private void ResetNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        // if node is already empty, skip this one
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceToGoal = float.MaxValue;
                    node.DistanceTraveled = float.MaxValue;
                }
            }
        }

        private SearchNode FindBestNode()
        {
            SearchNode current = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // find dat node bruh

            // iterate through list, finding the node with the smallest distance to the target node
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    current = openList[i];
                    smallestDistanceToGoal = current.DistanceToGoal;
                }
            }

            return current;
        }

        /// <summary>
        /// Using the parent nodes, find a path from the end to the start
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<Vector2> FindPath(SearchNode start, SearchNode end)
        {
            closedList.Add(end);

            SearchNode parent = end.Parent;

            // go back through the path using node.Parent to find the path

            while (parent != start)
            {
                closedList.Add(parent);
                parent = parent.Parent;
            }

            List<Vector2> path = new List<Vector2>();

            // reverse the path, transform into points(grids)

           

            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                path.Add(new Vector2(closedList[i].Position.X * 32, closedList[i].Position.Y * 32)); //<--------------------------------------------------Change 32 to global variables
            }

            return path;
        }

        /// <summary>
        /// Find the actual path, from start to end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Vector2> FindOptimalPath(Vector2 start, Vector2 end)
        {
            //Vector2 start = new Vector2((int)(source.sprite.X / 64), (int)(source.sprite.X / 64));
            //Vector2 end = new Vector2((int)(target.sprite.X / 64), (int)(target.sprite.X / 64));
            
            // if the first and last nodes are the same, the path is irrelevant
            if (start == end)
            {
                return new List<Vector2>();
            }

            // Start by clearing the open and closed lists, resetting state of every node
            ResetNodes();

            SearchNode startNode = searchNodes[(int)start.X, (int)start.Y];
            SearchNode endNode = searchNodes[(int)end.X, (int)end.Y];

            // set the start node's weight (distance travelled) to 0
            // set the start node's distance to goal as the distance between start and end

            startNode.InOpenList = true;
            startNode.DistanceToGoal = GetManhattanDistance(start, end);
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            // while there are nodes to search in the openlist, loop through
            // the open list and find the node that has the smallest distance to goal 

            while(openList.Count > 0)
            {
                SearchNode current = FindBestNode();

                // if the open list is empty, or no node could be found, no path can be 
                // found so terminate and handle it

                if(current == null)
                {
                    break;
                }

                // if current is the goal node, find and return the path.

                if(current == endNode)
                {
                    // find path back to the start
                    return FindPath(startNode,endNode);
                }

                // loop through each of current's neighbors

                for(int i=0;i<current.Neighbors.Length; i++)
                {
                    SearchNode neighbor = current.Neighbors[i];

                    // make sure the neighbor is walkable

                    if(neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    // get a new distancetravelled value for the neighbor

                    float distance = current.DistanceTraveled + 1;

                    // find distance between this node and the end
                    float manhattanDistance = GetManhattanDistance(neighbor.Position, end);

                    // if the neighbor is not in the open or closed list

                    if(neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        // set the neighbor's distancetravelled value to the one just calculated
                        neighbor.DistanceTraveled = distance;

                        // set the neighbor's distancetogoal to the new distance plus the distance
                        // between the neighbor and the goal

                        neighbor.DistanceToGoal = distance + manhattanDistance;

                        // set the neighbor's parent to current

                        neighbor.Parent = current;

                        // add the neighbor to openlist

                        neighbor.Parent  = current;

                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }

                    // else if the neighbor is in either the open or closed lists

                    else if(neighbor.InOpenList == true || neighbor.InClosedList == true)
                    {
                        // if the calculated distance to goal is less than the neighbor's 
                        // distance to goal, assign new distance to goal and distance travelled 
                        // to the neighbor, but do not add it to the open or closed list; it already
                        // exists in one

                        if(neighbor.DistanceTraveled > distance)
                        {
                            neighbor.DistanceTraveled = distance;
                            neighbor.DistanceToGoal = distance + manhattanDistance;

                            neighbor.Parent = current;
                        }
                    }
                }

                // remove current node from the open list, and move it to closed; its already been searched
                openList.Remove(current);
                current.InClosedList = true;
            }
            // no node was found, return empty list

            return new List<Vector2>();
                
        }

    }
}
