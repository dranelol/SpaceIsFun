using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DigiRuneGameEngine
{
    


    public class MapRow
    {
        public List<Cell> Columns = new List<Cell>();
    }

    public class Map
    {
        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 20;
        public int MapHeight = 20;

        private Texture2D mouseMap;

        // for now, all tiles are "walkable" except for those with extra tiles on them.
        public int GetIndex(int x, int y)
        {
            if (Rows[y].Columns[x].ExtraTiles.Count > 0)
            {
                return 1;
            }

            return 0;
        }

        public Map(Texture2D MouseMap)
        {
            mouseMap = MouseMap;

            for (int y = 0; y < MapHeight; y++)
            {
                MapRow row = new MapRow();

                for (int x = 0; x < MapWidth; x++)
                {
                    row.Columns.Add(new Cell(0));
                }

                Rows.Add(row);
            }

            #region Setting up the map (NOTE: DO THIS BETTER)

            // Setting base tiles
            for( int y = 0;y < MapHeight; y++)
            {
                for(int x = 0; x < MapWidth; x++)
                {
                    Rows[y].Columns[x].TileID = 1;
                }
            }


            //Rows[4].Columns[12].AddHeightTile(54);
            //Rows[4].Columns[12].IncrementHeight(1);
            //Rows[3].Columns[11].AddHeightTile(54);
            //Rows[3].Columns[11].DecrementHeight(1);

            List<ExtraTileStruct> cliff = new List<ExtraTileStruct>();

            cliff.Add(new ExtraTileStruct(0, 0, 54));
            cliff.Add(new ExtraTileStruct(0, -1, 54));
            Rows[4].Columns[12].AddHeightTiles(cliff);
            Rows[4].Columns[12].IncrementHeight(2);

            /*
            Rows[0].Columns[3].TileID = 3;
            Rows[0].Columns[4].TileID = 3;
            Rows[0].Columns[5].TileID = 1;
            Rows[0].Columns[6].TileID = 1;
            Rows[0].Columns[7].TileID = 1;

            Rows[1].Columns[3].TileID = 3;
            Rows[1].Columns[4].TileID = 1;
            Rows[1].Columns[5].TileID = 1;
            Rows[1].Columns[6].TileID = 1;
            Rows[1].Columns[7].TileID = 1;

            Rows[2].Columns[2].TileID = 3;
            Rows[2].Columns[3].TileID = 1;
            Rows[2].Columns[4].TileID = 1;
            Rows[2].Columns[5].TileID = 1;
            Rows[2].Columns[6].TileID = 1;
            Rows[2].Columns[7].TileID = 1;

            Rows[3].Columns[2].TileID = 3;
            Rows[3].Columns[3].TileID = 1;
            Rows[3].Columns[4].TileID = 1;
            Rows[3].Columns[5].TileID = 2;
            Rows[3].Columns[6].TileID = 2;
            Rows[3].Columns[7].TileID = 2;

            Rows[4].Columns[2].TileID = 3;
            Rows[4].Columns[3].TileID = 1;
            Rows[4].Columns[4].TileID = 1;
            Rows[4].Columns[5].TileID = 2;
            Rows[4].Columns[6].TileID = 2;
            Rows[4].Columns[7].TileID = 2;

            Rows[5].Columns[2].TileID = 3;
            Rows[5].Columns[3].TileID = 1;
            Rows[5].Columns[4].TileID = 1;
            Rows[5].Columns[5].TileID = 2;
            Rows[5].Columns[6].TileID = 2;
            Rows[5].Columns[7].TileID = 2;

            
            Rows[11].Columns[6].TileID = 3;
            Rows[13].Columns[6].TileID = 3;

            // Tiles for the stony waterfall
            Rows[16].Columns[4].AddHeightTile(54);

            Rows[17].Columns[3].AddHeightTile(54);

            Rows[15].Columns[3].AddHeightTile(54);
            Rows[16].Columns[3].AddHeightTile(53);

            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(51);

            Rows[18].Columns[3].AddHeightTile(51);
            Rows[19].Columns[3].AddHeightTile(50);
            Rows[18].Columns[4].AddHeightTile(55);

            Rows[14].Columns[4].AddHeightTile(54);

            Rows[14].Columns[5].AddHeightTile(62);
            Rows[14].Columns[5].AddHeightTile(61);
            Rows[14].Columns[5].AddHeightTile(63);

            // Tiles for the sloped hill, in the middle.
            Rows[12].Columns[7].AddHeightTile(33);
            Rows[13].Columns[7].AddHeightTile(31);
            Rows[14].Columns[8].AddHeightTile(30);
            Rows[12].Columns[8].AddHeightTile(34);
            Rows[13].Columns[8].AddHeightTile(32);
            Rows[12].Columns[9].AddHeightTile(35);
            Rows[11].Columns[8].AddHeightTile(37);
            Rows[10].Columns[8].AddHeightTile(38);
            Rows[11].Columns[7].AddHeightTile(36);


            // Tiles for the layered hill, to the east.
            Rows[7].Columns[10].AddHeightTile(34);
            Rows[7].Columns[10].AddHeightTile(34);
            Rows[7].Columns[10].AddHeightTile(34);

            Rows[8].Columns[10].AddHeightTile(34);
            Rows[8].Columns[10].AddHeightTile(34);

            Rows[9].Columns[10].AddHeightTile(34);
            Rows[9].Columns[10].AddHeightTile(34);

            Rows[8].Columns[11].AddHeightTile(34);
            Rows[8].Columns[11].AddHeightTile(34);

            Rows[7].Columns[9].AddHeightTile(34);
            Rows[7].Columns[9].AddHeightTile(34);

            Rows[6].Columns[10].AddHeightTile(34);
            Rows[6].Columns[10].AddHeightTile(34);

            Rows[5].Columns[10].AddHeightTile(34);
            Rows[5].Columns[10].AddHeightTile(34);

            Rows[6].Columns[11].AddHeightTile(34);
            Rows[6].Columns[11].AddHeightTile(34);

            Rows[7].Columns[11].AddHeightTile(34);
            Rows[7].Columns[11].AddHeightTile(34);

            Rows[9].Columns[9].AddHeightTile(34);
            Rows[10].Columns[10].AddHeightTile(34);
            Rows[11].Columns[10].AddHeightTile(34);
            Rows[10].Columns[11].AddHeightTile(34);
            Rows[9].Columns[11].AddHeightTile(34);

            Rows[8].Columns[9].AddHeightTile(34);
            Rows[7].Columns[8].AddHeightTile(34);
            Rows[6].Columns[9].AddHeightTile(34);
            Rows[5].Columns[9].AddHeightTile(34);
            Rows[4].Columns[10].AddHeightTile(34);
            Rows[3].Columns[10].AddHeightTile(34);
            Rows[4].Columns[11].AddHeightTile(34);
            Rows[5].Columns[11].AddHeightTile(34);
            Rows[6].Columns[12].AddHeightTile(34);
            Rows[7].Columns[12].AddHeightTile(34);
            Rows[8].Columns[12].AddHeightTile(34);
            */

            List<ExtraTileStruct> tree1 = new List<ExtraTileStruct>();
            
            tree1.Add(new ExtraTileStruct(0,-2,130));
            tree1.Add(new ExtraTileStruct(0, -1, 140));
            tree1.Add(new ExtraTileStruct(0, 0, 150));
            
            Rows[18].Columns[4].AddExtraTiles(tree1);
            Rows[18].Columns[4].IncrementHeight(3);

            List<ExtraTileStruct> tree2 = new List<ExtraTileStruct>();
            tree2.Add(new ExtraTileStruct(0, 0, 158));
            tree2.Add(new ExtraTileStruct(-1, 0, 157));
            tree2.Add(new ExtraTileStruct(1, 0, 159));
            tree2.Add(new ExtraTileStruct(0, -1, 148));
            tree2.Add(new ExtraTileStruct(-1, -1, 147));
            tree2.Add(new ExtraTileStruct(1, -1, 149));
            tree2.Add(new ExtraTileStruct(0, -2, 138));
            tree2.Add(new ExtraTileStruct(-1, -2, 137));
            tree2.Add(new ExtraTileStruct(1, -2, 139));

            Rows[12].Columns[8].AddExtraTiles(tree2);
            Rows[12].Columns[8].IncrementHeight(3);

            AddSingleExtraTile(13, 18, 114);
            AddSingleExtraTile(13, 19, 114);
            AddSingleExtraTile(13, 17, 114);
            AddSingleExtraTile(14, 17, 114);
            AddSingleExtraTile(15, 17, 114);
            AddSingleExtraTile(17, 18, 114);
            AddSingleExtraTile(17, 19, 114);
            AddSingleExtraTile(18, 17, 114);
            AddSingleExtraTile(18, 18, 114);
            AddSingleExtraTile(15, 19, 114);
            AddSingleExtraTile(15, 16, 114);
            AddSingleExtraTile(15, 15, 114);
            AddSingleExtraTile(14, 15, 114);
            AddSingleExtraTile(14, 14, 114);
            AddSingleExtraTile(13, 14, 114);
            AddSingleExtraTile(13, 13, 114);
            AddSingleExtraTile(12, 13, 114);
            AddSingleExtraTile(12, 12, 114);
            AddSingleExtraTile(11, 12, 114);
            AddSingleExtraTile(11, 11, 114);
            AddSingleExtraTile(10, 11, 114);

            
            //List<ExtraTileStruct> shittytree = new List<ExtraTileStruct>();
            //shittytree.Add(new ExtraTileStruct(0, 0, 192));
            //shittytree.Add(new ExtraTileStruct(0, -1, 182));
            //shittytree.Add(new ExtraTileStruct(0, -2, 172));
            //shittytree.Add(new ExtraTileStruct(0, -3, 162));
            //shittytree.Add(new ExtraTileStruct(-1, 0, 191));
            //shittytree.Add(new ExtraTileStruct(-1, -1, 181));
            //shittytree.Add(new ExtraTileStruct(1, -1, 183));
            //shittytree.Add(new ExtraTileStruct(-1, -2, 171));
            //shittytree.Add(new ExtraTileStruct(-2, -2, 170));
            //shittytree.Add(new ExtraTileStruct(1, -2, 173));
            //shittytree.Add(new ExtraTileStruct(-1, -3, 161));
            //shittytree.Add(new ExtraTileStruct(-2, -3, 160));
            //shittytree.Add(new ExtraTileStruct(1, -3, 163));

            //Rows[13].Columns[18].AddExtraTiles(shittytree);
            //Rows[13].Columns[18].IncrementHeight(4);
            
            /*
            #region 1height hill with bush

            Rows[21].Columns[11].AddHeightTile(34);
            Rows[23].Columns[11].AddHeightTile(34);
            Rows[25].Columns[11].AddHeightTile(34);
            Rows[22].Columns[11].AddHeightTile(34);
            Rows[24].Columns[11].AddHeightTile(34);
            Rows[23].Columns[10].AddHeightTile(34);
            Rows[22].Columns[12].AddHeightTile(34);
            Rows[24].Columns[12].AddHeightTile(34);
            Rows[23].Columns[12].AddHeightTile(34);

            List<ExtraTileStruct> bush = new List<ExtraTileStruct>();

            bush.Add(new ExtraTileStruct(0, 0, 129));

            Rows[23].Columns[11].AddExtraTiles(bush);

            Rows[23].Columns[11].IncrementHeight(1);

            
            */
            #endregion

            


        }

        /// <summary>
        /// maps a world point to a map point, based on the mouse map, returning the map cell point
        /// NOTE: DO NOT USE THIS FUNCTION, USE THE ONE BELOW IT
        /// </summary>
        /// <param name="world"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public Point WorldToMapCell(Point world, out Point local)
        {
            Point mapCell = new Point(
               (int)(world.X / mouseMap.Width),
               ((int)(world.Y / mouseMap.Height)) * 2
               );

            int localPointX = world.X % mouseMap.Width;
            int localPointY = world.Y % mouseMap.Height;

            int dx = 0;
            int dy = 0;

            uint[] myUint = new uint[1];

            if (new Rectangle(0, 0, mouseMap.Width, mouseMap.Height).Contains(localPointX, localPointY) == true)
            {
                mouseMap.GetData(0, new Rectangle(localPointX, localPointY, 1, 1), myUint, 0, 1);

                if (myUint[0] == 0xFF0000FF) // red
                {
                    dx = -1;
                    //dy = -1;
                    localPointX = localPointX + (mouseMap.Width / 2);
                    localPointY = localPointY + (mouseMap.Height / 2);
                }

                if (myUint[0] == 0xFF00FF00) // green
                {
                    //dx = -1;
                    localPointX = localPointX + (mouseMap.Width / 2);
                    dy = 1;
                    localPointY = localPointY - (mouseMap.Height / 2);
                }

                if (myUint[0] == 0xFF00FFFF) // yellow
                {
                    dy = -1;
                    localPointX = localPointX - (mouseMap.Width / 2);
                    localPointY = localPointY + (mouseMap.Height / 2);
                }

                if (myUint[0] == 0xFFFF0000) // blue
                {
                    //dy = +1;
                    dx = +1;
                    localPointX = localPointX - (mouseMap.Width / 2);
                    localPointY = localPointY - (mouseMap.Height / 2);
                }
            }

            mapCell.X = mapCell.X + dx;
            mapCell.Y = mapCell.Y + dy -2;

            local = new Point(localPointX, localPointY);

            if (mapCell.X < 0)
            {
                mapCell.X = 0;
            }

            if (mapCell.Y < 0)
            {
                mapCell.Y = 0;
            }


            return mapCell;

        }

        /// <summary>
        /// use this function to call worldtomapcell
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        public Point WorldToMapCell(Point world)
        {
            Point local;
            return WorldToMapCell(world, out local);
        }

        public Point WorldToMap(Point World)
        {
            // screenOriginPoint - the top-left most point in screen space 
            // of the top-left most tile on the map, where drawing starts
            // screenOffset - how far the screen has been shifted from origin; scrolling
            //float WorldX . World.X - ( screenOriginPoint.x + screenOffset.x);
            float WorldX = World.X - (640 + 0);
            float WorldY = World.Y - (0 + 0);

            float MapX = (float)(Math.Round( (WorldX / 64) + (WorldY / 32))) - 1;
            float MapY = (float)(Math.Round( (WorldX / 64 * -1) + (WorldY / 32)));

            return new Point((int)MapX -1, (int)MapY-1);
        }
        
        void AddSingleExtraTile(int row, int column, int tileID)
        {
            List<ExtraTileStruct> tile = new List<ExtraTileStruct>();

            tile.Add(new ExtraTileStruct(0,0,tileID));

            Rows[row].Columns[column].AddExtraTiles(tile);
            Rows[row].Columns[column].IncrementHeight(1);
        }

    }
}
