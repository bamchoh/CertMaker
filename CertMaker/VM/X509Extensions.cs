using System;
using System.Collections.Generic;
using System.Text;

namespace CertMaker.VM
{
    public class Util
    {
        public static List<(string k, string v)> GetParams(string text)
        {
            var s_idx = text.IndexOf(':');
            var vals = text.Substring(s_idx + 1).Replace("\n", ",").Split(",");
            var parameters = new List<(string, string)>();
            foreach(var val in vals)
            {
                var kv = val.Trim().ToLower().Split(':');
                if (kv.Length == 1)
                    parameters.Add((kv[0], ""));
                else
                    parameters.Add((kv[0], kv[1]));
            }
            return parameters;
        }
    }

    public class X509Extensions
    {
        public BasicConstraints BasicConstraints { get; set; }

        public KeyUsage KeyUsage { get; set; }

        public NSComment NSComment { get; set; }
    }

    public class NSComment
    {
        public string Comment;

        public override string ToString()
        {
            return "nsComment=\"" + Comment + "\"";
        }

        public static NSComment Parse(string text)
        {
            var i = text.IndexOf(':');
            return new NSComment()
            {
                Comment = text.Substring(i + 1).Replace('\n', ' ').Trim()
            };
        }
    }

    public class KeyUsage
    {
        public bool Critical;
        public bool DigitalSignature;
        public bool NonRepudiation;
        public bool KeyEncipherment;
        public bool DataEncipherment;
        public bool KeyAgreement;
        public bool KeyCertSign;
        public bool CRLSign;
        public bool EncipherOnly;
        public bool DecipherOnly;

        public override string ToString()
        {
            var str = "keyUsage=";
            var vals = new List<string>();
            if (Critical)
                vals.Add("critical");

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

            if (KeyCertSign)
                vals.Add("keyCertSign");

            if (CRLSign)
                vals.Add("cRLSign");

            if (EncipherOnly)
                vals.Add("encipherOnly");

            if (DecipherOnly)
                vals.Add("decipherOnly");

            str += string.Join(',', vals);

            return str;
        }

        public static KeyUsage Parse(string text)
        {
            var ret = new KeyUsage();

            var vals = Util.GetParams(text);
            foreach (var val in vals)
            {
                switch(val.k)
                {
                    case "critical":
                        ret.Critical = true;
                        break;
                    case "digital signature":
                        ret.DigitalSignature = true;
                        break;
                    case "non repudiation":
                        ret.NonRepudiation = true;
                        break;
                    case "key encipherment":
                        ret.KeyEncipherment = true;
                        break;
                    case "data encipherment":
                        ret.DataEncipherment = true;
                        break;
                    case "key agreement":
                        ret.KeyAgreement = true;
                        break;
                    case "certificate sign":
                        ret.KeyCertSign = true;
                        break;
                    case "crl sign":
                        ret.CRLSign = true;
                        break;
                    case "encipher only":
                        ret.EncipherOnly = true;
                        break;
                    case "decipher only":
                        ret.DecipherOnly = true;
                        break;
                }
            }

            return ret;
        }
    }

    public class BasicConstraints
    {
        public bool Critical;

        public bool CA;

        public int? PathLenghConstraint;

        public override string ToString()
        {
            var str = "basicConstraints=";
            if (CA)
            {
                str += "critical,CA:true";
                if (PathLenghConstraint != null && PathLenghConstraint >= 0)
                    str += string.Format(",pathlen:{0}", PathLenghConstraint);
            }
            else
            {
                str += "CA:FALSE";
            }
            return str;
        }

        public static BasicConstraints Parse(string text)
        {
            var ret = new BasicConstraints();

            var vals = Util.GetParams(text);
            foreach(var val in vals)
            {
                if (val.k.StartsWith("critical"))
                {
                    ret.Critical = true;
                    continue;
                }

                if(val.k.StartsWith("ca"))
                {
                    if (val.v == "true")
                        ret.CA = true;
                    else
                        ret.CA = false;

                    continue;
                }

                if(val.k.StartsWith("pathlen"))
                {
                    int pathlen;
                    if(int.TryParse(val.v, out pathlen))
                    {
                        ret.PathLenghConstraint = pathlen;
                    }
                }
            }
            return ret;
        }
    }
}
