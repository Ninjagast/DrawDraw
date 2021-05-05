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
using System.Windows.Forms;
using ButtonBase = DrawDraw.buttons.ButtonBase;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace DrawDraw
{
    public class Canvas
    {
//      static canvas call for singleton
        private static Canvas            _instance      = new Canvas();
        private        ArrayList         _buttons       = new ArrayList();
        private        List<MoveBorders> _moveBorders   = new List<MoveBorders>();
        private        ResizeBorders     _resizeBorders;
        private        Texture2D         _circleTexture;

        public MoveStages MoveStage = 0;
        
        private Point _startPos;
        private GraphicsDevice _graphicsDevice;
        public ButtonStages BtnStage;

        // main tree for the textures / groups
        private Composite _textures = new Composite();

//      private constructor for singleton
        private Canvas()
        {
        }

//      static _instance field for single instance accessibility
        public static Canvas Instance => _instance;

        
//      ######Initialization functions######
//      init function for setting up the textures and creating all the buttons
        public void Init(GraphicsDevice graphicsDevice, Texture2D circleButton, Texture2D eraserButton, Texture2D moveButton, Texture2D selectButton, Texture2D squareButton, Texture2D openButton, Texture2D saveButton, Texture2D resizeButton, Texture2D circleTexture)
        {
            _textures.Add(new Composite());
            _graphicsDevice = graphicsDevice;
            _circleTexture = circleTexture;
            CreateButtons(circleButton, eraserButton, moveButton, selectButton, squareButton, openButton, saveButton, resizeButton);
        }

//      creates all buttons
        private void CreateButtons(Texture2D circleButton, Texture2D eraserButton, Texture2D moveButton, Texture2D selectButton, Texture2D squareButton, Texture2D openButton, Texture2D saveButton, Texture2D resizeButton)
        {
            _buttons.Add(new RectangleButton(0,   0, squareButton, "Rectangle", ButtonStages.Rectangle));
            _buttons.Add(new CircleButton(   70,  0, circleButton, "Circle",    ButtonStages.Circle));
            _buttons.Add(new SelectButton(   140, 0, selectButton, "Select",    ButtonStages.Select));
            _buttons.Add(new MoveButton(     210, 0, moveButton,   "Move",      ButtonStages.Move));
            _buttons.Add(new OpenButton(     280, 0, openButton,   "Open",      ButtonStages.Open));
            _buttons.Add(new SaveButton(     350, 0, saveButton,   "Save",      ButtonStages.Save));
            _buttons.Add(new ResizeButton(   420, 0, resizeButton, "Resize",    ButtonStages.Resize));
        }

//      ######Insertion / delete functions######
//      function that creates a rectangle
        public Guid InsertRectangle(Point coords)
        {
//          creates static shape #todo make it so you can draw a different size shape from the start
            RectangleShape shape = new RectangleShape("", coords.X, coords.Y, 100, 100, 0);
            
//          adds it to the base branch
            _textures.GetFirstChild().Add(new Leaf(shape));
            
            return shape.id;
        }
        
//      function that creates an ellipse 
        public Guid InsertCircle(Point coords)
        {
//          creates static shape #todo make it so you can draw a different size shape from the start
            CircleShape circle = new CircleShape("", coords.X, coords.Y, 100, 100, 1);
//          give it a texture
            circle.Circle = _circleTexture;
            
//          adds it to the base branch
            _textures.GetFirstChild().Add(new Leaf(circle));
            
            return circle.id;
        }
        
//      deletes a texture
        public void DeleteTexture(Guid id)
        {
//          goes through all the non group shape #todo delete group branch when we have added groups or just make this group compatible in general
            List<ShapeBase> textures = _textures.GetFirstChild().GetAllShapes();
            for (int i = 0; i < textures.Count; i++)
            {
                if (textures[i].id == id)
                {
//                  destroy the child
                    _textures.GetFirstChild().Remove(i);                    
                    break;
                }
            }
        }
//      ######button functions######
//      checks if there user has clicked on a button
        public bool CheckButtonClick(MouseState mouseState, MouseState prevMouseState)
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

//      ######Behaviour functions######
//      selects a texture based on the mouse' position
        public void SelectTexture(MouseState mouseState)
        {
//          for all shapes
            foreach (ShapeBase shape in _textures.GetAllShapes())
            {
//              we get its position and dimensions
                Point LeftTop = shape.GetPoint();
                Point dimensions = shape.GetDimension();

//              the point is located between the x boundary
                if (mouseState.X > LeftTop.X && mouseState.X < LeftTop.X + dimensions.X)
                {
//                  the point is located between the y boundary
                    if (mouseState.Y > LeftTop.Y && mouseState.Y < LeftTop.Y + dimensions.Y)
                    {
//                      click click we found a fucking shape
                        shape.ToggleSelect();
                        return;
                    }
                }
            }
//          unselect everything if we have not clicked on anything
            UnSelectAllTextures();
        }

//      returns all selected shapes
        public List<ShapeBase> GetSelected()
        {
            List<ShapeBase> selected = new List<ShapeBase>();
            
//          for all shapes
            foreach (var texure in _textures.GetAllShapes())
            {
//              if it is selected
                if (texure.IsSelected())
                {
//                  add it to the return list
                    selected.Add(texure);    
                }
            }
            return selected;
        }

//      moves stuff
        public void MoveStuff(MouseState mouseState)
        {
//          if we have not yet created the borders
            if (MoveStage == 0)
            {
//              for all shapes
                foreach (ShapeBase shape in _textures.GetAllShapes())
                {
//                  if it is selected
                    if (shape.IsSelected())
                    {
//                      draw its move borders
                        _moveBorders.Add(shape.DrawBorders());
                        
//                      log the start position
                        _startPos = new Point(mouseState.X, mouseState.Y);
                        
//                      and move onto the next move stage
                        MoveStage = MoveStages.Move;
                    }
                }
            }
//          if we are currently at the end of moving something
            else if(MoveStage == MoveStages.Move)
            {
//              for all move borders
                foreach (MoveBorders border in _moveBorders)
                {
//                  for all shapes
                    foreach (ShapeBase shape in _textures.GetAllShapes())
                    {
//                      if the border matches the shape
                        if (shape.id == border.ShapeId)
                        {
//                          teleport it to the new position
                            Point finalPos = border.LeftMoveBorder.LatestPos;
                            Point dimensions = shape.GetDimension();
                            
                            shape.Update(finalPos.X, finalPos.Y, dimensions.X, dimensions.Y);
                        }
                    }
                }
//              reset all move borders and the current moveStage
                _moveBorders = new List<MoveBorders>();
                MoveStage = MoveStages.Undefined;
            }
        }

//      resizes drawn textures
        public ShapeBase ResizeStuff(MouseState mouseState)
        {
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            
//          if we have not selected a texture to resize
            if (MoveStage == MoveStages.Undefined)
            {
                UnSelectAllTextures();
                
//              for all shapes
                foreach (ShapeBase shape in _textures.GetAllShapes())
                {
//                  if the click location is within this shape
                    if (Canvas.PointWithinShape(shape, mouseState))
                    {
//                      we select it draw resize borders and move onto the next move stage
                        shape.ToggleSelect();
                        _resizeBorders = shape.DrawResizeBorders();
                        MoveStage      = MoveStages.Select;
                        break;
                    }
                }
                return null;
            }
//          if have selected a texture to resize
            else if(MoveStage == MoveStages.Select)
            {
//              for all shapes
                foreach (ShapeBase shape in _textures.GetAllShapes())
                {
//                  if it is selected
                    if (shape.IsSelected())
                    {
//                      we check which side we have just selected and save the start pos
                        _startPos = mousePoint;
                        _resizeBorders.SelectedSide = shape.DetectSide(mousePoint);
                        break;
                    }
                }
//              we move onto the next resize stage 
                MoveStage = MoveStages.Resize;
                return null;
            }
//          We can finally resize the texture
            else
            {
                ShapeBase selected = null;
                
//              for all shapes
                foreach (ShapeBase shape in _textures.GetAllShapes())
                {
//                  if the shape is selected
                    if (shape.IsSelected())
                    {
//                      clone the selected clone for the history
                        selected = shape.Clone(shape.id);
//                      resize the texture
                        shape.Resize(_resizeBorders.SelectedSide, mousePoint, _startPos);
                    }
                }
//              reset everything
                UnSelectAllTextures();
                MoveStage      = MoveStages.Undefined;
                _resizeBorders = null;
                return selected;
            }
        }
//      overloaded function for the history
        public ShapeBase ResizeStuff(Guid id, ShapeBase shape)
        {
            if (_textures.GetAllShapes().Count > 0)
            {
                int index = 0;
                foreach (ShapeBase texture in _textures.GetAllShapes())
                {
                    if (texture.id == id)
                    {
                        ShapeBase selected = texture.Clone(texture.id);
                        texture.Update(shape.X, shape.Y, shape.Width, shape.Height);
                        return selected;
                    }
                    index++;
                }
            }
            return null;
        }

//      moves the textures back
        public void MoveTexure(List<ShapeBase> selected, List<Point> oldPos)
        {
            int index = 0;
//          for all selected shapes
            foreach (ShapeBase select in selected)
            {
//              get its dimensions and move it back
                Point dimensions = select.GetDimension();
                select.Update(oldPos[index].X, oldPos[index].Y, dimensions.X, dimensions.Y);
                index++;
            }
        }
//      ######border functions######
//      updates the normal borders
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
//      updates the resize borders
        public void UpdateResizeBorders(MouseState mouseState)
        {
            _resizeBorders.Update(mouseState, _startPos);
        }
        
//      ######file I/O functions######
//      saves a file
        public void SaveFile()
        {
//          we create a savefile dialog box
            Stream stream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

//          sets file dialog settings
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

//          if the user has chosen a location for the file
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
//              open the file
                stream = saveFileDialog.OpenFile();
                
//              write the data to it
                stream.Write(JsonSerializer.SerializeToUtf8Bytes(_textures));
                
                stream.Close();
            }
            BtnStage = ButtonStages.Select;
        }
        
//      opens a file
         public void OpenFile()
        {
//          opens the file dialog and sets all settings
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Select a save file",
                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

//          if the user has selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
//              open the file
                Stream file = openFileDialog.OpenFile();
                String fileContent;
                
                using (StreamReader reader = new StreamReader(file))
                {
//                  read the file content
                    fileContent = reader.ReadToEnd();
                }

                List<CanvasSave> res = null;

                try
                {
//                  deserialize the file content if it is valid json
                    res = JsonSerializer.Deserialize<List<CanvasSave>>(fileContent);
                }
                catch (Exception e)
                {
//                  not a valid save file
                    BtnStage = ButtonStages.Select;
                    return;
                }
//              we can reset the entire canvas
                ResetCanvas();

//              for all saved shapes
                foreach (var shape in res)
                {
                    try
                    {
//                      if it is a circle
                        if (shape.Type == 1)
                        {
//                          we create the circle and set the texture and add it to the base list #todo needs group compatibility
                            CircleShape circle = new CircleShape("", shape.X, shape.Y, shape.Width, shape.Height, shape.Type);
                            circle.Circle = _circleTexture;
                            
                            _textures.GetFirstChild().Add(new Leaf(circle));
                        }
                        else
                        {
//                          we create the rectangle and add it to the base list #todo needs group compatibility
                            _textures.GetFirstChild().Add(new Leaf(new RectangleShape("", shape.X, shape.Y, shape.Width, shape.Height, shape.Type)));
                        }
                    }
                    catch (Exception e)
                    {
//                      not a valid save file
                        BtnStage = ButtonStages.Select;
                        return;
                    }
                }
            }
//          we always reset the button stage
            BtnStage = ButtonStages.Select;
        }
         
//      ######reset functions######
//       resets the entire canvas to an empty state
         private void ResetCanvas()
         {
             UnSelectAllTextures();
             _textures = new Composite();
             _textures.Add(new Composite());
            
             _moveBorders   = new List<MoveBorders>();
             _resizeBorders = null;
             MoveStage      = MoveStages.Undefined;
             BtnStage       = ButtonStages.Select;
         }
         
//      unselects all textures
        private void UnSelectAllTextures()
        {
            foreach (ShapeBase shape in _textures.GetAllShapes())
            {
                if (shape.IsSelected())
                {
                    shape.ToggleSelect();
                }
            }
        }
        
//      ######draw function######
//      basic draw function which draws all textures currently on the canvas
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ShapeBase texture in _textures.GetAllShapes())
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

//      ######enums######
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
        public enum MoveStages
        {
            Undefined,
            Select,
            Move,
            Resize
        }

//      ######static functions######
        static bool PointWithinShape(ShapeBase shape, MouseState mousePoint)
        {
//          we get its location and dimensions
            Point LeftTop = shape.GetPoint();
            Point dimensions = shape.GetDimension();

//          the point is located between the x boundary
            if (mousePoint.X > LeftTop.X && mousePoint.X < LeftTop.X + dimensions.X)
            {
//              the point is located between the x boundary
                if (mousePoint.Y > LeftTop.Y && mousePoint.Y < LeftTop.Y + dimensions.Y)
                {
//                  click click we found a shape
                    return true;
                }
            }

            return false;
        }
    }
}