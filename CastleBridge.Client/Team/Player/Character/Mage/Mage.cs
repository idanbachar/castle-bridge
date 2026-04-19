using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Mage: Character {

        private List<EnergyBall> Spells; //Mage's spells
        public int MaxSpells; //Mage's max spells
        public int CurrentSpells; //Mage's current spells capacity

        /// <summary>
        /// Receives character's type name, team, coordinates and size
        /// and creates a Mage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="teamName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Mage(CharacterName name, TeamName teamName, int x, int y, int width, int height) : base(name, teamName, x, y, width, height) {

            Spells = new List<EnergyBall>();
            MaxSpells = 5;
            CurrentSpells = MaxSpells;
            Health = 100;
        }

        /// <summary>
        /// Receives shoot direction, current shooting location, and player owner
        /// and start casting
        /// </summary>
        /// <param name="shootDirection"></param>
        /// <param name="currentLocation"></param>
        /// <param name="owner"></param>
        public void CastSpell(Direction shootDirection, Location currentLocation, Player owner) {

            Spells.Add(new EnergyBall(CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Left + CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Width / 2,
                                    CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Top + CurrentAnimation.GetCurrentSpriteImage().GetRectangle().Height / 2, Direction, shootDirection, currentLocation, owner));

            CurrentSpells--;
        }

        /// <summary>
        /// Returns true if there are spells to shoot, else returns false
        /// </summary>
        /// <returns></returns>
        public bool IsCanCast() {
            return CurrentSpells > 0;
        }

        /// <summary>
        /// Add 1 spell
        /// </summary>
        public void AddSpell() {
            CurrentSpells++;
        }

        /// <summary>
        /// Get spells
        /// </summary>
        /// <returns></returns>
        public List<EnergyBall> GetSpells() {
            return Spells;
        }

    }
}
