using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Six_Screens_Controller
{
    public partial class App : Application
    {
        public Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        readonly Process process = new Process();

        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (!string.IsNullOrEmpty(config.Protocol) && !string.IsNullOrEmpty(config.Host) && !string.IsNullOrEmpty(config.Port))
            {
                string[] args = { "--urls", $"{config.Protocol}://{config.Host}:{config.Port}" };
                Task.Run(() =>
                {
                    SixScreenControllerApi.Program.Main(args);
                });
            }
        }
    }
}
