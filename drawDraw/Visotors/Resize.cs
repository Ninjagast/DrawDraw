using System;
using DrawDraw.shapes;
using DrawDraw.strategies;
using Microsoft.Xna.Framework;

namespace DrawDraw
{
    public class Resize : IVisitor
    {
        private Canvas.BorderSides _selectedSide;
        private Point _mouseState;
        private Point _startPos;
        private Context _context;
        public Resize(Canvas.BorderSides selectedSide, Point mousePoint, Point startPos, Context context)
        {
            _selectedSide = selectedSide;
            _mouseState = mousePoint;
            _startPos = startPos;
            _context = context;
        } 
        public ShapeBase Visit(RectangleShape shape)
        {
            _context.SetStrategy(RectangleStrat.Instance);
            _context.Resize(shape, _selectedSide, _mouseState, _startPos);
            return shape;
        }

        public ShapeBase Visit(CircleShape shape)
        {
            _context.SetStrategy(CircleStrat.Instance);
            _context.Resize(shape, _selectedSide, _mouseState, _startPos);
            return shape;
        }
    }
}