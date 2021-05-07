using System;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;

namespace DrawDraw
{
    public class Resize : IVisitor
    {
        private Canvas.BorderSides _selectedSide;
        private Point _mouseState;
        private Point _startPos;
        public Resize(Canvas.BorderSides selectedSide, Point mousePoint, Point startPos)
        {
            _selectedSide = selectedSide;
            _mouseState = mousePoint;
            _startPos = startPos;
        } 
        public ShapeBase Visit(RectangleShape shape)
        {
            shape.Resize(_selectedSide, _mouseState, _startPos);
            Console.WriteLine("Resize Rectangle shape");
            return shape;
        }

        public ShapeBase Visit(CircleShape shape)
        {
            shape.Resize(_selectedSide, _mouseState, _startPos);
            Console.WriteLine("Resize Circle shape");
            return shape;
        }
    }
}