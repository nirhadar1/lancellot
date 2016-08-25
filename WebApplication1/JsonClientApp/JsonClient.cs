using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonClientApp
{
    class JsonClient
    {

        private Int32 _port;
        string _addrToConnect = "127.0.0.1";
        string _messagesFile ="";
        int _timerTickSec = 15;

        System.Timers.Timer _timer; // From System.Timers
        private JsonClientApp.tcpClient _tcpClient;
        private static JsonClient instance;

        public bool execute(string[] arguments)
        {
            //EBS_LOG_TRACE;

             if (arguments.Length > 1)
            {
                //EBS_LOG_CONSOLE("PricingManager " << PricingManager_VERSION);
                return false;
            }

            // Command Line: ./PricingManager <CONFIG_FILE> (-c)
            if (arguments.Length != 1)
            {
                //EBS_LOG_CONSOLE("Usage: ./PricingManager <CONFIG_FILE> (-c)\n\t-c\tUse consule mode")
                return false;
            }

            String configFilePath = arguments[0];

            if (!File.Exists(configFilePath))
            {
                return false;
            }

            var MyIni = new Common.IniFile(configFilePath);

            _port = Int32.Parse(MyIni.Read("port", "JSON"));
            _addrToConnect = MyIni.Read("addrToConnect", "JSON");

            _messagesFile = MyIni.Read("jsonFileName", "JSON");             

            _timerTickSec = Int32.Parse(MyIni.Read("timerTimeSeconds", "JSON"));

            setTimer(_timerTickSec);

            var threads = new List<Thread>();

            _tcpClient = new JsonClientApp.tcpClient();
            _tcpClient.init(configFilePath);
      
            Thread clientThread = new Thread(_tcpClient.start);
            clientThread.Start();
            threads.Add(clientThread);

            foreach (var thread in threads)
                thread.Join();

            return true;

        }

        public static JsonClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JsonClient();
                }
                return instance;
            }
        }

        void setTimer(int timerTickSec)
        {
            
            _timer = new System.Timers.Timer(timerTickSec *1000); // Set up the timer for 3 seconds
                                      //
                                      // Type "_timer.Elapsed += " and press tab twice.
                                      //
            _timer.Elapsed += new ElapsedEventHandler(onTimer);
            _timer.Enabled = true; // Enable it
        }

        void onTimer(object sender, ElapsedEventArgs e)
        {
            try {
                //var jsonObject = new JObject();
                //string requestType = jsonObject.GetValue("attrib1").Value<string>();

                // read JSON directly from a file
                StreamReader file = File.OpenText(_messagesFile);
                JsonTextReader reader = new JsonTextReader(file);
                
                JObject jsonObj = (JObject)JToken.ReadFrom(reader);

                string jsonMessage = jsonObj.ToString();

                _tcpClient.Send(jsonMessage);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
