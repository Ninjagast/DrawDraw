using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public class Game1 : Game
    {
//      creates the first and only instance of the canvas class
        private readonly Canvas _canvas = Canvas.Instance;
        private MouseState prevMouseState = new MouseState();
        private ModifyCanvas modifyCanvas = new ModifyCanvas();


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
//          initializes the canvas instance
            _canvas.Init(GraphicsDevice);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

//          borders have been draw ready to have them follow the mouse
            if (_canvas.BtnStage == ButtonStages.Move && _canvas.MoveStage == 1)
            {
                _canvas.UpdateBorders(mouseState);
            }
            
            if (_canvas.CheckButtons(mouseState, prevMouseState))
            {
                // You surely know how to press my buttons ; )
            }
            else if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                // if the user clicks with his left mouse button
                {
                    // #todo change the continuous nature of this code into the menu.
                    // #todo add some sort of modus so we can insert multiple shapes (trough the menu)
                    switch (_canvas.BtnStage)
                    {
                        case ButtonStages.Circle:
                            modifyCanvas.SetCommand(new AddCircle(mouseState));
                            break;
                        case ButtonStages.Rectangle:
                            modifyCanvas.SetCommand(new AddRectangle(mouseState));
                            break;
                        case ButtonStages.Select:
                            _canvas.SelectTexture(mouseState);
                            break;
                        case ButtonStages.Move:
                            modifyCanvas.SetCommand(new MoveTexure(mouseState, _canvas.GetSelected()));
                            break;
                    }
                    // creates circles and squares now
                    // squares.Add(GenerateCircleTexture(GraphicsDevice, 5, Color.Aqua, 1));
                }
            }
            else if(prevMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
            {
                modifyCanvas.UndoActions();
            } 
            else if (prevMouseState.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released)
            {
                modifyCanvas.RedoActions();
            }
            
            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
//          standard background
            GraphicsDevice.Clear(Color.CornflowerBlue);

//          draws all textures
            _canvas.Draw(_spriteBatch);

            base.Draw(gameTime);
            _spriteBatch.End();
        }

//      yoinked function from stackoverflow to generate a circle
        private static Texture2D GenerateCircleTexture(GraphicsDevice graphicsDevice, int radius, Color color, float sharpness)
        {
            var diameter = radius * 2;
            var circleTexture = new Texture2D(graphicsDevice, diameter, diameter, false, SurfaceFormat.Color);
            var colorData = new Color[circleTexture.Width * circleTexture.Height];
            var center = new Vector2(radius);
            for (var colIndex = 0; colIndex < circleTexture.Width; colIndex++)
            for (var rowIndex = 0; rowIndex < circleTexture.Height; rowIndex++)
            {
                var position = new Vector2(colIndex, rowIndex);
                var distance = Vector2.Distance(center, position);

                // hermite iterpolation
                var x = distance / diameter;
                var edge0 = radius * sharpness / diameter;
                var edge1 = radius / (float) diameter;
                var temp = MathHelper.Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
                var result = temp * temp * (3.0f - 2.0f * temp);

                colorData[rowIndex * circleTexture.Width + colIndex] = color * (1f - result);
            }

            circleTexture.SetData(colorData);
            return circleTexture;
        }
    }
}