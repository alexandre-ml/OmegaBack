using System.Xml;

namespace Infraestrutura.Configuracoes
{
    public class WFtpSettings : WBaseSettings
    {
        public string Descricao { get; protected set; }
        public string Servidor { get; protected set; }
        public int Porta { get; protected set; }
        public string Login { get; protected set; }
        public string Senha { get; protected set; }
        public bool Passiva { get; protected set; }
        public int ConTimeout { get; protected set; }
        public string ZipSenha { get; protected set; }
        public WFtpSettings()
        {
        }
        public void FromXml(XmlElement pElem)
        {
            // Obtem tipo da configuração; se não for FTP, abandonar a carga
            string lType = pElem.GetAttribute(OmegaXmlParam.TYPE);
            if (lType.ToLower() != "ftp")
                return;

            // Otem os parâmetros de configuração para a conexão FTP
            foreach (XmlElement lProp in pElem.ChildNodes)
            {
                string lPropName, lPropValue;
                lPropName = lProp.Name.ToLower();

                if (lPropName == "senha")
                {
                    lPropValue = GetEncryptText(lProp.InnerText, true);
                    Senha = lPropValue.Trim();
                }
                else
                if (lPropName == "zipSenha")
                {
                    lPropValue = GetEncryptText(lProp.InnerText, true);
                    ZipSenha = lPropValue.Trim();
                }
                else
                {
                    lPropValue = lProp.InnerText.Trim();
                    if (lPropName == "descricao")
                        Descricao = lPropValue;
                    else
                    if (lPropName == "login")
                        Login = lPropValue;
                    else
                    if (lPropName == "porta")
                        try { Porta = Convert.ToInt16(lPropValue); }
                        catch (Exception) { Porta = 0; }
                    else
                    if (lPropName == "servidor")
                        Servidor = lPropValue;
                    else
                     if (lPropName == "passiva")
                        try { Passiva = StrToBool(lPropValue); } catch (Exception) { Passiva = true; }
                    else
                    if (lPropName == "conTimeout")
                        try { ConTimeout = Convert.ToInt16(lPropValue); } catch (Exception) { ConTimeout = 10; };
                }
            }
        }
    }
}
