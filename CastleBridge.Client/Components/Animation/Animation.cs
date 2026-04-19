using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Animation {

        private Image[] Sprites; //Animation's sprites
        private int StartAnimationFromIndex; //Animation's start index
        private int EndAnimationInIndex; //Animation's end index
        private int CurrentFrame; //Animation's current frame
        private bool IsPlaying; //Animation's is playing indication
        private bool IsLoop; //Animation's is loop indication
        private int NextFrameDelay; //Animation's next frame delay
        private int NextFrameDelayTimer; //Animation's next frame delay timer
        private Direction Direction; //Animation's direction
        private bool IsReverse; //Animation's is reverse animation indication
        private AnimationState AnimationState; //Animation's state (forward/backward)
        public bool IsFinished; //Animation's is finished indication
        private Rectangle Rectangle; //Animation's rectangle

        /// <summary>
        /// Receives full path, rectangle, animation's start index, animation's end index, sprites length, next frame delay, is reverse indication, is loop indication
        /// and creates an animation
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="rectangle"></param>
        /// <param name="startAnimationFromIndex"></param>
        /// <param name="endAnimationInIndex"></param>
        /// <param name="spritesLength"></param>
        /// <param name="nextFrameDelay"></param>
        /// <param name="isReverse"></param>
        /// <param name="isLoop"></param>
        public Animation(string fullPath, Rectangle rectangle, int startAnimationFromIndex, int endAnimationInIndex, int spritesLength, int nextFrameDelay, bool isReverse, bool isLoop) {

            Sprites = new Image[spritesLength];
            StartAnimationFromIndex = startAnimationFromIndex;
            EndAnimationInIndex = endAnimationInIndex;
            CurrentFrame = StartAnimationFromIndex;
            IsPlaying = false;
            IsLoop = isLoop;
            NextFrameDelay = nextFrameDelay;
            NextFrameDelayTimer = 0;
            Direction = Direction.Right;
            IsReverse = isReverse;
            AnimationState = AnimationState.Forward;
            IsFinished = false;
            Rectangle = rectangle;

            //Load sprites:
            LoadSprites(fullPath);
        }

        /// <summary>
        /// Receives coordinates, size 
        /// and sets rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetRectangle(int x, int y, int width, int height) {

            //Sets rectangle for each sprite:
            foreach (Image sprite in Sprites)
                sprite.SetRectangle(x, y, width, height);
        }

        /// <summary>
        /// Receives color
        /// and applies it
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color) {

            //Sets color for each sprite:
            foreach (Image sprite in Sprites)
                sprite.SetColor(color);
        }

        /// <summary>
        /// Receives visible value
        /// and applies it
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value) {

            //Sets visible for each sprite:
            foreach (Image sprite in Sprites)
                sprite.SetVisible(value);
        }

        /// <summary>
        /// Receives direction
        /// and applies it
        /// </summary>
        /// <param name="direction"></param>
        public void SetDirection(Direction direction) {

            Direction = direction;

            //Sets direction for each sprite:
            foreach (Image sprite in Sprites)
                sprite.SetDirection(direction);
        }

        /// <summary>
        /// Receives rotation
        /// and applies it
        /// </summary>
        /// <param name="rotation"></param>
        public void SetRotation(float rotation) {

            //Sets direction for each sprite:
            foreach (Image sprite in Sprites)
                sprite.SetRotation(rotation);
        }

        /// <summary>
        /// Receives is reverse indication value
        /// and applies it
        /// </summary>
        /// <param name="value"></param>
        public void SetReverse(bool value) {
            IsReverse = value;
        }

        /// <summary>
        /// Receives full path and
        /// loads sprites
        /// </summary>
        /// <param name="fullPath"></param>
        private void LoadSprites(string fullPath) {

            for (int i = 0; i < Sprites.Length; i++) {

                Sprites[i] = new Image(fullPath + i,
                                        Rectangle.X,
                                        Rectangle.Y,
                                        Rectangle.Width,
                                        Rectangle.Height);
            }
        }

        /// <summary>
        /// Run animation in loop
        /// </summary>
        private void RunInLoop() {

            if (NextFrameDelayTimer < NextFrameDelay) {
                NextFrameDelayTimer++;
            }
            else {

                //Run animation if there is no reverse:
                if (!IsReverse) {

                    if (CurrentFrame < EndAnimationInIndex - 1)
                        CurrentFrame++;
                    else {
                        CurrentFrame = StartAnimationFromIndex;
                    }

                } //Run animation if there is reverse:
                else if (IsReverse) {

                    //Checks if animation's state is backword:
                    if (AnimationState == AnimationState.Backward) {

                        //Checks if animation isn't finished to run:
                        if (CurrentFrame > StartAnimationFromIndex)
                            CurrentFrame--;
                        else { //Checks if animation is finished to run:
                            CurrentFrame = StartAnimationFromIndex;
                            AnimationState = AnimationState.Forward; //Sets animation's state to forward
                        }
                    }//Checks if animation's state is forward:
                    else if (AnimationState == AnimationState.Forward) {
                        //Checks if animation isn't finished to run:
                        if (CurrentFrame < EndAnimationInIndex - 1)
                            CurrentFrame++;
                        else {//Checks if animation is finished to run:
                            CurrentFrame = EndAnimationInIndex - 1;
                            AnimationState = AnimationState.Backward; //Sets animation's state to backward
                        }
                    }
                }

                NextFrameDelayTimer = 0;
            }
        }

        private void RunInOneTime() {

            if (NextFrameDelayTimer < NextFrameDelay) {
                NextFrameDelayTimer++;
            }
            else {

                //Run animation if there is no reverse:
                if (!IsReverse) {

                    if (CurrentFrame < EndAnimationInIndex - 1)
                        CurrentFrame++;
                    else {
                        CurrentFrame = StartAnimationFromIndex;
                        IsPlaying = false;
                        IsFinished = true;
                    }

                } //Run animation if there is reverse:
                else if (IsReverse) {

                    //Checks if animation's state is forward:
                    if (AnimationState == AnimationState.Forward) {

                        //Checks if animation isn't finished to run:
                        if (CurrentFrame < EndAnimationInIndex - 1)
                            CurrentFrame++;
                        else {
                            //Checks if animation is finished to run:
                            CurrentFrame = EndAnimationInIndex - 1;
                            AnimationState = AnimationState.Backward;  //Sets animation's state to backward
                        }
                    }
                    //Checks if animation's state is backword:
                    else if (AnimationState == AnimationState.Backward) {

                        //Checks if animation isn't finished to run:
                        if (CurrentFrame > StartAnimationFromIndex)
                            CurrentFrame--;
                        else {
                            //Checks if animation is finished to run:
                            CurrentFrame = StartAnimationFromIndex;
                            IsPlaying = false;
                            IsFinished = true;
                            AnimationState = AnimationState.Forward;  //Sets animation's state to forward
                        }
                    }
                }

                NextFrameDelayTimer = 0;
            }
        }

        /// <summary>
        /// Play animation
        /// </summary>
        public void Play() {

            //Play only if is playing indication is true:
            if (IsPlaying) {

                //Play loop only if is loop indication is true:
                if (IsLoop) {
                    RunInLoop();
                }//else play only in one time:
                else if (!IsLoop) {
                    RunInOneTime();
                }
            }
        }

        /// <summary>
        /// Start animation
        /// </summary>
        public void Start() {
            IsPlaying = true;
        }

        /// <summary>
        /// Stop animation
        /// </summary>
        public void Stop() {
            IsPlaying = false;
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset() {
            IsFinished = false;
            IsPlaying = false;
        }

        /// <summary>
        /// Get current sprite image:
        /// </summary>
        /// <returns></returns>
        public Image GetCurrentSpriteImage() {
            return Sprites[CurrentFrame];
        }

        /// <summary>
        /// Draw current frame
        /// </summary>
        public void Draw() {

            //Draw current frame:
            GetCurrentSpriteImage().Draw();
        }
    }
}
