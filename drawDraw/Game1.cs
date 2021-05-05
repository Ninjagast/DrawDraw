using System.Runtime.Serialization;
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
            Texture2D CircleTexture = Content.Load<Texture2D>("circleTexture");
            
            _canvas.Init(GraphicsDevice, circleButton, eraserButton, moveButton, selectButton,squareButton, openButton, saveButton, resizeButton, CircleTexture);
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
                    Console.WriteLine("oooopen");
                    _canvas.OpenFile();
                    break;
                case Canvas.ButtonStages.Save:
                    Console.WriteLine("saaaaaaaaav");
                    _canvas.SaveFile();
                    break;
            }

//          borders have been drawn ready to have them follow the mouse
            if (_canvas.BtnStage == Canvas.ButtonStages.Move && _canvas.MoveStage == Canvas.MoveStages.Move)
            {
                _canvas.UpdateBorders(mouseState);
            }
            
//          borders have been drawn ready to have them follow the mouse
            if (_canvas.BtnStage == Canvas.ButtonStages.Resize && _canvas.MoveStage == Canvas.MoveStages.Resize)
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
                        _modifyCanvas.SetCommand(new MoveTexure(mouseState, _canvas.GetSelected()));
                        break;
                    case Canvas.ButtonStages.Resize:
                        _modifyCanvas.SetCommand(new ResizeTexure(mouseState));
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
    }
}