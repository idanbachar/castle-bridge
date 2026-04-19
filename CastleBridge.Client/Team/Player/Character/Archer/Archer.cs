using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Archer: Character {

        private List<Arrow> Arrows; //Archer's arrows
        public int MaxArrows; //Archer's max arrows
        public int CurrentArrows; //Archer current arrows capacity

        /// <summary>
        /// Receives character's type name, team, coordinates and size
        /// and creates an Archer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="teamName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Archer(CharacterName name, TeamName teamName, int x, int y, int width, int height) : base(name, teamName, x, y, width, height) {

            Arrows = new List<Arrow>();
            MaxArrows = 5;
            CurrentArrows = MaxArrows;
            Health = 100;
        }

        /// <summary>
        /// Receives shoot direction, current shooting location, and player owner
        /// and start shooting
        /// </summary>
        /// <param name="shootDirection"></param>
        /// <param name="currentLocation"></param>
        /// <param name="owner"></param>
        public void ShootArrow(Direction shootDirection, Location currentLocation, Player owner) {

            Arrows.Add(new Arrow(CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Left + CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Width / 2,
                                    CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Top + CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Height / 2, Direction, shootDirection, currentLocation, owner));

            CurrentArrows--;
        }

        /// <summary>
        /// Returns true if there are arrows to shoot, else returns false
        /// </summary>
        /// <returns></returns>
        public bool IsCanShoot() {
            return CurrentArrows > 0;
        }

        /// <summary>
        /// Add 1 arrow
        /// </summary>
        public void AddArrow() {
            CurrentArrows++;
        }

        /// <summary>
        /// Get arrows
        /// </summary>
        /// <returns></returns>
        public List<Arrow> GetArrows() {
            return Arrows;
        }
 
    }
}
