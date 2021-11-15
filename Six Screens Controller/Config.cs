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
        private string background_1;
        private string background_2;
        private string background_3;
        private string background_4;
        private string background_5;
        private string background_6;


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

        public string Background_1 
        { 
            get => background_1;

            set
            {
                if (value != null)
                {
                    background_1 = value;
                    RaisePropertyChanged("Background_1");
                }
            }
        }
        public string Background_2
        {
            get => background_2;
            set
            {
                if (value != null)
                {
                    background_2 = value;
                    RaisePropertyChanged("Background_2");
                }
            }
        }
        public string Background_3 
        { 
            get => background_3;
            set
            {
                if (value != null)
                {
                    background_3 = value;
                    RaisePropertyChanged("Background_3");
                }
            }
        }
        public string Background_4 
        { 
            get => background_4;
            set
            {
                if (value != null)
                {
                    background_4 = value;
                    RaisePropertyChanged("Background_4");
                }
            }
        }
        public string Background_5 
        { 
            get => background_5;
            set
            {
                if (value != null)
                {
                    background_5 = value;
                    RaisePropertyChanged("Background_5");
                }
            }
        }
        public string Background_6 
        { 
            get => background_6;
            set
            {
                if (value != null)
                {
                    background_6 = value;
                    RaisePropertyChanged("Background_6");
                }
            }
        }

        public override bool Equals(object obj)
        {
            if ((obj as Config) != null)
                return ((obj as Config).protocol == protocol) && ((obj as Config).host == host) && ((obj as Config).port == port) && ((obj as Config).defaultImage == defaultImage)
                    && ((obj as Config).background_1 == Background_1) && ((obj as Config).Background_2 == Background_2) && ((obj as Config).Background_3 == Background_3)
                    && ((obj as Config).Background_4 == Background_4) && ((obj as Config).Background_5 == Background_5) && ((obj as Config).Background_6 == Background_6);
            return false;
        }
    }
}
