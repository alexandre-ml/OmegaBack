using Infraestrutura.Enums;

namespace Infraestrutura.Configuracoes
{
    public class WGeneralSettings
    {
        public WApplicationType ApplicationType { get; set; }
        public WFtpSettings BiFtp { get; }

        public WGeneralSettings()
        {
            BiFtp = new WFtpSettings();
        }
    }
}
