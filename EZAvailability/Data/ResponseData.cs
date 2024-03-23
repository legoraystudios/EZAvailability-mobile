using EZAvailability.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Data
{
    public class ResponseData : BaseViewModel
    {
        private int _statusCode;
        public int StatusCode
        {
            get { return _statusCode; }
            set
            {
                if (_statusCode != value)
                {
                    _statusCode = value;
                    OnPropertyChanged(nameof(StatusCode));
                }
            }
        }

        private string _jsonResponse;
        public string JsonResponse
        {
            get { return _jsonResponse; }
            set
            {
                if (_jsonResponse != value)
                {
                    _jsonResponse = value;
                    OnPropertyChanged(nameof(JsonResponse));
                }
            }
        }
    }
}
