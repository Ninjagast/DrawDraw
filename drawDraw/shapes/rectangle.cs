using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class RectangleShape : ShapeBase
    {
        private Rectangle _rectangle;
        
        public RectangleShape(string name, int x, int y, int width, int height, int type) : base(x, y, width, height, type)
        {
        }

        public override ShapeBase Clone(Guid id)
        {
            RectangleShape rec = new RectangleShape("", X, Y, Width, Height, Type) {id = id};
            return rec;
        }

        public override ShapeBase Action(IVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Rectangle GetNewRectangle()
        {
            _rectangle = new Rectangle(X, Y, Width, Height);
            return _rectangle;
        }

        public override void SetRectangle(Rectangle rectangle)
        {
            _rectangle = rectangle;
        }
    }
}