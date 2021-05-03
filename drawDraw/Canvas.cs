using System;
using System.Collections;
using System.Collections.Generic;
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
        private List<ShapeBase> _textures = new List<ShapeBase>();
        private ArrayList _buttons = new ArrayList();
        
        private GraphicsDevice _graphicsDevice;

        public ButtonStages BtnStage; 
            
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
            _buttons.Add(new RectangleButton(0, 0, new Rectangle(0, 0, 30, 30), "Rectangle"));
            _buttons.Add(new CircleButtons(70, 0, new Rectangle(70, 0, 30, 30), "Circle"));
        }

        public Guid InsertRectangle(Point coords)
        {
            _textures.Add(new RectangleShape("", coords.X, coords.Y, 100, 100, 1));
            return _textures[^1].id;
        }
        public Guid InsertCircle(Point coords)
        {
            _textures.Add(new CircleShape("", coords.X, coords.Y, 100, 100, 1));
            return _textures[^1].id;
        }
        
        public void DeleteTexture(Guid id)
        {
            ShapeBase delete = null;
            foreach (ShapeBase texture in _textures)
            {
                if (texture.id == id)
                {
                    delete = texture; 
                    break;
                }
            }
            _textures.Remove(delete);
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

        public bool CheckButtons(MouseState mouseState, MouseState prevMouseState)
        {
            if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                foreach (ButtonBase button in _buttons)
                {
                    if (button.Update(mouseState))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public enum ButtonStages
    {
        Rectangle,
        Circle,
        Undo,
    }
}