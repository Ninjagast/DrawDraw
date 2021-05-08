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
using System.Xml;
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

        public int _numSelectedTextures = 0;

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
        public void Init(GraphicsDevice graphicsDevice, Texture2D circleButton, Texture2D eraserButton, Texture2D moveButton, Texture2D selectButton, Texture2D squareButton, Texture2D openButton, Texture2D saveButton, Texture2D resizeButton, Texture2D groupButton,Texture2D clearButton, Texture2D circleTexture)
        {
            _textures.Add(new Composite());
            _graphicsDevice = graphicsDevice;
            _circleTexture = circleTexture;
            CreateButtons(circleButton, eraserButton, moveButton, selectButton, squareButton, openButton, saveButton, resizeButton, groupButton, clearButton);
        }

//      creates all buttons
        private void CreateButtons(Texture2D circleButton, Texture2D eraserButton, Texture2D moveButton, Texture2D selectButton, Texture2D squareButton, Texture2D openButton, Texture2D saveButton, Texture2D resizeButton, Texture2D groupButton, Texture2D clearButton)
        {
            _buttons.Add(new RectangleButton(0,   0, squareButton, "Rectangle", ButtonStages.Rectangle));
            _buttons.Add(new CircleButton(   70,  0, circleButton, "Circle",    ButtonStages.Circle));
            _buttons.Add(new SelectButton(   140, 0, selectButton, "Select",    ButtonStages.Select));
            _buttons.Add(new MoveButton(     210, 0, moveButton,   "Move",      ButtonStages.Move));
            _buttons.Add(new OpenButton(     280, 0, openButton,   "Open",      ButtonStages.Open));
            _buttons.Add(new SaveButton(     350, 0, saveButton,   "Save",      ButtonStages.Save));
            _buttons.Add(new ResizeButton(   420, 0, resizeButton, "Resize",    ButtonStages.Resize));
            _buttons.Add(new GroupButton(    490, 0, groupButton,  "Group",     ButtonStages.Group));
            _buttons.Add(new ClearButton(    560, 0, clearButton,  "Clear",     ButtonStages.Clear));
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
//          for all shapes in the no group group
            foreach (ShapeBase shape in _textures.GetFirstChild().GetAllShapes())
            {
                if (PointWithinShape(shape, mouseState))
                {
                    if (shape.IsSelected())
                    {
                        _numSelectedTextures--;
                    }
                    else
                    {
                        _numSelectedTextures++;
                    }
                    shape.ToggleSelect();
                    return;
                }
            }

            _textures.SelectAll(mouseState);
            
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
                            Point finalPos = border.LeftMoveBorder.LatestPos;
                            Point dimensions = shape.GetDimension();
                            
                            MoveObject move = new MoveObject(finalPos, dimensions);
                            shape.Action(move);
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
                    if (PointWithinShape(shape, mouseState))
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
                        shape.Action(new Resize(_resizeBorders.SelectedSide, mousePoint, _startPos));
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
        
//      groups textures
        public void GroupTextures()
        {
//          get all selected non group shapes
            int selectedNonGroupShapes = 0;
            foreach (var shape in _textures.GetFirstChild().GetAllShapes())
            {
                if (shape.IsSelected())
                {
                    selectedNonGroupShapes++;
                }
            }

//          if there are selected shapes missing then a group is selected
            if (selectedNonGroupShapes != _numSelectedTextures)
            {
//              if there are only groups selected
                if (selectedNonGroupShapes == 0)
                {
//                  get a list of all selected branches
                    List<IComponent> branches = _textures.GetAllSelectedBranches();

//                  remove all these branches from the tree
                    foreach (var branch in branches)
                    {
                        _textures.Remove(branch);
                    }

//                  create a new group for these groups
                    int index = _textures.CreateGroup();

//                  make a branch pointer
                    IComponent insertBranch = _textures.GetBranch(index);
                    
                    bool first = true;
                    
//                  for all selected branches
                    foreach (var branch in branches)
                    {
//                      the first one does not need a new group
                        if (!first)
                        {
                            index = insertBranch.CreateGroup();
                            insertBranch = insertBranch.GetBranch(index);
                        }
                        else
                        {
                            first = false;
                        }
//                      insert all shapes
                        foreach (var shape in branch.GetAllShapes())
                        {
                            insertBranch.Add(new Leaf(shape));
                        }
                    }
                    
                    UnSelectAllTextures();
                }
                else
                {
//                  we are adding a non group with a group #todo make this function grab the last selected branch
                    IComponent branch = _textures.GetSelectedBranch();
                    List<ShapeBase> nonGroupedShapes = _textures.GetNonGroupedSelectedshapes();

//                  for all shapes in the non grouped shapes
                    foreach (var shape in nonGroupedShapes)
                    {
//                      we add it to the selected branch
                        branch.Add(new Leaf(shape));
                    }
                    UnSelectAllTextures();
                }
            }
            else // if a group is not selected
            {
                bool exit = true;
                int index = 0;
                List<int> removeIndexes = new List<int>(); 
                List<ShapeBase> groupShapes = new List<ShapeBase>();
//              for all selected shapes in the non group group
                foreach (ShapeBase shape in _textures.GetFirstChild().GetAllShapes())
                {
//                  if it is selected
                    if (shape.IsSelected())
                    {
//                      add it to the group
                        groupShapes.Add(shape);
                        removeIndexes.Add(index);
                        exit = false;
                    }

                    index++;
                }

//              if there are no selected textures
                if (exit)
                {
                    BtnStage = ButtonStages.Select;
                    UnSelectAllTextures();
                    return;
                }
                
//              if there are group shapes
                if (groupShapes.Count > 0)
                {
//                  we put it in the group
                    int groupIndex = _textures.CreateGroup();
                    foreach (ShapeBase groupShape in groupShapes)
                    {
                        _textures.GetBranch(groupIndex).Add(new Leaf(groupShape));
                    }
//                  and we remove all grouped shapes from the main branch
                    while (removeIndexes.Count > 0)
                    {
                        _textures.GetFirstChild().Remove(removeIndexes[removeIndexes.Count - 1]);
                        removeIndexes.RemoveAt(removeIndexes.Count - 1);
                    }
                    BtnStage = ButtonStages.Select;
                    UnSelectAllTextures();
                    return;
                }
            }
            UnSelectAllTextures();
            BtnStage = ButtonStages.Select;
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
                
//              create the json
                string json = _textures.Save();

//              remove the last ,
                json = json.Remove(json.Length - 1);

//              For each branch we close it
                var searchText="Branches";
                var arr=json.Split(new char[]{' ','\"'});
                var count=Array.FindAll(arr, s => s.Equals(searchText.Trim())).Length;
                
//              close it
                while (count != 0)
                {
                    json = json + "]}";
                    count--;
                }
                
//              write it to the file
                stream.Write(System.Text.Encoding.UTF8.GetBytes(json));
                
//              close the stream
                stream.Close();
            }
//          return to the select functionality
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

                CanvasSave res = null;

                try
                {
//                  deserialize the file content if it is valid json
                    res = JsonSerializer.Deserialize<CanvasSave>(fileContent);
                }
                catch (Exception e)
                {
//                  not a valid save file
                    BtnStage = ButtonStages.Select;
                    Console.WriteLine("invalid save");
                    return;
                }
//              we can reset the entire canvas
                ResetCanvas();

                _textures = new Composite();

//              gets the tree struct
                IComponent children = res.GetTreeStruct(_circleTexture);

                if (children.GetBranch(0).CountBranches() == 0)
                {
                    _textures.Add(children.GetBranch(0));
                    children.Remove(0);
                }
                else
                {
                    _textures.Add(new Composite());
                }
                
                int index = 0;

//              get all children outside
                int outsideGroup = children.GetNumChildren();
                
//              for all children outside
                for (int j = 0; j < outsideGroup; j++)
                {
//                  we look for the branch with the children
                    IComponent payLoad = children;
                    while (payLoad.GetBranch(j).CountBranches() == 1)
                    {
                        payLoad = payLoad.GetBranch(0);
                    }
                    
//                  we reverse the children back in order
                    payLoad.GetBranch(0).ReverseChildren();
                    
                    int numBranches = payLoad.GetBranch(0).CountBranches();

                    IComponent insertBranch = new Composite();
                    IComponent updateBranch = insertBranch;

//                  for all branches
                    for (int i = 0; i < numBranches; i++)
                    {
//                      we don't have to create a group for the first shape
                        if (i != 0)
                        {
                            int nestIndex = updateBranch.CreateGroup();
                            Console.WriteLine("test");
                            updateBranch = updateBranch.GetBranch(nestIndex);
                        }

                        int count = payLoad.GetBranch(0).GetBranch(i).CountBranches(); 
            
//                      if this branch has branches
                        if(count > 0)
                        {
//                          we nest this branch so that the groups persist
                            IComponent nestingBranch = new Composite();
                            IComponent pointBranch = nestingBranch;
                            bool first = true;
//                          for all nested branches
                            for (int NestIndex = 0; NestIndex < count; NestIndex++)
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    int newIndex = pointBranch.GetBranch(0).CreateGroup();
                                    pointBranch = pointBranch.GetBranch(0).GetBranch(newIndex);
                                }

//                              if it is not the last branch
                                if(payLoad.GetBranch(0).GetBranch(i).GetBranch(NestIndex).GetBranch(0).GetType() == typeof(Leaf))
                                {
//                                  insert the last branch
                                    pointBranch.Add(payLoad.GetBranch(0).GetBranch(i).GetBranch(NestIndex));
                                }
                                else
                                {
//                                  insert the last branch
                                    pointBranch.Add(payLoad.GetBranch(0).GetBranch(i).GetBranch(NestIndex).GetBranch(0));
                                }
                            }
                            _textures.Add(nestingBranch);
                            continue;
                        }
                        
//                      for all shapes
                        List<ShapeBase> shapes = payLoad.GetBranch(0).GetBranch(i).GetAllShapes();

//                      for all shapes we add it to the updateBranch
                        foreach (var shape in shapes)
                        {
                            updateBranch.Add(new Leaf(shape));
                        }
                    }
//                  we insert this new nested branch
                    _textures.Add(insertBranch);
                    
//                  and remove it from the result array
                    children.Remove(0);
                    index++;
                }
            }
//          we always reset the button stage
            BtnStage = ButtonStages.Select;
        }
         
//       ######reset functions######
//       resets the entire canvas to an empty state
         public void ResetCanvas()
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
            _numSelectedTextures = 0;
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
            Group,
            Resize,
            Clear
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
        public static bool PointWithinShape(ShapeBase shape, MouseState mousePoint)
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