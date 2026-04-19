using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Arrow {

        private int ShootTime; //Arrow's shoot time
        public bool IsFinished; //Arrow's finished shoot indication
        private Animation Animation; //Arrow's animation
        private int Speed; //Arrow's speed
        private Direction Direction; //Arrow's direction
        private Direction ShootUpDownDirection; //Arrow's shoot up direction
        private Location CurrentLocation; //Arrow's current location
        private int Damage; //Arrow's damage
        private Player Owner; //Arrow's player owner

        /// <summary>
        /// Receives starting coordinates positions, direction, shoot up/down direction, shoot location, and player owner
        /// and creates an arrow
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="direction"></param>
        /// <param name="shootUpDownDirection"></param>
        /// <param name="location"></param>
        /// <param name="owner"></param>
        public Arrow(int startX, int startY, Direction direction, Direction shootUpDownDirection, Location location, Player owner) {

            ShootTime = 0;
            IsFinished = false;
            Speed = 20;
            Direction = direction;
            Animation = new Animation("player/characters/teams/red/archer/weapons/arrow/arrow_", new Rectangle(startX, startY, 44, 21), 0, 0, 1, 3, false, false);
            
            //Set's animation direction:
            Animation.SetDirection(direction);
            
            ShootUpDownDirection = shootUpDownDirection;
            CurrentLocation = location;
            Damage = 30;
            Owner = owner;
        }

        /// <summary>
        /// Moves arrow after being shot
        /// </summary>
        public void Move() {
            if (ShootTime < 35) {
                ShootTime++;

                switch (Direction) {
                    case Direction.Right:

                        if (ShootUpDownDirection == Direction.Down) {
                            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X + Speed,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Y + Speed / 10,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Height);
                        }
                        else if(ShootUpDownDirection == Direction.Up) {
                            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X + Speed,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Y - Speed / 10,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Height);
                        }
                        break;
                    case Direction.Left:

                        if (ShootUpDownDirection == Direction.Down) {
                            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X - Speed,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Y + Speed / 10,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Height);
                        }
                        else if (ShootUpDownDirection == Direction.Up) {
                            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X - Speed,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Y - Speed / 10,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Height);
                        }
                        break;
                }

            }
            else {
                ShootTime = 0;
                IsFinished = true;
            }
        }

        /// <summary>
        /// Get direction
        /// </summary>
        /// <returns></returns>
        public Direction GetDirection() {
            return Direction;
        }

        /// <summary>
        /// Get Animation
        /// </summary>
        /// <returns></returns>
        public Animation GetAnimation() {
            return Animation;
        }

        /// <summary>
        /// Get speed
        /// </summary>
        /// <returns></returns>
        public int GetSpeed() {
            return Speed;
        }

        /// <summary>
        /// Receives a new speed and sets it
        /// </summary>
        /// <param name="speed"></param>
        public void SetSpeed(int speed) {
            Speed = speed;
        }

        /// <summary>
        /// Get damage
        /// </summary>
        /// <returns></returns>
        public int GetDamage() {
            return Damage;
        }

        /// <summary>
        /// Get current location
        /// </summary>
        /// <returns></returns>
        public Location GetCurrentLocation() {
            return CurrentLocation;
        }

        /// <summary>
        /// Get player owner
        /// </summary>
        /// <returns></returns>
        public Player GetOwner() {
            return Owner;
        }

        /// <summary>
        /// Draw arrow
        /// </summary>
        public void Draw() {
            Animation.Draw();
        }

    }
}
