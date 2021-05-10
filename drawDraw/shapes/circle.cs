using System;
using DrawDraw.VisitorsPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public class  CircleShape : ShapeBase
    {
        public Texture2D Circle;
        
        public CircleShape(string name, int x, int y, int width, int height, int type) : base(x, y, width, height, type)
        {
        }
        
        public override Texture2D GetCircle()
        {
            return Circle;
        }

        public override ShapeBase Clone(Guid id)
        {
            CircleShape shape = new CircleShape("", X, Y, Width, Height, Type) {id = id, Circle = Circle};
            return shape;
        }

        public override ShapeBase Action(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}