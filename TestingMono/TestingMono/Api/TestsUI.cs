using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace TestingMono.Api
{

    public class TestsUi// : DrawableGameComponent
    {
        private TestFixtureRunner _testFixtureRunner; 
        private SpriteFont _font;
        Vector2 _previousTouchPosition = new Vector2(0f, 0f);
        private float _yOffset;
        private float _xOffset;
        private float _fontHeight = 10;
        private List<UiTestLine> _testLines = new List<UiTestLine>();
        private float _widestLine;
        private Game game;

        public TestsUi(Game game)// : base(game)
        {
            this.game = game;
            _testFixtureRunner = new TestFixtureRunner(game);
        }

        public void SetFont(SpriteFont font)
        {
            _font = font;
        }

        public void RunTests(params Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                _testFixtureRunner.RunTests(assembly);
            }
        }

        public void Update(GameTime gameTime)
        {
            //base.Update(gameTime);

            TouchCollection touchCollection = TouchPanel.GetState();
            Vector2 touchPosition = new Vector2(_previousTouchPosition.X, _previousTouchPosition.Y);

            foreach (TouchLocation touchLocation in touchCollection)
            {
                touchPosition = touchLocation.Position;
            }

            float absoluteY = Math.Abs(touchPosition.Y - _previousTouchPosition.Y);
            float absoluteX = Math.Abs(touchPosition.X - _previousTouchPosition.X);

            if (absoluteY > absoluteX)
                ScrollVertically(touchPosition);
            else
                ScrollHorizontally(touchPosition);

            _previousTouchPosition = touchPosition;
        }

        private void ScrollVertically(Vector2 touchPosition)
        {
            if (_previousTouchPosition.Y < touchPosition.Y)
                _yOffset += _fontHeight / 4;
            else if (_previousTouchPosition.Y > touchPosition.Y)
                _yOffset -= _fontHeight / 4;

            //Don't allow to scroll off the page
            if (_yOffset < (-(_fontHeight * _testLines.Count)) + 100)
                _yOffset = (-(_fontHeight * _testLines.Count)) + 100;
            if (_yOffset > (_fontHeight * _testLines.Count) - 100)
                _yOffset = (_fontHeight * _testLines.Count) - 100;
        }

        private void ScrollHorizontally(Vector2 touchPosition)
        {
            if (_previousTouchPosition.X < touchPosition.X)
                _xOffset += _fontHeight / 2; //FontHeight will do as a rule of thumb
            else if (_previousTouchPosition.X > touchPosition.X)
                _xOffset -= _fontHeight / 2;

            //Don't allow to scroll off the page
            if (_xOffset < -(_widestLine / 2 + _widestLine / 3))
                _xOffset = -(_widestLine / 2 + _widestLine / 3);
            if (_xOffset > _widestLine / 2 + _widestLine / 3)
                _xOffset = _widestLine / 2 + _widestLine / 3;
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = new SpriteBatch(game.GraphicsDevice);

            _fontHeight = _font.MeasureString("W").Y + 5;

            int i = 1;

            if (_testLines == null || _testLines.Count == 0)
            {
                _testLines = new List<UiTestLine>();

                foreach (var line in _testFixtureRunner.GetTestExecutionTree())
                {
                    string newLine = string.Empty;

                    if (line.Passed && !string.IsNullOrEmpty(line.TestMethod))
                    {
                        newLine = string.Format("    {0} Test Passed", line.TestMethod);
                        _testLines.Add(new UiTestLine { Line = newLine });
                    }
                    else if (!line.Passed && !string.IsNullOrEmpty(line.TestMethod))
                    {
                        _testLines.Add(new UiTestLine { Line = string.Format("    {0} Test Failed", line.Message) });
                        _testLines.Add(new UiTestLine { Line = string.Format("       {0}", line.FailedReason) });
                    }
                    else
                    {
                        _testLines.Add(new UiTestLine { Line = line.Message });
                    }

                    float lineWidth = _font.MeasureString(newLine).X;

                    if (lineWidth > _widestLine)
                        _widestLine = lineWidth;
                }
            }

            spriteBatch.Begin();

            foreach (var testLine in _testLines)
            {
                testLine.YPosition = (int)((i * _fontHeight) + _yOffset);
                if (testLine.YPosition > 0 && testLine.YPosition < 600)
                {
                    Color color = Color.White;
                    string line = testLine.Line == null ? string.Empty : testLine.Line;

                    if (line.ToUpper().Contains("TEST FAILED") || line.ToUpper().Contains("TESTFIXTURE FAILED"))
                    {
                        color = Color.Red;
                    }
                    else if (line.ToUpper().Contains("TEST PASSED") || line.ToUpper().Contains("TESTFIXTURE PASSED"))
                    {
                        color = Color.Green;
                    }


                    spriteBatch.DrawString(_font, line, new Vector2(_xOffset, testLine.YPosition),
                       color);
                }
                i++;
            }

            spriteBatch.End();
        }
    }
}
