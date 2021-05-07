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
        
        public IComponent GetTreeStruct(Texture2D circleTexture)
        {
            IComponent returnTree = new Composite();
            if (Branches != null)
            {
                foreach (var branch in Branches)
                {
                    returnTree.Add(branch.GetTreeStruct(circleTexture));
                }
            }

            if (Leaf != null)
            {
                foreach (var child in Leaf)
                {
                    Leaf leaf = null;
                    if (child.Type == 0)
                    {
                        RectangleShape shape = new RectangleShape("", child.X, child.Y, child.Width, child.Height, 0);
                        leaf = new Leaf(shape);
                    }
                    else
                    {
                        CircleShape shape = new CircleShape("", child.X, child.Y, child.Width, child.Height, 1);
                        shape.Circle = circleTexture;
                        leaf = new Leaf(shape);
                    }
                    returnTree.Add(leaf);
                }
            }
            return returnTree;
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
    }
}