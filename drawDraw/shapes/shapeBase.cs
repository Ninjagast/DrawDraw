using System;
using DrawDraw.DecoratorsPattern;
using DrawDraw.VisitorsPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.shapes
{
    public abstract class ShapeBase
    {
        public Guid id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //      rectangle = 0
        //      circle = 1
        public int Type { get; set; }
        
        public bool Select = false;
        public Captions Caption = new ConcreteCaptions() ;
        public String SaveString { get; set; }

        public virtual Texture2D GetCircle()
        {
            throw new NotImplementedException();
        }        
        
        public virtual Rectangle GetNewRectangle()
        {
            throw new NotImplementedException();
        }

        public virtual void SetRectangle(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }
        
        protected ShapeBase(int x, int y, int width, int height, int type)
        {
            id = Guid.NewGuid();
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Type = type;
        }
        public void AddCaption(Decorator decorator)
        {
            Caption = decorator;
            SaveString = "[" + Caption.GetCaptionString() + "]";
        }
        
        public abstract ShapeBase Clone(Guid id);
        public abstract ShapeBase Action(IVisitor visitor);

        public MoveBorders DrawBorders()
        {
            MoveBorders moveBorders = new MoveBorders
            {
                BottomMoveBorder = new MoveBorder(X, Y, Width, Height, 0),
                TopMoveBorder = new MoveBorder(X, Y, Width, Height, 1),
                LeftMoveBorder = new MoveBorder(X, Y, Width, Height, 2),
                RightMoveBorder = new MoveBorder(X, Y, Width, Height, 3),
                ShapeId = id
            };
            return moveBorders;
        }

        public void Update(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public ResizeBorders DrawResizeBorders()
        {
            ResizeBorders resizeBorders = new ResizeBorders()
            {
                BottomResizeBorder = new ResizeBorder(X, Y, Width, Height, 0),
                TopResizeBorder = new ResizeBorder(X, Y, Width, Height, 1),
                RightResizeBorder = new ResizeBorder(X, Y, Width, Height, 2),
                LeftResizeBorder = new ResizeBorder(X, Y, Width, Height, 3)
            };
            return resizeBorders;
        }
        
        public Point GetPoint()
        {
            return new Point(X, Y);
        }        
        
        public Point GetDimension()
        {
            return new Point(Width, Height);
        }

        public void ToggleSelect()
        {
            Select = !Select;
        }

        public bool IsSelected()
        {
            return Select;
        }

        public Canvas.BorderSides DetectSide(Point mousePoint)
        {
//          creates a point per side of the texture
            Point right = new Point(X + Width, Y + Width / 2);
            Point left  = new Point(X, Y + Width / 2);
            Point top   = new Point(X + Width / 2, Y);
            Point bot   = new Point(X + Width / 2, Y + Width / 2);
            
//          calculates distance from mouse to each point
            int distanceToRight  = (int)Math.Floor(Vector2.Distance(mousePoint.ToVector2(), right.ToVector2()));
            int distanceToLeft   = (int)Math.Floor(Vector2.Distance(mousePoint.ToVector2(), left.ToVector2()));
            int distanceToTop    = (int)Math.Floor(Vector2.Distance(mousePoint.ToVector2(), top.ToVector2()));
            int distanceToBottom = (int)Math.Floor(Vector2.Distance(mousePoint.ToVector2(), bot.ToVector2()));

//          checks which one is the smallest
            if (distanceToRight <= distanceToLeft && distanceToRight <= distanceToTop && distanceToRight <= distanceToBottom)
            {
//              clicked on the right border
                return Canvas.BorderSides.Right;
            }
            else if (distanceToLeft <= distanceToRight && distanceToLeft <= distanceToTop && distanceToLeft <= distanceToBottom)
            {
//              clicked on the left border
                return Canvas.BorderSides.Left;
            }
            else if (distanceToTop <= distanceToRight && distanceToTop <= distanceToLeft && distanceToTop <= distanceToBottom)
            {
//              clicked on the top border
                return Canvas.BorderSides.Top;
            }
            else
            {
//              clicked on the bottom border
                return Canvas.BorderSides.Bottom;
            }
        }
    }
}