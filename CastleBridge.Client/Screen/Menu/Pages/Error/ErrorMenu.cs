using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class ErrorMenu : Menu {

        private Text Text; //Error text
        private Button BackButton; //Back button

        /// <summary>
        /// Receives title
        /// and creates a menu
        /// </summary>
        /// <param name="title"></param>
        public ErrorMenu(string title): base(title) {

            Text = new Text(FontType.Default_Bigger, "Connection lost :(", new Vector2(CastleBridge.Graphics.PreferredBackBufferWidth / 2 - 100, CastleBridge.Graphics.PreferredBackBufferHeight / 2 - 100), Color.Gold, true, Color.Black);
            Title.SetVisible(false);

            BackButton = new Button(new Image("menu/button backgrounds/empty", CastleBridge.Graphics.PreferredBackBufferWidth - 100, 20, 100, 35), new Image("menu/button backgrounds", "empty", CastleBridge.Graphics.PreferredBackBufferWidth - 100, 20, 100, 35, Color.Red), "Back", Color.Black);
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public override void Update() {

            //Updates weather:
            Weather.Update();

            //Update back button:
            BackButton.Update();
        }
 
        /// <summary>
        /// Get text
        /// </summary>
        /// <returns></returns>
        public Text GetText() {
            return Text;
        }

        /// <summary>
        /// Get back button
        /// </summary>
        /// <returns></returns>
        public Button GetBackButton() {
            return BackButton;
        }

        /// <summary>
        /// Draw loading menu
        /// </summary>
        public override void Draw() {
            base.Draw();

            //Draw text:
            Text.Draw();

            //Draw back button:
            BackButton.Draw();
        }

    }
}
