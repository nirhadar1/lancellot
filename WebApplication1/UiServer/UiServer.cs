using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Timers;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Net.Sockets;

namespace Lacellot
{
    using ConnectionId_t  = System.Int32;
    using Common;

    public class DrawsRequest
    {
        public string request_type { get; set; }
        public string  from_record { get; set; }
        public string to_record { get; set; }
        public string sort_type { get; set; }
        public string sort_dir { get; set; }
        public string device_type { get; set; }
        public string device_id { get; set; }
        public string  decode { get; set; }
        //public string Email { get; set; }
        //public bool Active { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public IList<string> Roles { get; set; }
    }

public class UIServer
    {

        private static System.Timers.Timer _timer;       
        private bool _isConnected;
        private Int32 _sendHBInterval;
        private Int32 _port;
        private Int32 _heartBeatInterval;

        private Dictionary<string, string> _jsonRequest;



        public UIServer() {
            _isConnected = false;
            _sendHBInterval = 0;
        }

        ~UIServer(){
        }

        public bool init( String configFilePath , int maxConnections = 0) {
           
            var jsonObject = new JObject();
            var MyIni = new Common.IniFile(configFilePath);
            
            _port = Int32.Parse(MyIni.Read("port", "JSON"));
            _heartBeatInterval = Int32.Parse(MyIni.Read("heartBeatInterval", "JSON"));


            return true;
	}


        public void onMessageReceived(Socket handler, String data )
        {
            //Q_UNUSED(connectionId);
            parseMsg(handler, data);
        }

  

        void parseMsg(Socket handler, String json)
        {
            
            JObject jo = JObject.Parse(json);
            //JArray items = (JArray)jo["getDrawsRequest"];
            int numberOfItems = jo.Count;

            JEnumerable<JToken> jEnum = jo.Children();
            string firstElement = jo.First.First.Path.ToString();
            //string firstElement = jEnum.ElementAt(0).ToString();
            //JToken token = (JArray)jo.Children
            Console.WriteLine(jo.ToString());

            switch (firstElement)
            {
                case "getDrawsRequest":

                    handleGetDrawsRequest(handler, jo);
                    break;
                    // ...
            }

        }

        public void start()
        {
            _timer = new System.Timers.Timer(_heartBeatInterval * 1000);// (_ => onSendHB(), null, 0, 1000 * 10); //every 10 seconds
            _timer.Enabled = true;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(onSendHB);
            _timer.Start();
        }

    

        private static void onSendHB(object sender, ElapsedEventArgs args)
        {
            int i = 1;
            i++;         
        }


        private void handleGetDrawsRequest(Socket handler, JObject jo)
        {
            JArray items = (JArray)jo["getDrawsRequest"];
            _jsonRequest = new Dictionary<string, string>();
            lock (_jsonRequest)
            {
               

                foreach (JObject content in items.Children<JObject>())
                {

                    foreach (JProperty prop in content.Properties())
                    {
                        string key = prop.Name;
                        string value = (string)prop.Value;

                        _jsonRequest.Add(key, value);
                    }
                }

            }

            sendDrawsRequestResponse(handler);
        }


        public void sendDrawsRequestResponse(Socket handler)
        {
            DrawRequestRespons drawResponse = new DrawRequestRespons();
            int amount = 4000;
            bool resualt = DBConnector.DBConnector.Instance.getDB().getDrawsRequestResualt("x", "y", "z", "m", "n", "c", "v", "b", amount, drawResponse);
           
            JObject jsonObj = new JObject();
                       
            JObject responseData = new JObject(                                                                  
                                   new JProperty("Id", drawResponse.Id),
                                   new JProperty("Amount", drawResponse.Amount));
            
            jsonObj["DrawsResponse"] = responseData;
          
            string jsonMessage = jsonObj.ToString();

            Console.WriteLine(drawResponse.print());
            Common.TCPSocketServer.Send(handler, jsonMessage);

        }
    } //Class UIServer
}//namespace Lancellot
