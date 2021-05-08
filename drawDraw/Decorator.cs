﻿using System;
using System.Collections.Generic;

namespace DrawDraw
{
    // The base Decorator class follows the same interface as the other
    // components. The primary purpose of this class is to define the wrapping
    // interface for all concrete decorators. The default implementation of the
    // wrapping code might include a field for storing a wrapped component and
    // the means to initialize it.
    public class Decorator : Captions
    {
        protected Captions _captions;

        public Decorator(Captions captions)
        {
            this._captions = captions;
        }

        // The Decorator delegates all work to the wrapped component.
        public override List<StorageText> GetCaption()
        {
            return _captions.GetCaption();
        }

    }
}