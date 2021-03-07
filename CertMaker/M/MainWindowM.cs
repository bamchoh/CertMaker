using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class MainWindowM : BindableBase, IDisposable, INotifyPropertyChanged
    {
        private OpenSLLConfig _openSSLConfig;
        public OpenSLLConfig OpenSSLConfig {
            get { return _openSSLConfig; }
            set { this.SetProperty(ref _openSSLConfig, value); }
        }

        public MainWindowM()
        {
            _openSSLConfig = new OpenSLLConfig();
        }

        public void Dispose()
        {
            // NOPE;
        }
    }
}
