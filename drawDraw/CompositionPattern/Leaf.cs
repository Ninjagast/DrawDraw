using System.Collections.Generic;
using System.Text.Json;
using DrawDraw.shapes;

namespace DrawDraw.CompositionPattern
{
    
    // is a item in a tree or branch
    class Leaf : IComponent
    {
        public ShapeBase _shape { get; set; }
        public string Caption { get; set; }
        public Leaf(ShapeBase shape)
        {
            _shape = shape;
        }
        public List<ShapeBase> GetAllShapes()
        {
            return new List<ShapeBase> {_shape};
        }

        public string Save()
        {
            return JsonSerializer.Serialize(_shape);
        }


        public ShapeBase GetShape()
        {
            return _shape;
        }
    }
}