using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapidzaCashier.Connection
{
    class DataSender
    {
        private TcpClient client;

        string ip;
        int port;

        public DataSender(string ip, int port)
        {
            setDestination(ip, port);
        }

        public void setDestination(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void sendMessage(string message)
        {
            byte[] data = new byte[3000];

            byte[] messageLength = BitConverter.GetBytes((UInt16)message.Length);
            data[0] = messageLength[0];
            data[1] = messageLength[1];

            Encoding.ASCII.GetBytes(message, 0, message.Length, data, 2);
            Console.WriteLine("Sending message:\n" + message);
            using (client = new TcpClient())
            {
                try
                {
                    client.ReceiveTimeout = 5000;
                    client.Connect(ip, port);
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }catch(Exception e)
                {
                    MessageBox.Show($"Could not send message \n {e.Message} \n Please check the connection information", "Connection error");
                }
                
            }
                
        }

    }
}
