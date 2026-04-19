using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class CharacterSelectionMenu : Menu {

        private Image RedCastle; //Red castle image
        private Image YellowCastle; //Yellow castle image
        private Image CurrentCastle; //Current castle image
        private List<Character> RedCharacters; //Red characters
        private List<Character> YellowCharacters; //Yellow characters
        private List<Character> CurrentCharacters; //Current characters

        private Button BackButton; //Back button
        private Button OkButton; //Ok button

        private Image ViSymbol; //Vi symbol image

        private CharacterName SelectedCharacter; //Selected character

        /// <summary>
        /// Receives title
        /// and creates a menu
        /// </summary>
        /// <param name="title"></param>
        public CharacterSelectionMenu(string title): base(title) {
            RedCastle = new Image("map/castles/teams/red/outside", "castle", 0, 100, 1400, 431, Color.White);
            YellowCastle = new Image("map/castles/teams/yellow/outside", "castle", 0, 100, 1400, 431, Color.White);
            CurrentCastle = RedCastle;

            BackButton = new Button(new Image("menu/button backgrounds/empty", CastleBridge.Graphics.PreferredBackBufferWidth - 100, 20, 100, 35), new Image("menu/button backgrounds", "empty", CastleBridge.Graphics.PreferredBackBufferWidth - 100, 20, 100, 35, Color.Red), "Back", Color.Black);
            OkButton = new Button(new Image("menu/button backgrounds/empty", 0, CastleBridge.Graphics.PreferredBackBufferHeight - 100, 100, 35), new Image("menu/button backgrounds", "empty", 0, CastleBridge.Graphics.PreferredBackBufferHeight - 100, 100, 35, Color.Red), "Next", Color.Black);

            ViSymbol = new Image("menu/symbols/vi", 0, 0, 105, 70);
            SelectedCharacter = CharacterName.None;

            //Initializes characters:
            InitCharacters();
        }

        /// <summary>
        /// Receives a selected character and 
        /// setting a vi image in top of selected character's position
        /// </summary>
        /// <param name="selectedCharacter"></param>
        public void SelectCharacter(Character selectedCharacter) {

            foreach (Character character in CurrentCharacters) {
                if (character == selectedCharacter) {

                    SelectedCharacter = selectedCharacter.GetName();

                    ViSymbol.SetRectangle(character.GetCurrentAnimation().GetCurrentSpriteImage().GetRectangle().X + character.GetCurrentAnimation().GetCurrentSpriteImage().GetRectangle().Width / 2,
                                          character.GetCurrentAnimation().GetCurrentSpriteImage().GetRectangle().Top - 50,
                                          ViSymbol.GetRectangle().Width,
                                          ViSymbol.GetRectangle().Height);
                }
            }
        }

        /// <summary>
        /// Initializes both teams's characters
        /// </summary>
        private void InitCharacters() {
            RedCharacters = new List<Character>();
            YellowCharacters = new List<Character>();
            CurrentCharacters = new List<Character>();

            RedCharacters.Add(new Archer(CharacterName.Archer, TeamName.Red, 300, 250, 250, 400));
            RedCharacters.Add(new Knight(CharacterName.Knight, TeamName.Red, 600, 250, 250, 400));
            RedCharacters.Add(new Mage(CharacterName.Mage, TeamName.Red, 900, 250, 250, 400));

            YellowCharacters.Add(new Archer(CharacterName.Archer, TeamName.Yellow, 300, 250, 250, 400));
            YellowCharacters.Add(new Knight(CharacterName.Knight, TeamName.Yellow, 600, 250, 250, 400));
            YellowCharacters.Add(new Mage(CharacterName.Mage, TeamName.Yellow, 900, 250, 250, 400));

            CurrentCharacters = RedCharacters;
        }

        /// <summary>
        /// Receives target team
        /// and selectes characters of target team
        /// </summary>
        /// <param name="targetTeam"></param>
        public void SelectCastleByTeam(TeamName targetTeam) {

            switch (targetTeam) {
                case TeamName.Red:
                    CurrentCastle = RedCastle;
                    CurrentCharacters = RedCharacters;
                    break;
                case TeamName.Yellow:
                    CurrentCastle = YellowCastle;
                    CurrentCharacters = YellowCharacters;
                    break;
            }
        }

        /// <summary>
        /// Updates stuff
        /// </summary>
        public override void Update() {

            //Updates weather:
            Weather.Update();

            //Updates back button:
            BackButton.Update();

            //Updates ok button:
            OkButton.Update();

            //Updates each character:
            foreach (Character character in CurrentCharacters)
                character.Update();
        }

        /// <summary>
        /// Get back button
        /// </summary>
        /// <returns></returns>
        public Button GetBackButton() {
            return BackButton;
        }

        /// <summary>
        /// Get ok button
        /// </summary>
        /// <returns></returns>
        public Button GetOkButton() {
            return OkButton;
        }

        /// <summary>
        /// Get current characters
        /// </summary>
        /// <returns></returns>
        public List<Character> GetCurrentCharacters() {
            return CurrentCharacters;
        }

        /// <summary>
        /// Draw character selection menu
        /// </summary>
        public override void Draw() {
            base.Draw();

            //Draw current castle:
            CurrentCastle.Draw();

            //Draw characters:
            foreach (Character character in CurrentCharacters)
                character.Draw();

            //Draw back button:
            BackButton.Draw();

            //Draw ok button:
            OkButton.Draw();

            //Draw vi symbol only if characters has been selected:
            if (SelectedCharacter != CharacterName.None)
                ViSymbol.Draw();
        }

    }
}
