using System;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;

namespace DrawDraw.VisitorsPattern
{
    public class MoveObject : IVisitor
    {
        private Point _finalPos;
        private Point _dimensions;

        public MoveObject(Point finalPos, Point dimensions)
        {
            _finalPos   = finalPos;
            _dimensions = dimensions;
        }
        public ShapeBase Visit(RectangleShape shape)
        {
            shape.Update(_finalPos.X, _finalPos.Y, _dimensions.X, _dimensions.Y);
            return shape;
        }

        public ShapeBase Visit(CircleShape shape)
        {
            shape.Update(_finalPos.X, _finalPos.Y, _dimensions.X, _dimensions.Y);
            return shape;
        }
    }
}