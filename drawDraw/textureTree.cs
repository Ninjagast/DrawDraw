using System;
using System.Collections.Generic;
using System.Linq;
using DrawDraw.shapes;

namespace DrawDraw
{
    public interface Component
    {
        public ShapeBase getShape();
        public List<ShapeBase> GetChildren();
        
        public virtual void Add(Component component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(Component component)
        {
            throw new NotImplementedException();
        }
    }
    
    // is a item in a tree or branch
    class Leaf : Component
    {
        private ShapeBase shape;
        public Leaf(ShapeBase shape)
        {
            this.shape = shape;
        }

        public ShapeBase getShape()
        {
            return shape;
        }

        public List<ShapeBase> GetChildren()
        {
            throw new NotImplementedException();
        }
    }
    
    // can be a tree or branch
    class Composite : Component
    {
        private List<Component> children = new List<Component>();
        
        public void Add(Component component)
        {
            this.children.Add(component);
        }

        public void Remove(Component component)
        {
            this.children.Remove(component);
        }
        
        public ShapeBase getShape()
        {
            return null;
        }
        public List<ShapeBase> GetChildren()
        {
            List<ShapeBase> result = new List<ShapeBase>();
            foreach (var child in children)
            {
                if (child.GetType() == typeof(Leaf))
                {
                    result.Add(child.getShape());
                }
                else
                {
                    result = result.Union(child.GetChildren()).ToList();
                }
            }
            return result;
        }
    }
}