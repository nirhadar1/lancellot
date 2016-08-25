using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace JsonClientApp
{
    class tcpClient
    {

        string _addrToConnect = "127.0.0.1";
        int _port = 5000;
        Socket s;

        Byte[] buffer = new Byte[32768];

        public tcpClient()
        {
            try
            {

                IPAddress host = IPAddress.Parse(_addrToConnect);

                IPEndPoint hostep = new IPEndPoint(host, _port);

                /* Connecting to the server. */

                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                s.Connect(hostep);

                if (s.Connected)
                {
                    /* if the client is connected, 
                    will show this information below, then call the Read() method. */

                    Console.WriteLine("\n [CLIENT] We are connected to the server: " + _addrToConnect + " on port: " + _port);

                    Thread readTread = new Thread(new ThreadStart(Read));

                    readTread.Start();

                    /* Sending the welcome message to the server, 
                    and the server will responds with HELLO CLIENT. */

                    // Send("HELLO SERVER!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public bool init(String configFilePath, int maxConnections = 0)
        {            
            var MyIni = new Common.IniFile(configFilePath);

            _port = Int32.Parse(MyIni.Read("port", "JSON"));
            _addrToConnect = MyIni.Read("addrToConnect", "JSON");         

            return true;
        }

        public void start()
        {
        }

        void Read()
        {
            while (true)
            {
                try
                {
                    /* Reading server messages. */
                   
                    int receivedDataLength = s.Receive(buffer, SocketFlags.None);

                    string message =
                    Encoding.ASCII.GetString(buffer, 0, receivedDataLength);

                    Console.WriteLine("\nMessage from the server: " + message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Send(string msg)
        {
            /* Sending message to the server. */

            Byte[] buff = new Byte[32768];
            buff = Encoding.ASCII.GetBytes(msg);

            s.Send(buff);
            
        }


    }
}

