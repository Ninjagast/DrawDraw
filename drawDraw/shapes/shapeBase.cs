using System;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Xna.Framework.Graphics;
using Xceed.Wpf.Toolkit.Converters;

namespace DrawDraw.shapes
{
    public abstract class ShapeBase
    {
        protected Canvas Canvas = Canvas.Instance;
        public Guid id { get; set; }
        protected int X;
        protected int Y;
        protected int Width;
        protected int Height;
        protected int Type;

        protected ShapeBase(int x, int y, int width, int height, int type)
        {
            id = Guid.NewGuid();
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