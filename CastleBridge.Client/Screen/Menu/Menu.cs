using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public abstract class Menu {

        protected Text Title; //Menu's title
        protected Image Grass; //Menu's grass image
        protected Weather Weather; //Menu's weather
        protected TeamName SelectedTeam; //Menu's selected team
        
        /// <summary>
        /// Receives title
        /// and creates a menu
        /// </summary>
        /// <param name="title"></param>
        public Menu(string title) {
            Title = new Text(FontType.Default_Bigger, title, new Vector2(CastleBridge.Graphics.PreferredBackBufferWidth / 2 - (CastleBridge.Graphics.PreferredBackBufferWidth / 4) + 15, 50), Color.Gold, true, Color.Black);
            Grass = new Image("map/forest/grass", 0, CastleBridge.Graphics.PreferredBackBufferHeight / 2 + 100, CastleBridge.Graphics.PreferredBackBufferWidth, CastleBridge.Graphics.PreferredBackBufferHeight / 2);
            Weather = new Weather(TimeType.Day, true, CastleBridge.Graphics.PreferredBackBufferWidth, 15);
        }

        /// <summary>
        /// Update
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Get selected team
        /// </summary>
        /// <returns></returns>
        public TeamName GetSelectedTeam() {
            return SelectedTeam;
        }

        /// <summary>
        /// Draw menu
        /// </summary>
        public virtual void Draw() {

            //Draw weather's stuck:
            Weather.DrawStuck();
            
            //Draw weather's clouds:
            Weather.DrawClouds();

            //Draw grass:
            Grass.Draw();

            //Draw title:
            Title.Draw();
        }

    }
}
