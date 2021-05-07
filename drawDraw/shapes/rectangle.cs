using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class RectangleShape : ShapeBase
    {
        private Rectangle _rectangle;
        
        public RectangleShape(string name, int x, int y, int width, int height, int type) : base(x, y, width, height, type)
        {
            _rectangle = new Rectangle((int) X, (int) Y, Width, Height);
        }
        
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D texture;
            texture = new Texture2D(graphicsDevice, 1, 1);
            if (Select)
            {
                texture.SetData(new Color[] { Color.Red });
            }
            else
            {
                texture.SetData(new Color[] { Color.DarkSlateGray });
            }
            
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

        public override void Resize(Canvas.BorderSides selectedSide, Point mousePoint, Point startPoint)
        {
            switch (selectedSide)
            {
                case DrawDraw.Canvas.BorderSides.Bottom:
                    Height -= (startPoint.Y - mousePoint.Y);
                    _rectangle = new Rectangle((int) X, (int) Y, Width, Height);
                    break;                
                case DrawDraw.Canvas.BorderSides.Top:
                    Height += (startPoint.Y - mousePoint.Y);
                    Y -= (startPoint.Y - mousePoint.Y);
                    _rectangle = new Rectangle((int) X, (int) Y, Width, Height);
                    break;
                case DrawDraw.Canvas.BorderSides.Right:
                    Width -= (startPoint.X - mousePoint.X);
                    _rectangle = new Rectangle((int) X, (int) Y, Width, Height);
                    break;
                case DrawDraw.Canvas.BorderSides.Left:
                    Width += (startPoint.X - mousePoint.X);
                    X -= (startPoint.X - mousePoint.X);
                    _rectangle = new Rectangle((int) X, (int) Y, Width, Height);
                    break;
            }
        }
        public override ShapeBase Clone(Guid id)
        {
            RectangleShape rec = new RectangleShape("", X, Y, Width, Height, Type) {id = id};
            return rec;
        }

        public override ShapeBase Action(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}