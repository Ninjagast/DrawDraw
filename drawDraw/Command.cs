using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
        public void UndoAction()
        {
            canvas.DeleteTexture(objectId);
            Console.WriteLine("REMOVING");
        }
    }
    
    // The concrete commands go here.
    // internal class CopyCommand : Command
    // {
    //     // the selected data type
    //     private Texture2D _textures;
    //
    //     public CopyCommand(Texture2D canvas)
    //     {
    //         _textures = canvas;
    //     }
    //
    //     public void Execute()
    //     {
    //         Clipboard = canvas;
    //     }
    // }
    //
    // internal class CutCommand : Command
    // {
    //     // the selected data type
    //     private readonly Canvas canvas = Canvas.Instance;
    //
    //     public CutCommand(Canvas canvas)
    //     {
    //         this.canvas = canvas;
    //     }
    //
    //     public override void Execute()
    //     {
    //         saveBackup();
    //         Clipboard = canvas;
    //     }
    // }

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
    internal class ModifyCanvas
    {
        private Texture2D Clipboard;
        private readonly CommandHistory history = new CommandHistory();
        private ICommand command;

        
        public void SetCommand(ICommand commands)
        {
            history.Push(commands);
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
                command.ExecuteAction();
            }
        }
    }
}