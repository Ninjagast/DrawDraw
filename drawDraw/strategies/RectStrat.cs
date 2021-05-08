using System;
using System.Collections.Generic;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json;

namespace DrawDraw.strategies
{
    public class RectangleStrat: IStrategy
    {
        private static RectangleStrat _instance = new RectangleStrat();
        
        public static RectangleStrat Instance => _instance;

        private RectangleStrat()
        {
            
        }
        public void Draw(ShapeBase shape, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D texture2D = new Texture2D(graphicsDevice, 1 , 1);

            Color[] color = new Color[] {Color.Red};
            if (!shape.Select)
            {
                color = new Color[] {Color.DarkSlateGray};
            }
        
            texture2D.SetData(color);
            spriteBatch.Draw(texture2D, shape.GetNewRectangle(), Color.White);
            DrawContext(shape, spriteBatch);
        }

        public void Resize(ShapeBase shape, Canvas.BorderSides selectedSide, Point mousePoint, Point startPoint)
        {
            switch (selectedSide)
            {
                case Canvas.BorderSides.Bottom:
                    shape.Height -= (startPoint.Y - mousePoint.Y);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;                
                case Canvas.BorderSides.Top:
                    shape.Height += (startPoint.Y - mousePoint.Y);
                    shape.Y -= (startPoint.Y - mousePoint.Y);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;
                case Canvas.BorderSides.Right:
                    shape.Width -= (startPoint.X - mousePoint.X);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;
                case Canvas.BorderSides.Left:
                    shape.Width += (startPoint.X - mousePoint.X);
                    shape.X -= (startPoint.X - mousePoint.X);
                    shape.SetRectangle(new Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
                    break;
            }
        }

        public void DrawContext(ShapeBase shape, SpriteBatch spriteBatch)
        {
            Canvas _canvas = Canvas.Instance;
            List<StorageText> captions = shape.Caption.GetCaption();
            foreach (StorageText caption in captions)
            {
                switch (caption._side)
                {
                    case 0: // Top
                        spriteBatch.DrawString(_canvas._font,caption._message, new Vector2(shape.X + (shape.Width / 2), shape.Y), Color.Black);
                        break;
                    case 1: // Right
                        spriteBatch.DrawString(_canvas._font,caption._message, new Vector2(shape.X + (shape.Width), shape.Y + (shape.Height / 2)), Color.Black);
                        break;
                    case 2: // Bottom
                        spriteBatch.DrawString(_canvas._font,caption._message, new Vector2(shape.X + (shape.Width / 2), shape.Y + shape.Height), Color.Black);
                        break;
                    case 3: // Left
                        spriteBatch.DrawString(_canvas._font,caption._message, new Vector2(shape.X, shape.Y + (shape.Height / 2)), Color.Black);
                        break;
                }
            }
        }
    }
}