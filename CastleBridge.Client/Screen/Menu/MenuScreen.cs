using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class MenuScreen: Screen {

        private MenuPage CurrentPage; //Current menu page
        private Dictionary<MenuPage, Menu> Menus; //Menu pages
        private TeamName SelectedTeam; //Selected team
        private CharacterName SelectedCharacter; //Selected character
        private string SelectedName; //Selected name
        private bool IsPressedLeftButton; //Left button pressed indication
        private Text CopyrightText;

        //Start join game event:
        public delegate void StartGame(CharacterName characterName, TeamName team, string selectedName);
        public event StartGame OnStartGame;

        /// <summary>
        /// Creates menu screen
        /// </summary>
        /// <param name="viewPort"></param>
        public MenuScreen(Viewport viewPort) : base(viewPort) {
            Menus = new Dictionary<MenuPage, Menu>();
            SelectedTeam = TeamName.None;
            SelectedCharacter = CharacterName.None;
            SelectedName = "Idan";
            IsPressedLeftButton = false;

            //Initializes:
            Init();
        }

        /// <summary>
        /// Initializes menu pages:
        /// </summary>
        private void Init() {

            Menus.Add(MenuPage.MainMenu, new MainMenu("Main Menu"));
            Menus.Add(MenuPage.TeamSelection, new TeamSelectionMenu("Select a team by pressing castle sides"));
            Menus.Add(MenuPage.CharacterSelection, new CharacterSelectionMenu("Select a character to play with"));
            Menus.Add(MenuPage.NameSelection, new NameSelectionMenu("Select a name for your character"));
            Menus.Add(MenuPage.Loading, new LoadingMenu("Loading. Please wait.."));
            Menus.Add(MenuPage.Error, new ErrorMenu("Connection lost :("));
            CurrentPage = MenuPage.TeamSelection;

            //init copyright text:
            CopyrightText = new Text(FontType.Default, "Made by Idan Bachar.", new Vector2(CastleBridge.Graphics.PreferredBackBufferWidth - 320, CastleBridge.Graphics.PreferredBackBufferHeight - 50), Color.White, false, Color.White);
        }

        /// <summary>
        /// Updates menu pages stuff:
        /// </summary>
        public override void Update() {

            //Updates only current menu page:
            Menus[CurrentPage].Update();

            switch (CurrentPage) {

                //Team selection menu:
                case MenuPage.TeamSelection:
                    TeamSelectionMenu teamSelectionMenu = Menus[CurrentPage] as TeamSelectionMenu;

                    //Checks if pressed 'ok' button -> go to character selection:
                    if (teamSelectionMenu.GetOkButton().IsClicking()) {
                        if (teamSelectionMenu.IsSelected) {
                            SelectedTeam = teamSelectionMenu.GetSelectedTeam();
                            GoToPage(MenuPage.CharacterSelection);
                            ((CharacterSelectionMenu)Menus[CurrentPage]).SelectCastleByTeam(SelectedTeam);
                        }
                    }
                    break;
                //Character selection menu:
                case MenuPage.CharacterSelection:
                    CharacterSelectionMenu characterSelectionMenu = Menus[CurrentPage] as CharacterSelectionMenu;

                    //Checks if pressed 'back' button -> go to team selection:
                    if (characterSelectionMenu.GetBackButton().IsClicking() && !IsPressedLeftButton) {

                        GoToPage(MenuPage.TeamSelection);
                        break;
                    }
                    //Run on all displayed selectable characters:
                    foreach (Character character in characterSelectionMenu.GetCurrentCharacters()) {

                        //Checks if pressed on a current character:
                        if (character.IsMouseOver()) {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !IsPressedLeftButton) {
                                IsPressedLeftButton = true;
                                SelectedCharacter = character.GetName();
                                characterSelectionMenu.SelectCharacter(character);
                                break;
                            }
                        }
                    }

                    //Checks if pressed 'ok' button:
                    if (characterSelectionMenu.GetOkButton().IsClicking() && !IsPressedLeftButton) {

                        //Checks if a character has been selected:
                        if (SelectedCharacter != CharacterName.None)
                            GoToPage(MenuPage.NameSelection); //go to name selection
                    }
                    break;
                //Name selection menu:
                case MenuPage.NameSelection:
                    NameSelectionMenu nameSelectionMenu = Menus[CurrentPage] as NameSelectionMenu;
                    nameSelectionMenu.SelectTeamAndCharacter(SelectedCharacter, SelectedTeam);

                    //Checks if pressed 'back' button -> go to character selection:
                    if (nameSelectionMenu.GetBackButton().IsClicking() && !IsPressedLeftButton) {
                        IsPressedLeftButton = true;
                        GoToPage(MenuPage.CharacterSelection);
                        break;
                    }

                    //Checks if pressed 'ok' button:
                    if (nameSelectionMenu.GetOkButton().IsClicking() && !IsPressedLeftButton) {

                        //Checks if a name has been selected:
                        if (nameSelectionMenu.IsSelected) {
                            SelectedName = nameSelectionMenu.GetSelectedName();

                            //Starts join game session event:
                            OnStartGame(SelectedCharacter, SelectedTeam, SelectedName);
                        }
                    }
                    break;
                //Error menu:
                case MenuPage.Error:
                    ErrorMenu errorMenu = Menus[CurrentPage] as ErrorMenu;

                    //Checks if pressed 'back' button -> go to name selection:
                    if (errorMenu.GetBackButton().IsClicking() && !IsPressedLeftButton) {
                        IsPressedLeftButton = true;
                        GoToPage(MenuPage.NameSelection);
                        break;
                    }
                    break;
            }

            //Check mouse's left button press:
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !IsPressedLeftButton) {
                IsPressedLeftButton = true;
            }

            //Check mouse's left button released:
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                IsPressedLeftButton = false;
        }

        /// <summary>
        /// Receives a page and applies it
        /// </summary>
        /// <param name="page"></param>
        public void GoToPage(MenuPage page) {
            CurrentPage = page;
        }

        /// <summary>
        /// Receives a menu page name and returns the menu
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Menu GetMenu(MenuPage page) {
            return Menus[page];
        }

        /// <summary>
        /// Change current menu's page to error page
        /// </summary>
        public void ConnectionLost() {
            GoToPage(MenuPage.Error);
        }

        /// <summary>
        /// Draw current menu page
        /// </summary>
        public override void Draw() {

            CastleBridge.SpriteBatch.Begin();

            //Draw only current menu page:
            Menus[CurrentPage].Draw();

            //Draw copyright text:
            CopyrightText.Draw();

            CastleBridge.SpriteBatch.End();
        }
    }
}
