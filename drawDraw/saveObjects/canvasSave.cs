﻿using System;
using System.Collections.Generic;
using DrawDraw.CompositionPattern;
using DrawDraw.shapes;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.saveObjects
{
    public class CanvasSave
    {
//      template class for json serialization
        public List<CanvasSave> Branches { get; set; }
        public List<canvasChild> Leaf { get; set; }
        public String Caption { get; set; }
        
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
                        returnTree.Caption = branch.Caption;
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
                        RectangleShape shape = new RectangleShape("", child.X, child.Y, child.Width, child.Height, 0)
                        {
                            SaveString = child.SaveString
                        };
                        leaf = new Leaf(shape);
                    }
                    else
                    {
                        CircleShape shape = new CircleShape("", child.X, child.Y, child.Width, child.Height, 1)
                        {
                            Circle = circleTexture, SaveString = child.SaveString
                        };
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
        public String SaveString { get; set; }
    }
}