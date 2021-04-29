using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.buttons
{
    public abstract class ButtonBase
    {
        protected int x;
        protected int y;
        protected Rectangle Texture;
        protected string Name;

        protected ButtonBase(int X, int Y, Rectangle texture, string name)
        {
            x = X;
            y = Y;
            Texture = texture;
            Name = name;
        }

        protected abstract bool CheckClick(MouseState mouseState);

        public abstract void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
        public abstract bool Update(MouseState mouseState);

    }
}