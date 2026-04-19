using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Castle {

        private Image Image; //Castle's image
        private TeamName TeamName; //Castle's team
        private Dictionary <string, Diamond> Diamonds; //Castle's diamonds
        private Door OutsideDoor; //Castle's outside door
        private Door InsideDoor; //Castle's inside door
        private Location CurrentLocation; //Castle's current location
        private Image InsideWall; //Castle's inside wall
        private Image InsideFloor; //Castle's inside floor

        /// <summary>
        /// Receives team and coordinates
        /// and creates a castle.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Castle(TeamName teamName, int x, int y) {
            TeamName = teamName;
            Image = new Image("map/castles/teams/" + teamName + "/outside", "castle", x, y, 1400, 431, Color.White);
            OutsideDoor = new Door(x + 639, y + 288, 120, 120, teamName, Location.Outside);
            InsideWall = new Image("map/castles/teams/" + teamName + "/inside/castle_wall", 0, y, 1400, 431);
            InsideDoor = new Door(614, 303, 188, 107, teamName, teamName == TeamName.Red ? Location.Inside_Red_Castle : Location.Inside_Yellow_Castle);
            InsideFloor = new Image("map/castles/teams/" + teamName + "/inside/floor/castle_floor", 0, CastleBridge.Graphics.PreferredBackBufferHeight / 2, CastleBridge.Graphics.PreferredBackBufferWidth, CastleBridge.Graphics.PreferredBackBufferHeight);

            Diamonds = new Dictionary<string, Diamond>();

            //Initializes diamonds:
            InitDiamonds();

            CurrentLocation = Location.Outside;
        }

        /// <summary>
        /// Receives coordinates and creates diamond
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddDiamond(int x, int y, string key) {

            switch (TeamName) {
                case TeamName.Red:
                    Diamonds.Add(key, new Diamond(TeamName, x, y, Location.Inside_Red_Castle, key));
                    break;
                case TeamName.Yellow:
                    Diamonds.Add(key, new Diamond(TeamName, x, y, Location.Inside_Yellow_Castle, key));
                    break;
            }
        }

        /// <summary>
        /// Initializes diamonds
        /// </summary>
        private void InitDiamonds() {

            for (int i = 1; i <= 3; i++) {
                string key = "diamond#" + TeamName + "#" + i;
                AddDiamond(100, InsideFloor.GetRectangle().Top - 10 + (i * 100), key);
            }
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public void Update() {

            //Update diamonds:
            foreach (KeyValuePair<string, Diamond> diamond in Diamonds)
                diamond.Value.Update();
        }

        /// <summary>
        /// Get team
        /// </summary>
        /// <returns></returns>
        public TeamName GetTeam() {
            return TeamName;
        }

        /// <summary>
        /// Get diamonds
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Diamond> GetDiamonds() {
            return Diamonds;
        }

        /// <summary>
        /// Get outside door
        /// </summary>
        /// <returns></returns>
        public Door GetOutsideDoor() {
            return OutsideDoor;
        }

        /// <summary>
        /// Get inside door
        /// </summary>
        /// <returns></returns>
        public Door GetInsideDoor() {
            return InsideDoor;
        }

        /// <summary>
        /// Get current location
        /// </summary>
        /// <returns></returns>
        public Location GetCurrentLocation() {
            return CurrentLocation;
        }

        /// <summary>
        /// Change to a new location
        /// </summary>
        /// <param name="newLocation"></param>
        public void ChangeLocationTo(Location newLocation) {
            CurrentLocation = newLocation;
        }

        /// <summary>
        /// Draw outside castle's stuff
        /// </summary>
        public void DrawOutside() {
            Image.Draw();
            OutsideDoor.Draw();
        }

        /// <summary>
        /// Draw inside castle's stuff
        /// </summary>
        public void DrawInside() {
            InsideFloor.Draw();
            InsideWall.Draw();
            InsideDoor.Draw();
        }
    }
}
