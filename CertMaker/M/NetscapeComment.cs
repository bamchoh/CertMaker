using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class NetscapeComment : BindableBase, IDisposable, INotifyPropertyChanged
    {
        private string nsComment = "OpenSSL Generated Certificate";
        public string NSComment
        {
            get { return nsComment; }
            set { this.SetProperty(ref nsComment, value); }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(nsComment))
                return $"nsComment = \"{nsComment}\"";
            else
                return "";
        }

        public void Dispose() { }
    }
}
