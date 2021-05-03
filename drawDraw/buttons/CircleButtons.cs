using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.buttons
{
    public class CircleButtons: ButtonBase
    {
        public CircleButtons(int x, int y, Rectangle texture, string name) : base(x, y, texture, name)
        {
        }
        
        // @return true: If a player enters the button with mouse
        protected override bool CheckClick(MouseState mouseState)
        {
            Point point = new Point(mouseState.X, mouseState.Y);
            if (point.X < x + Texture.Width &&
                point.X > x &&
                point.Y < x + Texture.Height &&
                point.X > y)
            {
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D _texture;
            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(_texture, Texture, Color.White);
            
        }

        public override bool Update(MouseState mouseState)
        {
            if (CheckClick(mouseState))
            {
                canvas.BtnStage = ButtonStages.Circle;
                Console.WriteLine(Name);
                Console.WriteLine("Button stage : " + canvas.BtnStage);
                return true;
            }
            return false;
        }
    }
}