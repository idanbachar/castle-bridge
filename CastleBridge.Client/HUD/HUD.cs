using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class HUD {

        private List<Text> Labels; //Hud's labels
        private Image PlayerAvatar; //Hud's player's avatar image
        private Image HorseAvatar; //Hud's horse's avatar image
        private Image PlayerWeapon; //Hud's player's weapon
        private Image PlayerHealthBar; //Hud's player's health bar
        private Image PlayerLevelBar; //Hud's player's level bar
        private Text PlayerWeaponAmmo; //Hud's player's weapon's ammo
        private Text PlayerHealth; //Hud's player's health
        private Text PlayerLevel; //Hud's player's level
        private List<Popup> TilePopups; //Hud's tile popups
        private List<Popup> StuckPopups; //Hud's stuck popups
        private Text RespawnTimerLabel; //Respawn timer label
        private Text VersionText; //Version text label

        /// <summary>
        /// Creates a hud
        /// </summary>
        public HUD() {

            Labels = new List<Text>();
            TilePopups = new List<Popup>();
            StuckPopups = new List<Popup>();
            PlayerAvatar = new Image(string.Empty, string.Empty, 30, CastleBridge.Graphics.PreferredBackBufferHeight - 135, 100, 100, Color.White);
            PlayerWeapon = new Image(string.Empty, string.Empty, PlayerAvatar.GetRectangle().Right, PlayerAvatar.GetRectangle().Top, 50, 50, Color.White);
            PlayerHealthBar = new Image("player/health", "health_bar", PlayerAvatar.GetRectangle().Left, PlayerAvatar.GetRectangle().Top - 50, 100, 25, Color.White);
            HorseAvatar = new Image("horse/teams/red/avatar", "horse_avatar", PlayerHealthBar.GetRectangle().Left, PlayerHealthBar.GetRectangle().Top - 100, 100, 100, Color.White);
            
            //Sets horse avatar visibilty to false:
            HorseAvatar.SetVisible(false);
            
            PlayerLevelBar = new Image("player/level", "level_bar", PlayerAvatar.GetRectangle().Left, PlayerAvatar.GetRectangle().Bottom, 0, 25, Color.White);
            PlayerWeaponAmmo = new Text(FontType.Default, "0", new Vector2(PlayerWeapon.GetRectangle().Left, PlayerWeapon.GetRectangle().Bottom + 5), Color.White, true, Color.Black);
            PlayerHealth = new Text(FontType.Default, "100/100", new Vector2(PlayerHealthBar.GetRectangle().Left + PlayerHealthBar.GetRectangle().Width / 2, PlayerHealthBar.GetRectangle().Top), Color.White, false, Color.Black);
            PlayerLevel = new Text(FontType.Default, "0/100", new Vector2(PlayerLevelBar.GetRectangle().Left + PlayerLevelBar.GetRectangle().Width / 2, PlayerLevelBar.GetRectangle().Top), Color.White, false, Color.Black);

            RespawnTimerLabel = new Text(FontType.Default_Bigger, string.Empty, new Vector2(CastleBridge.Graphics.PreferredBackBufferWidth / 3 + 50, 100), Color.Gold, true, Color.Black);
            RespawnTimerLabel.SetVisible(false);

            VersionText = new Text(FontType.Default_Bigger, "Alpha", new Vector2(CastleBridge.Graphics.PreferredBackBufferWidth - 150, CastleBridge.Graphics.PreferredBackBufferHeight - 50), Color.White, false, Color.White);
        }

        /// <summary>
        /// Receives a text and adds it to labels's list:
        /// </summary>
        /// <param name="text"></param>
        public void AddLabel(Text text) {
            Labels.Add(text);
        }

        /// <summary>
        /// Receives a popup, is tile indication and adds to the correct popup's list
        /// </summary>
        /// <param name="popup"></param>
        /// <param name="isTile"></param>
        public void AddPopup(Popup popup, bool isTile) {

            if (isTile)
                TilePopups.Add(popup);
            else
                StuckPopups.Add(popup);
        }

        /// <summary>
        /// Receives text and updates respawn label
        /// </summary>
        /// <param name="text"></param>
        public void SetRespawnLabel(string text) {

            RespawnTimerLabel.ChangeText(text);
        }

        /// <summary>
        /// Receives character's type name, team
        /// and sets player's avatar
        /// </summary>
        /// <param name="name"></param>
        /// <param name="teamName"></param>
        public void SetPlayerAvatar(CharacterName name, TeamName teamName) {
            PlayerAvatar.ChangeImage("player/characters/teams/" + teamName + "/" + name + "/avatar/" + name + "_avatar");
        }

        /// <summary>
        /// Receives weapon, character's type name, team
        /// and sets player's weapon
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="name"></param>
        /// <param name="teamName"></param>
        public void SetPlayerWeapon(Weapon weapon, CharacterName name, TeamName teamName) {
            PlayerWeapon.ChangeImage("player/characters/teams/" + teamName + "/" + name + "/weapons/" + weapon + "/" + name + "_" + weapon + "_avatar");
        }

        /// <summary>
        /// Receives ammo
        /// and sets player's weapon ammo
        /// </summary>
        /// <param name="ammo"></param>
        public void SetPlayerWeaponAmmo(string ammo) {
            PlayerWeaponAmmo.ChangeText(ammo);
        }

        /// <summary>
        /// Receives team
        /// and sets horse avatar by team
        /// </summary>
        /// <param name="teamName"></param>
        public void SetHorseAvatar(TeamName teamName) {
            HorseAvatar.ChangeImage("horse/teams/" + teamName + "/avatar/horse_avatar");
        }

        public void SetPlayerHealth(int health, int maxHealth) {

            PlayerHealthBar.SetRectangle(PlayerHealthBar.GetRectangle().X, PlayerHealthBar.GetRectangle().Y, health, PlayerHealthBar.GetRectangle().Height);
            PlayerHealth.ChangeText(health + "/" + maxHealth + "hp");
            PlayerHealth.SetPosition(new Vector2(PlayerHealthBar.GetRectangle().Left + PlayerHealthBar.GetRectangle().Width / 2, PlayerHealthBar.GetRectangle().Top));
        }

        /// <summary>
        /// Receives health, max health
        /// and updates player's health bar + adds a popup
        /// </summary>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        public void AddPlayerHealth(int health, int maxHealth) {
            if (PlayerHealthBar.GetRectangle().Width <= maxHealth) {
                PlayerHealthBar.SetRectangle(PlayerHealthBar.GetRectangle().X, PlayerHealthBar.GetRectangle().Y, PlayerHealthBar.GetRectangle().Width + health, PlayerHealthBar.GetRectangle().Height);
                PlayerHealth.ChangeText(PlayerHealthBar.GetRectangle().Width.ToString() + "/" + maxHealth + "hp");
                PlayerHealth.SetPosition(new Vector2(PlayerHealthBar.GetRectangle().Left + PlayerHealthBar.GetRectangle().Width / 2, PlayerHealthBar.GetRectangle().Top));

                AddPopup(new Popup("+" + health, PlayerHealthBar.GetRectangle().Left + 3, PlayerHealthBar.GetRectangle().Top, Color.White, Color.Green), false);
            }
        }

        /// <summary>
        /// Receives health, max health
        /// and updates player's health bar + adds a popup
        /// </summary>
        /// <param name="health"></param>
        /// <param name="maxHealth"></param>
        public void MinusPlayerHealth(int health, int maxHealth) {
            if (PlayerHealthBar.GetRectangle().Width >= 0) {
                PlayerHealthBar.SetRectangle(PlayerHealthBar.GetRectangle().X, PlayerHealthBar.GetRectangle().Y, PlayerHealthBar.GetRectangle().Width - health, PlayerHealthBar.GetRectangle().Height);
                PlayerHealth.ChangeText(PlayerHealthBar.GetRectangle().Width.ToString() + "/" + maxHealth + "hp");
                PlayerHealth.SetPosition(new Vector2(PlayerHealthBar.GetRectangle().Left + PlayerHealthBar.GetRectangle().Width / 2, PlayerHealthBar.GetRectangle().Top));

                AddPopup(new Popup("-" + health, PlayerHealthBar.GetRectangle().Left + 3, PlayerHealthBar.GetRectangle().Top, Color.White, Color.Red), false);
            }
        }

        /// <summary>
        /// Receives levelxp, max max level xp
        /// and sets player's xp bar
        /// </summary>
        /// <param name="levelXp"></param>
        /// <param name="maxLevelXp"></param>
        private void SetPlayerXp(int levelXp, int maxLevelXp) {
            PlayerLevelBar.SetRectangle(PlayerLevelBar.GetRectangle().X, PlayerLevelBar.GetRectangle().Y, levelXp, PlayerLevelBar.GetRectangle().Height);
            PlayerLevel.ChangeText(PlayerLevelBar.GetRectangle().Width.ToString() + "/" + maxLevelXp + "xp");
        }

        /// <summary>
        /// Receives levelxp, max max level xp
        /// and updates player's xp bar
        /// </summary>
        /// <param name="levelXp"></param>
        /// <param name="maxLevelXp"></param>
        public void AddPlayerXp(int levelXp, int maxLevelXp) {
            if (PlayerLevelBar.GetRectangle().Width + levelXp < maxLevelXp) {
                PlayerLevelBar.SetRectangle(PlayerLevelBar.GetRectangle().X, PlayerLevelBar.GetRectangle().Y, PlayerLevelBar.GetRectangle().Width + levelXp, PlayerLevelBar.GetRectangle().Height);
                PlayerLevel.ChangeText(PlayerLevelBar.GetRectangle().Width.ToString() + "/" + maxLevelXp + "xp");
            }
            else {
                SetPlayerXp(0, maxLevelXp + 100);
            }
        }

        /// <summary>
        /// Updates tile popups
        /// if popup timer is finished then removes
        /// </summary>
        private void UpdateTilePopups() {

            for (int i = 0; i < TilePopups.Count; i++) {
                if (!TilePopups [i].IsFinished)
                    TilePopups [i].Update();
                else {
                    TilePopups.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Updates stuck popups
        /// if popup timer is finished then removes
        /// </summary>
        private void UpdateStuckPopups() {

            for (int i = 0; i < StuckPopups.Count; i++) {
                if (!StuckPopups [i].IsFinished)
                    StuckPopups [i].Update();
                else {
                    StuckPopups.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Updates popups
        /// </summary>
        public void Update() {

            UpdateTilePopups();
            UpdateStuckPopups();
        }

        /// <summary>
        /// Get labels
        /// </summary>
        /// <returns></returns>
        public List<Text> GetLabels() {
            return Labels;
        }

        /// <summary>
        /// Get tile popups
        /// </summary>
        /// <returns></returns>
        public List<Popup> GetTilePopups() {
            return TilePopups;
        }

        /// <summary>
        /// Draw tile stuff
        /// </summary>
        public void DrawTile() {

            //Draw tile popups:
            foreach (Popup popup in TilePopups)
                popup.Draw();
        }

        /// <summary>
        /// Get player's avatar
        /// </summary>
        /// <returns></returns>
        public Image GetPlayerAvatar() {
            return PlayerAvatar;
        }

        /// <summary>
        /// Get horse's avatar
        /// </summary>
        /// <returns></returns>
        public Image GetHorseAvatar() {
            return HorseAvatar;
        }

        /// <summary>
        /// Get player's health bar
        /// </summary>
        /// <returns></returns>
        public Image GetPlayerHealthBar() {
            return PlayerHealthBar;
        }

        /// <summary>
        /// Get player's level bar
        /// </summary>
        /// <returns></returns>
        public Image GetPlayerLevelBar() {
            return PlayerLevelBar;
        }

        /// <summary>
        /// Get player's weapon ammo
        /// </summary>
        /// <returns></returns>
        public Text GetPlayerWeaponAmmo() {
            return PlayerWeaponAmmo;
        }

        /// <summary>
        /// Get respawn timer label
        /// </summary>
        /// <returns></returns>
        public Text GetRespawnTimerLabel() {
            return RespawnTimerLabel;
        }

        /// <summary>
        /// Draw stuck stuff:
        /// </summary>
        public void DrawStuck() {

            //Draw labels:
            foreach (Text label in Labels)
                label.Draw();

            PlayerAvatar.Draw();
            PlayerWeapon.Draw();
            PlayerWeaponAmmo.Draw();
            PlayerHealthBar.Draw();
            PlayerHealth.Draw();
            PlayerLevelBar.Draw();
            PlayerLevel.Draw();
            HorseAvatar.Draw();

            //Draw stuck popups:
            foreach (Popup popup in StuckPopups)
                popup.Draw();

            //Draw respawn label:
            RespawnTimerLabel.Draw();

            //Draw version label:
            VersionText.Draw();
        }
    
    }
}
