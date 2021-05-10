using System;
using System.Collections.Generic;
using DrawDraw.shapes;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.CompositionPattern
{
    public interface IComponent
    {
        public List<ShapeBase> GetAllShapes();
        public String Save();
        public string Caption { get; set; }

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
            Caption = caption;
        }
    }
}