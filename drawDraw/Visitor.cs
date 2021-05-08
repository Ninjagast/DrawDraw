using System;
using System.Collections.Generic;
using DrawDraw.shapes;
using DrawDraw.strategies;

namespace DrawDraw
{
    public interface IVisitor
    {
        //different shapes
        public ShapeBase Visit(RectangleShape shape);
        public ShapeBase Visit(CircleShape shape);
    }
}