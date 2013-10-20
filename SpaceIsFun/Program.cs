#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;




#endregion

namespace SpaceIsFun
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    
    



    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        

        public Game1 thisGame;

        [STAThread]
        static void Main()
        {

            Program myProgram = new Program();

        }

        public Program()
        {
            Game1 game = new Game1();
            thisGame = game;

            game.Run();
            
            /*
            using (var game = new Game1())
                game.Run();
             */
        }

        
    }
    [TestClass]
    public class testClass : Program
    {
        [TestMethod]
        public void dumb()
        {
            Texture2D gridSprite = base.thisGame.Content.Load<Texture2D>("Grid");
        }
    }
#endif
}
