using System;
using System.Collections.Generic;
using System.Linq;
using DrawDraw.shapes;

namespace DrawDraw
{
    public interface IComponent
    {
        public List<ShapeBase> GetAllShapes();
        
        public virtual void Add(IComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(IComponent component)
        {
            throw new NotImplementedException();
        }
        
        public virtual void Remove(int index)
        {
            throw new NotImplementedException();
        }
    }
    
    // is a item in a tree or branch
    class Leaf : IComponent
    {
        private ShapeBase _shape;
        public Leaf(ShapeBase shape)
        {
            _shape = shape;
        }
        public List<ShapeBase> GetAllShapes()
        {
            return new List<ShapeBase> {_shape};
        }
    }
    
    // can be a tree or branch
    class Composite : IComponent
    {
        private List<IComponent> _children = new List<IComponent>();
        
        public void Add(IComponent component)
        {
            _children.Add(component);
        }

        public IComponent GetFirstChild()
        {
            return _children[0];
        }

        public IComponent GetEntireBranch(int index)
        {
            if (_children.Count > index)
            {
                return _children[index];
            }
            else
            {
                Console.WriteLine("Out of bounds exception biiiiitch");
                return null;
            }
        }
        
        public void Remove(IComponent component)
        {
            _children.Remove(component);
        }

        public void Remove(int index)
        {
            _children.RemoveAt(index);
        }
        
        public List<ShapeBase> GetAllShapes()
        {
            List<ShapeBase> result = new List<ShapeBase>();
            foreach (var child in _children)
            {
                foreach (var childElement in child.GetAllShapes())
                {
                    result.Add(childElement);
                }
            }
            return result;
        }
    }
}