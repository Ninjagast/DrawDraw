using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.strategies
{
    public class CircleStrat: IStrategy
    {
        private static CircleStrat _instance = new CircleStrat();
        
        public static CircleStrat Instance => _instance;

        private CircleStrat()
        {
            
        }
        public void Draw(ShapeBase shape, SpriteBatch spriteBatch, Texture2D texture = null)
        {
            Texture2D circle = shape.GetCircle();
            Color color = Color.Red;
            if (!shape.Select)
            {
                color = Color.DarkSlateGray;
            }
            spriteBatch.Draw(circle, new Rectangle(shape.X, shape.Y, shape.Width, shape.Height), color);
        }

        public void Resize(ShapeBase shape, Canvas.BorderSides selectedSide, Point mousePoint, Point startPoint)
        {
            switch (selectedSide)
            {
                case Canvas.BorderSides.Bottom:
                    shape.Height -= (startPoint.Y - mousePoint.Y);
                    break;                
                case Canvas.BorderSides.Top:
                    shape.Height += (startPoint.Y - mousePoint.Y);
                    shape.Y -= (startPoint.Y - mousePoint.Y);
                    break;
                case Canvas.BorderSides.Right:
                    shape.Width -= (startPoint.X - mousePoint.X);
                    break;
                case Canvas.BorderSides.Left:
                    shape.Width += (startPoint.X - mousePoint.X);
                    shape.X -= (startPoint.X - mousePoint.X);
                    break;
            }
        }
    }
    
    public class RectangleStrat: IStrategy
    {
        private static RectangleStrat _instance = new RectangleStrat();
        
        public static RectangleStrat Instance => _instance;

        private RectangleStrat()
        {
            
        }
        public void Draw(ShapeBase shape, SpriteBatch spriteBatch, Texture2D texture = null)
        {
            if (texture != null)
            {
                Color[] color = new Color[] {Color.Red};
                if (!shape.Select)
                {
                    color = new Color[] {Color.DarkSlateGray};
                }
            
                texture.SetData(color);
                spriteBatch.Draw(texture, shape.GetNewRectangle(), Color.White);
            }
        }

        public void Resize(ShapeBase shape, Canvas.BorderSides selectedSide, Point mousePoint, Point startPoint)
        {
            switch (selectedSide)
            {
                case Canvas.BorderSides.Bottom:
                    shape.Height -= (startPoint.Y - mousePoint.Y);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;                
                case Canvas.BorderSides.Top:
                    shape.Height += (startPoint.Y - mousePoint.Y);
                    shape.Y -= (startPoint.Y - mousePoint.Y);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;
                case Canvas.BorderSides.Right:
                    shape.Width -= (startPoint.X - mousePoint.X);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;
                case Canvas.BorderSides.Left:
                    shape.Width += (startPoint.X - mousePoint.X);
                    shape.X -= (startPoint.X - mousePoint.X);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;
            }
        }
    }
}