using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ristorante.Properties;

namespace Ristorante
{
    public class SocketServer
    {
        private readonly int _port;
        private readonly IPAddress _ipAddress;

        private readonly DataBase _database;

        public event EventHandler<OrderEventArgs> NewData;

        /// <summary>
        /// Initialize a socket server object
        /// </summary>
        /// <param name="port">Listening port</param>
        /// <param name="database">Orders database</param>
        public SocketServer(int port, DataBase database)
        {
            _port = port;
            _ipAddress = IPAddress.Any;

            _database = database;
        }

        /// <summary>
        /// Start the TCP listener
        /// </summary>
        public async void Run()
        {
            var listener = new TcpListener(_ipAddress, _port);
            listener.Start();

            while (true)
            {
                try
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();
                    await ProcessAsync(tcpClient);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Process requests from client
        /// </summary>
        /// <param name="tcpClient">TCP client to process requests from</param>
        private async Task ProcessAsync(TcpClient tcpClient)
        {
            try
            {
                var networkStream = tcpClient.GetStream();
                var reader = new StreamReader(networkStream);
                var writer = new StreamWriter(networkStream) { AutoFlush = true };

                var request = await reader.ReadLineAsync();
                if (request != null)
                {
                    request = request.ToUpperInvariant();
                    var response = "";

                    if (request.Contains("ORDER"))
                        response = await AddOrderAsync(request.Remove(0, 5));
                    else if (request.Contains("INFO"))
                        response = await GetInfoAsync();
                    else
                        response = "NOT VALID REQUEST";

                    await writer.WriteLineAsync(response);
                }

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Invoke the order event to add a new order
        /// </summary>
        /// <param name="request">String with order information</param>
        /// <returns>Return the order plates and order number</returns>
        private async Task<string> AddOrderAsync(string request)
        {
            var response = "";

            await Task.Run(() =>
            {
                var eventArgs = new OrderEventArgs(request);
                NewData?.Invoke(this, eventArgs);
                response = eventArgs.Response;
            });

            return response;
        }

        /// <summary>
        /// Get all the info to comunicate to the client
        /// </summary>
        /// <returns>Return all info for client</returns>
        private async Task<string> GetInfoAsync()
        {
            var allInfo = new StringBuilder();

            foreach (var item in await _database.GetPrinterDescriptionsAsync())
            {
                allInfo.Append(item);
                allInfo.Append(",");
            }

            foreach (var item in await _database.GetDisplayDescriptionsAsync())
            {
                allInfo.Append(item);
                allInfo.Append(",");
            }

            allInfo.Append(Settings.Default.printerHeading);

            allInfo.Append(",");

            allInfo.Append(Settings.Default.printerFooter);

            allInfo.Append(",");

            foreach (var item in await _database.GetPricesAsync())
            {
                allInfo.Append(item.Replace(',', '.'));
                allInfo.Append(",");
            }

            return allInfo.ToString();
        }
    }

    public class OrderEventArgs : EventArgs
    {
        public string Value { get; }
        public string Response { get; set; }

        public OrderEventArgs(string val)
        {
            Value = val;
            Response = "";
        }
    }
}