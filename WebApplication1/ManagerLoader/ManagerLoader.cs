using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Lacellot
{

    using ConnectionId_t = System.Int32;

    public class ManagerLoader
    {

        private static ManagerLoader instance;

        private Int32 _port;
        private Int32 _heartBeatInterval;

        //private Common.TCPSocketServer _server;
//        private UIServer _uiServer;
        private DBConnector.DBConnector _dbConnector;
        public static ManagerLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ManagerLoader();
                }
                return instance;
            }
        }

        private ManagerLoader() {
            /*	EBS_LOG_TRACE;

                char* configDir = getenv("LOG4CXX_CONFIG_DIR");
                QByteArray log4cxxXmlFileName;
                if (configDir != NULL) {
                    log4cxxXmlFileName = configDir;
                }
                else {
                    log4cxxXmlFileName = ".";
                }
                log4cxxXmlFileName += "/PricingManager_log4cxx.xml";

                if (!QFile::exists(log4cxxXmlFileName)) {
                    qErrnoWarning("Failed to find log4cxx configuration XML: %s. Errno:", qPrintable(log4cxxXmlFileName));
                }

                // Loads the log4cxx file, and keep checking once every 5 minutes if to reload again
                log4cxx::xml::DOMConfigurator::configureAndWatch(qPrintable(log4cxxXmlFileName), RELOAD_LOG4CXX_INTERVAL_MILLIS);
            */
        }


        ~ManagerLoader() { }

        public bool execute(string[] arguments)
        {
            //EBS_LOG_TRACE;
           
            if (arguments.Length > 1)
            {
                //EBS_LOG_CONSOLE("PricingManager " << PricingManager_VERSION);
                return false;
            }

            // Command Line: ./PricingManager <CONFIG_FILE> (-c)
            if (arguments.Length != 1 )
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
            _heartBeatInterval = Int32.Parse(MyIni.Read("heartBeatInterval", "JSON"));

            bool ok = DBConnector.DBConnector.Instance.init(configFilePath);
            //_dbConnector = new DBConnector.DBConnector();
            //_dbConnector.init(configFilePath);

            //var threads = new List<Thread>();

            //ok = Common.TCPSocketServer.Instance.init(configFilePath, 100);
            ////_server = new Common.TCPSocketServer();
            ////_server.init(configFilePath, 100);

            //_uiServer = new UIServer();
            //_uiServer.init(configFilePath, 100);


            //Common.TCPSocketServer.Instance.OnMessageEvent += new Common.TCPSocketServer.onMessageHandler(_uiServer.onMessageReceived);
            //Common.TCPSocketServer.Instance.MessageReceived += new Common.TCPSocketServer.MessageReceivedHandler(_uiServer.onMessageReceived);

            //Thread dbConnectorThread = new Thread(DBConnector.DBConnector.Instance.start);
            //dbConnectorThread.Start();
            //threads.Add(dbConnectorThread);

            //Thread serverThread = new Thread(Common.TCPSocketServer.Instance.start);
            //serverThread.Start();
            //threads.Add(serverThread);

            //Thread uiServerThread = new Thread(_uiServer.start);
            //uiServerThread.Start();
            //threads.Add(uiServerThread);


            //foreach (var thread in threads)
            //    thread.Join();

        
            
            return true;

        }

        void onListeningOnPortFailed(int port)
        {
            //EBS_LOG_FATAL("ManagerLoader failed listening on port " << port);
        }

        void onListeningOnPortSucceeded(int port)
        {
            String msg = "ManagerLoader succeeded listening on port ";
            msg += port;
            Console.WriteLine(msg);
        }



        //void onMessageReceived(String data, ConnectionId_t connectionId)
        //{
        //    _uiServer.onMessageReceived(data);//, connectionId);
        //}

    }
}
