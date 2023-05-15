using Infraestrutura.Enums;
using Infraestrutura.Utils;

namespace Infraestrutura.Configuracoes
{
    /// <summary>
    /// A Base36 Encoder and Decoder
    /// </summary>
    public static class WBase36
    {
        private static readonly string Base36Characters = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static WRadixEncoding base36 = new(Base36Characters, WEndianFormat.Little, true);

        public static string Encode(byte[] input)
        {
            return base36.Encode(input);
        }

        public static byte[] Decode(string input)
        {
            return base36.Decode(input);
        }
    }
}
