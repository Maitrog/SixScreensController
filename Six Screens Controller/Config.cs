using System;
using System.ComponentModel;
using System.Linq;

namespace Six_Screens_Controller
{
    public class Config : INotifyPropertyChanged
    {
        private string protocol;
        private string host;
        private string port;
        private string defaultImage;
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
            if ((obj as Config) != null)
                return ((obj as Config).protocol == protocol) && ((obj as Config).host == host) && ((obj as Config).port == port) && ((obj as Config).defaultImage == defaultImage);
            return false;
        }
    }
}
