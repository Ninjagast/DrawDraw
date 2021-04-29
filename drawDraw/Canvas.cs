using System;
using System.Collections;
using DrawDraw.buttons;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public class Canvas
    {
        private static Canvas _instance = new Canvas();
        private ArrayList _textures = new ArrayList();
        private ArrayList _buttons = new ArrayList();

        private GraphicsDevice _graphicsDevice;

        private Canvas()
        {

        }

        public static Canvas Instance => _instance;

        public void Init(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            CreateButtons();
        }

        private void CreateButtons()
        {
            _buttons.Add(new RectangleButton(0, 0, new Rectangle(0, 0, 100, 20), ""));
        }

        public void InsertRectangle(Point coords)
        {
            _textures.Add(new RectangleShape("", coords.X, coords.Y, 100, 100, 1));
        }
        public void InsertButtons(Rectangle button)
        {
            _buttons.Add(button);
        }
        public int GetLastRectangle()
        {
            return _textures.Count;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ShapeBase texture in _textures)
            {
                texture.Draw(spriteBatch, _graphicsDevice);
            }
            
            foreach (ButtonBase button in _buttons)
            {
                button.Draw(spriteBatch, _graphicsDevice);
            }
        }

        public bool CheckButtons(MouseState mouseState)
        {
            foreach (ButtonBase button in _buttons)
            {
                if (button.Update(mouseState))
                {
                    return true;
                }
            }

            return false;
        }
    }
}