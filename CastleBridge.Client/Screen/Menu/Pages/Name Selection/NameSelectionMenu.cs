using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class NameSelectionMenu : Menu {

        private Image RedCastle; //Red castle image
        private Image YellowCastle; //Yellow castle image
        private Image CurrentCastle; //Current castle image
        private List<Character> RedCharacters; //Red characters
        private List<Character> YellowCharacters; //Yellow characters
        private Character CurrentCharacter; //Current character

        private Keys[] lastPressedKeys = new Keys[1]; //last pressed keys
        public Text Text; //Text
        private string Name; //Name
        private Image InputText; //Input text image
        private Button OkButton; //Ok button
        private Button BackButton; //Back button
        public bool IsSelected; //Is text selected indication
        private bool caps; //Caps lock

        /// <summary>
        /// Receives title
        /// and creates a menu
        /// </summary>
        /// <param name="title"></param>
        public NameSelectionMenu(string title): base(title) {

            RedCastle = new Image("map/castles/teams/red/outside", "castle", 0, 100, 1400, 431, Color.White);
            YellowCastle = new Image("map/castles/teams/yellow/outside", "castle", 0, 100, 1400, 431, Color.White);
            CurrentCastle = RedCastle;

            InputText = new Image("menu/button backgrounds/empty", CastleBridge.Graphics.PreferredBackBufferWidth / 2 - 250, CastleBridge.Graphics.PreferredBackBufferHeight / 2, 500, 50);
            InputText.SetColor(Color.Black);
            OkButton = new Button(new Image("menu/button backgrounds/empty", InputText.GetRectangle().Left, InputText.GetRectangle().Bottom + 10, 100, 35), new Image("menu/button backgrounds", "empty", InputText.GetRectangle().Left, InputText.GetRectangle().Bottom + 10, 100, 35, Color.Red), "Ok", Color.Black);
            BackButton = new Button(new Image("menu/button backgrounds/empty", CastleBridge.Graphics.PreferredBackBufferWidth - 100, 20, 100, 35), new Image("menu/button backgrounds", "empty", CastleBridge.Graphics.PreferredBackBufferWidth - 100, 20, 100, 35, Color.Red), "Back", Color.Black);
            Name = string.Empty;
            Text = new Text(FontType.Default_Bigger, Name, new Vector2(InputText.GetRectangle().X, InputText.GetRectangle().Y), Color.Gold, false, Color.Red);
            IsSelected = false;
            SelectedTeam = TeamName.None;

            //Initializes characters:
            InitCharacters();
        }

        /// <summary>
        /// Initializes both teams's characters
        /// </summary>
        private void InitCharacters() {
            RedCharacters = new List<Character>();
            YellowCharacters = new List<Character>();
 

            RedCharacters.Add(new Archer(CharacterName.Archer, TeamName.Red, 100, 250, 350, 500));
            RedCharacters.Add(new Knight(CharacterName.Knight, TeamName.Red, 100, 250, 350, 500));
            RedCharacters.Add(new Mage(CharacterName.Mage, TeamName.Red, 100, 250, 350, 500));

            YellowCharacters.Add(new Archer(CharacterName.Archer, TeamName.Yellow, 100, 250, 350, 500));
            YellowCharacters.Add(new Knight(CharacterName.Knight, TeamName.Yellow, 100, 250, 350, 500));
            YellowCharacters.Add(new Mage(CharacterName.Mage, TeamName.Yellow, 100, 250, 350, 500));

            CurrentCharacter = RedCharacters[0];
        }

        /// <summary>
        /// Receives selected character type name, selected team
        /// and updates current castle and current character by team and character parameters
        /// </summary>
        /// <param name="selectedCharacter"></param>
        /// <param name="selectedTeam"></param>
        public void SelectTeamAndCharacter(CharacterName selectedCharacter, TeamName selectedTeam) {

            switch (selectedTeam) {
                case TeamName.Red:
                    CurrentCastle = RedCastle;
                    break;
                case TeamName.Yellow:
                    CurrentCastle = YellowCastle;
                    break;
            }

            CurrentCharacter = GetSelectedCharacterByTeam(selectedCharacter ,selectedTeam);
        }

        /// <summary>
        /// Receives target character type name, team
        /// and returns selected character by team and character parameters
        /// </summary>
        /// <param name="targetCharacter"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        private Character GetSelectedCharacterByTeam(CharacterName targetCharacter, TeamName team) {

            if (team == TeamName.Red) {

                foreach (Character character in RedCharacters) {
                    if (character.GetName() == targetCharacter) {
                        return character;
                    }
                }
            }
            else if (team == TeamName.Yellow) {

                foreach (Character character in YellowCharacters) {
                    if (character.GetName() == targetCharacter) {
                        return character;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks which keys pressed
        /// </summary>
        private void GetKeys() {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //check if any of the previous update's keys are no longer pressed
            foreach (Keys key in lastPressedKeys) {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }

            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys) {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key);
            }
            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }

        /// <summary>
        /// Checks keys clicks and write name
        /// </summary>
        /// <param name="key"></param>
        private void OnKeyDown(Keys key) {
            //do stuff
            if (key == Keys.Back && Name.Length > 0) //Removes a letter from the name if there is a letter to remove
            {
                Name = Name.Remove(Name.Length - 1);
                Text.ChangeText(Name);
            }
            else if (key == Keys.Back && Name.Length == 0) {
                Name = string.Empty;
                Text.ChangeText(Name);
            }
            else if (key == Keys.CapsLock) {
                if (caps)
                    caps = false;
                else
                    caps = true;
            }
            else if (key == Keys.LeftShift || key == Keys.RightShift)//Sets caps to true if a shift key is pressed
            {
                caps = true;
            }
            else if(key == Keys.RightControl || key == Keys.LeftControl || key == Keys.RightAlt || key == Keys.LeftAlt || key == Keys.Enter ||
                key == Keys.Right || key == Keys.Left || key == Keys.Up || key == Keys.Down ||
                key == Keys.D1 || key == Keys.D2 || key == Keys.D3 || key == Keys.D4 || key == Keys.D5 || key == Keys.D6 || key == Keys.D7 ||
                key == Keys.D8 || key == Keys.D9 || key == Keys.D0) {

            }
            else if (!caps && Name.Length < 16) //If the name isn't too long, and !caps the letter will be added without caps
            {
                if (key == Keys.Space) {
                    Name += " ";
                    Text.ChangeText(Name);
                }
                else {
                    Name += key.ToString().ToLower();
                    Text.ChangeText(Name);
                }
            }
            else if (Name.Length < 16) //Adds the letter to the name in CAPS
            {
                Name += key.ToString();
                Text.ChangeText(Name);
            }
        }

        /// <summary>
        /// Checks if keys are up
        /// </summary>
        /// <param name="key"></param>
        private void OnKeyUp(Keys key) {
            //Sets caps to false if one of the shift keys goes up
            if (key == Keys.LeftShift || key == Keys.RightShift) {
                caps = false;
            }
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public override void Update() {

            //Check keys pressed:
            GetKeys();

            //Updates weather:
            Weather.Update();

            //Updates current character:
            CurrentCharacter.Update();

            //Checks when to activate name selected indication:
            if (Name.Length > 2)
                IsSelected = true;
            else
                IsSelected = false;

            //Update back button:
            BackButton.Update();

            //Update ok button:
            OkButton.Update();
        }

        /// <summary>
        /// Get back button
        /// </summary>
        /// <returns></returns>
        public Button GetBackButton() {
            return BackButton;
        }

        /// <summary>
        /// Get selected name
        /// </summary>
        /// <returns></returns>
        public string GetSelectedName() {
            return Name;
        }


        /// <summary>
        /// Get ok button
        /// </summary>
        /// <returns></returns>
        public Button GetOkButton() {
            return OkButton;
        }


        /// <summary>
        /// Draw name selection menu
        /// </summary>
        public override void Draw() {
            base.Draw();

            //Draw current castle:
            CurrentCastle.Draw();

            //Draw current character:
            CurrentCharacter.Draw();

            //Draw input text:
            InputText.Draw();

            //Draw text:
            Text.Draw();

            //Draw back button:
            BackButton.Draw();

            //Draw ok button only if name is written:
            if (IsSelected)
                OkButton.Draw();
        }
    }
}
