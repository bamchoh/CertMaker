using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class AuthorityKeyIdentifier : BindableBase, IDisposable, INotifyPropertyChanged
    {
        private bool keyID = true;
        public bool KeyID
        {
            get { return keyID; }
            set { this.SetProperty(ref keyID, value); }
        }

        private bool issuer = true;
        public bool Issuer
        {
            get { return issuer; }
            set { this.SetProperty(ref issuer, value); }
        }

        public override string ToString()
        {
            var vals = new List<string>();
            if (keyID)
                vals.Add("keyid");

            if (issuer)
                vals.Add("issuer");

            if (vals.Count > 0)
                return "authorityKeyIdentifier = " + string.Join(',', vals);
            else
                return "";
        }

        public void Dispose() { }
    }
}
