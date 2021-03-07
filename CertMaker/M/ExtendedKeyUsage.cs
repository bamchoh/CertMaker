using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class ExtendedKeyUsage : BindableBase, IDisposable, INotifyPropertyChanged
    {
		private bool serverAuth = true;
		public bool ServerAuth
		{
			get { return serverAuth; }
			set { this.SetProperty(ref serverAuth, value); }
		}

		private bool clientAuth = true;
		public bool ClientAuth
		{
			get { return clientAuth; }
			set { this.SetProperty(ref clientAuth, value); }
		}

		private bool codeSigning;
		public bool CodeSigning
		{
			get { return codeSigning; }
			set { this.SetProperty(ref codeSigning, value); }
		}

		private bool emailProtection;
		public bool EmailProtection
		{
			get { return emailProtection; }
			set { this.SetProperty(ref emailProtection, value); }
		}

		private bool timeStamping;
		public bool TimeStamping
		{
			get { return timeStamping; }
			set { this.SetProperty(ref timeStamping, value); }
		}

		private bool oCSPSigning;
		public bool OCSPSigning
		{
			get { return oCSPSigning; }
			set { this.SetProperty(ref oCSPSigning, value); }
		}

		private bool mSCTLSign;
		public bool MSCTLSign
		{
			get { return mSCTLSign; }
			set { this.SetProperty(ref mSCTLSign, value); }
		}

		private bool mSEFS;
		public bool MSEFS
		{
			get { return mSEFS; }
			set { this.SetProperty(ref mSEFS, value); }
		}

		public override string ToString()
		{
			var vals = new List<string>();
			if (ServerAuth)
				vals.Add("serverAuth");

			if (ClientAuth)
				vals.Add("clientAuth");

			if (CodeSigning)
				vals.Add("codeSigning");

			if (EmailProtection)
				vals.Add("emailProtection");

			if (TimeStamping)
				vals.Add("timeStamping");

			if (OCSPSigning)
				vals.Add("OCSPSigning");

			if (MSCTLSign)
				vals.Add("msCTLSign");

			if (MSEFS)
				vals.Add("msEFS");

			if (vals.Count > 0)
				return "extendedKeyUsage = " + string.Join(',', vals);
			else
				return "";
		}

		public void Dispose()
        {
            // NOPE;
        }
    }
}
