using System;
using System.Collections.Generic;
using System.Text;

namespace CastleBridge.OnlineLibraries {

    [Serializable]
    public class RectanglePacket {

        public int X;
        public int Y;
        public int Width;
        public int Height;

        public RectanglePacket(int x, int y, int width, int height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

    }
}
