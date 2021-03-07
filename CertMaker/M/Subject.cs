using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class Subject : BindableBase, IDisposable, INotifyPropertyChanged
    {
		private string _commonName = $"{System.Net.Dns.GetHostName()}";
		public string CommonName
		{
			get { return _commonName; }
			set { this.SetProperty(ref _commonName, value); }
		}

		private string _organization = "組織名";
		public string Organization
		{
			get { return _organization; }
			set { this.SetProperty(ref _organization, value); }
		}

		private string _organizationUnit = "組織単位名";
		public string OrganizationUnit
		{
			get { return _organizationUnit; }
			set { this.SetProperty(ref _organizationUnit, value); }
		}

		private string _locality = "市町村名";
		public string Locality
		{
			get { return _locality; }
			set { this.SetProperty(ref _locality, value); }
		}

		private string _state = "都道府県名";
		public string State
		{
			get { return _state; }
			set { this.SetProperty(ref _state, value); }
		}

		private string _country = "JP";
		public string Country
		{
			get { return _country; }
			set { this.SetProperty(ref _country, value); }
		}

		private string _emailAddress = "your-name@example.com";
		public string EmailAddress
        {
			get { return _emailAddress; }
			set { this.SetProperty(ref _emailAddress, value); }
        }

		private string _domainComponent = $"{System.Net.Dns.GetHostName()}";
		public string DomainComponent
        {
			get { return _domainComponent; }
			set { this.SetProperty(ref _domainComponent, value); }
        }

		public void Dispose()
        {
            // NOPE;
        }
    }
}
