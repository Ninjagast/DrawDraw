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
    }
}