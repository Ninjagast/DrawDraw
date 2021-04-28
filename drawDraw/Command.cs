using System;
using System.Collections.Generic;
using System.Linq;
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
    
    public enum CommandAction
    {
        Copy,
        Paste,
        Undo,
        Add,
        Delete
    }

    public class CanvasCommand : ICommand
    {
        private static Canvas _selected = Canvas.Instance;
        private static CommandAction _action;
        private static MouseState _mouseState;

        public CanvasCommand(MouseState mouseState,CommandAction action)
        {
            _mouseState = mouseState;
            _action = action;
        }

        public void ExecuteAction()
        {
            switch (_action)
            {
                case CommandAction.Copy:
                    break;
                case CommandAction.Paste:
                    break;
                case CommandAction.Undo:
                    break;                
                case CommandAction.Add:
                    _selected.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
                    Console.WriteLine("Adding stuff");
                    break;                
                case CommandAction.Delete:
                    break;
            }
        }

        public void UndoAction()
        {
            switch (_action)
            {
                case CommandAction.Copy:
                    break;
                case CommandAction.Paste:
                    break;
                case CommandAction.Undo:
                    break;                
                case CommandAction.Add:
                    // _selected.InsertRectangle(new Point(_mouseState.X, _mouseState.Y));
                    // Console.WriteLine("Undo Remove stuff");
                    break;                
                case CommandAction.Delete:
                    break;
            }
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

        // Last in...
        public void Push(ICommand action, int index)
        {
            if (history.Count < 0)
            {
                history.Add(action);
            }
            else
            {
                history.Insert(index, action);
            }
        }

        // ...first out
        public void Pop(int index)
        {
            if (history.Any()) //prevent IndexOutOfRangeException for empty list
                history.RemoveAt(index);
        }

        public ICommand getCommand(int index)
        {
            return history[index];
        } 
    }

    internal class ModifyCanvas
    {
        private Texture2D Clipboard;
        private readonly CommandHistory history = new CommandHistory();
        
        private ICommand command;
        
        private int current = 0;
        public void SetCommand(ICommand commands)
        {
            this.command = commands;
            this.command.ExecuteAction();
            history.Push(this.command, current);
            current++;
        }
        
        public void UndoActions()
        {
            command = history.getCommand(current - 1);
            current--;
            
            command.UndoAction();
        }
    }
}