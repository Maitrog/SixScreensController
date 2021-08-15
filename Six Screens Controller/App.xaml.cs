using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (config.Python != "" && config.Server != "")
                    process.Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
