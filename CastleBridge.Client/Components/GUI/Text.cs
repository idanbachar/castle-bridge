using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Text {

        private SpriteFont Font; //Text's font
        private Color Color; //Text's color
        private bool IsBackground; //Text's is background indication
        private string Value; //Text's value
        private Vector2 Position; //Text's position
        private bool Visible; //Text's visible
        private int Width; //Text's width
        private int Height; //Text's height
        private Image Background; //Text's background
        private Color BackgroundColor; //Text's background color

        /// <summary>
        /// Receives font type, text, text position, font color, is background indication, background color
        /// and creates text
        /// </summary>
        /// <param name="fontType"></param>
        /// <param name="textValue"></param>
        /// <param name="textPosition"></param>
        /// <param name="fontColor"></param>
        /// <param name="isBackground"></param>
        /// <param name="backgroundColor"></param>
        public Text(FontType fontType, string textValue, Vector2 textPosition, Color fontColor, bool isBackground, Color backgroundColor) {

            LoadFont(fontType);
            Width = 0;
            Height = 0;
            Background = new Image("colors", "empty", 0, 0, 0, 0, backgroundColor);
            Position = new Vector2(textPosition.X, textPosition.Y);
            Color = fontColor;
            IsBackground = isBackground;
            Visible = true;
            BackgroundColor = backgroundColor;
            ChangeText(textValue);
        }

        /// <summary>
        /// Receives font type and loads font
        /// </summary>
        /// <param name="fontType"></param>
        private void LoadFont(FontType fontType) {

            try {
                Font = CastleBridge.PublicContent.Load<SpriteFont>("fonts/" + fontType);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Font = CastleBridge.PublicContent.Load<SpriteFont>("fonts/" + FontType.Default);
            }
        }

        /// <summary>
        /// Receives a new text value and applies it
        /// </summary>
        /// <param name="newTextValue"></param>
        public void ChangeText(string newTextValue) {

            Value = newTextValue;
            Width = (int)Font.MeasureString(Value).X;
            Height = (int)Font.MeasureString(Value).Y;
            Background.SetRectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        /// <summary>
        /// Receives a visible value
        /// and applies it
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value) {
            Visible = value;
        }

        /// <summary>
        /// Receives a new position and applies it
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetPosition(Vector2 newPosition) {

            Position.X = newPosition.X;
            Position.Y = newPosition.Y;
            Width = (int)Font.MeasureString(Value).X;
            Height = (int)Font.MeasureString(Value).Y;
            Background.SetRectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        /// <summary>
        /// Get position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition() {
            return Position;
        }

        /// <summary>
        /// Get background image
        /// </summary>
        /// <returns></returns>
        public Image GetBackgroundImage() {
            return Background;
        }

        /// <summary>
        /// Get color
        /// </summary>
        /// <returns></returns>
        public Color GetColor() {
            return Color;
        }

        /// <summary>
        /// Receives a new color and applies it
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color) {
            Color = color;
        }

        /// <summary>
        /// Receives background visible value
        /// and applies it
        /// </summary>
        /// <param name="value"></param>
        public void SetBackgroundVisibility(bool value) {
            IsBackground = value;
        }

        /// <summary>
        /// Get background color
        /// </summary>
        /// <returns></returns>
        public Color GetBackgroundColor() {
            return BackgroundColor;
        }

        /// <summary>
        /// Get text length
        /// </summary>
        /// <returns></returns>
        public int GetLength() {
            return Value.Length;
        }

        /// <summary>
        /// Get text's value
        /// </summary>
        /// <returns></returns>
        public string GetValue() {
            return Value;
        }

        /// <summary>
        /// Draw text
        /// </summary>
        public void Draw() {

            //Draw text only if visible is true
            if (Visible) {

                //Draw background only if is background indication is true
                if (IsBackground)
                    Background.Draw();
 
                //Draw text:
                CastleBridge.SpriteBatch.DrawString(Font, Value, Position, Color);
            }
        }
    
    }
}
