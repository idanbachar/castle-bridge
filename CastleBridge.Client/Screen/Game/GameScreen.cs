using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class GameScreen: Screen {

        private HUD HUD; //Hud
        private Player Player; //Player
        private Camera Camera; //Camera
        private Map Map; //Map
        private int GenerateXpTimer; //Generate xp timer
        private bool IsPressedD1;
        private bool IsPressedD2;
        private bool IsPressedD3;
        private bool IsPressedE;
        private bool IsPressedF;
        private bool IsPressedX;
        private GameClient GameClient; //Game client

        private Thread RespawnThread; //Respawn thread

        //Start game after loading online stuff event:
        public delegate void StartGameAfterLoading();
        public event StartGameAfterLoading OnStartGameAfterLoading;

        /// <summary>
        /// Game screen
        /// </summary>
        /// <param name="viewPort"></param>
        public GameScreen(Viewport viewPort) : base(viewPort) {
            Init(viewPort);
        }

        /// <summary>
        /// Initializes game stuff
        /// </summary>
        /// <param name="viewPort"></param>
        private void Init(Viewport viewPort) {

            GameClient = new GameClient();
            Camera = new Camera(viewPort);

            //Initializes map:
            InitMap();

            //Initializes hud:
            InitHUD();
        }

        /// <summary>
        /// Initializes map
        /// </summary>
        private void InitMap() {
            Map = new Map();
        }

        /// <summary>
        /// Initializes hud
        /// </summary>
        private void InitHUD() {

            HUD = new HUD();

            //Only if player is created, update hud stuff:
            if (Player != null)
                UpdateHud();
        }

        /// <summary>
        /// Checks for player respawn and starts respawn countdown if can
        /// </summary>
        private void CheckForPlayerRespawn() {

            //Start respawn timer only if player is dead and can respawn:
            if (Player.CanStartRespawnTimer) {

                //If not currently respawning:
                if (RespawnThread == null) {

                    //Start respawn thread:
                    RespawnThread = new Thread(RespawnTimer);
                    RespawnThread.Start();
                }
            }
        }

        /// <summary>
        /// Respawn timer countdown
        /// </summary>
        private void RespawnTimer() {

            int PlayerRespawnTimer = 5; //Player's respawn timer countdown (5 seconds)
            HUD.GetRespawnTimerLabel().SetVisible(true); //Sets respawn timer label visibility to true

            while (true) {

                //Change respawn timer label text to timer countdown:
                HUD.SetRespawnLabel("Respawn in " + PlayerRespawnTimer + " seconds.");

                //Handle timer:
                if (PlayerRespawnTimer > 0)
                    PlayerRespawnTimer--;
                else {
                    PlayerRespawnTimer = 5;
                    Player.Respawn();
                    HUD.Update();
                    HUD.GetRespawnTimerLabel().SetVisible(false);
                    break;
                }

                Thread.Sleep(1000);
            }


            RespawnThread = null;
        }

        /// <summary>
        /// Generate xp for player:
        /// </summary>
        private void GenerateXp() {
            
            if(GenerateXpTimer < 1000) {
                GenerateXpTimer++;
            }
            else {
                GenerateXpTimer = 0;
                Player.GetCurrentCharacter().AddXp(1);
                HUD.AddPopup(new Popup("+1xp", HUD.GetPlayerLevelBar().GetRectangle().Left + 3, HUD.GetPlayerLevelBar().GetRectangle().Top, Color.White, Color.Green), false);
                HUD.AddPlayerXp(1, Player.GetCurrentCharacter().GetMaxXp());
            }
        }

        /// <summary>
        /// Check player's movement
        /// </summary>
        private void CheckMovement() {

            //Checks if player is pressing 'D' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                Player.SetDirection(Direction.Right); //Change direction to right

                //Checks if player is riding horse:
                if (Player.GetCurrentHorse() != null) {

                    //Checks if player's horse is not in right side of the map:
                    if (!Player.GetCurrentHorse().IsOnRightMap()) {

                        //Start walking right:
                        Player.SetState(PlayerState.Walk);
                        Player.Move(Direction.Right);
                    }
                }
                else { //If player not riding horse:

                    //Checks if player is not in the right side of the map:
                    if (!Player.IsOnRightMap()) {

                        //Start walking right:
                        Player.SetState(PlayerState.Walk);
                        Player.Move(Direction.Right);
                    }
                }
            }

            //Checks if player is pressing 'A' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                Player.SetDirection(Direction.Left); //Change direction to left

                //Checks if player is riding horse:
                if (Player.GetCurrentHorse() != null) {

                    //Checks if player's horse is not in left side of the map:
                    if (!Player.GetCurrentHorse().IsOnLeftMap()) {

                        //Start walking left:
                        Player.SetState(PlayerState.Walk);
                        Player.Move(Direction.Left);
                    }
                }
                else { //If player not riding horse:

                    //Checks if player's horse is not in left side of the map:
                    if (!Player.IsOnLeftMap()) {

                        //Start walking left:
                        Player.SetState(PlayerState.Walk);
                        Player.Move(Direction.Left);
                    }
                }
            }

            //Checks if player is pressing 'W' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.W)) {

                //Checks if player is riding horse:
                if (Player.GetCurrentHorse() != null) {

                    //Checks if player's horse is not in top side of the map:
                    if (!Player.GetCurrentHorse().IsOnTopMap(Map)) {

                        //Start walking up:
                        Player.Move(Direction.Up);
                        Player.SetState(PlayerState.Walk);
                    }
                }
                else { //If player not riding horse:

                    //Checks if player's horse is not in top side of the map:
                    if (!Player.IsOnTopMap(Map)) {

                        //Start walking up:
                        Player.Move(Direction.Up);
                        Player.SetState(PlayerState.Walk);
                    }
                }
            }

            //Checks if player is pressing 'S' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.S)) {

                //Checks if player is riding horse:
                if (Player.GetCurrentHorse() != null) {

                    //Checks if player's horse is not in bottom side of the map:
                    if (!Player.GetCurrentHorse().IsOnBottomMap()) {

                        //Start walking down:
                        Player.Move(Direction.Down);
                        Player.SetState(PlayerState.Walk);
                    }
                }
                else { //If player not riding horse:

                    //Checks if player's horse is not in bottom side of the map:
                    if (!Player.IsOnBottomMap()) {

                        //Start walking down:
                        Player.Move(Direction.Down);
                        Player.SetState(PlayerState.Walk);
                    }
                }
            }
        }

        /// <summary>
        /// Check player's character changing
        /// </summary>
        private void CheckChangeCharacter() {

            //Checks if player is pressing 'D1' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.D1) && !IsPressedD1) {
                IsPressedD1 = true;

                //If current character is not an archer then change to archer:
                if (Player.CurrentCharacter.GetName() != CharacterName.Archer) {
                    Player.ChangeCharacter(CharacterName.Archer);

                    //Update hud:
                    UpdateHud();
                }
            }

            //Checks if player is pressing 'D2' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.D2) && !IsPressedD2) {
                IsPressedD2 = true;

                //If current character is not a knight then change to knight:
                if (Player.CurrentCharacter.GetName() != CharacterName.Knight) {
                    Player.ChangeCharacter(CharacterName.Knight);

                    //Update hud:
                    UpdateHud();
                }
            }

            //Checks if player is pressing 'D3' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.D3) && !IsPressedD3) {
                IsPressedD3 = true;

                //If current character is not a mage then change to mage:
                if (Player.CurrentCharacter.GetName() != CharacterName.Mage) {
                    Player.ChangeCharacter(CharacterName.Mage);

                    //Update hud:
                    UpdateHud();
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.D1))
                IsPressedD1 = false;

            if (Keyboard.GetState().IsKeyUp(Keys.D2))
                IsPressedD2 = false;

            if (Keyboard.GetState().IsKeyUp(Keys.D3))
                IsPressedD3 = false;
        }

        /// <summary>
        /// Check player's attacking
        /// </summary>
        private void CheckAttack() {

            //Checks if player is pressing 'Space' button on keyboard at the moment the player's state is not attacking:
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && Player.GetState() != PlayerState.Attack) {

                //Change player's state to attack:
                Player.SetState(PlayerState.Attack);
            }

            //Checks if player's current state is attack and at the moment the attack animation is finished:
            if (Player.GetState() == PlayerState.Attack && Player.CurrentCharacter.AttackAnimation.IsFinished) {

                //Reset attack animation:
                Player.CurrentCharacter.AttackAnimation.Reset();

                //Sets player's state to afk:
                Player.SetState(PlayerState.Afk);

                //Checks if current player's character is archer:
                if (Player.CurrentCharacter is Archer) {

                    Direction shootDirection = Direction.Down;
                    Archer archer = Player.CurrentCharacter as Archer;

                    //Checks if player is pressing 'W' button on keyboard:
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                        shootDirection = Direction.Up; //Move arrow to up direction

                    //Checks if player is pressing 'D3' button on keyboard:
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                        shootDirection = Direction.Down; //Move arrow to down direction

                    //If archer can shoot:
                    if (archer.IsCanShoot()) {

                        //Shoot arrow:
                        archer.GetCurrentAnimation().SetReverse(false);
                        archer.ShootArrow(shootDirection, Player.GetCurrentLocation(), Player);
                    }
                    else { //If archer can't shoot:

                        //Sets attack animation's state to reverse:
                        archer.GetCurrentAnimation().SetReverse(true);
                        HUD.AddPopup(new Popup("No arrows!", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.Red, Color.Black), true);
                    }

                    HUD.SetPlayerWeaponAmmo(archer.CurrentArrows + "/" + archer.MaxArrows);
                }
                //Checks if current player's character is mage:
                else if (Player.CurrentCharacter is Mage) {

                    Direction castDirection = Direction.Down;
                    Mage mage = Player.CurrentCharacter as Mage;

                    //Checks if player is pressing 'W' button on keyboard:
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                        castDirection = Direction.Up; //Move arrow to up direction

                    //Checks if player is pressing 'D3' button on keyboard:
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                        castDirection = Direction.Down; //Move arrow to down direction

                    //If mage can cast:
                    if (mage.IsCanCast()) {

                        //Cast spell:
                        mage.GetCurrentAnimation().SetReverse(false);
                        mage.CastSpell(castDirection, Player.GetCurrentLocation(), Player);
                    }
                    else {  //If mage can't cast:

                        //Sets attack animation's state to reverse:
                        mage.GetCurrentAnimation().SetReverse(true);
                        HUD.AddPopup(new Popup("No spells!", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.Red, Color.Black), true);
                    }

                    HUD.SetPlayerWeaponAmmo(mage.CurrentSpells + "/" + mage.MaxSpells);
                }

                //Sets player's state to afk:
                Player.SetState(PlayerState.Afk);
            }
        }

        /// <summary>
        /// Checks player's mounting horse
        /// </summary>
        private void CheckMountHorse() {

            //Run on each team:
            foreach (KeyValuePair<TeamName, Team> team in Map.GetTeams()) {

                //Checks if player is touching horse of each team and the horse doesn't have an player owner:
                if (Player.IsTouchHorse(team.Value.GetHorse()) && !team.Value.GetHorse().IsHasOwner()) {

                    //Sets horse's tooltip visibility to true:
                    team.Value.GetHorse().GetTooltip().SetVisible(true);

                    //Checks if player is pressing 'E' button on keyboard:
                    if (Keyboard.GetState().IsKeyDown(Keys.E) && !IsPressedE) {
                        IsPressedE = true;

                        //Mount horse:
                        Player.MountHorse(team.Value.GetHorse());

                        //Set hud's horse avatar to current player's team:
                        HUD.SetHorseAvatar(Player.GetTeamName());

                        //Set hud's horse avatar visibility to true;
                        HUD.GetHorseAvatar().SetVisible(true);
                        break;
                    }
                }
                else //else sets horse's tooltip visibility to false:
                    team.Value.GetHorse().GetTooltip().SetVisible(false);
            }

            //Checks if player is owner of horse
            if (Player.GetCurrentHorse() != null) {

                //sets player's horse's tooltip visibility to true (tooltip for dismount):
                Player.GetCurrentHorse().GetTooltip().SetVisible(true);

                //Sets player's horse's tooltip's position below horse:
                Player.GetCurrentHorse().GetTooltip().SetPosition(new Vector2(Player.GetCurrentHorse().GetRectangle().X + 50,
                                                                              Player.GetCurrentHorse().GetRectangle().Bottom - 20));


                //Checks if player is pressing 'F' button on keyboard:
                if (Keyboard.GetState().IsKeyDown(Keys.F) && !IsPressedF) {

                    IsPressedF = true;

                    //Dismound horse:
                    Player.DismountHorse();

                    //Set hud's horse's tooltip visibility to false:
                    HUD.GetHorseAvatar().SetVisible(false);
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.E)) {
                IsPressedE = false;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.F)) {
                IsPressedF = false;
            }
        }

        /// <summary>
        /// Checks player's defencing
        /// </summary>
        private void CheckDefence() {

            //Checks if player is pressing 'Left Control' button on keyboard at the moment the player's state is not defencing:
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Player.GetState() != PlayerState.Defence) {

                //Change player's state to defence:
                Player.SetState(PlayerState.Defence);
            }

            //Checks if player's current state is defence and at the moment the defence animation is finished:
            if (Player.GetState() == PlayerState.Defence && Player.CurrentCharacter.DefenceAnimation.IsFinished) {

                //Reset defence animation:
                Player.CurrentCharacter.DefenceAnimation.Reset();

                //Sets player's state to afk:
                Player.SetState(PlayerState.Afk);
            }
        }

        /// <summary>
        /// Checks player's stealing
        /// </summary>
        private void CheckSteal() {

            //Run on each team:
            foreach (KeyValuePair<TeamName, Team> team in Map.GetTeams()) {

                //Run on each team's castle's diamonds:
                foreach (KeyValuePair<string, Diamond> diamond in team.Value.GetCastle().GetDiamonds()) {

                    //Checks if player is touching current diamond:
                    if (Player.IsTouchDiamond(diamond.Value)) {

                        //Checks if player is pressing 'E' button on keyboard at the moment the player's state is not looting:
                        if (Keyboard.GetState().IsKeyDown(Keys.E) && Player.GetState() != PlayerState.Loot) {

                                //Change player's state to loot:
                                Player.SetState(PlayerState.Loot);

                                //Set player as diamond's owner:
                                diamond.Value.SetOwner(Player.GetName());

                                //Sets diamond's visibility to false:
                                diamond.Value.SetVisible(false);

                                //Add 1 diamond to player:
                                Player.AddDiamond(diamond.Value);

                                //Send current taken diamond's packet data to server host:
                                new Thread(() => GameClient.SendDiamondChangesToServer(diamond.Value)).Start();

                                //Add popup:
                                HUD.AddPopup(new Popup("+1 Diamond", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.White, Color.Black), true);

                                //Add xp for player:
                                Player.GetCurrentCharacter().AddXp(50);

                                //Update hud's player's xp bar:
                                HUD.AddPlayerXp(50, Player.GetCurrentCharacter().GetMaxXp());

                                //Add popup:
                                HUD.AddPopup(new Popup("+50xp", HUD.GetPlayerLevelBar().GetRectangle().Left + 3, HUD.GetPlayerLevelBar().GetRectangle().Top, Color.White, Color.Green), false);
                            
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks player's looting
        /// </summary>
        private void CheckLoot() {

            //Using lock function to avoid multi task crash:
            lock (Map.GetWorldEntities()) {

                //Run on all map's entities:
                for (int i = 0; i < Map.GetWorldEntities().Count; i++) {

                    //Get current map's entity:
                    MapEntity currentEntity = Map.GetWorldEntities()[Map.GetWorldEntities().Keys.ElementAt(i)];

                    //Checks if player is touching current entity:
                    if (Player.IsTouchWorldEntity(currentEntity)) {

                        //Checks if player is pressing 'E' button on keyboard:
                        if (Keyboard.GetState().IsKeyDown(Keys.E) && Player.GetState() != PlayerState.Loot) {

                            //Change player's state to loot:
                            Player.SetState(PlayerState.Loot);

                            //Check current entity's name:
                            switch (currentEntity.GetName()) {

                                //If current entity is red flower:
                                case MapEntityName.Red_Flower:
                                    //Checks if player's current character's health is less than maximum health:
                                    if (Player.GetCurrentCharacter().GetHealth() < Player.GetCurrentCharacter().GetMaxHealth()) {

                                        //Increase player's health:
                                        Player.GetCurrentCharacter().IncreaseHp(15);

                                        //Update hud's player's health bar:
                                        HUD.AddPlayerHealth(15, Player.GetCurrentCharacter().GetMaxHealth());

                                        //Add popup:
                                        HUD.AddPopup(new Popup("+15hp", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.White, Color.Red), true);
                                        
                                        //Remove current entity by his key:
                                        Map.RemoveMapEntity(currentEntity.GetKey());

                                        //Send remove entity command to the server:
                                        new Thread(() => GameClient.SendText("Remove Entity_" + currentEntity.GetKey() + "_" + Player.GetName())).Start();
                                    }
                                    else { //If player has enough hp:

                                        //Add popup:
                                        HUD.AddPopup(new Popup("You have enough health!", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.Red, Color.Black), true);
                                    }
                                    break;

                                //If current entity is stone:
                                case MapEntityName.Stone:
                                    //Add 1 stone to player:
                                    Player.AddStones(1);

                                    //Add popup:
                                    HUD.AddPopup(new Popup("+1 Stone", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.White, Color.Black), true);
                                    
                                    //Add xp for player:
                                    Player.GetCurrentCharacter().AddXp(3);

                                    //Update hud's player's xp bar:
                                    HUD.AddPlayerXp(3, Player.GetCurrentCharacter().GetMaxXp());
                                    
                                    //Add popup:
                                    HUD.AddPopup(new Popup("+3xp", HUD.GetPlayerLevelBar().GetRectangle().Left + 3, HUD.GetPlayerLevelBar().GetRectangle().Top, Color.White, Color.Green), false);

                                    //Remove current entity by his key:
                                    Map.RemoveMapEntity(currentEntity.GetKey());

                                    //Send remove entity command to the server:
                                    new Thread(() => GameClient.SendText("Remove Entity_" + currentEntity.GetKey() + "_" + Player.GetName())).Start();
                                    break;
                                //If current entity is arrow:
                                case MapEntityName.Arrow:

                                    //Checks if player's current character is archer:
                                    if (Player.GetCurrentCharacter() is Archer) {

                                        Archer archer = Player.CurrentCharacter as Archer;

                                        //Add popup:
                                        HUD.AddPopup(new Popup("+1 Arrow", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.White, Color.Black), true);
                                        HUD.AddPopup(new Popup("+1", (int)HUD.GetPlayerWeaponAmmo().GetPosition().X, (int)HUD.GetPlayerWeaponAmmo().GetPosition().Y - 10, Color.White, Color.Black), false);

                                        //Add arrow for archer:
                                        archer.AddArrow();

                                        //Update hud's player's weapon's ammo:
                                        HUD.SetPlayerWeaponAmmo(archer.CurrentArrows + "/" + archer.MaxArrows);

                                        //Remove current entity by his key:
                                        Map.RemoveMapEntity(currentEntity.GetKey());

                                        //Send remove entity command to the server:
                                        new Thread(() => GameClient.SendText("Remove Entity_" + currentEntity.GetKey() + "_" + Player.GetName())).Start();
                                    }
                                    else { //If player's current character is not archer:

                                        //Add popup:
                                        HUD.AddPopup(new Popup("You must be archer to pickup arrows!", Player.GetRectangle().X, Player.GetRectangle().Y - 30, Color.Red, Color.Black), true);
                                    }
                                    break;
                            }

                            break;
                        }
                    }
                }
            }

            //Checks if player's current state is loot and at the moment the loot animation is finished:
            if (Player.GetState() == PlayerState.Loot && Player.CurrentCharacter.LootAnimation.IsFinished) {

                //Reset loot animation:
                Player.CurrentCharacter.LootAnimation.Reset();

                //Sets player's state to afk:
                Player.SetState(PlayerState.Afk);
            }
        }

        /// <summary>
        /// Checks player's entering and exiting castle doors:
        /// </summary>
        private void CheckEnterExitCastleDoors() {

            //Run on each team:
            foreach (KeyValuePair<TeamName, Team> team in Map.GetTeams()) {

                //Checks if player is touching team's castle's outside door:
                if (Player.IsTouchCastleDoor(team.Value.GetCastle().GetOutsideDoor())) {

                    //Checks if player is pressing 'E' button on keyboard:
                    if (Keyboard.GetState().IsKeyDown(Keys.E) && !IsPressedE) {
                        IsPressedE = true;

                        //Checks current team's name:
                        switch (team.Value.GetName()) {

                            //Checks if red team:
                            case TeamName.Red:
                                //Change player's location to inside team's red castle:
                                Player.ChangeLocationTo(Location.Inside_Red_Castle);

                                //Update map's location to inside team's red castle:
                                Map.UpdateLocationsTo(Location.Inside_Red_Castle);

                                //Set player's rectangle to team's red castle inside door:
                                Player.SetRectangle(new Rectangle(team.Value.GetCastle().GetInsideDoor().GetImage().GetRectangle().Left,
                                                                  team.Value.GetCastle().GetInsideDoor().GetImage().GetRectangle().Bottom - 50,
                                                                  Player.GetRectangle().Width,
                                                                  Player.GetRectangle().Height));
                                break;
                            //Checks if yellow team:
                            case TeamName.Yellow:

                                //Change player's location to inside team's yellow castle:
                                Player.ChangeLocationTo(Location.Inside_Yellow_Castle);

                                //Update map's location to inside team's yellow castle:
                                Map.UpdateLocationsTo(Location.Inside_Yellow_Castle);

                                //Set player's rectangle to team's yellow castle inside door:
                                Player.SetRectangle(new Rectangle(team.Value.GetCastle().GetInsideDoor().GetImage().GetRectangle().Left,
                                                                  team.Value.GetCastle().GetInsideDoor().GetImage().GetRectangle().Bottom - 50,
                                                                  Player.GetRectangle().Width,
                                                                  Player.GetRectangle().Height));
                                break;
                        }
                        break;
                    }
                }
                //Checks if player is touching team's castle's inside door:
                else if (Player.IsTouchCastleDoor(team.Value.GetCastle().GetInsideDoor())) {

                    //Checks if player is pressing 'E' button on keyboard:
                    if (Keyboard.GetState().IsKeyDown(Keys.E) && !IsPressedE) {
                        IsPressedE = true;

                        //Change player's location to outside:
                        Player.ChangeLocationTo(Location.Outside);

                        //Update map's location to outside:
                        Map.UpdateLocationsTo(Location.Outside);

                        //Set player's rectangle to team's castle outside door:
                        Player.SetRectangle(new Rectangle(team.Value.GetCastle().GetOutsideDoor().GetImage().GetRectangle().Left,
                                                          team.Value.GetCastle().GetOutsideDoor().GetImage().GetRectangle().Bottom - 50,
                                                          Player.GetRectangle().Width,
                                                          Player.GetRectangle().Height));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks player's diamonds drop
        /// </summary>
        private void CheckDiamondsDrops() {

            //Checks if player is pressing 'X' button on keyboard:
            if (Keyboard.GetState().IsKeyDown(Keys.X) && !IsPressedX) {
                IsPressedX = true;

                //Get dropped diamond only if player is carrying diamonds:
                Diamond diamond = Player.RemoveDiamond();

                //Check if player dropped diamond:
                if (diamond != null) {

                    //Update the right diamond:
                    Map.GetTeams()[diamond.GetTeam()].GetCastle().GetDiamonds()[diamond.GetKey()] = diamond;


                    //Send current taken diamond's packet data to server host:
                    new Thread(() => GameClient.SendDiamondChangesToServer(diamond)).Start();
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.X))
                IsPressedX = false;
        }

        private TeamName GetOppositeTeam(TeamName currentTeam) {
            return currentTeam == TeamName.Red ? TeamName.Yellow : TeamName.Red;
        }

        /// <summary>
        /// Checks keyboard buttons pressing:
        /// </summary>
        private void CheckKeyboard() {

            //Checks if player's state is not attacking/defencing/looting:
            if (Player.GetState() != PlayerState.Attack && Player.GetState() != PlayerState.Defence && Player.GetState() != PlayerState.Loot) {

                //Checks if player is not pressing buttons on keyboard:
                if (Keyboard.GetState().GetPressedKeys().Length == 0) {

                    //Sets player's state to afk:
                    Player.SetState(PlayerState.Afk);

                    //Checks if player is owner of horse:
                    if (Player.GetCurrentHorse() != null)
                        Player.GetCurrentHorse().SetState(HorseState.Afk); //Sets player's horse's state to afk
                }

                //Check movement:
                CheckMovement();
            }

            //Check other stuff:
            CheckChangeCharacter();
            CheckAttack();
            CheckDefence();
            CheckLoot();
            CheckSteal();
            CheckMountHorse();
            CheckEnterExitCastleDoors();
            CheckDiamondsDrops();
        }

        /// <summary>
        /// Receives character type name, team, player's name
        /// and start joining game session:
        /// </summary>
        /// <param name="characterName"></param>
        /// <param name="team"></param>
        /// <param name="name"></param>
        public void JoinGame(CharacterName characterName, TeamName team, string name) {

            //Create player:
            Player = new Player(characterName, team, name, Map.GetGrass().GetRectangle());

            //Initializes player's events:
            Player.OnAddHealth += HUD.AddPlayerHealth;
            Player.OnSetHealth += HUD.SetPlayerHealth;
            Player.OnMinusHealth += HUD.MinusPlayerHealth;
            Player.OnChangeLocation += Map.UpdateLocationsTo;
            
            //Respawn player:
            Player.Respawn();

            //Initializes game client's events:
            GameClient.OnGetThePlayer += GetPlayer;
            GameClient.OnGetRedPlayers += Map.GetTeams()[TeamName.Red].GetPlayers;
            GameClient.OnGetYellowPlayers += Map.GetTeams()[TeamName.Yellow].GetPlayers;
            GameClient.OnJoinPlayer += Map.AddPlayer;
            GameClient.OnGetTeams += Map.GetTeams;
            GameClient.OnAddPopup += HUD.AddPopup;
            GameClient.OnAddEntity += Map.AddEntity;
            GameClient.OnFinishedLoading += StartGame;
            GameClient.OnRemoveMapEntity += Map.RemoveMapEntity;

            //Connect to server thread:
            new Thread(() => GameClient.Connect("192.168.1.17", 4441)).Start();
        }

        /// <summary>
        /// Start game after loading data from server
        /// </summary>
        private void StartGame() {
            OnStartGameAfterLoading();

            //Add popup:
            HUD.AddPopup(new Popup("You joined to the " + Player.GetTeamName() + " team!", CastleBridge.Graphics.PreferredBackBufferWidth / 2 + 280, CastleBridge.Graphics.PreferredBackBufferHeight - 100, Color.Red, Color.Black, false), false);
        }
       
        /// <summary>
        /// Update hud:
        /// </summary>
        public void UpdateHud() {

            //Update hud's player's avatar:
            HUD.SetPlayerAvatar(Player.CurrentCharacter.GetName(), Player.CurrentCharacter.GetTeamName());

            //Checks if player's current character is archer:
            if (Player.CurrentCharacter is Archer) {

                //Update hud's player's weapon to bow:
                HUD.SetPlayerWeapon(Weapon.Bow, Player.CurrentCharacter.GetName(), Player.CurrentCharacter.GetTeamName());

                //Update hud's player's weapon's ammo:
                HUD.SetPlayerWeaponAmmo(((Archer)Player.CurrentCharacter).CurrentArrows + "/" + ((Archer)Player.CurrentCharacter).MaxArrows);
            }
            //Checks if player's current character is knight:
            else if (Player.CurrentCharacter is Knight) {

                //Update hud's player's weapon to sword:
                HUD.SetPlayerWeapon(Weapon.Sword, Player.CurrentCharacter.GetName(), Player.CurrentCharacter.GetTeamName());

                //Update hud's player's weapon's ammo to empty:
                HUD.SetPlayerWeaponAmmo(string.Empty);
            }
            //Checks if player's current character is mage:
            else if (Player.CurrentCharacter is Mage) {

                //Update hud's player's weapon to wand:
                HUD.SetPlayerWeapon(Weapon.Wand, Player.CurrentCharacter.GetName(), Player.CurrentCharacter.GetTeamName());

                //Update hud's player's weapon's ammo:
                HUD.SetPlayerWeaponAmmo(((Mage)Player.CurrentCharacter).CurrentSpells + "/" + ((Mage)Player.CurrentCharacter).MaxSpells);
            }
        }

        /// <summary>
        /// Updates player
        /// </summary>
        private void UpdatePlayer() {

            //Update player stuff:
            Player.Update();

            //Checks player's hit:
            CheckForPlayerHit();

            //Checks if player's current character is archer:
            if (Player.CurrentCharacter is Archer) {

                Archer archer = Player.CurrentCharacter as Archer;

                //Run on all archer's arrows:
                for (int i = 0; i < archer.GetArrows().Count; i++) {

                    //Checks if current arrow is still moving:
                    if (!archer.GetArrows()[i].IsFinished)
                        archer.GetArrows()[i].Move(); //Move arrow
                    else { //If arrow is fell:

                        //Add arrow to map entities:
                        Map.AddEntity(MapEntityName.Arrow,
                                      archer.GetArrows()[i].GetAnimation().GetCurrentSpriteImage().GetRectangle().X,
                                      archer.GetArrows()[i].GetAnimation().GetCurrentSpriteImage().GetRectangle().Y,
                                      archer.GetArrows()[i].GetDirection(), archer.GetArrows()[i].GetDirection() == Direction.Right ? 0.7f : -0.7f, Player.GetCurrentLocation(), true, "100_" + new Random().Next(1000));
                        
                        //Remove archer's arrow:
                        archer.GetArrows().RemoveAt(i);
                    }
                }
            }
            //Checks if player's current character is mage:
            else if (Player.CurrentCharacter is Mage) {

                Mage mage = Player.CurrentCharacter as Mage;

                //Run on all mage's spells:
                for (int i = 0; i < mage.GetSpells().Count; i++) {

                    //Checks if current spell is still moving:
                    if (!mage.GetSpells()[i].IsFinished)
                        mage.GetSpells()[i].Move(); //Move spell
                    else { //If spell is finished:

                        //Remove mage's spell:
                        mage.GetSpells().RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Updates online players
        /// </summary>
        private void UpdatePlayers() {

            //Run on each team:
            foreach (KeyValuePair<TeamName, Team> team in Map.GetTeams()) {

                //Run on each team's online players:
                foreach (KeyValuePair<string, Player> onlinePlayer in team.Value.GetPlayers()) {

                    //Update current online player:
                    onlinePlayer.Value.Update();

                    //Checks if current online player's character is archer:
                    if (onlinePlayer.Value.CurrentCharacter is Archer) {

                        Archer archer = onlinePlayer.Value.CurrentCharacter as Archer;

                        //Run on all archer's arrows:
                        for (int i = 0; i < archer.GetArrows().Count; i++) {

                            //Checks if current arrow is still moving:
                            if (!archer.GetArrows()[i].IsFinished)
                                archer.GetArrows()[i].Move(); //Move arrow
                            else { //If arrow is fell:

                                //Add arrow to map entities:
                                Map.AddEntity(MapEntityName.Arrow,
                                              archer.GetArrows()[i].GetAnimation().GetCurrentSpriteImage().GetRectangle().X,
                                              archer.GetArrows()[i].GetAnimation().GetCurrentSpriteImage().GetRectangle().Y,
                                              archer.GetArrows()[i].GetDirection(), archer.GetArrows()[i].GetDirection() == Direction.Right ? 0.7f : -0.7f, Player.GetCurrentLocation(), true, "200_" + new Random().Next(1000));

                                //Remove archer's arrow:
                                archer.GetArrows().RemoveAt(i);
                            }
                        }
                    }

                    //Checks if current online player's character is mage:
                    else if (onlinePlayer.Value.CurrentCharacter is Mage) {

                        Mage mage = onlinePlayer.Value.CurrentCharacter as Mage;

                        //Run on all mage's spells:
                        for (int i = 0; i < mage.GetSpells().Count; i++) {

                            //Checks if current spell is still moving:
                            if (!mage.GetSpells()[i].IsFinished)
                                mage.GetSpells()[i].Move(); //Move spell
                            else {  //If spell is finished:

                                //Remove mage's spell:
                                mage.GetSpells().RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check for player's hit
        /// </summary>
        private void CheckForPlayerHit() {

            //Run on each team:
            foreach (KeyValuePair<TeamName, Team> team in Map.GetTeams()) {

                //Run on each team's online players:
                foreach (KeyValuePair<string, Player> onlinePlayer in team.Value.GetPlayers()) {

                    PlayerState onlineState = onlinePlayer.Value.GetState();
                    PlayerState playerState = Player.GetState();

                    //Checks if current online player's current character is knight:
                    if (onlinePlayer.Value.GetCurrentCharacter() is Knight) {

                        //Checks if current player is touching current online player:
                        if (Player.IsTouchOnlinePlayer(onlinePlayer.Value)) {

                            //Checks if current player's team is not equals to current online player's team:
                            if (Player.GetTeamName() != onlinePlayer.Value.GetTeamName()) {

                                //Checks if current online player's state is attack and current player's state is not defence and current online player is finished is attack animation:
                                if (onlineState == PlayerState.Attack && playerState != PlayerState.Defence &&
                                    onlinePlayer.Value.GetCurrentAnimation().IsFinished) {

                                    //Get online player's knight attack damage:
                                    int onlinePlayerAttackDamage = ((Knight)Player.GetCurrentCharacter()).GetAttackDamage();

                                    //Hit current player by online player's knight attack damage:
                                    Player.Hit(onlinePlayerAttackDamage);
                                }
                            }
                        }
                    }
                    //Checks if current online player's current character is archer:
                    else if (onlinePlayer.Value.GetCurrentCharacter() is Archer) {

                        Archer archer = onlinePlayer.Value.GetCurrentCharacter() as Archer;

                        //Checks if current online player's state is attack and current online player is finished is attack animation:
                        if (onlineState == PlayerState.Attack && onlinePlayer.Value.CurrentCharacter.AttackAnimation.IsFinished) {
                            onlinePlayer.Value.CurrentCharacter.AttackAnimation.IsFinished = false;

                            //Current online player's archer start shoot arrow: 
                            archer.ShootArrow(Direction.Down, Player.GetCurrentLocation(), onlinePlayer.Value);
                        }

                        //Run on each online player's archer arrows:
                        for (int i = 0; i < archer.GetArrows().Count; i++) {

                            //Checks if current player is touching current online player's archer's current arrow:
                            if (Player.IsTouchArrow(archer.GetArrows()[i])) {

                                //Hit current player by current online player's archer's current arrow's damage:
                                Player.Hit(archer.GetArrows()[i].GetDamage());

                                //Remove current online player's archer's current arrow:
                                archer.GetArrows().RemoveAt(i);
                            }
                        }
                    }

                    //Checks if current online player's current character is mage:
                    else if (onlinePlayer.Value.GetCurrentCharacter() is Mage) {

                        Mage mage = onlinePlayer.Value.GetCurrentCharacter() as Mage;

                        //Checks if current online player's state is attack and current online player is finished is attack animation:
                        if (onlineState == PlayerState.Attack && onlinePlayer.Value.CurrentCharacter.AttackAnimation.IsFinished) {
                            onlinePlayer.Value.CurrentCharacter.AttackAnimation.IsFinished = false;

                            //Current online player's mage start cast spell: 
                            mage.CastSpell(Direction.Down, Player.GetCurrentLocation(), onlinePlayer.Value);
                        }

                        //Run on each online player's mage spells:
                        for (int i = 0; i < mage.GetSpells().Count; i++) {

                            //Checks if current player is touching current online player's mage's current spell:
                            if (Player.IsTouchSpell(mage.GetSpells()[i])) {

                                //Hit current player by current online player's mage's current spell's damage:
                                Player.Hit(mage.GetSpells()[i].GetDamage());

                                //Remove current online player's mage's current spell:
                                mage.GetSpells().RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks updates:
        /// </summary>
        public override void Update() {

            //Checks only if player is alive:
            if (!Player.IsDead) {

                //Check keyboard:
                CheckKeyboard();

                //Update player:
                UpdatePlayer();

                //Generate xp:
                GenerateXp();

            } //If player is dead:
            else
                CheckForPlayerRespawn(); //Check for player's respawn

            //Update players:
            UpdatePlayers();

            //Update map:
            Map.Update();

            //Update hud:
            HUD.Update();

            //Update camera's focus on current player:
            Camera.Focus(new Vector2(Player.GetRectangle().X, Player.GetRectangle().Y), Map.WIDTH, Map.HEIGHT);
        }

        /// <summary>
        /// Draw game
        /// </summary>
        public override void Draw() {

            //Draw stuck back (map's weather..):
            DrawStuckBack();

            //Draw tile:
            DrawTile();

            //Draw stuck front (Hud):
            DrawStuckFront();
        }

        /// <summary>
        /// Draw stuck back
        /// </summary>
        private void DrawStuckBack() {

            CastleBridge.SpriteBatch.Begin();

            //Draw stuck map's weather:
            Map.GetWeather().DrawStuck();

            CastleBridge.SpriteBatch.End();
        }

        /// <summary>
        /// Draw tile
        /// </summary>
        private void DrawTile() {

            CastleBridge.SpriteBatch.Begin(SpriteSortMode.Deferred,
                            BlendState.AlphaBlend,
                            null,
                            null,
                            null,
                            null,
                            Camera.Transform
                            );

            //Draw map's grass:
            Map.GetGrass().Draw();

            //Draw map's weather's clouds:
            Map.GetWeather().DrawClouds();

            //Draw map's castles:
            Map.DrawCastles(Player.GetCurrentLocation());

            //Run on map's grass's height pixels to draw by layers:
            for (int i = Map.GetGrass().GetRectangle().Top; i < Map.GetGrass().GetRectangle().Bottom; i++) {

                //Draw map's tiles:
                Map.DrawTile(i, Player.GetCurrentLocation());

                //Draw player:
                if (Player.GetCurrentAnimation().GetCurrentSpriteImage().GetRectangle().Bottom - 10 == i)
                    Player.Draw();

                //Checks if player's current character is archer:
                if (Player.CurrentCharacter is Archer) {
                    Archer archer = Player.CurrentCharacter as Archer;

                    //Run on each player's archer's arrows:
                    foreach (Arrow arrow in archer.GetArrows()) {

                        //Draw arrow:
                        if (arrow.GetAnimation().GetCurrentSpriteImage().GetRectangle().Bottom == i)
                            arrow.Draw();
                    }
                }
                //Checks if player's current character is mage:
                else if (Player.CurrentCharacter is Mage) {
                    Mage mage = Player.CurrentCharacter as Mage;

                    //Run on each player's mage's spells:
                    foreach (EnergyBall energyBall in mage.GetSpells()) {

                        //Draw spell:
                        if (energyBall.GetAnimation().GetCurrentSpriteImage().GetRectangle().Bottom == i)
                            energyBall.Draw();
                    }
                }
            }

            //Draw hud's tile:
            HUD.DrawTile();

            //Run on each map's weather's clouds:
            foreach (Cloud cloud in Map.GetWeather().GetClouds()) {

                //Run on each cloud's rain drop:
                foreach (RainDrop rainDrop in cloud.GetRainDrops()) {

                    //Checks if current cloud is raining:
                    if (cloud.IsRain)
                        rainDrop.Draw(); //Draw rain drop
                }
            }

            CastleBridge.SpriteBatch.End();
        }

        /// <summary>
        /// Draw stuck front
        /// </summary>
        private void DrawStuckFront() {

            CastleBridge.SpriteBatch.Begin();

            //Draw hud's stuck:
            HUD.DrawStuck();

            CastleBridge.SpriteBatch.End();
        }

        /// <summary>
        /// Get game client
        /// </summary>
        /// <returns></returns>
        public GameClient GetGameClient() {
            return GameClient;
        }

        /// <summary>
        /// Get player
        /// </summary>
        /// <returns></returns>
        public Player GetPlayer() {
            return Player;
        }
    }
}
