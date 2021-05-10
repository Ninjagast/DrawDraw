using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrawDraw.shapes
{
    public class ResizeBorders
    {
//      BottomBorder 0
//      TopBorder 1
//      LeftBorder 2
//      RightBorder 3
        
        public ResizeBorder LeftResizeBorder;
        public ResizeBorder RightResizeBorder;
        public ResizeBorder TopResizeBorder;
        public ResizeBorder BottomResizeBorder;
        public Canvas.BorderSides SelectedSide = Canvas.BorderSides.Undefined;
        
//      we update the border based on the side the user has selected
        public void Update(MouseState mouseState, Point startPos)
        {
            if (SelectedSide == Canvas.BorderSides.Bottom)
            {
                LeftResizeBorder = null;
                RightResizeBorder = null;
                TopResizeBorder = null;
                BottomResizeBorder.Update(new Point(mouseState.X, mouseState.Y), startPos, SelectedSide);
            }
            else if (SelectedSide == Canvas.BorderSides.Top)
            {
                LeftResizeBorder = null;
                RightResizeBorder = null;
                BottomResizeBorder = null;
                TopResizeBorder.Update(new Point(mouseState.X, mouseState.Y), startPos, SelectedSide);
            }
            else if (SelectedSide == Canvas.BorderSides.Right)
            {
                LeftResizeBorder = null;
                BottomResizeBorder = null;
                TopResizeBorder = null;
                RightResizeBorder.Update(new Point(mouseState.X, mouseState.Y), startPos, SelectedSide);
            }
            else
            {                
                RightResizeBorder = null;
                RightResizeBorder = null;
                TopResizeBorder = null;
                LeftResizeBorder.Update(new Point(mouseState.X, mouseState.Y), startPos, SelectedSide);
            }
        }

//      draws the border 
        public void DrawBorder(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
//          draws all borders if the user has not selected a side yet
            if (SelectedSide == Canvas.BorderSides.Undefined)
            {
                LeftResizeBorder.Draw(spriteBatch, graphicsDevice);
                RightResizeBorder.Draw(spriteBatch, graphicsDevice);
                TopResizeBorder.Draw(spriteBatch, graphicsDevice);
                BottomResizeBorder.Draw(spriteBatch, graphicsDevice);
            }
//          else draws only the selected border
            else
            {
                switch (SelectedSide)
                {
                    case Canvas.BorderSides.Bottom:
                        BottomResizeBorder.Draw(spriteBatch, graphicsDevice);
                        break;
                    case Canvas.BorderSides.Top:
                        TopResizeBorder.Draw(spriteBatch, graphicsDevice);
                        break;                
                    case Canvas.BorderSides.Right:
                        RightResizeBorder.Draw(spriteBatch, graphicsDevice);
                        break;
                    case Canvas.BorderSides.Left:                    
                        LeftResizeBorder.Draw(spriteBatch, graphicsDevice);
                        break;
                }
            }
        }
    }
}