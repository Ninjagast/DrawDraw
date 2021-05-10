using System.Collections.Generic;
using DrawDraw.shapes;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.CompositionPattern
{
    // can be a tree or branch
    class Composite : IComponent
    {
        public List<IComponent> Children { get; set; } = new List<IComponent>();
        public string Caption { get; set; }
        public void Add(IComponent component)
        {
            Children.Add(component);
        }
        public IComponent GetFirstChild()
        {
            return Children[0];
        }
        public void Remove(IComponent component)
        {
            Children.Remove(component);
        }
        public void Remove(int index)
        {
            Children.RemoveAt(index);
        }
        public List<ShapeBase> GetAllShapes()
        {
            List<ShapeBase> result = new List<ShapeBase>();
            foreach (var child in Children)
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
//                      select everything in this branch and its branches
                        foreach (var child in Children)
                        {
                            if (child.GetType() == typeof(Leaf))
                            {
                                if (child.GetShape().IsSelected())
                                {
                                    Canvas.Instance.NumSelectedTextures--;
                                }
                                else
                                {
                                    Canvas.Instance.NumSelectedTextures++;
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
            foreach (var child in Children)
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

//      gets all shapes from a branch
        public virtual List<IComponent> GetBranchShapes()
        {
            List<IComponent> shapes = new List<IComponent>();
            foreach (var child in Children)
            {
                if (child.GetType() == typeof(Leaf))
                {
                    shapes.Add(child);
                }
            }
            return shapes.Count > 0 ? shapes : null;
        }
        
//      gets all branches from a branch
        public virtual List<IComponent> GetAllBranches()
        {
            List<IComponent> branches = new List<IComponent>();
            foreach (var child in Children)
            {
                if (child.GetType() != typeof(Leaf))
                {
                    branches.Add(child);
                }
            }
            return branches.Count > 0 ? branches : null;
        }

        public int CreateGroup()
        {
            Children.Add(new Composite());
            return Children.Count - 1;
        }

        public string Save()
        {
            string result = "";
            
//          for all branches
            bool first = true;
            foreach (var child in Children)
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
                
                if (Caption != null)
                {
                    if (Caption.Length > 0)
                    {
                        result += ", \"Caption\" : \"" + Caption + "\"";
                    } 
                }
                else
                {
                    result += ", \"Caption\" : \" \"";
                }
                result += "},";
            }
            return result;
        }

//      gets the first fully selected branch
        public IComponent GetSelectedBranch()
        {
            bool first = true;
//          for all branches in this branch
            foreach (var child in Children)
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
            
//          if this is a with no branches

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
        public List<ShapeBase> GetNonGroupedSelectedShapes()
        {
            List<ShapeBase> shapes = new List<ShapeBase>();
            List<int> removerList  = new List<int>();
            int index = 0;
            foreach (var shape in Children[0].GetAllShapes())
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
                Children[0].Remove(removerList[removerList.Count - 1]);
                removerList.RemoveAt(removerList.Count - 1);
            }
            
            return shapes;
        }

        public IComponent GetBranch(int branchIndex)
        {
            if (Children.Count > branchIndex)
            {
                return Children[branchIndex];
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
            foreach (var child in Children)
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
            return Children.Count;
        }

        public int CountBranches()
        {
            int num = 0;
            foreach (var branch in Children)
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
            Children.Reverse();
        }
    }
}