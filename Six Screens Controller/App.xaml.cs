using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Six_Screens_Controller.Models;

namespace Six_Screens_Controller
{
    public partial class App : Application
    {
        public Config Config { get; set; }

        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            BeforeStartup();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void BeforeStartup()
        {
            try
            {
                if (!File.Exists("config.json"))
                {
                    CreateConfigFile();
                }

                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

                if (!string.IsNullOrEmpty(Config.Protocol) && !string.IsNullOrEmpty(Config.Host) && !string.IsNullOrEmpty(Config.Port))
                {
                    string[] args = { "--urls", $"{Config.Protocol}://{Config.Host}:{Config.Port}" };
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

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
        }

        private static void CreateConfigFile()
        {
            string localIP = GetLocalIPAddress();
            Config config = new Config() { Protocol = "http", Host = localIP, Port = "8800", FirstStart = true };
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
