using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public class Button
    {
        int buttonX, buttonY;

        public int ButtonX { get; set; }
        public int ButtonY { get; set; }
        public Rectangle Texture { get; set; }
        public string Name { get; set; }

        public Button(string name, Rectangle texture, int buttonX, int buttonY)
        {
            this.Name = name;
            this.Texture = texture;
            this.buttonX = buttonX;
            this.buttonY = buttonY;
        }

        // @return true: If a player enters the button with mouse
        public bool EnterButton(MouseState mouseState)
        {
            Point point = new Point(mouseState.X, mouseState.Y);
            if (point.X < buttonX + Texture.Width &&
                point.X > buttonX &&
                point.Y < buttonY + Texture.Height &&
                point.X > buttonY)
            {
                return true;
            }

            return false;
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            if (EnterButton(mouseState) && mouseState.LeftButton == ButtonState.Pressed)
            {
                switch (Name)
                {
                    default:
                        Console.WriteLine("I AM HERE");
                        break;
                }
            }
        }
    }
}
        

