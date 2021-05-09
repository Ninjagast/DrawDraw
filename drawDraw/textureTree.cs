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
        public string _caption { get; set; }

        public virtual bool SelectAll(MouseState mouseState, bool target = false)
        {
            throw new NotImplementedException();
        }

        public virtual List<IComponent> GetAllSelectedBranches()
        {
            throw new NotImplementedException();
        }

        public virtual int CreateGroup()
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
        public virtual IComponent GetSelectedBranch()
        {
            throw new NotImplementedException();
        }
        
        public virtual void Remove(int index)
        {
            throw new NotImplementedException();
        }

        public virtual ShapeBase GetShape()
        {
            throw new NotImplementedException();
        }

        public virtual IComponent GetBranch(int branchIndex)
        {
            throw new NotImplementedException();
        }

        public virtual int GetNumChildren()
        {
            throw new NotImplementedException();
        }

        public virtual int CountBranches()
        {
            throw new NotImplementedException();
        }

        public virtual void ReverseChildren()
        {
            throw new NotImplementedException();
        }
        
        public void SetCaption(string caption)
        {
            _caption = caption;
        }
    }
    
    // is a item in a tree or branch
    class Leaf : IComponent
    {
        public ShapeBase _shape { get; set; }
        public string _caption { get; set; }
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
            // saveString = "{" + _shape.Caption.GetCaptionString() + "}";
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
        public string _caption { get; set; }
        public void Add(IComponent component)
        {
            _children.Add(component);
        }
        public IComponent GetFirstChild()
        {
            return _children[0];
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
        public virtual bool SelectAll(MouseState mouseState, bool target = false)
        {
//          for all leaves in this branch
            List<IComponent> branchShapes = GetBranchShapes();
            if (branchShapes != null)
            {
                foreach (var leaf in branchShapes)
                {
//                  if it is the shape we are looking for or if it is a child branch of the target branch
                    if (Canvas.PointWithinShape(leaf.GetShape(), mouseState) || target)
                    {
                        Console.WriteLine("this is the branch or a target branch");
//                      select everything in this branch and its branches
                        foreach (var child in _children)
                        {
                            if (child.GetType() == typeof(Leaf))
                            {
                                if (child.GetShape().IsSelected())
                                {
                                    Canvas.Instance._numSelectedTextures--;
                                }
                                else
                                {
                                    Canvas.Instance._numSelectedTextures++;
                                }
                                child.GetShape().ToggleSelect();
                            }
                            else
                            {
                                child.SelectAll(mouseState, true);
                            }
                        }
//                      and return true
                        return true;
                    }
                }
            }

//          if we have not found the correct leave

//          for all children
            bool foundit = false;
            List<ShapeBase> allShapes = null;
            foreach (var child in _children)
            {
//              if it is not a leaf
                if (child.GetType() != typeof(Leaf))
                {
//                  we call this function again
                    if (child.SelectAll(mouseState))
                    {
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
        
        public virtual List<IComponent> GetAllBranches()
        {
            List<IComponent> branches = new List<IComponent>();
            foreach (var child in _children)
            {
                if (child.GetType() != typeof(Leaf))
                {
                    branches.Add(child);
                }
            }
            return branches;
        }

        public int CreateGroup()
        {
            _children.Add(new Composite());
            return _children.Count - 1;
        }

        public string Save()
        {
            string result = "";
            
//          for all branches
            bool first = true;
            foreach (var child in _children)
            {
//              if it is not a leaf
                if (child.GetType() != typeof(Leaf))
                {
//                  we nest the branch
                    if (first)
                    {
                        result += "{\"Branches\":[";
                    }
                    
//                  and call this function
                    result += child.Save();
                    first = false;
                }
            }
            
//          formats the shapes into this branch
            List<IComponent> shapes = GetBranchShapes();
            result += FormatShapes(shapes);
//          returns the json
            return result;
        }
        private string FormatShapes(List<IComponent> shapes)
        {
//          if there are shapes in this branch
            string result = "";
            if (shapes != null)
            {
//              adds a leaf object
                result += "{\"Leaf\":[";
                foreach (var leaf in shapes)
                {
//                  fills the array with data
                    result += leaf.Save() + ",";
                }
//              remove the last , which is not needed
                result = result.Remove(result.Length - 1);
                result += "]";
                
                if (_caption != null)
                {
                    if (_caption.Length > 0)
                    {
                        result += ", \"_caption\" : \"" + _caption + "\"";
                    } 
                }
                else
                {
                    result += ", \"_caption\" : \" \"";
                }
                
                result += "},";
            }
            return result;
        }

//      gets the first selected branch
        public IComponent GetSelectedBranch()
        {
            bool first = true;
//          for all branches in this branch
            foreach (var child in _children)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    IComponent res = null;
                    if (child.GetType() != typeof(Leaf))
                    {
//                      recall this function
                        res = child.GetSelectedBranch();
                    }

                    if (res != null)
                    {
                        return res;
                    }
                }
            }
            
//          if this is a branch with only leaves
            bool allSelected = false;
            List<IComponent> leaves = GetBranchShapes();
            
//          if it has leaves
            if (leaves != null)
            {
//              we check if all of them are selected
                allSelected = true;
                foreach (var leaf in GetBranchShapes())
                {
                    if (!leaf.GetShape().IsSelected())
                    {
                        allSelected = false;
                    }
                }
            }
//          if it is? return this branch otherwise return null
            if (allSelected)
            {
                return this;
            }
            else
            {
                return null;
            }
        }

//      getsAll non grouped shapes and removes them from the tree
        public List<ShapeBase> GetNonGroupedSelectedshapes()
        {
            List<ShapeBase> shapes = new List<ShapeBase>();
            List<int> removerList = new List<int>();
            int index = 0;
            foreach (var shape in _children[0].GetAllShapes())
            {
                if (shape.IsSelected())
                {
                    shapes.Add(shape);
                    removerList.Add(index);
                }
                index++;
            }

            while (removerList.Count > 0)
            {
                _children[0].Remove(removerList[removerList.Count - 1]);
                removerList.RemoveAt(removerList.Count - 1);
            }
            
            return shapes;
        }

        public IComponent GetBranch(int branchIndex)
        {
            if (_children.Count > branchIndex)
            {
                return _children[branchIndex];
            }
            else
            {
                return null;
            }
        }

//      works the same as the singular branch function "GetSelectedBranch"
        public List<IComponent> GetAllSelectedBranches()
        {
            List<IComponent> returnList = new List<IComponent>();
            foreach (var child in _children)
            {
                if (child.GetType() != typeof(Leaf))
                {
                    List<IComponent> res = child.GetAllSelectedBranches();
                    if (res != null)
                    {
                        returnList.AddRange(res);
                    }
                }
            }

            bool allSelected = false;
            List<IComponent> leaves = GetBranchShapes();
            
            if (leaves != null)
            {
                allSelected = true;
                foreach (var leaf in GetBranchShapes())
                {
                    if (!leaf.GetShape().IsSelected())
                    {
                        allSelected = false;
                    }
                }
            }
            
            if (allSelected)
            {
                returnList.Add(this);
                return returnList;
            }
            if (returnList.Count > 0)
            {
                return returnList;
            }
            else
            {
                return null;
            }
        }

        public int GetNumChildren()
        {
            return _children.Count;
        }

        public int CountBranches()
        {
            int num = 0;
            foreach (var branch in _children)
            {
                if (branch.GetType() != typeof(Leaf))
                {
                    num++;
                }
            }

            return num;
        }

        public void ReverseChildren()
        {
            _children.Reverse();
        }
    }
}