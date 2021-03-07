using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class KeyUsage : BindableBase, IDisposable, INotifyPropertyChanged
    {

		private bool digitalSignature;
		public bool DigitalSignature
		{
			get { return digitalSignature; }
			set { this.SetProperty(ref digitalSignature, value); }
		}

		private bool nonRepudiation;
		public bool NonRepudiation
		{
			get { return nonRepudiation; }
			set { this.SetProperty(ref nonRepudiation, value); }
		}

		private bool keyEncipherment;
		public bool KeyEncipherment
        {
			get { return keyEncipherment; }
			set { this.SetProperty(ref keyEncipherment, value); }
		}

		private bool dataEncipherment;
		public bool DataEncipherment
		{
			get { return dataEncipherment; }
			set { this.SetProperty(ref dataEncipherment, value); }
		}

		private bool keyAgreement;
		public bool KeyAgreement
		{
			get { return keyAgreement; }
			set { this.SetProperty(ref keyAgreement, value); }
		}

		private bool certificateSigning;
		public bool CertificateSigning
		{
			get { return certificateSigning; }
			set { this.SetProperty(ref certificateSigning, value); }
		}

		private bool cRLSigning;
		public bool CRLSigning
		{
			get { return cRLSigning; }
			set { this.SetProperty(ref cRLSigning, value); }
		}

		private bool encipherOnly;
		public bool EncipherOnly
		{
			get { return encipherOnly; }
			set { this.SetProperty(ref encipherOnly, value); }
		}

		private bool decipherOnly;
		public bool DecipherOnly
		{
			get { return decipherOnly; }
			set { this.SetProperty(ref decipherOnly, value); }
		}

        public override string ToString()
        {
			var vals = new List<string>();
			if (DigitalSignature)
				vals.Add("digitalSignature");

			if (NonRepudiation)
				vals.Add("nonRepudiation");

			if (KeyEncipherment)
				vals.Add("keyEncipherment");

			if (DataEncipherment)
				vals.Add("dataEncipherment");

			if (KeyAgreement)
				vals.Add("keyAgreement");

			if (CertificateSigning)
				vals.Add("keyCertSign");

			if (CRLSigning)
				vals.Add("cRLSign");

			if (EncipherOnly)
				vals.Add("encipherOnly");

			if (DecipherOnly)
				vals.Add("decipherOnly");

			if (vals.Count > 0)
				return "keyUsage = " + string.Join(',', vals);
			else
				return "";
		}

		public void Dispose()
        {
            // NOPE;
        }
    }
}
