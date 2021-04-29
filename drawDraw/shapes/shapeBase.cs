using System.Runtime.InteropServices.ComTypes;
using Microsoft.Xna.Framework.Graphics;
using Xceed.Wpf.Toolkit.Converters;

namespace DrawDraw.shapes
{
    public abstract class ShapeBase
    {
        protected Canvas Canvas = Canvas.Instance;
        protected string Name;
        protected int X;
        protected int Y;
        protected int Width;
        protected int Height;
        protected int Type;

        protected ShapeBase(string name, int x, int y, int width, int height, int type)
        {
            Name = name;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = type;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);

        public abstract void Update(int x, int y, int width, int height);
    }
}