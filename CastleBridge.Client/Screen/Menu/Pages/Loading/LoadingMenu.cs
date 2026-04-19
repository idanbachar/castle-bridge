using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class LoadingMenu : Menu {

        private Text Text; //Loading text

        /// <summary>
        /// Receives title
        /// and creates a menu
        /// </summary>
        /// <param name="title"></param>
        public LoadingMenu(string title): base(title) {

            Text = new Text(FontType.Default_Bigger, "Connecting to server...", new Vector2(CastleBridge.Graphics.PreferredBackBufferWidth / 3, CastleBridge.Graphics.PreferredBackBufferHeight / 2 - 100), Color.Gold, true, Color.Black);
            Title.SetVisible(false);
        }

        /// <summary>
        /// Receives current items downloaded, max items downloaded
        /// and updates loading map in percents
        /// </summary>
        /// <param name="currentItemsDownloaded"></param>
        /// <param name="maxItemsToDownload"></param>
        public void UpdateText(int currentItemsDownloaded, int maxItemsToDownload) {

            double percent = ((double)currentItemsDownloaded / (double)maxItemsToDownload) * 100;

            Text.ChangeText("Downloading map data: (" + (int)percent + "%)");
        }

        /// <summary>
        /// Update stuff
        /// </summary>
        public override void Update() {

            //Updates weather:
            Weather.Update();

        }
 
        /// <summary>
        /// Get text
        /// </summary>
        /// <returns></returns>
        public Text GetText() {
            return Text;
        }

        /// <summary>
        /// Draw loading menu
        /// </summary>
        public override void Draw() {
            base.Draw();

            //Draw text:
            Text.Draw();
        }

    }
}
