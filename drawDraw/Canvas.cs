using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DrawDraw.buttons;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DrawDraw
{
    public class Canvas
    {
        private static Canvas _instance = new Canvas();
        private List<ShapeBase> _textures = new List<ShapeBase>();
        private ArrayList _buttons = new ArrayList();
        private List<MoveBorders> _moveBorders = new List<MoveBorders>();
        private ResizeBorders _resizeBorders = null;

        public int MoveStage = 0;
        private Point _startPos;

        private GraphicsDevice _graphicsDevice;

        public ButtonStages BtnStage;


        private Canvas()
        {

        }

        public static Canvas Instance => _instance;

        public void Init(GraphicsDevice graphicsDevice, Texture2D circleButton, Texture2D eraserButton, Texture2D moveButton, Texture2D selectButton, Texture2D squareButton, Texture2D openButton, Texture2D saveButton, Texture2D resizeButton)
        {
            _graphicsDevice = graphicsDevice;
            CreateButtons(circleButton, eraserButton, moveButton, selectButton, squareButton, openButton, saveButton, resizeButton);
        }

        private void CreateButtons(Texture2D circleButton, Texture2D eraserButton, Texture2D moveButton, Texture2D selectButton, Texture2D squareButton, Texture2D openButton, Texture2D saveButton, Texture2D resizeButton)
        {
            _buttons.Add(new RectangleButton(0, 0, squareButton, "Rectangle", ButtonStages.Rectangle));
            _buttons.Add(new CircleButton(70, 0, circleButton, "Circle", ButtonStages.Circle));
            _buttons.Add(new SelectButton(140, 0, selectButton, "Select", ButtonStages.Select));
            _buttons.Add(new MoveButton(210, 0, moveButton, "Move", ButtonStages.Move));
            _buttons.Add(new OpenButton(280, 0, openButton, "Open", ButtonStages.Open));
            _buttons.Add(new SaveButton(350, 0, saveButton, "Save", ButtonStages.Save));
            _buttons.Add(new ResizeButton(420, 0, resizeButton, "Resize", ButtonStages.Resize));
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

            foreach (MoveBorders border in _moveBorders)
            {
                border.BottomMoveBorder.Draw(spriteBatch, _graphicsDevice);
                border.TopMoveBorder.Draw(spriteBatch, _graphicsDevice);
                border.RightMoveBorder.Draw(spriteBatch, _graphicsDevice);
                border.LeftMoveBorder.Draw(spriteBatch, _graphicsDevice);
            }

            _resizeBorders?.drawBorder(spriteBatch, _graphicsDevice);
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
            UnSelectAllTextures();
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
//          draw borders of selected shit
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
                foreach (MoveBorders border in _moveBorders)
                {
                    foreach (ShapeBase shape in _textures)
                    {
                        if (shape.id == border.ShapeId)
                        {
                            Point finalPos = border.LeftMoveBorder.LatestPos;
                            Point dimensions = shape.GetDimension();
                            shape.Update(finalPos.X, finalPos.Y, dimensions.X, dimensions.Y);
                        }
                    }
                }
                _moveBorders = new List<MoveBorders>();
                MoveStage = 0;
            }
        }

        public void ResizeStuff(MouseState mouseState)
        {
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            
//          select texture to resize
            if (MoveStage == 0)
            {
                UnSelectAllTextures();
                
                foreach (ShapeBase shape in _textures)
                {
                    Point LeftTop = shape.GetPoint();
                    Point dimensions = shape.GetDimension();

//                  the point is located between the x boundary
                    if (mousePoint.X > LeftTop.X && mousePoint.X < LeftTop.X + dimensions.X)
                    {
                        if (mousePoint.Y > LeftTop.Y && mousePoint.Y < LeftTop.Y + dimensions.Y)
                        {
//                          click click we found a shape
                            shape.ToggleSelect();
                            _resizeBorders = shape.DrawResizeBorders();
                            MoveStage = 1;
                            return;
                        }
                    }
                }
            }
//          We select a side to resize
            else if(MoveStage == 1)
            {
                foreach (ShapeBase shape in _textures)
                {
                    if (shape.IsSelected())
                    {
                        _startPos = mousePoint;
                        _resizeBorders.SelectedSide = shape.DetectSide(mousePoint);
                    }
                }
                MoveStage = 2;
            }
//          We move it to the newest location
            else
            {
                MoveStage = 0;
                foreach (ShapeBase shape in _textures)
                {
                    if (shape.IsSelected())
                    {
                        shape.Resize(_resizeBorders.SelectedSide, mousePoint, _startPos);
                        _resizeBorders.SelectedSide = shape.DetectSide(mousePoint);
                    }
                }
                UnSelectAllTextures();
                _resizeBorders = null;
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
            foreach (MoveBorders border in _moveBorders)
            {
                border.BottomMoveBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
                border.TopMoveBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
                border.LeftMoveBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
                border.RightMoveBorder.Update(new Point(mouseState.X, mouseState.Y), _startPos);
            }
        }
        public void UpdateResizeBorders(MouseState mouseState)
        {
            _resizeBorders.Update(mouseState, _startPos);
        }

        public async void SaveFile()
        {
            BtnStage = ButtonStages.Select;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = path + "\\drawdraw";

            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }

            int i = 0;
            while (File.Exists(subFolderPath + "\\saveFile" + i + ".txt"))
            {
                i++;
            }

            await using FileStream createStream = File.Create(subFolderPath + "\\saveFile"+ i +".txt");
            await JsonSerializer.SerializeAsync(createStream, _textures);
        }
        private void UnSelectAllTextures()
        {
            foreach (ShapeBase shape in _textures)
            {
                if (shape.IsSelected())
                {
                    shape.ToggleSelect();
                }
            }
        }
        public enum ButtonStages
        {
            Rectangle,
            Circle,
            Undo,
            Select,
            Move,
            Open,
            Save,
            Resize
        }

        public enum BorderSides
        {
            Undefined,
            Top,
            Left,
            Bottom,
            Right
        }
    }
}