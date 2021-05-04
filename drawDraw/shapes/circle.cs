using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class  CircleShape : ShapeBase
    {
        private Rectangle _rectangle;
        
        public CircleShape(string name, int x, int y, int width, int height, int type) : base(x, y, width, height, type)
        {
            _rectangle = new Rectangle((int) X, (int) Y, Width, Height);
        }
        
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D texture;
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Pink });
            
            spriteBatch.Draw(texture, _rectangle, Color.White);
        }
        public override void Update(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _rectangle = new Rectangle((int) X, (int) Y, Width, Height);
        }

        public override void Resize(Canvas.BorderSides resizeBordersSelectedSide, Point mousePoint, Point startPoint)
        {
            throw new NotImplementedException();
        }

        public override MoveBorders DrawBorders()
        {
            throw new NotImplementedException();
        }

        public override ResizeBorders DrawResizeBorders()
        {
            throw new NotImplementedException();
        }
    }
}