using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.strategies
{
    public class Context
    {
        private IStrategy _strategy;

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Draw(ShapeBase shape, SpriteBatch spriteBatch, Texture2D texture = null)
        {
            _strategy.Draw(shape, spriteBatch, texture);
        }

        public void Resize(ShapeBase shape, Canvas.BorderSides selectedSide, Point mousePoint, Point startPoint)
        {
            _strategy.Resize(shape, selectedSide, mousePoint, startPoint);
        }
    }
}