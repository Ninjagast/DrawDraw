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
        public void Draw(ShapeBase shape, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
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
}