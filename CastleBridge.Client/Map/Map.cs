using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Map {

        private MapName Name; //Map's name
        public static int WIDTH; //Map's width
        public static int HEIGHT; //Map's height
        private Image Grass; //Map's grass
        private Weather Weather; //Map's weather
        private Dictionary<string, MapEntity> WorldEntities; //Map's world entities
        private Dictionary<TeamName, Team> Teams; //Map's teams

        public Map() {
            Name = MapName.Forest;
            WIDTH = 10000;
            HEIGHT = 2000;
            WorldEntities = new Dictionary<string, MapEntity>();

            //Initialize:
            Init();
        }

        /// <summary>
        /// Initializes map
        /// </summary>
        private void Init() {

            InitGrass();
            InitWeather();
            InitBackgroundWorldEntities();
            InitTeams();
        }

        /// <summary>
        /// Init teams
        /// </summary>
        private void InitTeams() {

            Teams = new Dictionary<TeamName, Team>();
            Teams.Add(TeamName.Red, new Team(TeamName.Red, Grass.GetRectangle()));
            Teams.Add(TeamName.Yellow, new Team(TeamName.Yellow, Grass.GetRectangle()));
        }

        /// <summary>
        /// Initializes grass
        /// </summary>
        private void InitGrass() {
            Grass = new Image("map/" + Name, "grass", 0, HEIGHT / 5, WIDTH, HEIGHT, Color.White);
        }

        /// <summary>
        /// Initializes weather
        /// </summary>
        private void InitWeather() {

            Weather = new Weather(TimeType.Day, true, WIDTH, 50);
        }

        /// <summary>
        /// Initializes background entities like trees and ground leavs
        /// </summary>
        private void InitBackgroundWorldEntities() {

            for (int i = 0; i < 60; i++) {
                string key = "Tree_" + i;
                WorldEntities.Add(key, new MapEntity(MapEntityName.Tree, MapName.Forest, (i * 200) + 25, Grass.GetRectangle().Top - 250, 200, 250, false, Direction.Left, 0f, Location.Outside, true, key));
            }
            for (int i = 0; i < 156; i++) {
                string key = "Ground_Leaves_" + i;
                WorldEntities.Add("Ground_Leaves_" + i, new MapEntity(MapEntityName.Ground_Leaves, MapName.Forest, (i * 65), Grass.GetRectangle().Top - 60, 75, 75, false, Direction.Left, 0f, Location.Outside, true, key));
            }
        }

        /// <summary>
        /// Update's map stuff like weather/teams's horses
        /// </summary>
        public void Update() {

            //Update weather:
            Weather.Update();

            //Update teams:
            foreach (KeyValuePair<TeamName, Team> team in Teams) {

                //Update team:
                team.Value.Update();
            }
        }
 
        /// <summary>
        /// Receives a string key and removes an entity with it
        /// </summary>
        /// <param name="key"></param>
        public void RemoveMapEntity(string key) {
            if (WorldEntities.ContainsKey(key)) {
                lock (WorldEntities) {
                    WorldEntities.Remove(key);
                }
            }
        }

        /// <summary>
        /// Receives character type name, team, and name
        /// and adds the online player to current team
        /// </summary>
        /// <param name="character"></param>
        /// <param name="team"></param>
        /// <param name="name"></param>
        public void AddPlayer(CharacterName character, TeamName team, string name) {
            Teams[team].AddPlayer(character, team, name);
        }

        /// <summary>
        /// Receives entity name, coordinates, direction, rotation, location, active indication, and key
        /// and adds an entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        /// <param name="rotation"></param>
        /// <param name="location"></param>
        /// <param name="isActive"></param>
        /// <param name="key"></param>
        public void AddEntity(MapEntityName entityName, int x, int y, Direction direction, float rotation, Location location, bool isActive, string key) {

            int width = 0;
            int height = 0;

            bool isTouchable = false;

            if (direction != Direction.Left && direction != Direction.Right)
                direction = Direction.Left;


            switch (entityName) {
                case MapEntityName.Bush:
                    width = 100;
                    height = 45;
                    isTouchable = false;
                    break;
                case MapEntityName.Red_Flower:
                    width = 36;
                    height = 36;
                    isTouchable = true;
                    break;
                case MapEntityName.Tree:
                    width = 400;
                    height = 500;
                    isTouchable = false;
                    break;
                case MapEntityName.Stone:
                    width = 50;
                    height = 50;
                    isTouchable = true;
                    break;
                case MapEntityName.Arrow:
                    width = 44;
                    height = 21;
                    isTouchable = true;
                    break;
            }

            lock (WorldEntities) {
                WorldEntities.Add(key, new MapEntity(entityName, Name, x, y + height, width, height, isTouchable, direction, rotation, location, isActive, key));
            }
        }

        /// <summary>
        /// Receives a new location and applies it
        /// </summary>
        /// <param name="newLocation"></param>
        public void UpdateLocationsTo(Location newLocation) {

            //Update castles inside/out locations by new location received:
            foreach (KeyValuePair<TeamName, Team> team in Teams)
                team.Value.GetCastle().ChangeLocationTo(newLocation);

            //Updates map size by new location received:
            switch (newLocation) {
                case Location.Outside:
                    WIDTH = 10000;
                    HEIGHT = 2000;
                    break;
                case Location.Inside_Red_Castle:
                case Location.Inside_Yellow_Castle:
                    WIDTH = CastleBridge.Graphics.PreferredBackBufferWidth;
                    HEIGHT = CastleBridge.Graphics.PreferredBackBufferHeight;
                    break;
            }
        }

        /// <summary>
        /// Get Grass
        /// </summary>
        /// <returns></returns>
        public Image GetGrass() {
            return Grass;
        }

        /// <summary>
        /// Get world entities
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, MapEntity> GetWorldEntities() {
            return WorldEntities;
        }

        /// <summary>
        /// Get teams
        /// </summary>
        /// <returns></returns>
        public Dictionary<TeamName, Team> GetTeams() {
            return Teams;
        }


        /// <summary>
        /// Get weather
        /// </summary>
        /// <returns></returns>
        public Weather GetWeather() {
            return Weather;
        }

        /// <summary>
        /// Receives player's location and draws castles by it
        /// </summary>
        /// <param name="playerLocation"></param>
        public void DrawCastles(Location playerLocation) {
            foreach (KeyValuePair<TeamName, Team> team in Teams)
                if (team.Value.GetCastle().GetCurrentLocation() == playerLocation || team.Value.GetCastle().GetCurrentLocation() == Location.All) {
                    switch (team.Value.GetName()) {
                        case TeamName.Red:
                            if (playerLocation == Location.Inside_Red_Castle)
                                team.Value.GetCastle().DrawInside();
                            else if (playerLocation == Location.Outside)
                                team.Value.GetCastle().DrawOutside();
                            break;
                        case TeamName.Yellow:
                            if (playerLocation == Location.Inside_Yellow_Castle)
                                team.Value.GetCastle().DrawInside();
                            else if (playerLocation == Location.Outside)
                                team.Value.GetCastle().DrawOutside();
                            break;
                    }
                }
        }

        /// <summary>
        /// Receives i (layer pos), and player's location 
        /// and draws map as a tile
        /// </summary>
        /// <param name="i"></param>
        /// <param name="playerLocation"></param>
        public void DrawTile(int i, Location playerLocation) {

            //Uses 'try' to avoid crashes due world entities dictionary changes during an online session.
            try {
                foreach (KeyValuePair<string, MapEntity> mapEntity in WorldEntities) {
                    if (mapEntity.Value.GetAnimation().GetCurrentSpriteImage().GetRectangle().Bottom == i)
                        if (mapEntity.Value.GetCurrentLocation() == playerLocation || mapEntity.Value.GetCurrentLocation() == Location.All)
                            mapEntity.Value.Draw();
                }
            }
            catch (Exception) {

            }

            //Run on all teams (Red/Yellow):
            foreach (KeyValuePair<TeamName, Team> team in Teams) {

                //Draw all online players of each team:
                foreach (KeyValuePair<string, Player> player in team.Value.GetPlayers()) {
                    if (player.Value.GetCurrentAnimation().GetCurrentSpriteImage().GetRectangle().Bottom - 10 == i)
                        if (player.Value.GetCurrentLocation() == playerLocation || player.Value.GetCurrentLocation() == Location.All)
                            player.Value.Draw();

                    //Checks if current online player's character is archer, then draw his arrows if being shot:
                    if (player.Value.CurrentCharacter is Archer) {
                        Archer archer = player.Value.CurrentCharacter as Archer;
                        foreach (Arrow arrow in archer.GetArrows()) {
                            if (arrow.GetAnimation().GetCurrentSpriteImage().GetRectangle().Bottom == i)
                                if (arrow.GetCurrentLocation() == playerLocation || arrow.GetCurrentLocation() == Location.All)
                                    arrow.Draw();
                        }
                    }//Checks if current online player's character is mage, then draw his spells if being cast:
                    else if (player.Value.CurrentCharacter is Mage) {
                        Mage mage = player.Value.CurrentCharacter as Mage;
                        foreach (EnergyBall energyBall in mage.GetSpells()) {
                            if (energyBall.GetAnimation().GetCurrentSpriteImage().GetRectangle().Bottom == i)
                                if (energyBall.GetCurrentLocation() == playerLocation || energyBall.GetCurrentLocation() == Location.All)
                                    energyBall.Draw();
                        }
                    }
                }

                //Draw diamonds of each team:
                foreach (KeyValuePair<string, Diamond> diamond in team.Value.GetCastle().GetDiamonds()) {
                    if (diamond.Value.GetImage().GetRectangle().Bottom == i)
                        if (diamond.Value.GetCurrentLocation() == playerLocation || diamond.Value.GetCurrentLocation() == Location.All)
                            diamond.Value.Draw();
                }

                //Draw horses of eache team:
                if (team.Value.GetHorse().GetCurrentAnimation().GetCurrentSpriteImage().GetRectangle().Bottom - 10 == i) {
                    if (team.Value.GetHorse().GetCurrentLocation() == playerLocation || team.Value.GetHorse().GetCurrentLocation() == Location.All)
                        team.Value.GetHorse().Draw();
                }                
            }
        }
    }
}
