using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Six_Screens_Controller
{
    public class Config : INotifyPropertyChanged
    {
        private string protocol;
        private string host;
        private string port;
        private string defaultImage;
        private string python;
        private string server;
        private string[] serverArgs;
        private bool firstStart;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Protocol
        {
            get { return protocol; }
            set
            {
                if (value != null)
                {
                    protocol = value;
                    RaisePropertyChanged("Protocol");
                }
            }
        }
        public string Host
        {
            get { return host; }
            set
            {
                if (value != null)
                {
                    host = value;
                    RaisePropertyChanged("Host");
                }
            }
        }
        public string Port
        {
            get { return port; }
            set
            {
                if (value != null)
                {
                    port = value;
                    RaisePropertyChanged("Port");
                }
            }
        }
        public string DefaultImage
        {
            get { return defaultImage; }
            set
            {
                if (value != null)
                {
                    defaultImage = value;
                    RaisePropertyChanged("DefaultImage");
                }
            }
        }
        public string Python
        {
            get { return python; }
            set
            {
                if (value != null)
                {
                    python = value;
                    RaisePropertyChanged("Python");
                }
            }
        }
        public string Server
        {
            get { return server; }
            set
            {
                if (value != null)
                {
                    server = value;
                    RaisePropertyChanged("Server");
                }
            }
        }
        public string[] ServerArgs
        {
            get { return serverArgs; }
            set
            {
                if (value != null)
                {
                    Array.Resize(ref serverArgs, value.Length);
                    value.CopyTo(serverArgs, 0);
                    RaisePropertyChanged("ServerArgs");
                }
            }
        }
        public bool FirstStart
        {
            get { return firstStart; }
            set
            {
                firstStart = value;
                RaisePropertyChanged("FirstStart");
            }
        }

        public override bool Equals(object obj)
        {
            if((obj as Config) != null)
                return ((obj as Config).protocol==this.protocol) && ((obj as Config).host == this.host) && ((obj as Config).port == this.port) && ((obj as Config).defaultImage== this.defaultImage) &&
                    ((obj as Config).python== this.python) && ((obj as Config).server== this.server) && ((obj as Config).serverArgs.Intersect(serverArgs).Any());
            return false;
        }
    }
}
