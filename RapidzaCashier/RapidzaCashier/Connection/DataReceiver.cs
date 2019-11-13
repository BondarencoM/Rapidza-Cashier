using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RapidzaCashier.Connection
{
    class DataReceiver
    {
        private TcpListener listener;
        private bool isAborted = false;

        public event EventHandler<string> messageReceived;
        public DataReceiver(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            byte[] data = new byte[3000];
            listener.AcceptTcpClientAsync().ContinueWith(handleRequest);
        }

        private void handleRequest(Task<TcpClient> task)
        {
            if (isAborted)
                return;

            byte[] data = new byte[3000];
            UInt16 messageLength=0;
            try
            {
                NetworkStream stream = task.Result.GetStream();
                stream.Read(data, 0, data.Length);

                messageLength = BitConverter.ToUInt16(data, 0);

                string message = Encoding.ASCII.GetString(data, 2, messageLength);

                Console.WriteLine($"Received a message of {messageLength} bytes:\n{message}");

                if (!string.IsNullOrWhiteSpace(message) && message != "ping")
                    messageReceived?.Invoke(this, message);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Ignoring request with invalid length prefix({messageLength}):");
                Console.WriteLine(Encoding.ASCII.GetString(data));
            }
            finally
            {
                listener.AcceptTcpClientAsync().ContinueWith(handleRequest);
            }

        }

        public void close()
        {

            isAborted = true;
            listener.Stop();
        }


    }
}
