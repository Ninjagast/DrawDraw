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

//      textures
        private Texture2D corkel;
        private Texture2D snaly;
        private Texture2D squearl;
        private Texture2D triganelk;
        
//      temporary vars to hack around shape placement
        private Vector2 drawPos;
        private ArrayList squares = new ArrayList();
        private Boolean modus = true;
        private int activeCd = 0;
        private int cd = 120;
        
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

//          #todo change to actual names
//          creates sprites from these textures and loads them in
            corkel =    this.Content.Load<Texture2D>("corkel");
            snaly =     this.Content.Load<Texture2D>("snaly");
            squearl =   this.Content.Load<Texture2D>("squearl");
            triganelk = this.Content.Load<Texture2D>("triganelk");
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
//              #todo add a shape to the tree here so we can draw it in the draw function.
//              #todo change the continuous nature of this code into the menu.
                drawPos = mousePosition.ToVector2();
                squares.Add(new Rectangle((int) drawPos.X, (int) drawPos.Y, 100, 100));
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                modus = !modus;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            Texture2D _texture;
            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.DarkSlateGray });

//          #todo replace this with the shape tree
            foreach (Rectangle square in squares)
            {
                _spriteBatch.Draw(_texture, square, Color.White);
            }

//          draw example for textures
            _spriteBatch.Draw(corkel,   new Vector2(0,0), Color.Brown);
            _spriteBatch.Draw(snaly,    new Vector2(50,0), Color.Brown);
            _spriteBatch.Draw(triganelk,new Vector2(100,0), Color.Brown);
            _spriteBatch.Draw(squearl,  new Vector2(150,0), Color.Brown);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}