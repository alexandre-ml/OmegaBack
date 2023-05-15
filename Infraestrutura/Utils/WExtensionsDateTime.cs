namespace Infraestrutura.Utils
{
    public static class WExtensionsDateTime
    {
        public static int DataAntiga(this DateTime data)
        {
            return data.Year * 10000 + data.Month * 100 + data.Day;
        }

        public static short HoraAntiga(this DateTime data)
        {
            return (short)(data.Hour * 100 + data.Minute);
        }
    }
}
