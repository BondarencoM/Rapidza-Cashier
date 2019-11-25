using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapidzaCashier.Connection
{
    class WebConnection
    {

        public Socket socket;
        public EndPoint endpointLocal, endpointRemote;

        byte[] tempMessageBuffer;

        // this list stores all the messages 
        public event EventHandler<String> messageReceived;

        //constructore
        public WebConnection()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        public void connect(string connectonIP, string port )
        {
            endpointLocal = new IPEndPoint(IPAddress.Parse(GetLocalIP()), Convert.ToInt32(port));
            socket.Bind(endpointLocal);

            endpointRemote = new IPEndPoint(IPAddress.Parse(connectonIP), Convert.ToInt32(port));
            socket.Connect(endpointRemote);

            startConnection();

        }

        private void startConnection()
        {
            tempMessageBuffer = new byte[1500];
            socket.BeginReceiveFrom(tempMessageBuffer, 0, tempMessageBuffer.Length, SocketFlags.None, ref endpointRemote, new AsyncCallback(MessageCallBack), tempMessageBuffer);
        }

        // A method to get your IP address from DNS server
        public static string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";

        }
        public void sendMessage(string msg)
        {
            byte[] sendingMessage = encodeMessage(msg);
            socket.Send(sendingMessage);
            Console.WriteLine("con: message sent");
        }

        private static byte[] encodeMessage(string msg)
        {
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];
            sendingMessage = aEncoding.GetBytes(msg);
            return sendingMessage;
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                byte[] receivedData = new byte[1500];
                receivedData = (byte[])aResult.AsyncState;

                string receivedMessage = decodeBytes(receivedData);
                if (!String.IsNullOrWhiteSpace(receivedMessage))
                {
                    MessageBox.Show(receivedMessage);
                    messageReceived?.Invoke(this, receivedMessage);
                }

                startConnection();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static string decodeBytes(byte[] receivedData)
        {
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            string receivedMessage = aEncoding.GetString(receivedData);
            return receivedMessage;
        }

        public void close()
        {
            socket.Close();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

    }
}
