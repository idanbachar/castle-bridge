using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Image {

        private Texture2D Texture; //Image's texture
        private Rectangle Rectangle; //Image's rectangle
        private Color Color; //Image's Color
        private bool Visible; //Image's visible
        private string FullPath; //Image's full path
        private Direction Direction; //Image's direction
        private float Rotation; //Image's rotation

        /// <summary>
        /// Receives folder path, image name, coordinates, size, image color
        /// and creates an image
        /// </summary>
        /// <param name="imageFolderPath"></param>
        /// <param name="imageName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imageColor"></param>
        public Image(string imageFolderPath, string imageName, int x, int y, int width, int height, Color imageColor) {

            FullPath = imageFolderPath.Length == 0 ? imageName : (imageFolderPath.Replace("_", " ") + "/" + imageName);
            LoadImage(FullPath);
            Rectangle = new Rectangle(x, y, width, height);
            Color = imageColor;
            Direction = Direction.Left;
            Visible = true;
            Rotation = 0f;
        }

        /// <summary>
        /// Receives folder path, coordinates, size
        /// and creates an image
        /// </summary>
        /// <param name="imageFolderFullPath"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Image(string imageFolderFullPath, int x, int y, int width, int height) {

            LoadImage(imageFolderFullPath);
            Rectangle = new Rectangle(x, y, width, height);
            Color = Color.White;
            Direction = Direction.Left;
            Visible = true;
            Rotation = 0f;
        }

        /// <summary>
        /// Receives full folder path (folder path + image's name)
        /// and loads image
        /// </summary>
        /// <param name="fullPath"></param>
        private void LoadImage(string fullPath) {

            try {
                Texture = CastleBridge.PublicContent.Load<Texture2D>("images/" + fullPath);
            }catch(Exception e) {
                Console.WriteLine(e.Message);
                Texture = CastleBridge.PublicContent.Load<Texture2D>("images/undefined");
            }
        }

        /// <summary>
        /// Receives full folder path (folder path + image's name)
        /// and changes image
        /// </summary>
        /// <param name="fullPath"></param>
        public void ChangeImage(string fullPath) {

            LoadImage(fullPath);
        }

        /// <summary>
        /// Receives new direction and applies it
        /// </summary>
        /// <param name="newDirection"></param>
        public void SetDirection(Direction newDirection) {
            Direction = newDirection;
        }

        /// <summary>
        /// Get direction
        /// </summary>
        /// <returns></returns>
        public Direction GetDirection() {
            return Direction;
        }

        /// <summary>
        /// Receives coordinates and sets new rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetRectangle(int x, int y, int width, int height) {

            Rectangle.X = x;
            Rectangle.Y = y;
            Rectangle.Width = width;
            Rectangle.Height = height;
        }

        /// <summary>
        /// Get rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle() {
            return Rectangle;
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
        /// Get visible value
        /// </summary>
        /// <returns></returns>
        public bool GetVisible() {
            return Visible;
        }

        /// <summary>
        /// Receives a rotation
        /// and applies it
        /// </summary>
        /// <param name="rotation"></param>
        public void SetRotation(float rotation) {
            Rotation = rotation;
        }

        /// <summary>
        /// Get rotation
        /// </summary>
        /// <returns></returns>
        public float GetRotation() {
            return Rotation;
        }

        /// <summary>
        /// Get full path
        /// </summary>
        /// <returns></returns>
        public string GetFullPath() {
            return FullPath;
        }

        /// <summary>
        /// Receives color and applies it
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color) {
            Color = color;
        }

        /// <summary>
        /// Draw image
        /// </summary>
        public void Draw() {

            //Draw image only if visible is true
            if (Visible) {

                //Draw image by direction (none/horizontally)
                switch (Direction) {
                    case Direction.Right:
                        CastleBridge.SpriteBatch.Draw(Texture, Rectangle, null, Color, Rotation, new Vector2(), SpriteEffects.FlipHorizontally, 1);
                        break;
                    case Direction.Left:
                        CastleBridge.SpriteBatch.Draw(Texture, Rectangle, null, Color, Rotation, new Vector2(), SpriteEffects.None, 1);
                        break;
                }
                
            }
        }
    }
}
