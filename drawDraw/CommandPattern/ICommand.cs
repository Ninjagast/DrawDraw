namespace DrawDraw.CommandPattern
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
}