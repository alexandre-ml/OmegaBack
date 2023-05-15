namespace Infraestrutura.Utils
{
    public class WNotificacao
    {
        public WNotificacao()
        {
            WNotificacoes = new List<WNotificacao>();
            NomeProp = "Campo Obigratório";
            Mensagem = string.Empty;
        }

        public string NomeProp { get; set; }
        public string Mensagem { get; set; }
        public List<WNotificacao> WNotificacoes { get; set; }

        public bool ValidarPropString(string valor, string prop)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrEmpty(prop))
            {
                WNotificacoes.Add(new WNotificacao
                {
                    NomeProp = prop
                });
                return false;
            }

            return true;
        }

        public bool ValidarPropInt(int? valor, string prop)
        {
            if (valor == null || valor < 1 || string.IsNullOrEmpty(prop))
            {
                WNotificacoes.Add(new WNotificacao
                {
                    NomeProp = prop
                });

                return false;
            }

            return true;
        }

        public bool ValidarPropDecimal(decimal? valor, string prop)
        {
            if (valor == null || string.IsNullOrEmpty(prop))
            {
                WNotificacoes.Add(new WNotificacao
                {
                    NomeProp = prop
                });

                return false;
            }
            return true;
        }
    }
}
