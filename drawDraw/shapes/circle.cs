using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class  CircleShape : ShapeBase
    {
        public Texture2D Circle;
        
        public CircleShape(string name, int x, int y, int width, int height, int type) : base(x, y, width, height, type)
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (Select)
            {
                spriteBatch.Draw(Circle, destinationRectangle: new Rectangle(X,Y, Width, Height), Color.Red);
            }
            else
            {
                spriteBatch.Draw(Circle, destinationRectangle: new Rectangle(X,Y, Width, Height), Color.DarkSlateGray);
            }

        }
        public override void Update(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override void Resize(Canvas.BorderSides selectedSide, Point mousePoint, Point startPoint)
        {
            switch (selectedSide)
            {
                case DrawDraw.Canvas.BorderSides.Bottom:
                    Height -= (startPoint.Y - mousePoint.Y);
                    break;                
                case DrawDraw.Canvas.BorderSides.Top:
                    Height += (startPoint.Y - mousePoint.Y);
                    Y -= (startPoint.Y - mousePoint.Y);
                    break;
                case DrawDraw.Canvas.BorderSides.Right:
                    Width -= (startPoint.X - mousePoint.X);
                    break;
                case DrawDraw.Canvas.BorderSides.Left:
                    Width += (startPoint.X - mousePoint.X);
                    X -= (startPoint.X - mousePoint.X);
                    break;
            }
        }

        public override MoveBorders DrawBorders()
        {
            MoveBorders moveBorders = new MoveBorders
            {
                BottomMoveBorder = new MoveBorder(X, Y, Width, Height, 0),
                TopMoveBorder = new MoveBorder(X, Y, Width, Height, 1),
                LeftMoveBorder = new MoveBorder(X, Y, Width, Height, 2),
                RightMoveBorder = new MoveBorder(X, Y, Width, Height, 3),
                ShapeId = id
            };

            return moveBorders;
        }

        public override ResizeBorders DrawResizeBorders()
        {
            ResizeBorders resizeBorders = new ResizeBorders()
            {
                BottomResizeBorder = new ResizeBorder(X, Y, Width, Height, 0),
                TopResizeBorder = new ResizeBorder(X, Y, Width, Height, 1),
                RightResizeBorder = new ResizeBorder(X, Y, Width, Height, 2),
                LeftResizeBorder = new ResizeBorder(X, Y, Width, Height, 3)
            };
            return resizeBorders;
        }
    }
}