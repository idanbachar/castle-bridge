using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class Weather {

        private TimeType TimeType; //Weather's time type
        private Image Sun; //Weather's sun
        private Image Moon; //Weather's moon
        private Image DaySky; //Weather's day sky
        private Image NightSky; //Weather's night sky
        private Image CurrentPlanet; //Weather's current displayed planet (sun/moon)
        private Image CurrentSky; //Weather's current displayed sky (day/night)
        private List<Cloud> Clouds; ////Weather's clouds
        private Random Rnd;
        private bool IsRain; //Weather's is raining indication
        private int ScreenWidth; //Screen width
        private int NumberClouds; //Weather's number of clouds

        /// <summary>
        /// Receives time type, is raining indication, screen width, number of clouds
        /// </summary>
        /// <param name="timeType"></param>
        /// <param name="isRain"></param>
        /// <param name="screenWidth"></param>
        /// <param name="numberClouds"></param>
        public Weather(TimeType timeType, bool isRain, int screenWidth, int numberClouds) {

            Clouds = new List<Cloud>();

            ScreenWidth = screenWidth;
            NumberClouds = numberClouds;
            IsRain = isRain;

            //Initializes:
            Init();

            //Sets time by time type parameter:
            SetTime(timeType);
        }

        /// <summary>
        /// Update clouds
        /// </summary>
        public void Update() {

            //Update clouds:
            foreach (Cloud cloud in Clouds)
                cloud.Update();
        }

        /// <summary>
        /// Initializes stuff
        /// </summary>
        private void Init() {

            Rnd = new Random();

            //Initializes clouds:
            InitClouds();

            //Initializes planets:
            InitPlanets();
        }

        /// <summary>
        /// Initializes clouds
        /// </summary>
        private void InitClouds() {

            for (int i = 0; i < NumberClouds; i++)
                Clouds.Add(new Cloud(Rnd.Next(0, ScreenWidth), Rnd.Next(0, 200), 125, 75, IsRain, ScreenWidth));
        }

        /// <summary>
        /// Initializes planets
        /// </summary>
        private void InitPlanets() {

            Sun = new Image("map/sun", "sun_0", CastleBridge.Graphics.PreferredBackBufferWidth / 2 - 75, 0, 150, 150, Color.White);
            Moon = new Image("map/moon", "moon_0", CastleBridge.Graphics.PreferredBackBufferWidth / 2 - 75, 0, 150, 150, Color.White);

            DaySky = new Image("map/sky", "day", 0, 0, CastleBridge.Graphics.PreferredBackBufferWidth, CastleBridge.Graphics.PreferredBackBufferHeight, Color.White);
            NightSky = new Image("map/sky", "night", 0, 0, CastleBridge.Graphics.PreferredBackBufferWidth, CastleBridge.Graphics.PreferredBackBufferHeight, Color.White);
        }

        /// <summary>
        /// Receivs time type and applies it
        /// </summary>
        /// <param name="timeType"></param>
        public void SetTime(TimeType timeType) {

            TimeType = timeType;

            switch (TimeType) {
                case TimeType.Day:
                    CurrentPlanet = Sun;
                    CurrentSky = DaySky;
                    break;
                case TimeType.Night:
                    CurrentPlanet = Moon;
                    CurrentSky = NightSky;
                    break;
            }
        }

        /// <summary>
        /// Get clouds
        /// </summary>
        /// <returns></returns>
        public List<Cloud> GetClouds() {
            return Clouds;
        }

        /// <summary>
        /// Get sky
        /// </summary>
        /// <returns></returns>
        public Image GetSky() {
            return CurrentSky;
        }

        /// <summary>
        /// Get sun
        /// </summary>
        /// <returns></returns>
        public Image GetSun() {
            return Sun;
        }

        /// <summary>
        /// Draw clouds
        /// </summary>
        public void DrawClouds() {

            //Draw clouds:
            foreach (Cloud cloud in Clouds)
                cloud.Draw();
        }

        /// <summary>
        /// Draw stuck
        /// </summary>
        public void DrawStuck() {
            
            //Draw current displayed sky:
            CurrentSky.Draw();

            //Draw current displayed planet:
            CurrentPlanet.Draw();
        }
    }
}
