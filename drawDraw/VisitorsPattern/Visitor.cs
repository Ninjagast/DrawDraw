using DrawDraw.shapes;

namespace DrawDraw.VisitorsPattern
{
    public interface IVisitor
    {
        //different shapes
        public ShapeBase Visit(RectangleShape shape);
        public ShapeBase Visit(CircleShape shape);
    }
}