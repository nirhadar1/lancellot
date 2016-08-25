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
using System.Data.SqlClient;


namespace WebApplication1.DBConnector
{
    public class DBConnector
    {

        string _dbSchema;
        string _dbHostname;
        string _dbSid;
        int _dbPort;
        string _dbUser;
        string _dbPassword;
        int _dbReconnectInterval;

        private SqlConnection _connection = null;
        //private static DBConnector _instance = null;
        private string _databaseName = string.Empty;
        System.Timers.Timer _timer; // From System.Timers
        public Database _database;

        private static DBConnector instance;
        public static DBConnector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBConnector();
                    DBConnector.Instance._database = new Database();
                }
                return instance;
            }
        }

        public Database getDB()
        {
           return _database;
        }


        public void start()
        {
            bool ok = Connect();
            if (ok)
            {
                if (_database != null) {
                    _database = new Database();
                    _database.init(_connection);
                }
            }
        }

        void setTimer(int timerTickSec)
        {

            _timer = new System.Timers.Timer(timerTickSec * 1000); // Set up the timer for 3 seconds
                                                                   //
                                                                   // Type "_timer.Elapsed += " and press tab twice.
                                                                   //
            _timer.Elapsed += new ElapsedEventHandler(onTimer);
            _timer.Enabled = true; // Enable it
        }


        void onTimer(object sender, ElapsedEventArgs e)
        {
            if (! IsConnected)
            {
                start();
            }
        }

        public bool init(String configFilePath)
        {         
            if (!File.Exists(configFilePath))
            {
                return false;
            }

            var MyIni = new Common.IniFile(configFilePath);

            _dbSchema = MyIni.Read("db_schema", "DATABASE");
            _dbHostname = MyIni.Read("db_hostname", "DATABASE");
            _dbSid = MyIni.Read("db_sid", "DATABASE");
            _dbPort = Int32.Parse(MyIni.Read("db_port", "DATABASE"));
            _dbUser = MyIni.Read("db_user", "DATABASE");
            _dbPassword = MyIni.Read("db_password", "DATABASE");
            _dbReconnectInterval = Int32.Parse(MyIni.Read("db_reconnectInterval", "DATABASE"));

            //setTimer(_dbReconnectInterval);
            start();

            return true;

        }

        
        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        public string Password { get; set; }
        
        public SqlConnection IsConnect
        {
            get { return _connection; }
        }

        public bool IsConnected
        {
            get { return _connection.State.ToString() == System.Data.ConnectionState.Open.ToString(); }
        }

        public bool Connect()
        {
            bool result = true;
            if (IsConnect == null)
            {
                try {
                    if (String.IsNullOrEmpty(_databaseName))
                        result = false;
                    string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}",
                        _dbHostname, _dbSchema, _dbUser, _dbPassword);
                    _connection = new SqlConnection(connstring);
                    _connection.Open();
                    result = true;
                }

                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
