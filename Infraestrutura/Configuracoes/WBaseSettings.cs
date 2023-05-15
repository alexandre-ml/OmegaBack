using System.Text;

namespace Infraestrutura.Configuracoes
{
    public class WBaseSettings
    {
        //public readonly byte[] chave = Encoding.ASCII.GetBytes("tSdf@d\u00C7TsdE$diJuA%pifDmdv");
        public readonly sbyte[] chave = {116, 83, 100, 102, 64, 100, -57, 84, 115, 100, 69, 36,
                                        100, 105, 74, 117, 65, 37, 112, 105, 102, 68, 109, 100, 118 };

        protected bool StrToBool(string pValor)
        {
            bool ret = false;
            string lValor = pValor.ToLowerInvariant();

            if (lValor == "true")
                ret = true;
            else
            if (lValor != "false")
                try
                {
                    ret = Convert.ToInt64(lValor) != 0;
                }
                catch (Exception) { }

            return (ret);
        }

        protected int StrToInt(string pValor, int defaultValue = 0)
        {
            int ret = defaultValue;

            try
            {
                ret = Convert.ToInt32(pValor);
            }
            catch (Exception) { }

            return (ret);
        }

        protected string GetEncryptText(string pText, bool pEncrypt)
        {
            string ret;

            if (!pEncrypt)
                ret = pText;
            else
            {
                byte[] senha = new byte[20];

                for (int j = 0; j < 40; j += 2)
                    try
                    {
                        var c1 = pText[j];
                        var c2 = pText[j + 1];
                        senha[j / 2] = Convert.FromHexString(c1.ToString() + c2.ToString())[0];
                    }
                    catch (Exception)
                    {
                        senha[j / 2] = 0;
                    }

                Decode(senha);
                ret = Encoding.ASCII.GetString(senha);
            }

            return (ret);
        }

        protected void Decode(byte[] str)
        {
            int comprChave = chave.Length, comprTexto = str.Length;

            for (int i = 0; i < comprTexto; i++)
            {
                byte b1 = (byte)(~str[i]);
                byte b2 = (byte)chave[i % comprChave];
                str[i] = (byte)(b1 ^ b2);
            }
        }
    }
}
