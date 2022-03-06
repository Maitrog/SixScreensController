using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Six_Screens_Controller
{
    public partial class App : Application
    {
        public Config config;

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (!File.Exists("config.json"))
                {
                    CreateConfigFile();
                }
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

                if (!string.IsNullOrEmpty(config.Protocol) && !string.IsNullOrEmpty(config.Host) && !string.IsNullOrEmpty(config.Port))
                {
                    string[] args = { "--urls", $"{config.Protocol}://{config.Host}:{config.Port}" };
                    Task.Run(() =>
                    {
                        SixScreenControllerApi.Program.Main(args);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void CreateConfigFile()
        {
            string localIP = GetLocalIPAddress();
            Config config = new Config() { Protocol = "http", Host = localIP, Port = "5000", FirstStart = true };
            Encoding utf8 = Encoding.UTF8;

            using (FileStream fs = File.Create("config.json"))
            {
                fs.Write(utf8.GetBytes(JsonConvert.SerializeObject(config)));
            }
        }

        private static string GetLocalIPAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
    }
}
