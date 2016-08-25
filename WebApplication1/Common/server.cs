using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;



namespace WebApplication1.Common
{
    using ConnectionId_t = System.Int32;

    //class StateObject
    //{
    //    /* Contains the state information. */
    //    private int id;
    //    private bool close = false; // Used to close the socket after the message sent.

    //    public Socket listener = null;
    //    public const int BufferSize = 1024;
    //    public byte[] buffer = new byte[BufferSize];
    //    public StringBuilder sb = new StringBuilder();

    //    public StateObject() { }

    //    public int Id
    //    {
    //        get { return this.id; }
    //        set { this.id = value; }
    //    }

    //    public bool Close
    //    {
    //        get { return this.close; }
    //        set { this.close = value; }
    //    }
    //}


    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class TCPSocketServer
    {

        public delegate void onMessageHandler(Socket handler, string message);
        //Defining event based on the above delegate
        public event onMessageHandler OnMessageEvent;


        private static Int32 _port;
        ConnectionId_t _currentConnectionId = 0;
        private Int32 _maxConnections;

//        Socket _listener;

        private static ushort limit = 250;

        private static ManualResetEvent mre = new ManualResetEvent(false);
        private static Dictionary<int, StateObject> clients = new Dictionary<int, StateObject>();

        #region Event handler
        public delegate void MessageReceivedHandler(Socket handler, String msg);
        public event MessageReceivedHandler MessageReceived;

        //public delegate void MessageSubmittedHandler(int id, bool close);
       // public static event MessageSubmittedHandler MessageSubmitted;
        #endregion

        private static TCPSocketServer instance;
        public static TCPSocketServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TCPSocketServer();                   
                }
                return instance;
            }
        }

        public bool init(String configFilePath, int maxConnections = 0)
        {
            _maxConnections = maxConnections;

            var MyIni = new Common.IniFile(configFilePath);

            _port = Int32.Parse(MyIni.Read("port", "JSON"));

            return true;
        }

        public void start()
        {
            StartListening();
        }


        ///* Starts the AsyncSocketListener */
        //public void StartListening()
        //{                     
        //    IPAddress ip = System.Net.IPAddress.Parse("127.0.0.1");
        //    IPEndPoint socket = new IPEndPoint(ip, _port);

        //    try
        //    {
        //        using (_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        //        {
        //            _listener.Bind(socket);
        //            _listener.Listen(limit);
        //            while (true)
        //            {
        //                mre.Reset();
        //                _listener.BeginAccept(new AsyncCallback(OnClientConnect), _listener);
        //                mre.WaitOne();
        //            }
        //        }
        //    }
        //    catch (SocketException)
        //    {
        //        // TODO:
        //    }
        //}

        ///* Gets a socket from the clients dictionary by his Id. */
        //private static StateObject getClient(int id)
        //{
        //    StateObject state = new StateObject();
        //    return clients.TryGetValue(id, out state) ? state : null;
        //}

        ///* Checks if the socket is connected. */
        //public static bool IsConnected(int id)
        //{
        //    StateObject state = getClient(id);
        //    return !(state.listener.Poll(1000, SelectMode.SelectRead) && state.listener.Available == 0);
        //}

        ///* Add a socket to the clients dictionary. Lock clients temporary to handle multiple access.
        // * ReceiveCallback raise a event, after the message receive complete. */
        //#region Receive data
        //public void OnClientConnect(IAsyncResult result)
        //{
        //    mre.Set();
        //    StateObject state = new StateObject();
        //    try
        //    {
        //        lock (clients)
        //        {
        //            state.Id = !clients.Any() ? 1 : clients.Keys.Max() + 1;
        //            clients.Add(state.Id, state);
        //            Console.WriteLine("Client connected. Get Id " + state.Id);
        //        }
        //        state.listener = (Socket)result.AsyncState;
        //        state.listener = state.listener.EndAccept(result);
        //        state.listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
        //    }
        //    catch (SocketException)
        //    {
        //        // TODO:
        //    }
        //}

        //public void ReceiveCallback(IAsyncResult result)
        //{
        //    StateObject state = (StateObject)result.AsyncState;
        //    try
        //    {
        //        String content = String.Empty;

        //        int receive = state.listener.EndReceive(result);
        //        if (receive > 0)
        //            state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, receive));
        //        if (receive == StateObject.BufferSize)
        //            state.listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
        //        else
        //        {
        //            MessageReceived(state.Id, state.sb.ToString());


        //            // Check for end-of-file tag. If it is not there, read 
        //            // more data.
        //            content = state.sb.ToString();

        //            // All the data has been read from the 
        //            // client. Display it on the console.
        //            Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
        //                content.Length, content);
        //            // Parse the Message from client.
        //            OnMessageEvent(state.Id, content);


        //            state.sb = new StringBuilder();
        //        }
        //    }
        //    catch (SocketException)
        //    {
        //        // TODO:
        //    }
        //}
        //#endregion

        ///* Send(int id, String msg, bool close) use bool to close the connection after the message sent. */
        //#region Send data
        //public static void Send(int id, String msg, bool close)
        //{
        //    StateObject state = getClient(id);
        //    if (state == null)
        //        throw new Exception("Client does not exist.");
        //    if (!IsConnected(state.Id))
        //        throw new Exception("Destination socket is not connected.");
        //    try
        //    {
        //        byte[] send = Encoding.UTF8.GetBytes(msg);
        //        state.Close = close;
        //        state.listener.BeginSend(send, 0, send.Length, SocketFlags.None, new AsyncCallback(SendCallback), state);
        //    }
        //    catch (SocketException)
        //    {
        //        // TODO:
        //    }
        //    catch (ArgumentException)
        //    {
        //        // TODO:
        //    }
        //}

        //private static void SendCallback(IAsyncResult result)
        //{
        //    StateObject state = (StateObject)result.AsyncState;
        //    try
        //    {
        //        //state.listener.EndSend(result);
        //    }
        //    catch (SocketException)
        //    {
        //        // TODO:
        //    }
        //    catch (ObjectDisposedException)
        //    {
        //        // TODO:
        //    }
        //    finally
        //    {
        //        MessageSubmitted(state.Id, state.Close);
        //    }
        //}
        //#endregion

        //public static void Close(int id)
        //{
        //    StateObject state = getClient(id);
        //    if (state == null)
        //        throw new Exception("Client does not exist.");
        //    try
        //    {
        //        state.listener.Shutdown(SocketShutdown.Both);
        //        state.listener.Close();
        //    }
        //    catch (SocketException)
        //    {
        //        // TODO:
        //    }
        //    finally
        //    {
        //        lock (clients)
        //        {
        //            clients.Remove(state.Id);
        //            Console.WriteLine("Client disconnected with Id {0}", state.Id);
        //        }
        //    }
        //}

      

       // public class AsynchronousSocketListener
        //{
            // Thread signal.
            public static ManualResetEvent allDone = new ManualResetEvent(false);

            public TCPSocketServer()
            {
            }

            public void StartListening()
            {
                // Data buffer for incoming data.
                byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            // IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ip = System.Net.IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ip, _port);

            ///IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP socket.
                Socket listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);

                    while (true)
                    {
                        // Set the event to nonsignaled state.
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.
                        Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        // Wait until a connection is made before continuing.
                        allDone.WaitOne();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                //Console.WriteLine("\nPress ENTER to continue...");
                //Console.Read();

            }

            public void AcceptCallback(IAsyncResult ar)
            {
                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }

            public void ReadCallback(IAsyncResult ar)
            {
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();
                    state.sb.Clear();
                        // All the data has been read from the 
                        // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content);

                    MessageReceived(handler, content);

                 content = String.Empty;

                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                                                
            }
                else {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }

            }

            public static void Send(Socket handler, String data)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.
                    Socket handler = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.
                    int bytesSent = handler.EndSend(ar);
                    Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                   // handler.Shutdown(SocketShutdown.Both);
                   // handler.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }


        }

    }
