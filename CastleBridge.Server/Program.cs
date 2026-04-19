using System;
using System.Net;
using System.Net.Sockets;

namespace CastleBridge.Server {
    class Program {
        static void Main(string[] args) {

            //Start server:
            Server server = new Server("127.0.0.1", 4441);
            server.Start();
        }
    }
}
