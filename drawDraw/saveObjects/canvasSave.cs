using System;
using System.Collections.Generic;
using System.Linq;
using DrawDraw.shapes;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw
{
    public class CanvasSave
    {
        public List<CanvasSave> Branches { get; set; }
        public List<canvasChild> Leaf { get; set; }
        
        public String _caption { get; set; }
        
//      returns the saved state of the tree
        public IComponent GetTreeStruct(Texture2D circleTexture)
        {
//          if this tree has a branch
            if (Branches != null)
            {
//              creates a new tree for the branch
                IComponent returnTree = new Composite();

                foreach (var branch in Branches)
                {
                    IComponent res = branch.GetTreeStruct(circleTexture);
                    if (res != null)
                    {
//                      puts the result from this function into this branch
                        returnTree.Add(branch.GetTreeStruct(circleTexture));
                        returnTree._caption = branch._caption;
                    }
                }
                return returnTree;
            }

//          if this branch is not nested

//          and if it has leaves
            if (Leaf != null)
            {
                IComponent returnTree = new Composite();

//              we create leaves for all leaves in this branch
                foreach (var child in Leaf)
                {
                    Leaf leaf = null;
//                  is this child a ellipse or a rectangle?
                    if (child.Type == 0)
                    {
                        RectangleShape shape = new RectangleShape("", child.X, child.Y, child.Width, child.Height, 0);
                        shape.saveString = child.saveString;
                        leaf = new Leaf(shape);
                    }
                    else
                    {
                        CircleShape shape = new CircleShape("", child.X, child.Y, child.Width, child.Height, 1);
                        shape.Circle = circleTexture;
                        shape.saveString = child.saveString;
                        leaf = new Leaf(shape);
                    }
//                  add them to the tree
                    returnTree.Add(leaf);
                }
//              return the tree
                return returnTree;
            }
            return null;
        }
    }

    public class canvasChild
    {
        public Guid id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
//      rectangle = 0
//      circle = 1
        public int Type { get; set; }
        public List<canvasChild> Children;
        public String saveString { get; set; }
    }
}