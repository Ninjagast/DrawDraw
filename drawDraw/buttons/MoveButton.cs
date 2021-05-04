using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.buttons
{
    public class MoveButton: ButtonBase
    {

        public MoveButton(int X, int Y, Texture2D texture, string name, Canvas.ButtonStages buttonStage) : base(X, Y, texture, name, buttonStage)
        {
        }
        
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Vector2 position = new Vector2(x, y);
            spriteBatch.Draw(Texture, new Vector2(x, y), Color.White);
        }


    }
}