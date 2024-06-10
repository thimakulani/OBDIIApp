using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OBDIIApp.Services
{
    public class OBDIIScannerWIFI
    {
        private TcpClient client;
        private NetworkStream stream;

        public async Task<bool> ConnectToOBDIIAdapter(string ipAddress, int port)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ipAddress, port);
                stream = client.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to OBD-II adapter: " + ex.Message);
                return false;
            }
        }

        public async Task<string> SendCommand(string command)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(command);
                await stream.WriteAsync(data, 0, data.Length);

                byte[] responseBuffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending command to OBD-II adapter: " + ex.Message);
                return null;
            }
        }

        public async Task DisconnectFromOBDIIAdapter()
        {
            try
            {
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disconnecting from OBD-II adapter: " + ex.Message);
            }
        }
    }
}