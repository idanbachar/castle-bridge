using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class EnergyBall {

        public int ShootTime; //Energy ball's shoot time
        public bool IsFinished; //Energy ball's finished cast indication
        public Animation Animation; //Energy ball's animation
        private int Speed; //Energy ball's speed
        private Direction Direction; //Energy ball's direction
        private Direction CastUpDownDirection; //Energy ball's cast up direction
        private Location CurrentLocation; //Energy ball's current location
        private int Damage; //Energy ball's damage
        private Player Owner; //Energy ball's player owner

        /// <summary>
        /// Receives starting coordinates positions, direction, cast up/down direction, cast location, and player owner
        /// and creates an energy ball
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="direction"></param>
        /// <param name="castUpDownDirection"></param>
        /// <param name="location"></param>
        /// <param name="owner"></param>
        public EnergyBall(int startX, int startY, Direction direction, Direction castUpDownDirection, Location location, Player owner) {

            ShootTime = 0;
            IsFinished = false;
            Speed = 20;
            Direction = direction;
            Animation = new Animation("player/characters/teams/red/mage/weapons/energy ball/energy_ball_", new Rectangle(startX, startY, 40, 40), 0, 0, 1, 3, false, false);
            
            //Set's animation direction:
            Animation.SetDirection(direction);
            
            CastUpDownDirection = castUpDownDirection;
            CurrentLocation = location;
            Damage = 30;
            Owner = owner;
        }

        /// <summary>
        /// Moves energy ball after being shot
        /// </summary>
        public void Move() {
            if (ShootTime < 35) {
                ShootTime++;

                switch (Direction) {
                    case Direction.Right:

                        if (CastUpDownDirection == Direction.Down) {
                            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X + Speed,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Y + Speed / 10,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Height);
                        }
                        else if(CastUpDownDirection == Direction.Up) {
                            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X + Speed,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Y - Speed / 10,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Height);
                        }
                        break;
                    case Direction.Left:

                        if (CastUpDownDirection == Direction.Down) {
                            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X - Speed,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Y + Speed / 10,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                                      Animation.GetCurrentSpriteImage().GetRectangle().Height);
                        }
                        else if (CastUpDownDirection == Direction.Up) {
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
                Animation.SetRotation(Direction == Direction.Right ? 0.7f : -0.7f);
            }
        }

        /// <summary>
        /// Get current location
        /// </summary>
        /// <returns></returns>
        public Location GetCurrentLocation() {
            return CurrentLocation;
        }

        /// <summary>
        /// Get damage
        /// </summary>
        /// <returns></returns>
        public int GetDamage() {
            return Damage;
        }

        /// <summary>
        /// Get player owner
        /// </summary>
        /// <returns></returns>
        public Player GetOwner() {
            return Owner;
        }

        /// <summary>
        /// Get Animation
        /// </summary>
        /// <returns></returns>
        public Animation GetAnimation() {
            return Animation;
        }

        /// <summary>
        /// Draw energy ball
        /// </summary>
        public void Draw() {
            Animation.Draw();
        }

    }
}
