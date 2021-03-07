using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class SubjectKeyIdentifier : BindableBase, IDisposable, INotifyPropertyChanged
    {
        private bool hash = true;
        public bool Hash
        {
            get { return hash; }
            set { this.SetProperty(ref hash, value); }
        }

        public override string ToString()
        {
            if (hash)
                return "subjectKeyIdentifier=hash";
            else
                return "";
        }

        public void Dispose() { }
    }
}
