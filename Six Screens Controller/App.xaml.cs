using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Six_Screens_Controller
{
    public partial class App : Application
    {
        public Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));

        private System.Threading.Mutex mutex;
        Process process = new Process();
        private void App_Startup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            string mutName = "Application";
            mutex = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew)
            {
                this.Shutdown();
            }

            try
                {
                if (config.Python != null && config.Server != null)
                    Task.Run(() =>
                {
                    var psi = new ProcessStartInfo();
                    psi.FileName = config.Python;

                    string argument = $"\"{config.Server}\"";
                    foreach (string arg in config.ServerArgs)
                    {
                        argument += $" \"{arg}\"";
                    }

                    psi.Arguments = argument;
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardError = true;

                    process.StartInfo = psi;
                    process.Start();
                    var errors = process.StandardError.ReadToEnd();
                    var result = process.StandardOutput.ReadToEnd();

                    Console.WriteLine("ERRORS:");
                    Console.WriteLine(errors);
                    Console.WriteLine();
                    Console.WriteLine("RESULT:");
                    Console.WriteLine(result);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                process.Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
