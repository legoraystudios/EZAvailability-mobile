using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Model
{
    public class Connection : INotifyPropertyChanged
    {

        private string _apiUrl;

        public string ApiUrl { get { return _apiUrl; } 
            set {
                if (_apiUrl != value)
                {
                    _apiUrl = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_apiUrl)));
                }
            } 
        }

        public Connection(string url)
        {
            ApiUrl = url;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ApiUrl)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
