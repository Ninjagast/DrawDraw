using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Accessibility;
using DrawDraw.shapes;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw
{
    public interface IComponent
    {
        public List<ShapeBase> GetAllShapes();
        public String Save();

        public virtual bool SelectAll(MouseState mouseState)
        {
            throw new NotImplementedException();
        }

        public virtual List<ShapeBase> GetAllGroupedShapes()
        {
            throw new NotImplementedException();
        }
        
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

        public virtual List<ShapeBase> GetBranchShapes()
        {
            throw new NotImplementedException();
        }        
        
        public virtual ShapeBase GetShape()
        {
            throw new NotImplementedException();
        }
    }
    
    // is a item in a tree or branch
    class Leaf : IComponent
    {
        public ShapeBase _shape { get; set; }

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
    
    // can be a tree or branch
    class Composite : IComponent
    {
        public List<IComponent> _children { get; set; } = new List<IComponent>();

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
                Console.WriteLine("Out of bounds exception");
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

        public List<ShapeBase> GetAllGroupedShapes()
        {
            List<ShapeBase> returnShapes = new List<ShapeBase>();
            if (_children.Count > 1)
            {
                for (int index = 1; index < _children.Count; index++)
                {
                    foreach (var childElement in _children[index].GetAllShapes())
                    {
                        returnShapes.Add(childElement);
                    }
                }
                return returnShapes;
            }
            else
            {
                return null;
            }
        }

        public virtual bool SelectAll(MouseState mouseState)
        {
//          for all leaves in this branch
            foreach (var leaf in GetBranchShapes())
            {
//              if it is the shape we are looking for
                if (Canvas.PointWithinShape(leaf.GetShape(), mouseState))
                {
//                  select everything in this branch
                    foreach (var nestedLeaf in GetBranchShapes())
                    {
                        nestedLeaf.GetShape().ToggleSelect();
                    }
//                  and return true
                    return true;
                }
            }

//          if we have not found the correct leave

//          for all children
            foreach (var child in _children)
            {
//              if it is not a leaf
                if (child.GetType() != typeof(Leaf))
                {
                    if (child.SelectAll(mouseState))
                    {
                        foreach (var leaves in GetBranchShapes())
                        {
                            leaves.GetShape().ToggleSelect();
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public virtual List<IComponent> GetBranchShapes()
        {
            List<IComponent> shapes = new List<IComponent>();
            foreach (var child in _children)
            {
                if (child.GetType() == typeof(Leaf))
                {
                    shapes.Add(child);
                }
            }
            return shapes.Count > 0 ? shapes : null;
        }

        public int CreateGroup()
        {
            _children.Add(new Composite());
            return _children.Count - 1;
        }

        public string Save()
        {
            Console.WriteLine("saving  rn");
            string result = "[";
            List<IComponent> shapes = GetBranchShapes();
            if (shapes != null)
            {
                foreach (var leaf in shapes)
                {
                    Console.WriteLine("found  leaf");
                    result += leaf.Save() + ",";
//                  if it is the shape we are looking for
                }
                result = result.Remove(result.Length - 1);
            }
            
            
//          for all children
            foreach (var child in _children)
            {
//              if it is not a leaf
                if (child.GetType() != typeof(Leaf))
                {
                    result += child.Save();
                }
            }
            
            return result + "],";
        }
    }
}