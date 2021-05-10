using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.buttons
{
    public class ButtonBase
    {
        protected static Canvas Canvas = Canvas.Instance;
        protected int X;
        protected int Y;
        protected Texture2D Texture;
        protected string Name;
        protected Canvas.ButtonStages ButtonValue;

        public ButtonBase(int x, int y, Texture2D texture, string name, Canvas.ButtonStages buttonStage)
        {
            ButtonValue = buttonStage;
            this.X = x;
            this.Y = y;
            Texture = texture;
            Name = name;
        }
        
        // @return true: if the player has clicked on this button
        protected bool CheckClick(MouseState mouseState)
        {
            Point point = new Point(mouseState.X, mouseState.Y);

            if (point.X > X && point.X < X + Texture.Width)
            {
                if (point.Y > Y && point.Y < Y + Texture.Height)
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), Color.White);
        }
        
//      when a button gets clicked: Do something!
        public bool OnClick(MouseState mouseState)
        {
            if (CheckClick(mouseState))
            {
                Canvas.BtnStage = ButtonValue;
                return true;
            }
            return false;
        }

    }
}