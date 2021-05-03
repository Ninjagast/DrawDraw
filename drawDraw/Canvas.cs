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
        private List<Borders> _moveBorders = new List<Borders>();

        public int MoveStage = 0;
        private Point _startPos;

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
            _buttons.Add(new RectangleButton(0, 0, new Rectangle(0, 0, 30, 30), "Rectangle", ButtonStages.Rectangle));
            _buttons.Add(new CircleButtons(70, 0, new Rectangle(70, 0, 30, 30), "Circle", ButtonStages.Circle));
            _buttons.Add(new SelectButton(140, 0, new Rectangle(140, 0, 30, 30), "Select", ButtonStages.Select));
            _buttons.Add(new MoveButton(210, 0, new Rectangle(210, 0, 30, 30), "Move", ButtonStages.Move));
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

            foreach (Borders border in _moveBorders)
            {
                border.BottomBorder.Draw(spriteBatch, _graphicsDevice);
                border.TopBorder.Draw(spriteBatch, _graphicsDevice);
                border.RightBorder.Draw(spriteBatch, _graphicsDevice);
                border.LeftBorder.Draw(spriteBatch, _graphicsDevice);
            }
        }

        public bool CheckButtons(MouseState mouseState, MouseState prevMouseState)
        {
            if (prevMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                foreach (ButtonBase button in _buttons)
                {
                    if (button.OnClick(mouseState))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SelectTexture(MouseState mouseState)
        {
            Point mousePoint = new Point(mouseState.X, mouseState.Y);

            foreach (ShapeBase shape in _textures)
            {
                Point LeftTop = shape.GetPoint();
                Point dimensions = shape.GetDimension();

//              the point is located between the x boundary
                if (mousePoint.X > LeftTop.X && mousePoint.X < LeftTop.X + dimensions.X)
                {
                    if (mousePoint.Y > LeftTop.Y && mousePoint.Y < LeftTop.Y + dimensions.Y)
                    {
//                      click click we found a shape
                        shape.ToggleSelect();
                        return;
                    }
                }
            }
            Console.WriteLine("nothing found");
        }

        public List<ShapeBase> GetSelected()
        {
            List<ShapeBase> selected = new List<ShapeBase>();
            foreach (var texure in _textures)
            {
                if (texure.IsSelected())
                {
                    selected.Add(texure);    
                }
            }
            return selected;
        }

        public void MoveStuff(MouseState mouseState)
        {
            if (MoveStage == 0)
            {
                foreach (ShapeBase shape in _textures)
                {
                    if (shape.IsSelected())
                    {
                        _moveBorders.Add(shape.DrawBorders());
                        _startPos = new Point(mouseState.X, mouseState.Y);
                        MoveStage = 1;
                    }
                }
            }
            else
            {
                foreach (Borders border in _moveBorders)
                {
                    foreach (ShapeBase shape in _textures)
                    {
                        if (shape.id == border.ShapeId)
                        {
                            Point finalPos = border.LeftBorder.LatestPos;
                            Point dimensions = shape.GetDimension();
                            shape.Update(finalPos.X, finalPos.Y, dimensions.X, dimensions.Y);
                            
                        }
                    }
                }
                _moveBorders = new List<Borders>();
                MoveStage = 0;
            }
        }

        public void MoveTexure(List<ShapeBase> selected, int x, int y)
        {
            foreach (ShapeBase select in selected)
            {
                Point dimensions = select.GetDimension();
                select.Update(x, y, dimensions.X, dimensions.Y);
            }
        }
        public void MoveTexure(List<ShapeBase> selected, List<Point> oldPos)
        {
            int index = 0;
            foreach (ShapeBase select in selected)
            {
                Point dimensions = select.GetDimension();
                select.Update(oldPos[index].X, oldPos[index].Y, dimensions.X, dimensions.Y);
                index++;
            }
        }

        public void UpdateBorders(MouseState mouseState)
        {
            foreach (Borders border in _moveBorders)
            {
                border.BottomBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
                border.TopBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
                border.LeftBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
                border.RightBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
            }
        }
    }

    public enum ButtonStages
    {
        Rectangle,
        Circle,
        Undo,
        Select,
        Move
    }
}