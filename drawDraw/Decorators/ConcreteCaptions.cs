using System;
using System.Collections.Generic;

namespace DrawDraw.Decorators
{
    public class TopCaptions : Decorator
    {
        private string _message;
        public TopCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }

        public override List<StorageText> GetCaption()
        {
            List<StorageText> storageText = base.GetCaption();
            storageText.Add(new StorageText(0, _message));
            return storageText;
        }
        
        public override string GetCaptionString()
        {
            String result = _captions.GetCaptionString();
            if (result.Length > 0)
                result += ",";
            result += "{\"side\" : 0," +"\"message\" :\""+ _message + "\"}";
            return result;
        }
    }    
    
    public class RightCaptions : Decorator
    {
        private string _message;
        public RightCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }
        
        public override List<StorageText> GetCaption()
        {
            List<StorageText> storageText = base.GetCaption();
            storageText.Add(new StorageText(1, _message));
            return storageText;
        }
        public override string GetCaptionString()
        {
            String result = _captions.GetCaptionString();
            if (result.Length > 0)
                result += ",";
            result += "{\"side\" : 1," +"\"message\" :\""+ _message + "\"}";
            return result;
        }
    }
    
    public class BottomCaptions : Decorator
    {
        private string _message;
        public BottomCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }
        public override List<StorageText> GetCaption()
        {
            List<StorageText> storageText = base.GetCaption();
            storageText.Add(new StorageText(2, _message));
            return storageText;
        }
        public override string GetCaptionString()
        {
            String result = _captions.GetCaptionString();
            if (result.Length > 0)
                result += ",";
            result += "{\"side\" : 2," +"\"message\" :\""+ _message + "\"}";
            return result;
        }
    }
    
    public class LeftCaptions : Decorator
    {
        private string _message;
        public LeftCaptions(Captions captions, String message) : base(captions)
        {
            _message = message;
        }
        public override List<StorageText> GetCaption()
        {
            List<StorageText> storageText = base.GetCaption();
            storageText.Add(new StorageText(3, _message));
            return storageText;
        }
        public override string GetCaptionString()
        {
            String result = _captions.GetCaptionString();
            if (result.Length > 0)
                result += ",";
            result += "{\"side\" : 3," +"\"message\" :\""+ _message + "\"}";
            return result;
        }
    }
}