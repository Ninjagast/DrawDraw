using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class Border: ShapeBase
    {
        private Rectangle _border;
        public Point LatestPos;
        
        public Border(int x, int y, int width, int height, int type) : base(x, y, width, height, type)
        {
            // BottomBorder 0
            // TopBorder 1
            // LeftBorder 2
            // RightBorder 3
            switch (type)
            {
                case 0:
                    _border = new Rectangle(x, y + height, width, 5);
                    break;
                case 1:
                    _border = new Rectangle(x, y, width, 5);
                    break;                
                case 2:
                    _border = new Rectangle(x, y, 5, height);
                    break;
                case 3:                    
                    _border = new Rectangle(x + width, y, 5, height + 5);
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D texture;
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(texture, _border, Color.White);
        }

        public override void Update(int x, int y, int width, int height)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Point currentPos, Point startPos)
        {
            int x = X + (currentPos.X - startPos.X);
            int y = Y + (currentPos.Y - startPos.Y);
            LatestPos = new Point(x, y);
            switch (Type)
            {
                case 0:
                    _border = new Rectangle(x, y + Height, Width, 5);
                    break;
                case 1:
                    _border = new Rectangle(x, y, Width, 5);
                    break;                
                case 2:
                    _border = new Rectangle(x, y, 5, Height);
                    break;
                case 3:                    
                    _border = new Rectangle(x + Width, y, 5, Height + 5);
                    break;
            }
        }

        public override Borders DrawBorders()
        {
            throw new System.Exception("Don't draw borders of borders! Are you insane???");
        }
    }
}