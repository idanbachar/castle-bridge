using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class MapEntity {

        private MapEntityName Name; //Entity's name
        private Animation Animation; //Entity's animation
        private Text Tooltip; //Entity's tooltip
        public bool IsTouchable; //Entity's touchable indication
        private Direction Direction; //Entity's direction
        private Location CurrentLocation; //Entity's location
        public bool IsActive; //Entity's active indication
        private string Key; //Entity's unique key name

        /// <summary>
        /// Receives entity's name, map, coordinates, size, touchable indication, direction, rotation, location, active indication, unique key
        /// and creates an entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="mapName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isTouchable"></param>
        /// <param name="direction"></param>
        /// <param name="rotation"></param>
        /// <param name="location"></param>
        /// <param name="isActive"></param>
        /// <param name="key"></param>
        public MapEntity(MapEntityName entityName, MapName mapName, int x, int y, int width, int height, bool isTouchable, Direction direction, float rotation, Location location, bool isActive, string key) {
            
            Name = entityName;
            Direction = direction;
            IsTouchable = isTouchable;
            Animation = new Animation("map/" + mapName + "/" + entityName.ToString().Replace("_", " ") + "/" + entityName + "_", new Rectangle(x, y, width, height), 0, 0, 1, 5, true, true);
            
            //Sets direction:
            Animation.SetDirection(direction);
            
            //Sets rotation:
            Animation.SetRotation(rotation);
            
            IsActive = isActive;
            Key = key;

            Tooltip = new Text(FontType.Default, string.Empty, new Vector2(x + 50, y - 65), Color.Gold, true, Color.Black);
            Tooltip.SetVisible(false);

            switch (entityName) {
                case MapEntityName.Red_Flower:
                    Tooltip.ChangeText("Press 'E' to eat" +
                        "\n" +
                        "(+15) Hp.");
                    break;
                case MapEntityName.Stone:
                    Tooltip.ChangeText("Press 'E' to take" +
                        "\n" +
                        "(+1) Stone.");
                    break;
                case MapEntityName.Tree:
                    Tooltip.ChangeText("Press 'E' to cut" +
                        "\n" +
                        "(+5) Woods.");
                    break;
                case MapEntityName.Arrow:
                    Tooltip.ChangeText("Press 'E' to take" +
                        "\n" +
                        "(+1) Arrow.");
                    break;
            }

            CurrentLocation = location;
        }

        /// <summary>
        /// Update animation
        /// </summary>
        public void Update() {
            Animation.Play();
        }

        /// <summary>
        /// Get name
        /// </summary>
        /// <returns></returns>
        public MapEntityName GetName() {
            return Name;
        }

        /// <summary>
        /// Receives new rectangle and applies it
        /// </summary>
        /// <param name="newRectangle"></param>
        public void SetRectangle(Rectangle newRectangle) {
            Animation.SetRectangle(newRectangle.X, newRectangle.Y, newRectangle.Width, newRectangle.Height);
        }

        /// <summary>
        /// Get animation
        /// </summary>
        /// <returns></returns>
        public Animation GetAnimation() {
            return Animation;
        }

        /// <summary>
        /// Get direction
        /// </summary>
        /// <returns></returns>
        public Direction GetDirection() {
            return Direction;
        }

        /// <summary>
        /// Get tooltip
        /// </summary>
        /// <returns></returns>
        public Text GetTooltip() {
            return Tooltip;
        }

        /// <summary>
        /// Get location
        /// </summary>
        /// <returns></returns>
        public Location GetCurrentLocation() {
            return CurrentLocation;
        }

        /// <summary>
        /// Receives a new location
        /// and applies it
        /// </summary>
        /// <param name="newLocation"></param>
        public void ChangeLocationTo(Location newLocation) {
            CurrentLocation = newLocation;
        }

        /// <summary>
        /// Get unique key
        /// </summary>
        /// <returns></returns>
        public string GetKey() {
            return Key;
        }

        /// <summary>
        /// Draw entity
        /// </summary>
        public void Draw() {

            //Draw entity:
            Animation.Draw();

            //Draw entity's tooltip:
            Tooltip.Draw();
        }
    }
}
