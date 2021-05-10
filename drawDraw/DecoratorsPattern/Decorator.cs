using System.Collections.Generic;

namespace DrawDraw.DecoratorsPattern
{
    public class Decorator : Captions
    {
        protected Captions Captions;

        public Decorator(Captions captions)
        {
            Captions = captions;
        }

        // The Decorator delegates all work to the wrapped component.
        public override List<StorageText> GetCaption()
        {
            return Captions.GetCaption();
        }

        public override string GetCaptionString()
        {
            return Captions.GetCaptionString(); 
        }
    }
}