namespace DrawDraw
{
    // The base Component interface defines operations that can be altered by
    // decorators.
    public abstract class Captions
    {
        public abstract string GetCaption();
    }
    
    // Concrete Components provide default implementations of the operations.
    // There might be several variations of these classes.
    public class ConcreteCaptions : Captions
    {
        public override string GetCaption()
        {
            return "";
        }
    }
}