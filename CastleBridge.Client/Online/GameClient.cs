using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CastleBridge.OnlineLibraries;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CastleBridge.Client {
    public class GameClient {

        private TcpClient Client; //Client

        //Get the player from GameScreen event:
        public delegate Player GetThePlayer();
        public event GetThePlayer OnGetThePlayer;

        //Get the red team's players from GameScreen event:
        public delegate Dictionary<string, Player> GetRedPlayers();
        public event GetRedPlayers OnGetRedPlayers;

        //Get the yellow team's players from GameScreen event:
        public delegate Dictionary<string, Player> GetYellowPlayers();
        public event GetYellowPlayers OnGetYellowPlayers;

        //Start join game session from GameScreen event:
        public delegate void JoinPlayer(CharacterName character, TeamName team, string name);
        public event JoinPlayer OnJoinPlayer;

        //Add popup from GameScreen event:
        public delegate void AddPopup(Popup popup, bool isTile);
        public event AddPopup OnAddPopup;

        //Get both teams from GameScreen event:
        public delegate Dictionary<TeamName, Team> GetTeams();
        public event GetTeams OnGetTeams;

        //Start game on finished loading from GameScreen event:
        public delegate void FinishedLoading();
        public event FinishedLoading OnFinishedLoading;

        //Update loading data from server in percents from CastleBridge event:
        public delegate void UpdateLoadingPercent(int current, int max);
        public event UpdateLoadingPercent OnUpdateLoadingPercent;

        //Add entity to world entites from GameScreen event:
        public delegate void AddEntity(MapEntityName entityName, int x, int y, Direction direction, float rotation, Location location, bool isActive, string key);
        public event AddEntity OnAddEntity;

        //Remove world entity with his key from GameScreen event:
        public delegate void RemoveMapEntity(string key);
        public event RemoveMapEntity OnRemoveMapEntity;

        //Connection lost event when can't connect to the server host:
        public delegate void ConnectionLost();
        public event ConnectionLost OnConnectionLost;

        //Max number of entities to load from server:
        private int MaxEntitiesToLoad;

        //Max number of current entities that are being loaded:
        private int CurrentEntitiesLoaded;

        //Thread sleep time:
        private const int ThreadSleep = 100;

        private bool IsConnectedToServer;
        
        /// <summary>
        /// Creates a game client
        /// </summary>
        public GameClient() {

            Client = new TcpClient();
            IsConnectedToServer = false;
            MaxEntitiesToLoad = 0;
            CurrentEntitiesLoaded = 0;
        }

        /// <summary>
        /// Receives ip, port
        /// and tries to connect to server host
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(string ip, int port) {

            try {

                //Try to connect server host:
                Client.Connect(ip, port);

                IsConnectedToServer = true;

                //Only if connected successfully to server host:
                if (IsConnectedToServer) {

                    //Sends only 1 time connected player's packet to server:
                    SendAllPlayerData(false);

                    //Start receiving data from server:
                    StartReceivingDataFromServer();
                }
            }
            catch (Exception) {

                IsConnectedToServer = false;

                //Start connection lost event when can't connect to the server host:
                OnConnectionLost();
            }
        }

        /// <summary>
        /// Receives a text string
        /// and sends it to the server
        /// </summary>
        /// <param name="text"></param>
        public void SendText(string text) {

            NetworkStream netStream = Client.GetStream();
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            netStream.Write(bytes, 0, bytes.Length);

            Thread.Sleep(ThreadSleep);
        }

        /// <summary>
        /// Start Sending player's packet data to server thread
        /// </summary>
        /// <param name="isFinishedLoad"></param>
        public void StartSendingPlayerData(bool isFinishedLoad) {

            if (IsConnectedToServer)
                new Thread(() => SendAllPlayerData(isFinishedLoad)).Start();
        }

        /// <summary>
        /// Start Receiving data from server thread
        /// </summary>
        public void StartReceivingDataFromServer() {

            if (IsConnectedToServer)
                new Thread(ReceiveDataFromServer).Start();
        }

        /// <summary>
        /// Receives an is finished load indication, when it's false,
        /// sends player's packet data only once, when it's true, send player's packet in infinite loop thread
        /// </summary>
        /// <param name="isFinishedLoad"></param>
        private void SendAllPlayerData(bool isFinishedLoad) {

            //Run always:
            while (true) {

                //Get player from GameScreen event:
                Player player = OnGetThePlayer();

                //Create a new player packet:
                PlayerPacket playerPacket = new PlayerPacket();

                //Sets all player packet vars to player's vars:
                playerPacket.Name = player.GetName().ToString();
                playerPacket.CharacterName = player.CurrentCharacter.GetName().ToString();
                playerPacket.TeamName = player.GetTeamName().ToString();
                playerPacket.Rectangle = new RectanglePacket(player.GetRectangle().X, player.GetRectangle().Y, player.GetRectangle().Width, player.GetRectangle().Height);
                playerPacket.PacketType = PacketType.PlayerData;
                playerPacket.Direction = player.GetDirection().ToString();
                playerPacket.PlayerState = player.GetState().ToString();
                playerPacket.CurrentLocation = player.GetCurrentLocation().ToString();
                playerPacket.CurrentCharacterHp = player.GetCurrentCharacter().GetHealth();
                playerPacket.IsAttackAnimationFinished = player.GetCurrentAnimation().IsFinished;
                playerPacket.CurrentCharacterIsDead = player.CurrentCharacter.IsDead;
                playerPacket.IsHorseOwner = player.GetCurrentHorse() != null;
                playerPacket.IsAllMapEntitiesLoaded = isFinishedLoad;
                playerPacket.CarryingDiamondsCount = player.GetCurrentCarryingDiamonds();

                //Try to convert player packet's object into array of bytes and send it to server:
                try {

                    byte[] bytes = ObjectToByteArray(playerPacket);
                    NetworkStream netStream = Client.GetStream();
                    netStream.Write(bytes, 0, bytes.Length);

                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

                if (!isFinishedLoad)
                    break;

                //Start thread sleep:
                Thread.Sleep(ThreadSleep);
            }
        }

        /// <summary>
        /// Receives diamond of player and sends it to server host
        /// </summary>
        /// <param name="takenDiamond"></param>
        public void SendDiamondChangesToServer(Diamond takenDiamond) {

            //Create a new diamond packet:
            DiamondPacket diamondPacket = new DiamondPacket(takenDiamond.GetImage().GetRectangle().X, takenDiamond.GetImage().GetRectangle().Y, takenDiamond.GetTeam().ToString(), takenDiamond.GetKey());

            diamondPacket.CurrentLocation = takenDiamond.GetCurrentLocation().ToString();
            diamondPacket.Visible = takenDiamond.GetVisible();
            diamondPacket.X = takenDiamond.GetRectangle().X;
            diamondPacket.Y = takenDiamond.GetRectangle().Y;
            diamondPacket.Width = takenDiamond.GetRectangle().Width;
            diamondPacket.Height = takenDiamond.GetRectangle().Height;
            diamondPacket.Owner = takenDiamond.GetOwnerName();

            //Sets all diamond packet vars to diamond's vars:

            //Try to convert player packet's object into array of bytes and send it to server:
            try {
                    
                byte[] bytes = ObjectToByteArray(diamondPacket);
                NetworkStream netStream = Client.GetStream();
                netStream.Write(bytes, 0, bytes.Length);

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            Thread.Sleep(ThreadSleep);

        }
 
        /// <summary>
        /// Receives data from server
        /// </summary>
        private void ReceiveDataFromServer() {

            //Run allways:
            while (true) {

                //Try to read stream data from server host:
                try {

                    NetworkStream netStream = Client.GetStream();
                    byte[] bytes = new byte[1024];
                    netStream.Read(bytes, 0, bytes.Length);
                    object obj = null;

                    //Try to convert received array of bytes data into packet's object:
                    try {
                        obj = ByteArrayToObject(bytes);

                        //Checks if received obj is type of player packet:
                        if (obj is PlayerPacket) {

                            PlayerPacket playerPacket = obj as PlayerPacket;

                            //Checks player packet's type:
                            switch (playerPacket.PacketType) {

                                //If packet type is player data:
                                case PacketType.PlayerData:

                                    //Sets all player packet vars into vars:
                                    TeamName team = (TeamName)Enum.Parse(typeof(TeamName), playerPacket.TeamName);
                                    CharacterName character = (CharacterName)Enum.Parse(typeof(CharacterName), playerPacket.CharacterName);
                                    Direction direction = (Direction)Enum.Parse(typeof(Direction), playerPacket.Direction);
                                    PlayerState playerState = (PlayerState)Enum.Parse(typeof(PlayerState), playerPacket.PlayerState);
                                    Rectangle rectangle = new Rectangle(playerPacket.Rectangle.X, playerPacket.Rectangle.Y, playerPacket.Rectangle.Width, playerPacket.Rectangle.Height);
                                    Location currentLocation = (Location)Enum.Parse(typeof(Location), playerPacket.CurrentLocation);
                                    int currentCharacterHp = playerPacket.CurrentCharacterHp;
                                    string name = playerPacket.Name;
                                    bool isAttackAnimationFinished = playerPacket.IsAttackAnimationFinished;
                                    bool currentCharacterIsDead = playerPacket.CurrentCharacterIsDead;
                                    bool isHorseOwner = playerPacket.IsHorseOwner;
                                    int currentCarryingDiamonds = playerPacket.CarryingDiamondsCount;

                                    //Checks if this is the first time of receiving current player's data (never connected before):
                                    if (!OnGetRedPlayers().ContainsKey(playerPacket.Name) && !OnGetYellowPlayers().ContainsKey(playerPacket.Name)) {
                                        
                                        //Start joining session from game screen event (Add new player to the correct team):
                                        OnJoinPlayer(character, team, name);

                                        //Add popup:
                                        OnAddPopup(new Popup(name + " has joined to the " + team + " team!", CastleBridge.Graphics.PreferredBackBufferWidth / 2 + 280, CastleBridge.Graphics.PreferredBackBufferHeight - 100, Color.Red, Color.Black, false), false);
                                    }
                                    else { //If this is not the first time receving current player's data (connected before):

                                        //Checks for his team:
                                        switch (team) {

                                            //If he is on red team:
                                            case TeamName.Red:

                                                //Using lock function to avoid multi task crash:
                                                lock (OnGetRedPlayers()) {

                                                    //Get current red player from GameScreen event and sets all received player packet's vars into current red player's vars:
                                                    OnGetRedPlayers()[name].ChangeTeam(team);
                                                    OnGetRedPlayers()[name].ChangeCharacter(character);
                                                    OnGetRedPlayers()[name].SetRectangle(rectangle);
                                                    OnGetRedPlayers()[name].SetDirection(direction);
                                                    OnGetRedPlayers()[name].SetState(playerState);
                                                    OnGetRedPlayers()[name].ChangeLocationTo(currentLocation);
                                                    OnGetRedPlayers()[name].GetCurrentCharacter().SetHealth(currentCharacterHp);
                                                    OnGetRedPlayers()[name].GetCurrentAnimation().IsFinished = isAttackAnimationFinished;
                                                    OnGetRedPlayers()[name].CurrentCharacter.IsDead = currentCharacterIsDead;
                                                    OnGetRedPlayers()[name].SetCurrentCarryingDiamonds(currentCarryingDiamonds);

                                                    if (isHorseOwner) {
                                                        Horse currentHorse = OnGetTeams()[team].GetHorse();
                                                        OnGetRedPlayers()[name].MountHorse(currentHorse);
                                                    }
                                                    else 
                                                        OnGetRedPlayers()[name].DismountHorse();
                                                }
                                                break;

                                            //If he is on yellow team:
                                            case TeamName.Yellow:

                                                //Using lock function to avoid multi task crash:
                                                lock (OnGetYellowPlayers()) {

                                                    //Get current yellow player from GameScreen event and sets all received player packet's vars into current yellow player's vars:
                                                    OnGetYellowPlayers()[name].ChangeTeam(team);
                                                    OnGetYellowPlayers()[name].ChangeCharacter(character);
                                                    OnGetYellowPlayers()[name].SetRectangle(rectangle);
                                                    OnGetYellowPlayers()[name].SetDirection(direction);
                                                    OnGetYellowPlayers()[name].SetState(playerState);
                                                    OnGetYellowPlayers()[name].ChangeLocationTo(currentLocation);
                                                    OnGetYellowPlayers()[name].GetCurrentCharacter().SetHealth(currentCharacterHp);
                                                    OnGetYellowPlayers()[name].GetCurrentAnimation().IsFinished = isAttackAnimationFinished;
                                                    OnGetYellowPlayers()[name].CurrentCharacter.IsDead = currentCharacterIsDead;
                                                    OnGetYellowPlayers()[name].SetCurrentCarryingDiamonds(currentCarryingDiamonds);

                                                    if (isHorseOwner) {
                                                        Horse currentHorse = OnGetTeams()[team].GetHorse();
                                                        OnGetYellowPlayers()[name].MountHorse(currentHorse);
                                                    }
                                                    else
                                                        OnGetYellowPlayers()[name].DismountHorse();
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        //Checks if received obj is type of map entity packet:
                        else if (obj is MapEntityPacket) {

                            MapEntityPacket MapEntityPacket = obj as MapEntityPacket;

                            //Sets all map entity packet vars into vars:
                            MapEntityName entityName = (MapEntityName)Enum.Parse(typeof(MapEntityName), MapEntityPacket.Name);
                            int entityX = MapEntityPacket.X;
                            int entityY = MapEntityPacket.Y;
                            Direction entityDirection = (Direction)Enum.Parse(typeof(Direction), MapEntityPacket.Direction);
                            Location entityLocation = (Location)Enum.Parse(typeof(Location), MapEntityPacket.CurrentLocation);
                            string key = MapEntityPacket.Key;
                            bool isActive = MapEntityPacket.IsActive;

                            //Update loading data from server in percents from CastleBridge event:
                            OnUpdateLoadingPercent(++CurrentEntitiesLoaded, MaxEntitiesToLoad);

                            //Add entity to world entites from GameScreen event:
                            OnAddEntity(entityName, entityX, entityY, entityDirection, 0f, entityLocation, isActive, key);
                        }
                        //Checks if received obj is type of diamond packet:
                        else if (obj is DiamondPacket) {

                            DiamondPacket DiamondPacket = obj as DiamondPacket;

                            //Sets all diamond packet vars into vars:
                            TeamName diamondTeam = (TeamName)Enum.Parse(typeof(TeamName), DiamondPacket.TeamName);
                            int diamondX = DiamondPacket.X;
                            int diamondY = DiamondPacket.Y;
                            int diamondWidth = DiamondPacket.Width;
                            int diamondHeight = DiamondPacket.Height;
                            Location diamondLocation = (Location)Enum.Parse(typeof(Location), DiamondPacket.CurrentLocation);
                            string key = DiamondPacket.Key;
                            bool visible = DiamondPacket.Visible;
                            string owner = DiamondPacket.Owner;

                            //Get current diamond from GameScreen event and sets all received diamond packet's vars into current diamond vars:
                            OnGetTeams()[diamondTeam].GetCastle().GetDiamonds()[key].SetVisible(visible);
                            OnGetTeams()[diamondTeam].GetCastle().GetDiamonds()[key].SetTeam(diamondTeam);
                            OnGetTeams()[diamondTeam].GetCastle().GetDiamonds()[key].SetRectangle(new Rectangle(diamondX, diamondY, diamondWidth, diamondHeight));
                            OnGetTeams()[diamondTeam].GetCastle().GetDiamonds()[key].SetOwner(owner);
                        }
                    }
                    //Checks if failed to convert received array of bytes into an object, because of the received data is string:
                    catch(Exception) {

                        //Get data and convert it from array of bytes into string:
                        string data = Encoding.ASCII.GetString(bytes).Split('\0')[0];

                        //Checkes if data is 'Completed Map Entities' command:
                        if (data.Equals("Completed Map Entities")) {

                            //Start game on finished loading from GameScreen event:
                            OnFinishedLoading();

                            //Start Sending player's packet data to server thread:
                            StartSendingPlayerData(true);
                        }
                        //Checks if data contains 'map_entities_count..' command:
                        else if (data.IndexOf("map_entities_count") != -1) {

                            //Get max entities to load:
                            MaxEntitiesToLoad = int.Parse(data.Split('|')[0]);
                        }
                        //Checks if data contains 'Remove Entity..' command:
                        else if (data.IndexOf("Remove Entity") != -1) {

                            //Get entity's key:
                            string key = data.Split('_')[1];

                            //Remove world entity with his key from GameScreen event:
                            OnRemoveMapEntity(key);
                        }
                    }

                }catch(Exception e) {
                    Console.WriteLine(e.Message);

                }

                //Start thread sleep:
                Thread.Sleep(ThreadSleep);
            }
        }
 
        /// <summary>
        /// Receives an object
        /// and returns it as array of bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] ObjectToByteArray<T>(T obj) {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Receives array of bytes
        /// and returns it as an object
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public object ByteArrayToObject(byte[] arrBytes) {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }

        /// <summary>
        /// Get client
        /// </summary>
        /// <returns></returns>
        public TcpClient GetClient() {
            return Client;
        }
    }
}
