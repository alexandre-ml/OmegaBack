using System.Xml;

namespace Infraestrutura.Configuracoes
{
    // configuração de um database
    public class WDbSettings : WBaseSettings
    {
        private Dictionary<string, string> FParams;
        private string? FConnectionString;
        private string? FDescricao;
        protected WOmegaSettings FOwner;
        protected WFtpSettings? FBiFtp;

        public string AliasName { get; protected set; }
        public string? Descricao => FDescricao;
        public string? TipoBanco { get; protected set; }
        public string? UserId { get; protected set; }
        public string? Provider => FParams.GetValueOrDefault(OmegaXmlParam.PROVIDER);
        public int GeneralTimeout => StrToInt(FParams.GetValueOrDefault(OmegaXmlParam.GENERAL_TIMEOUT), 300);
        public string? SiglaSoftware { get; protected set; }
        public bool SharedLogin => !StrToBool(FParams.GetValueOrDefault(OmegaXmlParam.SHARED_LOGIN));
        public WFtpSettings? BiFtp => FBiFtp;

        public WDbSettings(string pName, WOmegaSettings AOwner)
        {
            FBiFtp = null;
            FDescricao = string.Empty;
            FParams = new Dictionary<string, string>();

            FOwner = AOwner;
            AliasName = pName;
        }

        public void FromXml(XmlElement pElem)
        {
            // Esses parâmetros não entrão na construção online da string de conexão (alguns entram depois)
            string[] NoCsParams = { OmegaXmlParam.TYPE, OmegaXmlParam.USER_ID, OmegaXmlParam.PASSWORD,
                                    OmegaXmlParam.PERSIST_SECURITY_INFO, OmegaXmlParam.ADO_CACHE_DIR, OmegaXmlParam.ENABLE_ADO_CACHE,
                                    OmegaXmlParam.AUTO_TRANSLATE, OmegaXmlParam.GENERAL_TIMEOUT, OmegaXmlParam.LOCALE_IDENTIFIER,
                                    OmegaXmlParam.PROVIDER , OmegaXmlParam.SIGLA_SOFTWARE, OmegaXmlParam.SHARED_LOGIN };
            string lPropName, lPropValue, lConnectionString;

            lConnectionString = string.Empty;
            TipoBanco = pElem.GetAttribute(OmegaXmlParam.TYPE);
            FParams.Clear();

            XmlElement? lProp = pElem.FirstChild as XmlElement;
            while (lProp != null)
            {
                lPropName = lProp.GetAttribute("name");
                if (lPropName != string.Empty)
                {
                    bool lEncrypt = StrToBool(lProp.GetAttribute("encrypt"));
                    lPropValue = GetEncryptText(lProp.InnerText, lEncrypt);

                    if (lPropName == OmegaXmlParam.USER_ID)
                        UserId = lPropValue;
                    else
                    //if (lPropName == OmegaXmlParam.PASSWORD)
                    //    FPassword = lPropValue;
                    //else
                    if (lPropName == OmegaXmlParam.SIGLA_SOFTWARE)
                        SiglaSoftware = lPropValue;

                    if (NoCsParams.Contains(lPropName) == false)
                        lConnectionString += (lPropName + "=" + lPropValue + ";");

                    // Guarda parâmetros de conexão
                    FParams.Add(lPropName, lPropValue);
                }
                else
                if (lProp.Name == OmegaXmlParam.DESCRICAO)
                    FDescricao = lProp.InnerText;

                lProp = lProp.NextSibling as XmlElement;
            }

            var ftpListElem = pElem.GetElementsByTagName("FTP_BI");
            if (ftpListElem.Count > 0)
            {
                XmlElement ftpElem = (XmlElement)ftpListElem.Item(0);
                FBiFtp = new WFtpSettings();
                FBiFtp.FromXml(ftpElem);
            }

            if (lConnectionString != string.Empty)
                FConnectionString = lConnectionString;
        }
        public bool ExistsParamValue(string pParamName)
        {
            return FParams.ContainsKey(pParamName);
        }
        public string GetParamValue(string pParamName)
        {
            if (FParams.TryGetValue(pParamName, out var paramValue) == false)
                paramValue = string.Empty;
            return paramValue;
        }
        public override string ToString()
        {
            return AliasName;
        }
        public string GetConnectionString()
        {
            var shared = !StrToBool(FParams.GetValueOrDefault(OmegaXmlParam.SHARED_LOGIN));
            return GetConnectionString(string.Empty, string.Empty, shared, true);
        }
        public string GetConnectionString(string pUserID, string pPassword)
        {
            return GetConnectionString(pUserID, pPassword, false, true);
        }
        public string GetConnectionString(string pUserID, string pPassword, bool pTrustedConnection, bool pUsaLoginOmegaAdm = true)
        {
            string lConnectionString;
            var dbSetting = FOwner.GetDbSettingsByName(AliasName);

            if (pUsaLoginOmegaAdm && dbSetting.GetParamValue(OmegaXmlParam.USER_ID) != string.Empty)
            {
                pUserID = dbSetting.GetParamValue(OmegaXmlParam.USER_ID).Trim();
                pPassword = dbSetting.GetParamValue(OmegaXmlParam.PASSWORD).Trim();
            }

            // Trata password aspas caso exista na senha
            pPassword.Replace("\"", "\"\"");
            // Acrescenta aspas na senha
            pPassword = "\"" + pPassword + "\"";

            // Adiciona userid e password e Persist Security Indo (deve ser sempre true)
            lConnectionString = FConnectionString;

            if (pTrustedConnection)
                lConnectionString += OmegaXmlParam.TRUSTED_CONNECTION + "=yes;";
            else
            {
                lConnectionString += OmegaXmlParam.USER_ID + "=" + pUserID + ";";
                lConnectionString += OmegaXmlParam.PASSWORD + "=" + pPassword + ";";
            }

            if (TipoBanco.ToUpper() != "POSTGRESQL")
            {
                lConnectionString += OmegaXmlParam.PERSIST_SECURITY_INFO + "=True;";
                lConnectionString += OmegaXmlParam.ENCRYPT + "=False;";
            }

            return (lConnectionString);
        }
    }
}
