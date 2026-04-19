using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Knight: Character {

        private int AttackDamage; //Knight's attack damage

        /// <summary>
        /// Receives character's type name, team, coordinates and size
        /// and creates a Knight
        /// </summary>
        /// <param name="name"></param>
        /// <param name="teamName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Knight(CharacterName name, TeamName teamName, int x, int y, int width, int height) : base(name, teamName, x, y, width, height) {
 
            Health = 100;
            AttackDamage = 15;
        }

        /// <summary>
        /// Get attack damage
        /// </summary>
        /// <returns></returns>
        public int GetAttackDamage() {
            return AttackDamage;
        }
 
    }
}
