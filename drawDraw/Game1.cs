﻿using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public class Game1 : Game
    {
//      creates the first and only instance of the canvas class
        private readonly Canvas _canvas = Canvas.Instance;
        private MouseState _prevMouseState = new MouseState();
        private ModifyCanvas _modifyCanvas = new ModifyCanvas();

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D circleButton = Content.Load<Texture2D>("circle");
            Texture2D eraserButton = Content.Load<Texture2D>("eraser");
            Texture2D moveButton   = Content.Load<Texture2D>("move");
            Texture2D selectButton = Content.Load<Texture2D>("select");
            Texture2D squareButton = Content.Load<Texture2D>("square");
            Texture2D openButton   = Content.Load<Texture2D>("Open");
            Texture2D saveButton   = Content.Load<Texture2D>("save");
            Texture2D resizeButton = Content.Load<Texture2D>("resize");
            
            _canvas.Init(GraphicsDevice, circleButton, eraserButton, moveButton, selectButton,squareButton, openButton, saveButton, resizeButton);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            
            switch (_canvas.BtnStage)
            {
                case Canvas.ButtonStages.Open:
                    //#todo Add file browser here which can open files and load them into the system
                    break;
                case Canvas.ButtonStages.Save:
                    Console.WriteLine("saaaaaaaaav");
                    _canvas.SaveFile();
                    break;
            }

//          borders have been drawn ready to have them follow the mouse
            if (_canvas.BtnStage == Canvas.ButtonStages.Move && _canvas.MoveStage == 1)
            {
                _canvas.UpdateBorders(mouseState);
            }
            
//          borders have been drawn ready to have them follow the mouse
            if (_canvas.BtnStage == Canvas.ButtonStages.Resize && _canvas.MoveStage == 2)
            {
                _canvas.UpdateResizeBorders(mouseState);
            }
            
            if (_canvas.CheckButtons(mouseState, _prevMouseState))
            {
//              You surely know how to press my buttons ; )
            }
            else if (_prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
//              if the user clicks with his left mouse button
                switch (_canvas.BtnStage)
                {
                    case Canvas.ButtonStages.Circle:
                        _modifyCanvas.SetCommand(new AddCircle(mouseState));
                        break;
                    case Canvas.ButtonStages.Rectangle:
                        _modifyCanvas.SetCommand(new AddRectangle(mouseState));
                        break;
                    case Canvas.ButtonStages.Select:
                        _canvas.SelectTexture(mouseState);
                        break;
                    case Canvas.ButtonStages.Move:
                        modifyCanvas.SetCommand(new MoveTexure(mouseState, _canvas.GetSelected()));
                        break;
                    case Canvas.ButtonStages.Resize:
                        _canvas.ResizeStuff(mouseState);
                        break;
                            
                }
                // creates circles and squares now
                // squares.Add(GenerateCircleTexture(GraphicsDevice, 5, Color.Aqua, 1));
            }
            else if(_prevMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
            {
                _modifyCanvas.UndoActions();
            } 
            else if (_prevMouseState.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released)
            {
                _modifyCanvas.RedoActions();
            }
            
            _prevMouseState = mouseState;
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