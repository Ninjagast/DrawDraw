using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public class Canvas
    {
        private static Canvas _instance = new Canvas();

        private ArrayList _textures = new ArrayList();

        private GraphicsDevice _graphicsDevice;

        private Canvas()
        {

        }

        public static Canvas Instance => _instance;

        public void Init(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }
        
        public void InsertRectangle(Point coords)
        {
            _textures.Add(new Rectangle((int) coords.X, (int) coords.Y, 100, 100));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D _texture;
            _texture = new Texture2D(_graphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.DarkSlateGray });
            foreach (Rectangle square in _textures)
            {
                spriteBatch.Draw(_texture, square, Color.White);
            }

        }
    }
}