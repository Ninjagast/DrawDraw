using System.Collections.Generic;
using System.Linq;

namespace DrawDraw.CommandPattern
{
    // The History of command 
    internal class CommandHistory
    {
        private List<ICommand> _history = new List<ICommand>();
        private int _current = -1;
        
        // push to last
        public void Push(ICommand action)
        {
            if (_current != -1 && _history.Count > 0)
            {
                _history = _history.GetRange(0, _current + 1);
            }
            else if (_current == -1)
            {
                _history = new List<ICommand>();
            }

            _history.Add(action);
            _current = _history.Count - 1;
        }
        // undo last
        public bool Undo()
        {
            if (_current < 0) return false;
            
            _current--;
            return true;
        }
        // redo last
        public bool Redo()
        {
            if (_current + 1 >= _history.Count) return false;
            
            _current++;
            return true;
        }
        // Return last command
        public ICommand GetCurrent()
        {
            return _current >= 0 ? _history.ElementAt(_current) : null;
        }
        public void ClearHistory()
        {
            _history = new List<ICommand>();
            _current = -1;
        }
        
    }
    // Command controller
    internal class CanvasCommands
    {
        private static readonly Canvas Canvas = Canvas.Instance;
        private readonly CommandHistory _history = new CommandHistory();
        private ICommand _command;
        
        // Set new command and add it to the history
        public void SetCommand(ICommand commands)
        {
            if (Canvas.MoveStage == Canvas.MoveStages.Move 
                || Canvas.MoveStage == Canvas.MoveStages.Resize 
                || Canvas.BtnStage == Canvas.ButtonStages.Circle 
                || Canvas.BtnStage == Canvas.ButtonStages.Rectangle)
            {
                _history.Push(commands);
            } 
            else if (Canvas.BtnStage == Canvas.ButtonStages.Clear)
            {
                _history.ClearHistory();
            }
            _command = commands;
            _command.ExecuteAction();
        }
        
        // Undo last command
        public void UndoActions()
        {
            _command = _history.GetCurrent();
            if (_history.Undo())
            {
                _command.UndoAction();
            }
        }

        // Redo last command
        public void RedoActions()
        {
            if (_history.Redo())
            {
                _command = _history.GetCurrent();
                _command.RedoAction();
            }
        }
    }
}