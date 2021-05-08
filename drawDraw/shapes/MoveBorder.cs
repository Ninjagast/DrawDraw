using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class MoveBorder: ShapeBase
    {
        private Rectangle _border;
        public Point LatestPos;
        
        public MoveBorder(int x, int y, int width, int height, int type) : base(x, y, width, height, type)
        {
            // BottomBorder 0
            // TopBorder 1
            // LeftBorder 2
            // RightBorder 3
            switch (type)
            {
                case 0:
                    _border = new Rectangle(x, y + height, width, 2);
                    break;
                case 1:
                    _border = new Rectangle(x, y, width, 2);
                    break;                
                case 2:
                    _border = new Rectangle(x, y, 2, height);
                    break;
                case 3:                    
                    _border = new Rectangle(x + width, y, 2, height + 2);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D texture;
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(texture, _border, Color.White);
        }

//      actual update function which makes the border follow the mouse
        public void Update(Point currentPos, Point startPos)
        {
            int x = X + (currentPos.X - startPos.X);
            int y = Y + (currentPos.Y - startPos.Y);
            LatestPos = new Point(x, y);
            switch (Type)
            {
                case 0:
                    _border = new Rectangle(x, y + Height, Width, 2);
                    break;
                case 1:
                    _border = new Rectangle(x, y, Width, 2);
                    break;                
                case 2:
                    _border = new Rectangle(x, y, 2, Height);
                    break;
                case 3:                    
                    _border = new Rectangle(x + Width, y, 2, Height + 2);
                    break;
            }
        }
        public override ShapeBase Clone(Guid id)
        {
            throw new NotImplementedException();
        }
        public override ShapeBase Action(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}