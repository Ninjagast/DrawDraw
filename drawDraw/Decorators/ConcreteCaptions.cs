using System;

namespace DrawDraw.Decorators
{
    public class TopCaptions : Decorator
    {
        private String _message;
        public TopCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }

        public override string GetCaption()
        {
            return base.GetCaption() + "{Top : " + _message + "}";
        }
    }    
    
    public class BottomCaptions : Decorator
    {
        private String _message;
        public BottomCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }
        
        public override string GetCaption()
        {
            return base.GetCaption() + "{Bottom : " + _message + "}";
        }
    }
    
    public class RightCaptions : Decorator
    {
        private String _message;
        public RightCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }
        
        public override string GetCaption()
        {
            return base.GetCaption() + "{Right : " + _message + "}";
        }
    }
    
    public class LeftCaptions : Decorator
    {
        private String _message;
        public LeftCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }
        
        public override string GetCaption()
        {
            return base.GetCaption() + "{Left : " + _message + "}";
        }
    }
}