﻿using Microsoft.Xna.Framework;
using System;
using DrawDraw.CommandPattern;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public class Game1 : Game
    {
//      creates the first and only instance of the canvas class
        private readonly Canvas _canvas = Canvas.Instance;
//      Save the previus mouseState
        private MouseState _prevMouseState; 
//      Save the previus KeyState
        private KeyboardState _previousState;
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
            _previousState = Keyboard.GetState();
        }
        
//      loads the content of the "game" aka mouse buttons and the circleTexture
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D menuBackground= Content.Load<Texture2D>("background");
            Texture2D circleTexture = Content.Load<Texture2D>("circleTexture");
            Texture2D resizeButton  = Content.Load<Texture2D>("resize");
            Texture2D circleButton  = Content.Load<Texture2D>("circle");
            Texture2D eraserButton  = Content.Load<Texture2D>("eraser");
            Texture2D selectButton  = Content.Load<Texture2D>("select");
            Texture2D squareButton  = Content.Load<Texture2D>("square");
            Texture2D moveButton    = Content.Load<Texture2D>("move");
            Texture2D openButton    = Content.Load<Texture2D>("Open");
            Texture2D saveButton    = Content.Load<Texture2D>("save");
            Texture2D groupButton   = Content.Load<Texture2D>("group");
            Texture2D clearButton   = Content.Load<Texture2D>("emptyCanvas");
            Texture2D captionButton = Content.Load<Texture2D>("captionCanvas");
            SpriteFont font         = Content.Load<SpriteFont>("fonts");
            
//          inits the canvas with all needed textures and the graphics device
            _canvas.Init(GraphicsDevice, circleButton, eraserButton, moveButton, selectButton,squareButton, openButton, saveButton, resizeButton,groupButton, clearButton, circleTexture, captionButton, font, menuBackground);
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
            
//          Poll for current keyboard state
            KeyboardState state = Keyboard.GetState();

//          we exit the "game" when the player clicks on the back button or the escape key
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            
//          we automatically open the save or open dialogue box when we have clicked on that button
            switch (_canvas.BtnStage)
            {
                case Canvas.ButtonStages.Open:
                    _canvas.OpenFile();
                    break;
                case Canvas.ButtonStages.Save:
                    _canvas.SaveFile();
                    break;
                case Canvas.ButtonStages.Group:
                    _canvasCommands.SetCommand(new GroupTexure());
                    break;
                case Canvas.ButtonStages.Clear:
                    _canvasCommands.SetCommand(new ClearCanvas());
                    break;
            }

//          mouse button handler
//          if there was no button press and there was a mouse click
            if (!_canvas.CheckButtonClick(mouseState, _prevMouseState) && (_prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)) 
            {
//              we handle the click based on the current btnState
                MouseClick(mouseState);
            }

//          we check for a key press
            CheckKeyPress(state);
            
//          we save the current mouse state for single click checks
            _prevMouseState = mouseState;
            
//          updates the resize and move borders
            UpdateBorders(mouseState);
            
            base.Update(gameTime);
        }
//      this function check all key pressed
        private void CheckKeyPress(KeyboardState state)
        {
            bool ctrl = false;
            bool shift = false;
            bool keyZ = false;
            
            foreach (Keys key in state.GetPressedKeys())
            {
                if (key == Keys.LeftControl)
                    ctrl = true;
                
                if(key == Keys.Z)
                    keyZ = true;

                if (key == Keys.LeftShift)
                    shift = true;
            }
            
            if (ctrl && keyZ && !shift &&  _previousState.IsKeyUp(Keys.Z))
                _canvasCommands.UndoActions();
            
            if (ctrl && keyZ && shift && _previousState.IsKeyUp(Keys.Z))
                _canvasCommands.RedoActions();

            _previousState = state;
        }
//      this function handles a mouse click
        private void MouseClick(MouseState mouseState)
        {
//          we do different things during different times
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
                case Canvas.ButtonStages.Caption:
                    _canvas.AddTextureCaption("",mouseState);
                    break;
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