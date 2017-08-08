using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoClient
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var dResponse = await SendRequest();

            MessageBox.Show(dResponse);
        }

        private async Task<string> SendRequest()
        {
            try
            {
                var ipAddress = IPAddress.Parse(ipAddressTextBox.Text);
                var port = Convert.ToInt32(portNumericUpDown.Value);
                var client = new TcpClient();

                await client.ConnectAsync(ipAddress, port);

                var networkStream = client.GetStream();

                var writer = new StreamWriter(networkStream) {AutoFlush = true};
                var reader = new StreamReader(networkStream);

                var requestData = textBox1.Text;

                await writer.WriteLineAsync(requestData);

                var response = await reader.ReadLineAsync();

                client.Close();

                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
