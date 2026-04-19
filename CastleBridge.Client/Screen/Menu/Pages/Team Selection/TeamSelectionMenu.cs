using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class TeamSelectionMenu : Menu {

        private Button RedCastle; //Red castle button
        private Button YellowCastle; //Yellow castle buttn
        private Button OkButton; //Ok button
        public bool IsSelected; //Is team selected indication

        /// <summary>
        /// Receives title
        /// and creates a menu
        /// </summary>
        /// <param name="title"></param>
        public TeamSelectionMenu(string title): base(title) {

            RedCastle = new Button(new Image("menu/castles selection/castle_left_side_noteam", 0, 100, 700, 431), new Image("menu/castles selection/castle_left_side_redteam", 0, 100, 700, 431), string.Empty, Color.White);
            YellowCastle = new Button(new Image("menu/castles selection/castle_right_side_noteam", RedCastle.GetCurrentImage().GetRectangle().Right, 100, 700, 431), new Image("menu/castles selection/castle_right_side_yellowteam", RedCastle.GetCurrentImage().GetRectangle().Right, 100, 700, 431), string.Empty, Color.White);
            OkButton = new Button(new Image("menu/button backgrounds/empty", 0, CastleBridge.Graphics.PreferredBackBufferHeight - 100, 100, 35), new Image("menu/button backgrounds" ,"empty", 0, CastleBridge.Graphics.PreferredBackBufferHeight - 100, 100, 35, Color.Red), "Next", Color.Black);

            IsSelected = false;
            SelectedTeam = TeamName.None;
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public override void Update() {

            //Updates red castle:
            RedCastle.Update();

            //Update yellow castle:
            YellowCastle.Update();

            //Checks if clicking on red castle:
            if (RedCastle.IsClicking()) {
                IsSelected = RedCastle.PressedCounts <= RedCastle.MaxPresses ? true : false;
                SelectedTeam = TeamName.Red;
                YellowCastle.Reset();
            }

            //Checks if clicking on yellow castle:
            if (YellowCastle.IsClicking()) {
                IsSelected = YellowCastle.PressedCounts <= YellowCastle.MaxPresses ? true : false;
                SelectedTeam = TeamName.Yellow;
                RedCastle.Reset();
            }

            //Checks if there is no team selected:
            if(RedCastle.PressedCounts < RedCastle.MaxPresses && YellowCastle.PressedCounts < YellowCastle.MaxPresses) {
                IsSelected = false;
                SelectedTeam = TeamName.None;
            }

            //Updates weather:
            Weather.Update();

            //Updates ok button:
            OkButton.Update();
        }

        //Get red castle button:
        public Button GetRedCastle() {
            return RedCastle;
        }

        //Get yellow castle button:
        public Button GetYellowCastle() {
            return YellowCastle;
        }

        //Get ok button:
        public Button GetOkButton() {
            return OkButton;
        }
 
        //Draw team selection menu
        public override void Draw() {
            base.Draw();

            //Draw red castle:
            RedCastle.Draw();

            //Draw yellow castle:
            YellowCastle.Draw();

            //Draw ok button only if team is selected:
            if (IsSelected)
                OkButton.Draw();
        }
    }
}
