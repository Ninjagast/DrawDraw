using System;
using System.Collections.Generic;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.CommandPattern
{
   public class AddRectangle : ICommand
    {
        private static Canvas _canvas = Canvas.Instance;
        private MouseState _mouseState;
        private Guid _objectId;

        // Add texture to canvas
        public AddRectangle(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        public void ExecuteAction()
        {
            _objectId = _canvas.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
        }
        // Add texure to canvas
        public void RedoAction()
        {
            _objectId = _canvas.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
        }
        // Delete texure to canvas
        public void UndoAction()
        {
            _canvas.DeleteTexture(_objectId);
        }
    }
   
    // Add a Circle to the canvas
    public class AddCircle : ICommand
    {
        private static Canvas _canvas = Canvas.Instance;
        private MouseState _mouseState;
        private Guid _objectId; 
        // Add texture to canvas
        public AddCircle(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        public void ExecuteAction()
        {
            _objectId = _canvas.InsertCircle(new Point(_mouseState.X, _mouseState.Y));
        }
        // Add texure to canvas
        public void RedoAction()
        {
            _objectId = _canvas.InsertCircle(new Point(_mouseState.X, _mouseState.Y));
        }
        // Delete texure to canvas
        public void UndoAction()
        {
            _canvas.DeleteTexture(_objectId);
        }
    }
    
    // Moves one or all selected textures on the canvas
    public class MoveTexure : ICommand
    {
        private static Canvas _canvas = Canvas.Instance;
        private MouseState _mouseState;
        private List<ShapeBase> _selected;
        private List<Point> _selectedOldPos = new List<Point>();
        private List<Point> _newPositionings = new List<Point>();
        public MoveTexure(MouseState mouseState, List<ShapeBase> selected)
        {
            _mouseState = mouseState;
            _selected = selected;

            foreach (ShapeBase select in selected)
            {
                _selectedOldPos.Add(new Point(select.X, select.Y));
            }
        }
        // Move texture to new positioning
        public void ExecuteAction()
        {
            _canvas.MoveStuff(_mouseState);
        }
        // Move texture to new positioning
        public void RedoAction()
        {
            _canvas.MoveTexture(_selected, _newPositionings);
        }
        // Move texture to old positioning
        public void UndoAction()
        {
            foreach (ShapeBase select in _selected)
            {
                _newPositionings.Add(new Point(select.X, select.Y));
            }
            _canvas.MoveTexture(_selected, _selectedOldPos);
        }
    }
    
    // Resizes one texture
    public class ResizeTexure : ICommand
    {
        private static Canvas _canvas = Canvas.Instance;
        private MouseState _mouseState;
        private ShapeBase _selected;

        public ResizeTexure(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        // Resize one side to new size
        public void ExecuteAction()
        {
            _selected = _canvas.ResizeStuff(_mouseState);
        }
        // Resize one side to new Size
        public void RedoAction()
        {
            _selected = _canvas.ResizeStuff(_selected.id, _selected);
        }
        // Resize one side to old Size
        public void UndoAction()
        {
            _selected = _canvas.ResizeStuff(_selected.id, _selected);
        }
    }
    
    // Groups selected groups or textures
    public class GroupTexure : ICommand
    {
        private static Canvas _canvas = Canvas.Instance;
        // Group selected textures 
        public void ExecuteAction()
        {
            _canvas.GroupTextures();
        }
        public void RedoAction()
        {
            throw new NotImplementedException();
        }
        public void UndoAction()
        {
            throw new NotImplementedException();
        }
    }
    
    // Clears the Canvas
    public class ClearCanvas : ICommand
    {
        private static Canvas _canvas = Canvas.Instance;
        // Clear canvas
        public void ExecuteAction()
        {
            _canvas.ResetCanvas();
        }
        public void RedoAction()
        {
            throw new NotImplementedException();
        }
        public void UndoAction()
        {
            throw new NotImplementedException();
        }
    }
}