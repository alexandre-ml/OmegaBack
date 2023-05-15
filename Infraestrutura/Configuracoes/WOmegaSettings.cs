using Infraestrutura.Enums;
using System.Xml;

namespace Infraestrutura.Configuracoes
{
    public class WOmegaSettings
    {
        public List<WDbSettings> DataBases { get; protected set; } = new();
        public WGeneralSettings GeneralSettings { get; protected set; } = new();

        public string LastError { get; protected set; }
        public bool HasError { get => LastError.Trim() != string.Empty; }

        private readonly string FFilePath;
        private DateTime? FLoaded = null;

        public WOmegaSettings(string pFilePath)
        {
            LastError = string.Empty;
            FFilePath = pFilePath;
        }

        public void LoadOmegaSettings()
        {
            LastError = string.Empty;
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(FFilePath);
                LoadDataBases(xmlDoc);
                LoadGeneralSettings(xmlDoc);

                FLoaded = File.GetLastWriteTime(FFilePath);
            }
            catch (Exception exc)
            {
                LastError = exc.Message;
            }
        }
        public void ReLoadOmegaSettings()
        {
            bool reload = false;
            if (FLoaded != null)
            {
                var ct = File.GetLastWriteTime(FFilePath);
                reload = (ct > FLoaded);
            }
            if (reload)
                LoadOmegaSettings();
        }

        public string GetTemporaryDir()
        {
            return Directory.GetCurrentDirectory();
        }

        public string GetBinaryDir()
        {
            return System.Reflection.Assembly.GetEntryAssembly().Location;
        }

        public bool ExistsAliasName(string pName)
        {
            var ret = DataBases.FirstOrDefault((dbSetting) => dbSetting.AliasName == pName);
            return ret != null;
        }

        public WDbSettings GetDbSettingsByName(string pName)
        {
            var ret = DataBases.FirstOrDefault((dbSetting) => dbSetting.AliasName == pName);
            
            return ret;
        }

        public bool GetLoaded()
        {
            return (FLoaded != null);
        }

        private void LoadDataBases(XmlDocument pDoc)
        {
            DataBases.Clear();

            var lDbListElem = pDoc.GetElementsByTagName("databases");
            
            if (lDbListElem.Count <= 0)
                return;

            var dbXml = lDbListElem.Item(0).FirstChild;
            
            while (dbXml != null)
            {
                var lAliasName = dbXml.Name;
                var lDbSettings = new WDbSettings(lAliasName, this);
                lDbSettings.FromXml((XmlElement)dbXml);

                DataBases.Add(lDbSettings);

                dbXml = dbXml.NextSibling;
            }
        }

        private void LoadGeneralSettings(XmlDocument pDoc)
        {
            var lGenListElem = pDoc.GetElementsByTagName("general");
            
            if (lGenListElem.Count <= 0)
                return;
            
            var genElem = lGenListElem.Item(0);
            
            foreach (XmlElement item in genElem.ChildNodes)
            {
                if (item.Name == "ApplicationType")
                {
                    _ = Enum.TryParse(item.InnerText, out WApplicationType lAppType);

                    if (lAppType != WApplicationType.atOmega &&
                        lAppType != WApplicationType.atERPPronto &&
                        lAppType != WApplicationType.atERPOmegaCloud)
                    {
                        lAppType = WApplicationType.atOmega;
                    }

                    GeneralSettings.ApplicationType = lAppType;
                }

                if (item.Name == "FTP_BI")
                    GeneralSettings.BiFtp.FromXml(item);
            }
        }
    }
}
