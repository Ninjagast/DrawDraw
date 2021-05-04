using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.buttons
{
    public abstract class ButtonBase
    {
        protected static Canvas canvas = Canvas.Instance;
        protected int x;
        protected int y;
        protected Texture2D Texture;
        protected string Name;
        protected Canvas.ButtonStages ButtonValue;

        protected ButtonBase(int X, int Y, Texture2D texture, string name, Canvas.ButtonStages buttonStage)
        {
            ButtonValue = buttonStage;
            x = X;
            y = Y;
            Texture = texture;
            Name = name;
        }
        
        // @return true: If a player enters the button with mouse
        protected bool CheckClick(MouseState mouseState)
        {
            Point point = new Point(mouseState.X, mouseState.Y);

            if (point.X > x && point.X < x + Texture.Width)
            {
                if (point.Y > y && point.Y < y + Texture.Height)
                {
                    return true;
                }
            }
            return false;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
        public bool OnClick(MouseState mouseState)
        {
            if (CheckClick(mouseState))
            {
                canvas.BtnStage = ButtonValue;
                Console.WriteLine(Name);
                Console.WriteLine("Button stage : " + canvas.BtnStage);
                return true;
            }
            return false;
        }

    }
}