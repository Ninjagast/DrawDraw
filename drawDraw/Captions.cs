using System;
using System.Collections.Generic;

namespace DrawDraw
{
    // The base Component interface defines operations that can be altered by
    // decorators.
    public abstract class Captions
    {
        public abstract List<StorageText> GetCaption();
        public abstract String GetCaptionString();
    }
    
    // Concrete Components provide default implementations of the operations.
    // There might be several variations of these classes.
    public class ConcreteCaptions : Captions
    {
        public override List<StorageText> GetCaption()
        {
            return new List<StorageText>();
        }

        public override string GetCaptionString()
        {
            return "";
        }
    }

    public class StorageText
    {
        public int _side;
        public string _message;

        public StorageText(int side, string message)
        {
            _side = side;
            _message = message;
        }
    }
}