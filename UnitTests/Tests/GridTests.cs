using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceIsFun;
using TestingMono.Api;
using TestingMono.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace UnitTests.Tests
{
    public class TestGrid : TestFixture
    {
        public override void Context()
        {
            //throw new NotImplementedException();
        }


        [TestMethod]
        public void TestGridConstruction()
        {

            Texture2D gridTexture = base.game.Content.Load<Texture2D>("Grid");
            Texture2D gridTextureHighlight = base.game.Content.Load<Texture2D>("GridHighlight");
            Texture2D gridTextureNotWalkable = base.game.Content.Load<Texture2D>("GridNotWalkable");

            Vector2 position = new Vector2(1.0f, 1.0f);
            Vector2 gPosition = new Vector2(1.0f, 1.0f);

            Grid testGrid = new Grid(gridTexture, gridTextureHighlight, position, gPosition);

            Assert.AreEqual<Vector2>(position, testGrid.Sprite.Position2D, "positions are equal");
            

        }
    }
}
