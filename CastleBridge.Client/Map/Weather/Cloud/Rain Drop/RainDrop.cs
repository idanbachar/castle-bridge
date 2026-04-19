using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBridge.Client {
    public class RainDrop {

        private Animation Animation; //Rain drop's animation
        private int Speed; //Rain drop's speed
        public bool IsFinished; //Rain drop's is finished indication

        /// <summary>
        /// Receives x,y coordinates and
        /// creates a rain drop
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public RainDrop(int x, int y) {

            Animation = new Animation("map/clouds/rain drop/drop_", new Rectangle(x, y, 5, 10), 0, 0, 1, 1, false, false);
            Animation.Start();
            Speed = 15;
            IsFinished = false;
        }

        /// <summary>
        /// Makes rain drop to fall
        /// </summary>
        public void Fall() {

            Animation.SetRectangle(Animation.GetCurrentSpriteImage().GetRectangle().X,
                                   Animation.GetCurrentSpriteImage().GetRectangle().Y + Speed,
                                   Animation.GetCurrentSpriteImage().GetRectangle().Width,
                                   Animation.GetCurrentSpriteImage().GetRectangle().Height);


            if (Animation.GetCurrentSpriteImage().GetRectangle().Bottom >= Map.HEIGHT)
                IsFinished = true;

        }

        /// <summary>
        /// Draw rain drop
        /// </summary>
        public void Draw() {

            //Draw rain drop:
            Animation.Draw();
        }
    }
}
