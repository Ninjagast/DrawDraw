using System;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xceed.Wpf.Toolkit.Converters;

namespace DrawDraw.shapes
{
    public abstract class ShapeBase
    {
        protected Canvas Canvas = Canvas.Instance;
        public Guid id { get; set; }
        public int X;
        public int Y;
        protected int Width;
        protected int Height;
        protected int Type;
        protected bool Select = false;

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

        public abstract Borders DrawBorders();
        
        public Point GetPoint()
        {
            return new Point(X, Y);
        }        
        
        public Point GetDimension()
        {
            return new Point(Width, Height);
        }

        public void ToggleSelect()
        {
            Select = !Select;
        }

        public bool IsSelected()
        {
            return Select;
        }
    }
}