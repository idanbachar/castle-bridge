using System;
using System.Collections.Generic;
using System.Text;

namespace CastleBridge.OnlineLibraries {

    [Serializable]
    public class MapEntityPacket {

        public int X;
        public int Y;
        public string Direction;
        public string Name;
        public bool IsTouchable;
        public string CurrentLocation;
        public bool IsActive;
        public string Key;
        public MapEntityPacket() {


        }

    }
}
