using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.strategies
{
    public interface IStrategy
    {
        public void Draw(ShapeBase shape, SpriteBatch spriteBatch, Texture2D texture = null);

        public void Resize(ShapeBase shape, Canvas.BorderSides selectedSide, Point mousePoint, Point startPoint);
    }
}