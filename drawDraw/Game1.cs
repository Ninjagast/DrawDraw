using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

//      temporary vars to hack around shape placement
        
        private Canvas _canvas = Canvas.Instance;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _canvas.Init(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
//              #todo add a shape to the tree here so we can draw it in the draw function.
//              #todo change the continuous nature of this code into the menu.

//              creates circles and squares now
                _canvas.InsertRectangle(new Point(mouseState.X, mouseState.Y));
                // squares.Add(GenerateCircleTexture(GraphicsDevice, 5, Color.Aqua, 1));
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _canvas.Draw(_spriteBatch);
                
            base.Draw(gameTime);
            _spriteBatch.End();
        }

        private static Texture2D GenerateCircleTexture(GraphicsDevice graphicsDevice, int radius, Color color, float sharpness)
        {
            var diameter = radius * 2;
            var circleTexture = new Texture2D(graphicsDevice, diameter, diameter, false, SurfaceFormat.Color);
            var colorData = new Color[circleTexture.Width * circleTexture.Height];
            var center = new Vector2(radius);
            for (var colIndex = 0; colIndex < circleTexture.Width; colIndex++)
            {
                for (var rowIndex = 0; rowIndex < circleTexture.Height; rowIndex++)
                {
                    var position = new Vector2(colIndex, rowIndex);
                    var distance = Vector2.Distance(center, position);

                    // hermite iterpolation
                    var x = distance / diameter;
                    var edge0 = (radius * sharpness) / (float)diameter;
                    var edge1 = radius / (float)diameter;
                    var temp = MathHelper.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
                    var result = temp * temp * (3.0f - 2.0f * temp);

                    colorData[rowIndex * circleTexture.Width + colIndex] = color * (1f - result);
                }
            }
            circleTexture.SetData<Color>(colorData);

            return circleTexture;
        }
    }
}