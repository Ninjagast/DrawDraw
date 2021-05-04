using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class ResizeBorder: ShapeBase
    {
        private Rectangle _border;
        public ResizeBorder(int x, int y, int width, int height, int type) : base(x, y, width, height, type)
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

        public override void Resize(Canvas.BorderSides resizeBordersSelectedSide, Point mousePoint, Point startPoint)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Point currentPos, Point startPos, Canvas.BorderSides side)
        {
//          BottomBorder 0
//          TopBorder 1
//          LeftBorder 2
//          RightBorder 3
            int x = X + (currentPos.X - startPos.X);
            int y = Y + (currentPos.Y - startPos.Y);
            switch (side)
            {
                case Canvas.BorderSides.Bottom:
                    _border = new Rectangle(X, y + Height, Width, 2);
                    break;
                case Canvas.BorderSides.Top:
                    _border = new Rectangle(X, y, Width, 2);
                    break;                
                case Canvas.BorderSides.Left:
                    _border = new Rectangle(x, Y, 2, Height);
                    break;
                case Canvas.BorderSides.Right:                    
                    _border = new Rectangle(x + Width, Y, 2, Height + 2);
                    break;
            }
        }

        public override MoveBorders DrawBorders()
        {
            throw new System.Exception("Don't draw borders of borders! Are you insane???");
        }
        
        public override ResizeBorders DrawResizeBorders()
        {
            throw new System.Exception("Don't draw resize borders of borders! Are you insane???");

        }
    }
}