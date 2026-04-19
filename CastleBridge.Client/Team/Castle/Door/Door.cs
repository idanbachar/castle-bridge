using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Door {

        private Image Image; //Door's image
        private Text Tooltip; //Door's tooltip
        private Location CurrentLocation; //Door's location

        /// <summary>
        /// Receives coordinates, size, team, and location
        /// and creates a door
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="teamName"></param>
        /// <param name="location"></param>
        public Door(int x, int y, int width, int height, TeamName teamName, Location location) {

            Image = new Image("map/castles/teams/" + teamName + "/outside/door", "door", x, y, width, height, Color.White);
            Tooltip = new Text(FontType.Default, string.Empty, new Vector2(x , y - 170), Color.Gold, true, Color.Black);
            CurrentLocation = location;

            //Creates door's tooltip by current location:
            switch (CurrentLocation) {
                case Location.Inside_Red_Castle:
                case Location.Inside_Yellow_Castle:
                    Tooltip.ChangeText("Press 'E' to exit castle.");
                    break;
                case Location.Outside:
                    Tooltip.ChangeText("Press 'E' to enter castle.");
                    break;
            }
            
            //Sets visible to false:
            Tooltip.SetVisible(false);
        }
        
        /// <summary>
        /// Get tooltip
        /// </summary>
        /// <returns></returns>
        public Text GetTooltip() {
            return Tooltip;
        }

        /// <summary>
        /// Get image
        /// </summary>
        /// <returns></returns>
        public Image GetImage() {
            return Image;
        }

        /// <summary>
        /// Get location
        /// </summary>
        /// <returns></returns>
        public Location GetCurrentLocation() {
            return CurrentLocation;
        }

        /// <summary>
        /// Draw door
        /// </summary>
        public void Draw() {

            //Draw door:
            Image.Draw();

            //Draw door's tooltip:
            Tooltip.Draw();
        }
    
    }
}
