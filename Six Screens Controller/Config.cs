using System;
using System.ComponentModel;
using System.Linq;

namespace Six_Screens_Controller
{
    /// <summary>
    /// Application configuration
    /// </summary>
    public class Config : INotifyPropertyChanged
    {
        private string protocol;
        private string host;
        private string port;
        private string defaultImage;
        private bool firstStart;

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Server protocol
        /// </summary>
        public string Protocol
        {
            get => protocol;
            set
            {
                if (value != null)
                {
                    protocol = value;
                    RaisePropertyChanged("Protocol");
                }
            }
        }

        /// <summary>
        /// Server host
        /// </summary>
        public string Host
        {
            get => host;
            set
            {
                if (value != null)
                {
                    host = value;
                    RaisePropertyChanged("Host");
                }
            }
        }
        /// <summary>
        /// Server port
        /// </summary>
        public string Port
        {
            get => port;
            set
            {
                if (value != null)
                {
                    port = value;
                    RaisePropertyChanged("Port");
                }
            }
        }
        /// <summary>
        /// Application default image
        /// </summary>
        public string DefaultImage
        {
            get => defaultImage;
            set
            {
                if (value != null)
                {
                    defaultImage = value;
                    RaisePropertyChanged("DefaultImage");
                }
            }
        }

        /// <summary>
        /// Is first start or not
        /// </summary>
        public bool FirstStart
        {
            get => firstStart;
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
