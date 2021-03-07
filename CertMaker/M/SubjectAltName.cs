using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class SubjectAltName : BindableBase, IDisposable, INotifyPropertyChanged
    {
        private string uri = $"urn:{System.Net.Dns.GetHostName()}:Example:CertGenApplication";
        public string URI
        {
            get { return uri; }
            set { this.SetProperty(ref uri, value); }
        }

        private string dns = $"{System.Net.Dns.GetHostName()}";
        public string DNS
        {
            get { return dns; }
            set { this.SetProperty(ref dns, value); }
        }

        private string ip = "";
        public string IP
        {
            get { return ip; }
            set { this.SetProperty(ref ip, value); }
        }

        public override string ToString()
        {
            var dnsList = new List<string>();
            foreach(var text in dns.Split('\n'))
            {
                if (!string.IsNullOrEmpty(text.Trim()))
                    dnsList.Add(text.Trim());
            }

            var ipList = new List<string>();
            foreach(var text in ip.Split('\n'))
            {
                if (!string.IsNullOrEmpty(text.Trim()))
                    ipList.Add(text.Trim());
            }

            if(string.IsNullOrEmpty(uri) && dnsList.Count == 0 && ipList.Count == 0)
            {
                return "";
            }

            var s = new System.Text.StringBuilder();
            s.AppendLine("subjectAltName = @alt_names");
            s.AppendLine();
            s.AppendLine("[alt_names]");

            if (!string.IsNullOrEmpty(uri))
            {
                s.AppendLine(string.Format("URI={0}", uri));
            }

            for(int i=0;i<dnsList.Count;i++)
            {
                if (string.IsNullOrEmpty(dnsList[i]))
                    continue;

                if(string.IsNullOrEmpty(dnsList[i].Trim()))
                    continue;

                s.AppendLine(string.Format("DNS.{0}={1}", i+1, dnsList[i].Trim()));
            }

            for(int i=0;i<ipList.Count;i++)
            {
                if (string.IsNullOrEmpty(ipList[i]))
                    continue;

                if (string.IsNullOrEmpty(ipList[i].Trim()))
                    continue;

                s.AppendLine(string.Format("IP.{0}={1}", i+1, ipList[i].Trim()));
            }
            
            return s.ToString();
        }

        public void Dispose() { }
    }
}
