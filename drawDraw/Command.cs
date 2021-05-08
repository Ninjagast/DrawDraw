using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Transactions;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;

namespace DrawDraw
{
    // The Command interface declares a method for executing a commands.
    public interface ICommand
    {
        // Execute action
        void ExecuteAction();
        // Redo undo action
        void RedoAction();
        // Undo executed action
        void UndoAction();
    }
    // Add a Rectangle to the canvas
    public class AddRectangle : ICommand
    {
        private static Canvas canvas = Canvas.Instance;
        private MouseState _mouseState;
        private Guid objectId;

        // Add texture to canvas
        public AddRectangle(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        public void ExecuteAction()
        {
            objectId = canvas.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
            Console.WriteLine("ADDING");
        }
        // Add texure to canvas
        public void RedoAction()
        {
            objectId = canvas.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
        }
        // Delete texure to canvas
        public void UndoAction()
        {
            canvas.DeleteTexture(objectId);
            Console.WriteLine("REMOVING");
        }
    }
    // Add a Circle to the canvas
    public class AddCircle : ICommand
    {
        private static readonly Canvas canvas = Canvas.Instance;
        private MouseState _mouseState;
        private Guid objectId; 
        // Add texture to canvas
        public AddCircle(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        public void ExecuteAction()
        {
            objectId = canvas.InsertCircle(new Point(_mouseState.X, _mouseState.Y));
            Console.WriteLine("ADDING");
        }
        // Add texure to canvas
        public void RedoAction()
        {
            objectId = canvas.InsertCircle(new Point(_mouseState.X, _mouseState.Y));
        }
        // Delete texure to canvas
        public void UndoAction()
        {
            canvas.DeleteTexture(objectId);
            Console.WriteLine("REMOVING");
        }
    }
    // Moves one or all selected textures on the canvas
    public class MoveTexure : ICommand
    {
        private static readonly Canvas _canvas = Canvas.Instance;
        private MouseState _mouseState;
        private List<ShapeBase> _selected;
        private List<Point> _selected_old_pos = new List<Point>();
        private List<Point> _new_positionings = new List<Point>();
        public MoveTexure(MouseState mouseState, List<ShapeBase> selected)
        {
            _mouseState = mouseState;
            _selected = selected;

            foreach (ShapeBase select in selected)
            {
                _selected_old_pos.Add(new Point(select.X, select.Y));
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
            _canvas.MoveTexure(_selected, _new_positionings);
        }
        // Move texture to old positioning
        public void UndoAction()
        {
            foreach (ShapeBase select in _selected)
            {
                _new_positionings.Add(new Point(select.X, select.Y));
            }
            _canvas.MoveTexure(_selected, _selected_old_pos);
        }
    }
    // Resizes one texture
    public class ResizeTexure : ICommand
    {
        private static readonly Canvas _canvas = Canvas.Instance;
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
        private static readonly Canvas _canvas = Canvas.Instance;
        // Group selected textures 
        public void ExecuteAction()
        {
            _canvas.GroupTextures();
        }
        public void RedoAction()
        {
            //TODO implement a redo
            throw new NotImplementedException();
        }
        public void UndoAction()
        {
            //TODO implement a undo
            throw new NotImplementedException();
        }
    }
    // Clears the Canvas
    public class ClearCanvas : ICommand
    {
        private static readonly Canvas _canvas = Canvas.Instance;
        // Clear canvas
        public void ExecuteAction()
        {
            _canvas.ResetCanvas();
        }
        public void RedoAction()
        {
            //TODO implement a redo
            throw new NotImplementedException();
        }
        public void UndoAction()
        {
            //TODO implement a undo
            throw new NotImplementedException();
        }
    }
    // The History of command 
    internal class CommandHistory
    {
        private List<ICommand> history = new List<ICommand>();
        private int current = -1;
        
        // push to last
        public void Push(ICommand action)
        {
            if (current != -1 && history.Count > 0)
                history = history.GetRange(0, current + 1);
            else if (current == -1)
                history = new List<ICommand>();

            history.Add(action);
            current = history.Count - 1;
        }
        // undo last
        public bool Undo()
        {
            if (current < 0) return false;
            current--;
            return true;
        }
        // redo last
        public bool Redo()
        {
            if (current + 1 >= history.Count) return false;
            current++;
            return true;
        }
        // Return last command
        public ICommand GetCurrent()
        {
            Console.WriteLine(current);
            return current >= 0 ? history.ElementAt(current) : null;
        }
        public void ClearHistory()
        {
            history = new List<ICommand>();
            current = -1;
        }
        
    }
    // Command controller
    internal class CanvasCommands
    {
        private static readonly Canvas Canvas = Canvas.Instance;
        private readonly CommandHistory history = new CommandHistory();
        private ICommand command;
        
        // Set new command and add it to the history
        public void SetCommand(ICommand commands)
        {
            if (Canvas.MoveStage == Canvas.MoveStages.Move 
                || Canvas.MoveStage == Canvas.MoveStages.Resize 
                || Canvas.BtnStage == Canvas.ButtonStages.Circle 
                || Canvas.BtnStage == Canvas.ButtonStages.Rectangle)
            {
                history.Push(commands);
            } 
            else if (Canvas.BtnStage == Canvas.ButtonStages.Clear)
            {
                history.ClearHistory();
            }
            command = commands;
            command.ExecuteAction();
        }
        
        // Undo last command
        public void UndoActions()
        {
            command = history.GetCurrent();
            if (history.Undo())
            {
                command.UndoAction();
            }
        }

        // Redo last command
        public void RedoActions()
        {
            if (history.Redo())
            {
                command = history.GetCurrent();
                command.RedoAction();
            }
        }
    }
}