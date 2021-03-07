using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class BasicConstraints : BindableBase, IDisposable, INotifyPropertyChanged
    {
        private bool ca;
        public bool CA
        {
            get { return ca; }
            set { this.SetProperty(ref ca, value); }
        }

        public override string ToString()
        {
            if (ca)
                return $"basicConstraints = CA:true,pathlen:0";
            else
                return "basicConstraints = CA:FALSE";
        }

        public void Dispose() { }
    }
}
