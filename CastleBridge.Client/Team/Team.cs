using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Team {

        private TeamName Name; //Team's name
        private Castle Castle; //Team's Castle
        private Dictionary<string, Player> Players; //Team's players
        private Rectangle MapDimensionRectangle; //Map's dimensions
        private int MaxPlayers; //Max players each team
        private int MinPlayers; //Min players each team
        private Horse Horse; //Team's horse

        /// <summary>
        /// Receives team name and map dimensions
        /// and creates a team.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="mapDimensionRectangle"></param>
        public Team(TeamName teamName, Rectangle mapDimensionRectangle) {
            Name = teamName;
            Players = new Dictionary<string, Player>();
            MaxPlayers = 6;
            MinPlayers = 2;
            MapDimensionRectangle = mapDimensionRectangle;

            //Init castle:
            InitCastle(mapDimensionRectangle);
            
            //Init horse:
            InitHorse();
        }

        /// <summary>
        /// Initialize horse
        /// </summary>
        private void InitHorse() {

            switch (Name) {

                case TeamName.Red:
                    Horse = new Horse(Name, 600, 600, 400, 300);
                    Horse.SetDirection(Direction.Right);
                    break;
                case TeamName.Yellow:
                    Horse = new Horse(Name, MapDimensionRectangle.Width - 1200, 600, 400, 300);
                    Horse.SetDirection(Direction.Left);
                    break;
            }
        }

        /// <summary>
        /// Initialize castle
        /// </summary>
        /// <param name="mapDimensionRectangle"></param>
        private void InitCastle(Rectangle mapDimensionRectangle) {

            switch (Name) {
                case TeamName.Red:
                    Castle = new Castle(Name, 300, mapDimensionRectangle.Top - 400);
                    break;
                case TeamName.Yellow:
                    Castle = new Castle(Name, mapDimensionRectangle.Width - 1700, mapDimensionRectangle.Top - 400);
                    break;
            }
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public void Update() {

            //Update castle:
            Castle.Update();

            //Update horse:
            Horse.Update();
        }

        /// <summary>
        /// Add online player to team by selected character, team, name.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="team"></param>
        /// <param name="name"></param>
        public void AddPlayer(CharacterName character, TeamName team, string name) {

            //Creates player:
            Player player = new Player(character, team, name, MapDimensionRectangle);
            
            //Respawn player:
            player.Respawn();

            //Add player to players list using lock function to avoid multi task crash:
            lock (Players) {
                Players.Add(name, player);
            }
        }

        /// <summary>
        /// Get players dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Player> GetPlayers() {
            return Players;
        }

        /// <summary>
        /// Get team name
        /// </summary>
        /// <returns></returns>
        public TeamName GetName() {
            return Name;
        }

        /// <summary>
        /// Get Castle
        /// </summary>
        /// <returns></returns>
        public Castle GetCastle() {
            return Castle;
        }

        /// <summary>
        /// Get max players
        /// </summary>
        /// <returns></returns>
        public int GetMaxPlayers() {
            return MaxPlayers;
        }

        /// <summary>
        /// Get min players
        /// </summary>
        /// <returns></returns>
        public int GetMinPlayers() {
            return MinPlayers;
        }

        /// <summary>
        /// Get horse
        /// </summary>
        /// <returns></returns>
        public Horse GetHorse() {
            return Horse;
        }
    }
}
