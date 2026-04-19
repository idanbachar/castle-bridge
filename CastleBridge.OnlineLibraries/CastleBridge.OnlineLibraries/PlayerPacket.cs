using System;
using System.Net.Sockets;

namespace CastleBridge.OnlineLibraries {

    [Serializable]
    public class PlayerPacket {

        public string Name;
        public string CharacterName;
        public string TeamName;
        public int CurrentSpeed;
        public RectanglePacket Rectangle;
        public PacketType PacketType;
        public string Direction;
        public string PlayerState;
        public string CurrentLocation;
        public int CurrentCharacterHp;
        public bool IsAttackAnimationFinished;
        public bool CurrentCharacterIsDead;
        public bool IsHorseOwner;
        public bool IsAllMapEntitiesLoaded;
        public bool IsDiamondOwner;
        public int CarryingDiamondsCount;

        public PlayerPacket() {
 

        }
    }
}
