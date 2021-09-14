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
            config.FirstStart = false;
            config.DefaultImage = Directory.GetCurrentDirectory() + "\\assets\\Emblem_of_the_Russian_Ground_Forces.jpg";
            File.WriteAllText("config.json", JsonConvert.SerializeObject(config));

            Task.Run(() =>
            {
                string curDir = Directory.GetCurrentDirectory();
                var psi = new ProcessStartInfo();
                string fileName = $"{curDir}/Api/SixScreenControllerApi.exe";
                psi.FileName = fileName;
                psi.Arguments = $"--urls {config.Protocol}://{config.Host}:{config.Port}";
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
