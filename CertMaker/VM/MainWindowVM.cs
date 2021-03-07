using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.IO;

using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Security.Cryptography.X509Certificates;

using Prism.Mvvm;
using Prism.Commands;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using CertMaker.M;

namespace CertMaker.VM
{
    public class MainWindowVM : BindableBase
    {
        public ReactiveProperty<string> OpenSSLExec { get; set; } = new ReactiveProperty<string>(@"C:\msys64\usr\bin\openssl.exe");

        private OpenSLLConfig openSSLConfig;

        // subject

        public ReactiveProperty<string> CommonName { get; }
        public ReactiveProperty<string> OrganizationUnit { get; }
        public ReactiveProperty<string> Organization { get; }
        public ReactiveProperty<string> Locality { get; }
        public ReactiveProperty<string> State { get; }
        public ReactiveProperty<string> Country { get; }
        public ReactiveProperty<string> EmailAddress { get; }
        public ReactiveProperty<string> DomainComponent { get; }

        // keyUsage

        public ReactiveProperty<bool> DigitalSignature { get; }
        public ReactiveProperty<bool> NonRepudiation { get; }
        public ReactiveProperty<bool> KeyEncipherment { get; }
        public ReactiveProperty<bool> DataEncipherment { get; }
        public ReactiveProperty<bool> KeyAgreement { get; }
        public ReactiveProperty<bool> CertificateSigning { get; }
        public ReactiveProperty<bool> CRLSigning { get; }
        public ReactiveProperty<bool> EncipherOnly { get; }
        public ReactiveProperty<bool> DecipherOnly { get; }

        // extKeyUsage

        public ReactiveProperty<bool> ServerAuth { get; }
        public ReactiveProperty<bool> ClientAuth { get; }
        public ReactiveProperty<bool> CodeSigning { get; }
        public ReactiveProperty<bool> EmailProtection { get; }
        public ReactiveProperty<bool> TimeStamping { get; }
        public ReactiveProperty<bool> OCSPSigning { get; }
        public ReactiveProperty<bool> MSCTLSign { get; }
        public ReactiveProperty<bool> MSEFS { get; }

        // basicConstraints

        public ReactiveProperty<bool> IsCreatedAsCA { get; }

        // NetscapeComment

        public ReactiveProperty<string> NetscapeComment { get; }

        // SubjectKeyIdentifier

        public ReactiveProperty<bool> SubjectKeyIdentifier { get; }

        // AuthorityKeyIdentifier

        public ReactiveProperty<bool> AuthorityKeyIdentifier_KeyID { get; }

        public ReactiveProperty<bool> AuthorityKeyIdentifier_Issuer { get; }

        // SubjectAltName

        public ReactiveProperty<string> SubjectAltName_URI { get; }

        public ReactiveProperty<string> SubjectAltName_DNS { get; }

        public ReactiveProperty<string> SubjectAltName_IP { get; }

        // StartDate

        public ReactiveProperty<System.DateTime> StartDate { get; }

        public ReactiveProperty<List<int>> StartTimeHour { get; }

        public ReactiveProperty<List<int>> StartTimeMinutes { get; }

        public ReactiveProperty<List<int>> StartTimeSeconds { get; }

        public ReactiveProperty<int> StartTimeHourIndex { get; }

        public ReactiveProperty<int> StartTimeMinutesIndex { get; }

        public ReactiveProperty<int> StartTimeSecondsIndex { get; }

        // EndDate

        public ReactiveProperty<System.DateTime> EndDate { get; }

        public ReactiveProperty<List<int>> EndTimeHour { get; }

        public ReactiveProperty<List<int>> EndTimeMinutes { get; }

        public ReactiveProperty<List<int>> EndTimeSeconds { get; }

        public ReactiveProperty<int> EndTimeHourIndex { get; }

        public ReactiveProperty<int> EndTimeMinutesIndex { get; }

        public ReactiveProperty<int> EndTimeSecondsIndex { get; }

        // Private Key Password

        public ReactiveProperty<string> PrivateKeyPassword { get; }


        public ReactiveProperty<string> SaveFolder { get; set; } = new ReactiveProperty<string>(Directory.GetCurrentDirectory());

        public ReactiveProperty<string> CAPrivateKey { get; set; } = new ReactiveProperty<string>("private/caprivkey.key");

        public ReactiveProperty<string> CACertificate { get; set; } = new ReactiveProperty<string>("certs/cacert.crt");

        public ReactiveProperty<string> TargetPrivateKey { get; set; } = new ReactiveProperty<string>("server.key");

        public ReactiveProperty<string> TargetCertificate { get; set; } = new ReactiveProperty<string>("server.crt");

        public ReactiveProperty<bool> IsSignedByItself { get; set; } = new ReactiveProperty<bool>(false);

        public ReactiveProperty<string> OriginalCertificate { get; set; } = new ReactiveProperty<string>();

        public ReactiveProperty<X509Extensions> X509Ext { get; set; } = new ReactiveProperty<X509Extensions>();

        public DelegateCommand OpenSaveFolder { get; set; }

        public DelegateCommand OpenOpenSSLExec { get; set; }

        public DelegateCommand OpenCAPrivateKey { get; set; }

        public DelegateCommand OpenCACertificate { get; set; }

        public DelegateCommand CreateCerts { get; set; }

        public DelegateCommand OpenOriginalCertificate { get; set; }

        public DelegateCommand LoadOpenSSLConf { get; set; }

        public DelegateCommand SaveOpenSSLConf { get; set; }

        private string temporaryFolder;

        public MainWindowVM()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            openSSLConfig = new OpenSLLConfig();

            // Subject

            CommonName = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.CommonName);
            OrganizationUnit = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.OrganizationUnit);
            Organization = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.Organization);
            Locality = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.Locality);
            State = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.State);
            Country  = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.Country);
            EmailAddress = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.EmailAddress);
            DomainComponent = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.Subject.DomainComponent);

            // KeyUsage

            DigitalSignature = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.DigitalSignature);
            NonRepudiation = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.NonRepudiation);
            KeyEncipherment = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.KeyEncipherment);
            DataEncipherment = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.DataEncipherment);
            KeyAgreement = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.KeyAgreement);
            CertificateSigning = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.CertificateSigning);
            CRLSigning = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.CRLSigning);
            EncipherOnly = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.EncipherOnly);
            DecipherOnly = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.KeyUsage.DecipherOnly);

            // ExtKeyUsage

            ServerAuth = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.ServerAuth);
            ClientAuth = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.ClientAuth);
            CodeSigning = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.CodeSigning);
            EmailProtection = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.EmailProtection);
            TimeStamping = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.TimeStamping);
            OCSPSigning = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.OCSPSigning);
            MSCTLSign = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.MSCTLSign);
            MSEFS = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.ExtKeyUsage.MSEFS);

            // BasicConstraints

            IsCreatedAsCA = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.BasicConstraints.CA);

            // BasicConstraints

            NetscapeComment = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.NetscapeComment.NSComment);

            // SubjectKeyIdentifier

            SubjectKeyIdentifier = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.SubjectKeyIdentifier.Hash);

            // AuthorityKeyIdentifier

            AuthorityKeyIdentifier_KeyID = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.AuthorityKeyIdentifier.KeyID);
            AuthorityKeyIdentifier_Issuer = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.AuthorityKeyIdentifier.Issuer);

            // SubjectAltName

            SubjectAltName_URI = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.SubjectAltName.URI);
            SubjectAltName_DNS = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.SubjectAltName.DNS);
            SubjectAltName_IP = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.SubjectAltName.IP);

            // StartDate

            StartDate = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.StartDateTime.Date);
            StartTimeHour = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.StartDateTime.Hours);
            StartTimeMinutes = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.StartDateTime.Minutes);
            StartTimeSeconds = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.StartDateTime.Seconds);
            StartTimeHourIndex = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.StartDateTime.HoursIndex);
            StartTimeMinutesIndex = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.StartDateTime.MinutesIndex);
            StartTimeSecondsIndex = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.StartDateTime.SecondsIndex);

            // EndDate

            EndDate = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.EndDateTime.Date);
            EndTimeHour = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.EndDateTime.Hours);
            EndTimeMinutes = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.EndDateTime.Minutes);
            EndTimeSeconds = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.EndDateTime.Seconds);
            EndTimeHourIndex = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.EndDateTime.HoursIndex);
            EndTimeMinutesIndex = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.EndDateTime.MinutesIndex);
            EndTimeSecondsIndex = openSSLConfig.ToReactivePropertyAsSynchronized(x => x.EndDateTime.SecondsIndex);

            // Private Key Password

            PrivateKeyPassword = new ReactiveProperty<string>("");

            OpenSaveFolder = new DelegateCommand(() => openFolder(SaveFolder));
            OpenOpenSSLExec = new DelegateCommand(() => openFile(OpenSSLExec, "実行ファイル (*.exe)|*.exe"));
            OpenCAPrivateKey = new DelegateCommand(() => openFile(CAPrivateKey, "すべてのファイル (*.*)|*.*"));
            OpenCACertificate = new DelegateCommand(() => openFile(CACertificate, "すべてのファイル (*.*)|*.*"));
            OpenOriginalCertificate = new DelegateCommand(() => openFile(OriginalCertificate, "すべてのファイル (*.*)|*.*"));

            CreateCerts = new DelegateCommand(createCerts);

            LoadOpenSSLConf = new DelegateCommand(loadOpenSSLConf);

            SaveOpenSSLConf = new DelegateCommand(saveOpenSSLConf);
        }

        private void openFolder(ReactiveProperty<string> folder)
        {
            // ダイアログのインスタンスを生成
            var dialog = new CommonOpenFileDialog("フォルダーの選択")
            {
                IsFolderPicker = true,
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                folder.Value = dialog.FileName;
            }
        }

        private void openFile(ReactiveProperty<string> file, string filter)
        {
            // ダイアログのインスタンスを生成
            var dialog = new OpenFileDialog();

            // ファイルの種類を設定
            dialog.Filter = filter;

            // ダイアログを表示する
            if (dialog.ShowDialog() == true)
            {
                file.Value = dialog.FileName;
            }
        }

        private void saveOpenSSLConf()
        {
            openSSLConfig.Generate(OriginalCertificate.Value);
        }

        private void createCerts()
        {
            var caPrivateKeyFile = $"{CAPrivateKey.Value}";
            var caCertificateFile = $"{CACertificate.Value}";
            var targetPrivateKeyFile = $"{TargetPrivateKey.Value}";
            var targetCertificateFile = $"{TargetCertificate.Value}";
            var newTargetCertificateFile = $"newcerts/{TargetCertificate.Value}";
            var confFilename = "server.cnf";

            if (!Directory.Exists(SaveFolder.Value))
                Directory.CreateDirectory(SaveFolder.Value);

            temporaryFolder = Path.Combine(SaveFolder.Value, "tmp");
            if (!Directory.Exists(temporaryFolder))
                Directory.CreateDirectory(temporaryFolder);

            Directory.SetCurrentDirectory(temporaryFolder);
            Directory.CreateDirectory("private");
            Directory.CreateDirectory("certs");

            openSSLConfig.WorkDirectory = temporaryFolder;
            openSSLConfig.CAPrivateKey = caPrivateKeyFile;
            openSSLConfig.CACertificate = caCertificateFile;
            openSSLConfig.CA = IsCreatedAsCA.Value;
            openSSLConfig.Generate(confFilename);

            int ret;
            string stdout, stderr;
            (ret, stdout, stderr) = createPrivateKey(targetPrivateKeyFile);
            if (ret > 0)
            {
                MessageBox.Show(string.Format("Private Key Creation was failed.\n{0}", stderr));
                return;
            }

            (ret, stdout, stderr) = createCSR(targetPrivateKeyFile, "server.csr", confFilename);
            if (ret > 0)
            {
                MessageBox.Show(string.Format("CSR creation was failed.\n{0}", stderr));
                return;
            }

            (ret, stdout, stderr) = createCertificate(targetPrivateKeyFile, "server.csr", targetCertificateFile);
            if (ret > 0)
            {
                MessageBox.Show(string.Format("Certificate creation was failed.\n{0}", stderr));
                return;
            }

            if(IsSignedByItself.Value)
            {
                File.Copy(targetPrivateKeyFile, caPrivateKeyFile, true);
                File.Copy(targetCertificateFile, caCertificateFile, true);
            }

            (ret, stdout, stderr) = signedbyself(targetCertificateFile, newTargetCertificateFile, "server.csr", "server.cnf");
            if (ret > 0)
            {
                MessageBox.Show(string.Format("Self-Signed was failed.\n{0}", stderr));
                return;
            }

            PemToDer(newTargetCertificateFile, "user-server.der");

            copyCerts(targetCertificateFile, newTargetCertificateFile);

            /*
            (ret, stdout, stderr) = CreatePfx(targetPrivateKeyFile, newTargetCertificateFile, "server.pfx");
            if (ret > 0)
            {
                MessageBox.Show(string.Format("PFX creation was failed.\n{0}", stderr));
                return;
            }
            */

            MessageBox.Show("作成完了");
        }

        private void appendArguments(ProcessStartInfo info, IList<string> args)
        {
            foreach(var arg in args)
            {
                info.ArgumentList.Add(arg);
            }
        }

        private (int, string, string) executeOpenSSL(IList<string> args)
        {
            var info = new ProcessStartInfo(OpenSSLExec.Value)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            appendArguments(info, args);

            var proc = Process.Start(info);

            proc.WaitForExit();

            var stdout = proc.StandardOutput.ReadToEnd();
            var stderr = proc.StandardError.ReadToEnd();

            var ret = proc.ExitCode;

            proc.Close();

            return (ret, stdout, stderr);
        }

        private (int, string, string) createPrivateKey(string targetPrivateKeyFile)
        {
            return executeOpenSSL(new List<string>()
            {
                "genrsa",
                "-aes256",
                "-passout", $"pass:{PrivateKeyPassword.Value}",
                "-out", targetPrivateKeyFile,
            });
        }

        private (int, string, string) createCSR(string keyFilename, string csrFilename, string confFilename)
        {
            return executeOpenSSL(new List<string>()
            {
                "req",
                "-new",
                "-utf8",
                "-nameopt", "multiline,utf8",
                "-nodes",
                "-passin", $"pass:{PrivateKeyPassword.Value}",
                "-config", confFilename,
                "-key", keyFilename,
                "-out", csrFilename
            });
        }

        private (int, string, string) createCertificate(string keyFilename, string csrFilename, string crtFilename)
        {
            return executeOpenSSL(new List<string>()
            {
                "x509",
                "-req",
                "-passin", $"pass:{PrivateKeyPassword.Value}",
                "-days", "365",
                "-in", csrFilename,
                "-signkey", keyFilename,
                "-out", crtFilename
            });
        }

        private (int, string, string) signedbyself(string crtFilename, string newCrtFilename, string csrFilename, string cnfFilename)
        {
            // フォルダを作成する
            Directory.CreateDirectory("crl");
            Directory.CreateDirectory("newcerts");

            // index.txt 作成
            using (var index_txt = File.Create("index.txt")) { }

            // serial に シリアル番号を保存
            using (var fs = File.Create("serial"))
            using (var sw = new StreamWriter(fs))
            {
                var cert = X509Certificate2.CreateFromCertFile(crtFilename);
                sw.Write(cert.GetSerialNumberString());
            }

            var args = new List<string>()
            {
                "ca",
                "-passin", $"pass:{PrivateKeyPassword.Value}",
                "-batch",
                "-startdate", $"{openSSLConfig.StartDateTime}",
                "-enddate", $"{openSSLConfig.EndDateTime}",
                "-config", cnfFilename,
                "-in", csrFilename,
                "-out", newCrtFilename
            };

            if (IsSignedByItself.Value)
                args.Add("-selfsign");

            return executeOpenSSL(args);
        }

        private (int, string, string) PemToDer(string pemFilename, string derFilename)
        {
            return executeOpenSSL(new List<string>()
            {
                "x509",
                "-in", pemFilename,
                "-inform", "pem",
                "-out", derFilename,
                "-outform", "der"
            });
        }

        private void copyCerts(string keyFile, string crtFile)
        {
            File.Copy(Path.Combine(temporaryFolder, keyFile), Path.Combine(SaveFolder.Value, TargetPrivateKey.Value), true);
            File.Copy(Path.Combine(temporaryFolder, crtFile), Path.Combine(SaveFolder.Value, TargetCertificate.Value), true);
        }

        private (int, string, string) CreatePfx(string keyFilename, string crtFilename, string pfxFilename)
        {
            return executeOpenSSL(new List<string>()
            {
                "pkcs12",
                "-passin", $"pass:{PrivateKeyPassword.Value}",
                "-passout", "pass:",
                "-export",
                "-inkey", keyFilename,
                "-in", crtFilename,
                "-out", pfxFilename,
            });
        }

        private (int, string, string) GetExtention(string certfilename, string name)
        {
            var ext = "pem";

            if (Path.GetExtension(certfilename) == ".der")
                ext = "der";

            return executeOpenSSL(new List<string>()
            {
                "x509",
                "-in", OriginalCertificate.Value,
                "-inform", ext,
                "-nocert",
                "-ext", name
            });
        }

        private void loadOpenSSLConf()
        {
            var cert = new X509Certificate2(OriginalCertificate.Value);

            var subject = SubjectKeyValuePair(cert.Subject);
            if (subject.ContainsKey("CN"))
                CommonName.Value = subject["CN"];
            else
                CommonName.Value = "";

            if (subject.ContainsKey("OU"))
                OrganizationUnit.Value = subject["OU"];
            else
                OrganizationUnit.Value = "";

            if (subject.ContainsKey("O"))
                Organization.Value = subject["O"];
            else
                Organization.Value = "";

            if (subject.ContainsKey("L"))
                Locality.Value = subject["L"];
            else
                Locality.Value = "";

            if (subject.ContainsKey("S"))
                State.Value = subject["S"];
            else
                State.Value = "";

            if (subject.ContainsKey("C"))
                Country.Value = subject["C"];
            else
                Country.Value = "";

            if (subject.ContainsKey("E"))
                EmailAddress.Value = subject["E"];
            else
                EmailAddress.Value = "";

            if (subject.ContainsKey("DC"))
                DomainComponent.Value = subject["DC"];
            else
                DomainComponent.Value = "";

            int ret;
            string stdout, stderr;
            (ret, stdout, stderr) = GetExtention(OriginalCertificate.Value, "basicConstraints");
            X509Ext.Value.BasicConstraints = BasicConstraints.Parse(stdout);

            (ret, stdout, stderr) = GetExtention(OriginalCertificate.Value, "keyUsage");
            X509Ext.Value.KeyUsage = KeyUsage.Parse(stdout);

            (ret, stdout, stderr) = GetExtention(OriginalCertificate.Value, "nsComment");
            X509Ext.Value.NSComment = NSComment.Parse(stdout);

        }

        private Dictionary<string, string> SubjectKeyValuePair(string subj)
        {
            var subjKeyValuePair = new Dictionary<string, string>();

            var subjs = subj.Split(',');
            foreach(var record in subjs)
            {
                var kv = record.Split('=');
                var k = kv[0].Trim();
                var v = kv[1].Trim();
                subjKeyValuePair[k] = v;
            }
            return subjKeyValuePair;
        }

        private void generateCRL()
        {
            // TODO
            // echo 00 > crlnumber
            // openssl ca -config server.cnf -gencrl -out crl/server.pem
        }
    }
}