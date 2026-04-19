using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Cloud {

        private Animation Animation; //Cloud's animation
        private bool IsOnDestination; //Clouds is on destination indication
        private Random Rnd;
        private int Speed; //Cloud's moving speed
        public bool IsRain; //Cloud's is raining indication
        private List<RainDrop> RainDrops; //Cloud's rain drops
        private int GenerateRainDropTimer; //Cloud's generate rain drop timer
        private int ScreenWidth; //Screen width

        /// <summary>
        /// Receuves coordinates, size, is raining indication, screen width
        /// and creates a cloud
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isRain"></param>
        /// <param name="screenWidth"></param>
        public Cloud(int x, int y, int width, int height, bool isRain, int screenWidth) {
            Animation = new Animation("map/clouds/cloud_", new Rectangle(x, y, width, height), 0, 1, 2, 15, true, true);
            Animation.Start();
            IsOnDestination = false;
            Speed = 1;
            Rnd = new Random();
            IsRain = isRain;
            RainDrops = new List<RainDrop>();
            GenerateRainDropTimer = 0;
            ScreenWidth = screenWidth;
        }

        /// <summary>
        /// Update cloud
        /// </summary>
        public void Update() {

            Animation.Play();

            //Checks if cloud's coordinates at destination:
            if (Animation.GetCurrentSpriteImage().GetRectangle().Right < 0)
                IsOnDestination = true;

            //Checks if cloud's coordinates not at destination:
            if (!IsOnDestination) {

                //Move cloud:
                Move();
            }
            else { //If cloud's coordinates in destination:

                //Reset position:
                ResetPosition();

                IsOnDestination = false;
            }

            //Checks if is raining:
            if (IsRain) {

                //Generate rain drops:
                if (GenerateRainDropTimer < 20)
                    GenerateRainDropTimer++;
                else {
                    GenerateRainDropTimer = 0;
                    GenerateRainDrop();
                }

                //Make rain drops fall:
                for(int i = 0; i < RainDrops.Count; i++) {
                    if (!RainDrops[i].IsFinished)
                        RainDrops[i].Fall();
                    else
                        RainDrops.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Get current animation
        /// </summary>
        /// <returns></returns>
        public Animation GetCurrentAnimation() {
            return Animation;
        }

        /// <summary>
        /// Get rain drops
        /// </summary>
        /// <returns></returns>
        public List<RainDrop> GetRainDrops() {
            return RainDrops;
        }

        /// <summary>
        /// Generates rain drop
        /// </summary>
        private void GenerateRainDrop() {
            RainDrops.Add(new RainDrop(Rnd.Next(Animation.GetCurrentSpriteImage().GetRectangle().Left, Animation.GetCurrentSpriteImage().GetRectangle().Right), Animation.GetCurrentSpriteImage().GetRectangle().Bottom));
        }

        /// <summary>
        /// Resets cloud position
        /// </summary>
        public void ResetPosition() {
            Animation.SetRectangle(ScreenWidth + 100, Rnd.Next(0, 200), 125, 75);
        }

        /// <summary>
        /// Move cloud
        /// </summary>
        public void Move() {
            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X - Speed,
                                   Animation.GetCurrentSpriteImage().GetRectangle().Y,
                                   Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                   Animation.GetCurrentSpriteImage().GetRectangle().Height);
        }

        /// <summary>
        /// Draw cloud
        /// </summary>
        public void Draw() {

            //Draw cloud:
            Animation.Draw();
        }
    }
}
