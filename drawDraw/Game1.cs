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
        private MouseState _prevMouseState;
        
//      changes and modifies the canvas
        private CanvasCommands _canvasCommands = new CanvasCommands();
        
        private SpriteBatch _spriteBatch;
        
        public Game1()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

//      loads the content of the "game" aka mouse buttons and the circleTexture
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Texture2D circleTexture = Content.Load<Texture2D>("circleTexture");
            Texture2D resizeButton  = Content.Load<Texture2D>("resize");
            Texture2D circleButton  = Content.Load<Texture2D>("circle");
            Texture2D eraserButton  = Content.Load<Texture2D>("eraser");
            Texture2D selectButton  = Content.Load<Texture2D>("select");
            Texture2D squareButton  = Content.Load<Texture2D>("square");
            Texture2D moveButton    = Content.Load<Texture2D>("move");
            Texture2D openButton    = Content.Load<Texture2D>("Open");
            Texture2D saveButton    = Content.Load<Texture2D>("save");
            
//          inits the canvas with all needed textures and the graphics device
            _canvas.Init(GraphicsDevice, circleButton, eraserButton, moveButton, selectButton,squareButton, openButton, saveButton, resizeButton, circleTexture);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

//      update function for behaviour
        protected override void Update(GameTime gameTime)
        {
//          get the mouse state
            MouseState mouseState = Mouse.GetState();

//          we exit the "game" when the player clicks on the back button or the escape key
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            
//          we automatically open the save or open dialogue box when we have clicked on that button
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

//          updates the resize and move borders
            UpdateBorders(mouseState);

//          mouse button handler
            if (!_canvas.CheckButtonClick(mouseState, _prevMouseState)) // if there was no button press
            {
//              we handle the click based on the current btnState
                MouseClick(mouseState);
            }

//          we check for a button press
            CheckButtonPress(mouseState);
            
//          we save the current mouse state for single click checks
            _prevMouseState = mouseState;
            
            base.Update(gameTime);
        }

//      this function checks for all the button presses
        private void CheckButtonPress(MouseState mouseState)
        {
            if(_prevMouseState.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
            {
                _canvasCommands.UndoActions();
            } 
            else if (_prevMouseState.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released)
            {
                _canvasCommands.RedoActions();
            }
        }

//      this function handles a mouse click
        private void MouseClick(MouseState mouseState)
        {
//          we handle the click based on the current btnState
            if (_prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released) // click with left mouse button
            {
//              we do different things during different times
                switch (_canvas.BtnStage)
                {
                    case Canvas.ButtonStages.Circle:
                        _canvasCommands.SetCommand(new AddCircle(mouseState));
                        break;
                    case Canvas.ButtonStages.Rectangle:
                        _canvasCommands.SetCommand(new AddRectangle(mouseState));
                        break;
                    case Canvas.ButtonStages.Select:
                        _canvas.SelectTexture(mouseState);
                        break;
                    case Canvas.ButtonStages.Move:
                        _canvasCommands.SetCommand(new MoveTexure(mouseState, _canvas.GetSelected()));
                        break;
                    case Canvas.ButtonStages.Resize:
                        _canvasCommands.SetCommand(new ResizeTexure(mouseState));
                        break;
                }
            }
        }

//      makes the borders follow the mouse
        private void UpdateBorders(MouseState mouseState)
        {
//          borders have been drawn ready to have them follow the mouse
            if (_canvas.BtnStage == Canvas.ButtonStages.Move && _canvas.MoveStage == Canvas.MoveStages.Move)
            {
                _canvas.UpdateBorders(mouseState);
            }
            
            if (_canvas.BtnStage == Canvas.ButtonStages.Resize && _canvas.MoveStage == Canvas.MoveStages.Resize)
            {
                _canvas.UpdateResizeBorders(mouseState);
            }
        }
        
//      ##########################
//      ###End update functions###
//      ##########################
        
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            
//              standard background
                GraphicsDevice.Clear(Color.CornflowerBlue);

//              draws all textures
                _canvas.Draw(_spriteBatch);

                base.Draw(gameTime);
            
            _spriteBatch.End();
        }
    }
}