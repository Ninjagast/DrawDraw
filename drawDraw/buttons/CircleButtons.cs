using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.buttons
{
    public class CircleButtons: ButtonBase
    {
        public CircleButtons(int X, int Y, Rectangle texture, string name, ButtonStages buttonStage) : base(X, Y, texture, name, buttonStage)
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D _texture;
            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(_texture, Texture, Color.White);
        }
    }
}