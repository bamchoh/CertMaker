using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;

using Prism.Mvvm;

namespace CertMaker.M
{
    public class OpenSLLConfig : BindableBase, IDisposable, INotifyPropertyChanged
    {
        public Subject Subject { get; set; }

        public KeyUsage KeyUsage { get; set; }

        public ExtendedKeyUsage ExtKeyUsage { get; set; }

        public BasicConstraints BasicConstraints { get; set; }

        public NetscapeComment NetscapeComment { get; set; }

        public SubjectKeyIdentifier SubjectKeyIdentifier { get; set; }

        public AuthorityKeyIdentifier AuthorityKeyIdentifier { get; set; }

        public SubjectAltName SubjectAltName { get; set; }

        public ValidityPeriod StartDateTime { get; set; }

        public ValidityPeriod EndDateTime { get; set; }

        public OpenSLLConfig()
        {
            Subject = new Subject();

            KeyUsage = new KeyUsage();

            ExtKeyUsage = new ExtendedKeyUsage();

            BasicConstraints = new BasicConstraints();

            NetscapeComment = new NetscapeComment();

            SubjectKeyIdentifier = new SubjectKeyIdentifier();

            AuthorityKeyIdentifier = new AuthorityKeyIdentifier();

            SubjectAltName = new SubjectAltName();

            StartDateTime = new ValidityPeriod(DateTime.Now);

            EndDateTime = new ValidityPeriod(DateTime.Now.AddDays(365));
        }

        public string WorkDirectory;

        public string CAPrivateKey;

        public string CACertificate;

        public bool CA;

        public void Generate(string confFilename)
        {
            var s = new System.Text.StringBuilder();
            s.AppendLine("[ca]");
            s.AppendLine("default_ca = ca_default");
            s.AppendLine();
            s.AppendLine("[ca_default]");
            s.AppendLine($"dir          = {WorkDirectory.Replace('\\', '/')}");
            s.AppendLine("certs         = $dir/certs");
            s.AppendLine("crl_dir       = $dir/crl");
            s.AppendLine("new_certs_dir = $dir/newcerts");
            s.AppendLine("database      = $dir/index.txt");
            s.AppendLine($"certificate   = $dir/{CACertificate}");
            s.AppendLine("serial        = $dir/serial");
            s.AppendLine("crlnumber     = $dir/crlnumber");
            s.AppendLine("crl           = $dir/crl.pem");
            s.AppendLine($"private_key   = $dir/{CAPrivateKey}");
            s.AppendLine();
            s.AppendLine("x509_extensions  = usr_cert");
            s.AppendLine();
            s.AppendLine("name_opt = ca_default");
            s.AppendLine("cert_opt = ca_default");
            s.AppendLine();
            s.AppendLine("default_days     = 365");
            s.AppendLine("default_crl_days = 30");
            s.AppendLine("default_md       = default");
            s.AppendLine("preserve         = no");
            s.AppendLine();
            s.AppendLine("policy = policy_anything");
            s.AppendLine();
            s.AppendLine("[ policy_anything ]");
            s.AppendLine("countryName            = optional");
            s.AppendLine("stateOrProvinceName    = optional");
            s.AppendLine("localityName           = optional");
            s.AppendLine("organizationName       = optional");
            s.AppendLine("organizationalUnitName = optional");
            s.AppendLine("commonName             = supplied");
            s.AppendLine("emailAddress           = optional");
            s.AppendLine("domainComponent        = optional");
            s.AppendLine();
            s.AppendLine("[req]");
            s.AppendLine("prompt = no");
            s.AppendLine("distinguished_name = dn");
            s.AppendLine();
            s.AppendLine("[dn]");
            if (!string.IsNullOrEmpty(Subject.CommonName))
                s.AppendLine($"CN = {Subject.CommonName}");

            if (!string.IsNullOrEmpty(Subject.Organization))
                s.AppendLine($"O  = {Subject.Organization}");

            if (!string.IsNullOrEmpty(Subject.OrganizationUnit))
                s.AppendLine($"OU = {Subject.OrganizationUnit}");

            if (!string.IsNullOrEmpty(Subject.Locality))
                s.AppendLine($"L  = {Subject.Locality}");

            if (!string.IsNullOrEmpty(Subject.State))
                s.AppendLine($"ST = {Subject.State}");

            if (!string.IsNullOrEmpty(Subject.Country))
                s.AppendLine($"C  = {Subject.Country}");

            if (!string.IsNullOrEmpty(Subject.DomainComponent))
                s.AppendLine($"domainComponent = {Subject.DomainComponent}");

            if (!string.IsNullOrEmpty(Subject.EmailAddress))
                s.AppendLine($"emailAddress = {Subject.EmailAddress}");

            s.AppendLine();
            s.AppendLine("[usr_cert]");
            GenerateX509Extentions(s);

            File.WriteAllText(confFilename, s.ToString());
        }

        public void GenerateX509Extentions(System.Text.StringBuilder s)
        {
            s.AppendLine($"{BasicConstraints}");
            s.AppendLine($"{KeyUsage}");
            s.AppendLine($"{NetscapeComment}");
            s.AppendLine($"{ExtKeyUsage}");
            s.AppendLine($"{SubjectKeyIdentifier}");
            s.AppendLine($"{AuthorityKeyIdentifier}");
            s.AppendLine($"{SubjectAltName}");
        }

        public void Dispose()
        {
            // NOPE;
        }
    }
}
