using System;
using System.Collections.Generic;
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
        void ExecuteAction();
        void RedoAction();
        void UndoAction();
    }
    public class AddRectangle : ICommand
    {
        private static Canvas canvas = Canvas.Instance;
        private MouseState _mouseState;
        private Guid objectId;

        public AddRectangle(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        public void ExecuteAction()
        {
            objectId = canvas.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
            Console.WriteLine("ADDING");
        }

        public void RedoAction()
        {
            objectId = canvas.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
        }

        public void UndoAction()
        {
            canvas.DeleteTexture(objectId);
            Console.WriteLine("REMOVING");
        }
    }
    public class AddCircle : ICommand
    {
        private static readonly Canvas canvas = Canvas.Instance;
        private MouseState _mouseState;
        private Guid objectId; 

        public AddCircle(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        public void ExecuteAction()
        {
            objectId = canvas.InsertCircle(new Point(_mouseState.X, _mouseState.Y));
            Console.WriteLine("ADDING");
        }

        public void RedoAction()
        {
            objectId = canvas.InsertCircle(new Point(_mouseState.X, _mouseState.Y));
        }

        public void UndoAction()
        {
            canvas.DeleteTexture(objectId);
            Console.WriteLine("REMOVING");
        }
    }
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
        public void ExecuteAction()
        {
            _canvas.MoveStuff(_mouseState);
        }
        public void RedoAction()
        {
            _canvas.MoveTexure(_selected, _new_positionings);
        }
        public void UndoAction()
        {
            foreach (ShapeBase select in _selected)
            {
                _new_positionings.Add(new Point(select.X, select.Y));
            }
            _canvas.MoveTexure(_selected, _selected_old_pos);
        }
    }

    public class ResizeTexure : ICommand
    {
        private static readonly Canvas _canvas = Canvas.Instance;
        private MouseState _mouseState;
        private ShapeBase _selected;

        public ResizeTexure(MouseState mouseState)
        {
            _mouseState = mouseState;
        }
        public void ExecuteAction()
        {
            _selected = _canvas.ResizeStuff(_mouseState);
        }

        public void RedoAction()
        {
            _selected = _canvas.ResizeStuff(_selected.id, _selected);
        }

        public void UndoAction()
        {
            _selected = _canvas.ResizeStuff(_selected.id, _selected);
        }
    }
    

    internal class CommandHistory
    {
        private List<ICommand> history = new List<ICommand>();
        private int current = -1;
        
        // push to last
        public void Push(ICommand action)
        {
            if(current != -1 && history.Count > 0)
                history = history.GetRange(0, current + 1);

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
    }
    internal class CanvasCommands
    {
        private static readonly Canvas Canvas = Canvas.Instance;
        private readonly CommandHistory history = new CommandHistory();
        private ICommand command;
        
        public void SetCommand(ICommand commands)
        {
            if (Canvas.MoveStage == Canvas.MoveStages.Move || Canvas.MoveStage == Canvas.MoveStages.Resize || Canvas.BtnStage == Canvas.ButtonStages.Circle || Canvas.BtnStage == Canvas.ButtonStages.Rectangle)
            {
                history.Push(commands);
            }
            command = commands;
            command.ExecuteAction();
        }
        
        public void UndoActions()
        {
            command = history.GetCurrent();
            if (history.Undo())
            {
                command.UndoAction();
            }
        }

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