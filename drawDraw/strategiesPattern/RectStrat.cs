using System.Collections.Generic;
using System.Text.Json;
using DrawDraw.DecoratorsPattern;
using DrawDraw.saveObjects;
using DrawDraw.shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrawDraw.strategiesPattern
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
            DrawCaption(shape, spriteBatch);
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

        public void DrawCaption(ShapeBase shape, SpriteBatch spriteBatch)
        {
            List<StorageText> captions = shape.Caption.GetCaption();

            if (captions.Count == 0 && shape.SaveString != null)
            {
                List<TextObject> res = JsonSerializer.Deserialize<List<TextObject>>(shape.SaveString);
                
                foreach (var textObject in res)
                {
                    switch (textObject.side)
                    {
                        case 0:
                            shape.AddCaption(new TopCaptions(shape.Caption, "NEW MESSAGE T"));
                            break;
                        case 1:
                            shape.AddCaption(new RightCaptions(shape.Caption, "NEW MESSAGE R"));
                            break;
                        case 2:
                            shape.AddCaption(new BottomCaptions(shape.Caption, "NEW MESSAGE B"));
                            break;
                        case 3:
                            shape.AddCaption(new LeftCaptions(shape.Caption, "NEW MESSAGE L"));
                            break;
                    }
                }
            }
            
            foreach (StorageText caption in captions)
            {
                switch (caption.Side)
                {
                    case 0: // Top
                        spriteBatch.DrawString(Canvas.Instance.Font,caption.Message, new Vector2(shape.X + (shape.Width / 2), shape.Y), Color.Black);
                        break;
                    case 1: // Right
                        spriteBatch.DrawString(Canvas.Instance.Font,caption.Message, new Vector2(shape.X + (shape.Width), shape.Y + (shape.Height / 2)), Color.Black);
                        break;
                    case 2: // Bottom
                        spriteBatch.DrawString(Canvas.Instance.Font,caption.Message, new Vector2(shape.X + (shape.Width / 2), shape.Y + shape.Height), Color.Black);
                        break;
                    case 3: // Left
                        spriteBatch.DrawString(Canvas.Instance.Font,caption.Message, new Vector2(shape.X, shape.Y + (shape.Height / 2)), Color.Black);
                        break;
                }
            }
        }
    }
}