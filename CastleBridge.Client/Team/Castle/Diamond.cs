using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Diamond {

        private Image Image; //Diamond's image
        private TeamName TeamName; //Diamon'ds team
        private string OwnerName; //Diamond's player owner
        private Location CurrentLocation; //Diamond's current location
        private Text Tooltip; //Diamond's tooltip
        private string Key; //Diamond's unique key

        /// <summary>
        /// Receives team, coordinates and location
        /// and creates a diamond
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="location"></param>
        public Diamond(TeamName teamName, int x, int y, Location location, string key) {
            TeamName = teamName;
            Image = new Image("map/castles/teams/" + TeamName + "/inside/diamond/diamond", x, y, 75, 75);
            OwnerName = string.Empty;
            CurrentLocation = location;
            Key = key;

            Tooltip = new Text(FontType.Default, "Press 'E' to steal\n(+1) Diamond.", new Vector2(x + 50, y - 65), Color.Gold, true, Color.Black);
            Tooltip.SetVisible(false);
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public void Update() {
            Tooltip.SetPosition(new Vector2(Image.GetRectangle().X + 50, Image.GetRectangle().Y - 65));
        }

        /// <summary>
        /// Get team
        /// </summary>
        /// <returns></returns>
        public TeamName GetTeam() {
            return TeamName;
        }

        /// <summary>
        /// Receives a team and applies it
        /// </summary>
        /// <param name="team"></param>
        public void SetTeam(TeamName team) {
            TeamName = team;
        }

        /// <summary>
        /// Receives a new rectangle and applies it
        /// </summary>
        /// <param name="newRectangle"></param>
        public void SetRectangle(Rectangle newRectangle) {
            Image.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
        }

        /// <summary>
        /// Get rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle() {
            return Image.GetRectangle();
        }

        /// <summary>
        /// Receives a visible value and applies it
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value) {
            Image.SetVisible(value);
        }

        /// <summary>
        /// Get visible value
        /// </summary>
        /// <returns></returns>
        public bool GetVisible() {
            return Image.GetVisible();
        }

        /// <summary>
        /// Get tooltip
        /// </summary>
        /// <returns></returns>
        public Text GetTooltip() {
            return Tooltip;
        }

        /// <summary>
        /// Receives a new location and applies it
        /// </summary>
        /// <param name="newLocation"></param>
        public void ChangeLocationTo(Location newLocation) {
            CurrentLocation = newLocation;
        }

        /// <summary>
        /// Get player owner
        /// </summary>
        /// <returns></returns>
        public string GetOwnerName() {
            return OwnerName;
        }

        /// <summary>
        /// Receives a player and applies it
        /// </summary>
        /// <param name="player"></param>
        public void SetOwner(string name) {
            OwnerName = name;
        }

        /// <summary>
        /// Removes player owner (sets owner to null)
        /// </summary>
        public void RemoveOwner() {
            OwnerName = string.Empty;
        }

        /// <summary>
        /// Get image
        /// </summary>
        /// <returns></returns>
        public Image GetImage() {
            return Image;
        }

        /// <summary>
        /// Get diamond's key
        /// </summary>
        /// <returns></returns>
        public string GetKey() {
            return Key;
        }

        /// <summary>
        /// Get current location
        /// </summary>
        /// <returns></returns>
        public Location GetCurrentLocation() {
            return CurrentLocation;
        }

        /// <summary>
        /// Draw diamond
        /// </summary>
        public void Draw() {

            //Draw diamond only if there is no owner:
            if (OwnerName == string.Empty)
                Image.Draw();

            //Draw diamond's tooltip:
            Tooltip.Draw();
        }

    }
}
