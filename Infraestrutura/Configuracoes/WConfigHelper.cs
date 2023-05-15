using System.Reflection;
using System.Text;
using System.Text.Json;
using Infraestrutura.Utils;

namespace Infraestrutura.Configuracoes
{
    public static class WConfigHelper
    {
        public static string CfgFileName { get => $"{Assembly.GetExecutingAssembly().GetName().Name}.json"; }
        public static string FileNameLocal { get => $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{CfgFileName}"; }
        public static string FileNameGlobal { get => AppDomain.CurrentDomain.BaseDirectory + CfgFileName; }

        static public WConfig Load()
        {
            WConfig ret;
            try
            {
                string json = string.Empty;
                string fileName = FileNameLocal;
                if (!File.Exists(fileName))
                    fileName = FileNameGlobal;

                using (var reader = new StreamReader(fileName, Encoding.Latin1))
                    json = reader.ReadToEnd();

                ret = WStringUtils.Deserialize<WConfig>(json);
            }
            catch (Exception)
            {
                ret = CreateDefautlCfg();
            }

            return ret;
        }

        private static WConfig CreateDefautlCfg()
        {
            var ret = new WConfig
            {
                OmegaXmlFolder = @"c:\desenv\bin\",
                DbAlias = string.Empty,
                Colunas = new List<WConfigItem>
                {
                },
                frmMain = null,
                frmQryModel = null
            };

            return ret;
        }

        static public void Save(WConfig cfg)
        {
            var dataStr = JsonSerializer.Serialize(cfg);
            var fileName = FileNameLocal;

            using var writer = new StreamWriter(fileName, false, Encoding.Latin1);
            writer.Write(dataStr);
        }
        public static WConfig Deserialize(string json)
        {
            return WStringUtils.Deserialize<WConfig>(json);
        }
    }
}
