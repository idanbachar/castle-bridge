using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Button {

        private Image DefaultImage; //Button's default image
        private Image OverImage; //Button's over image
        private Image CurrentImage; //Button's current image
        private Text Text; //Button's text
        public bool IsClicked; //Button's is clicked indication
        private bool IsPressedLeftButton; //Button's is pressed left button
        public int PressedCounts; //Button's pressed counts
        public int MaxPresses; //Button's maximum presses

        /// <summary>
        /// Receives default image, over image, text, text color
        /// and creates button
        /// </summary>
        /// <param name="defaultImage"></param>
        /// <param name="overImage"></param>
        /// <param name="text"></param>
        /// <param name="textColor"></param>
        public Button(Image defaultImage, Image overImage, string text, Color textColor) {

            DefaultImage = defaultImage;
            OverImage = overImage;
            CurrentImage = defaultImage;
            Text = new Text(FontType.Default, text, new Vector2(CurrentImage.GetRectangle().Left + 5, CurrentImage.GetRectangle().Top + 5), textColor, false, Color.Black);
            IsClicked = false;
            IsPressedLeftButton = false;
            PressedCounts = 0;
            MaxPresses = 1;
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public void Update() {

            //Update mouse over features:
            IsMouseOver();

            //Check if released pressing left button:
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                IsPressedLeftButton = false;
        }

        /// <summary>
        /// Checks mouse over button
        /// </summary>
        private bool IsMouseOver() {

            Rectangle mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 10, 10);

            if (mouseRectangle.Intersects(CurrentImage.GetRectangle())) {

                //Replace current button's image to over image:
                CurrentImage = OverImage;

                //Sets current button's color:
                CurrentImage.SetColor(Color.WhiteSmoke);

                return true;
            }
            else {

                //Checks pressed counts:
                if (PressedCounts < MaxPresses) {

                    //Replace current button's image to default:
                    CurrentImage = DefaultImage;

                    //Sets current button's color:
                    CurrentImage.SetColor(Color.White);
                }
            }

            return false;
        }


        /// <summary>
        /// Checks if clicking on button
        /// </summary>
        /// <returns></returns>
        public bool IsClicking() {

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !IsPressedLeftButton) {

                IsPressedLeftButton = true;

                if (IsMouseOver()) {

                    if (PressedCounts < MaxPresses)
                        PressedCounts++;
                    else {
                        Reset();
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Reset button's stuff
        /// </summary>
        public void Reset() {

            IsClicked = false;
            CurrentImage = DefaultImage;
            PressedCounts = 0;
        }

        /// <summary>
        /// Get current button's image
        /// </summary>
        /// <returns></returns>
        public Image GetCurrentImage() {
            return CurrentImage;
        }

        /// <summary>
        /// Draw button
        /// </summary>
        public void Draw() {

            //Draw current button's image:
            CurrentImage.Draw();

            //Draw text:
            Text.Draw();
        }
    }
}
