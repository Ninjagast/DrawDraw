using System;

namespace DrawDraw
{
    public class CanvasSave
    {
        public Guid id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
//      rectangle = 0
//      circle = 1
        public int Type { get; set; }
    }
}