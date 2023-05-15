using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Infraestrutura.Enums;
using Infraestrutura.Configuracoes;

namespace Infraestrutura.Utils
{
    public static class WStringUtils
    {
        #region Serialização JSON
        public static string CamelCase(string s)
        {
            var x = s.Replace("_", "");
            
            if (x.Length == 0) 
                return "Null";
            
            x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            
            return char.ToLower(x[0]) + x[1..];
        }

        public static string CamelCaseForLegacy(string s)
        {
            var parts = s.ToLowerInvariant().Split(new char[] { ' ', '_', '-' });
            string ret = parts[0];

            for (var i = 1; i < parts.Length; i++)
            {
                ret += Char.ToUpperInvariant(parts[i][0]);
                if (parts[i].Length > 0)
                    ret += parts[i][1..];
            }
            return ret;
        }

        /// <summary>
        /// Pega um texto em formato JSON e cria sua representação como um
        /// objeto do tipo especificado no generic.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser retornado</typeparam>
        /// <param name="json">O texto em formato JSON a ser deserializado.</param>
        /// <param name="converters">conversores auxiliares opcionais.</param>
        /// <returns>O objeto convertido a partir do texto.</returns>
        public static T Deserialize<T>(string json, params JsonConverter[] converters)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            foreach (var c in converters)
                options.Converters.Add(c);

            T o = JsonSerializer.Deserialize<T>(json, options);

            return o;
        }
        public static string ToJson(object obj, bool indented = false, params JsonConverter[] converters)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                WriteIndented = indented
            };

            foreach (var c in converters)
                options.Converters.Add(c);
            
            string ret = JsonSerializer.Serialize(obj, options);

            return ret;
        }
        #endregion // Serialização JSON

        #region Encoding
        /// <summary>
        /// Codifica um texto em sua representação base64. Se o encoding do texto
        /// não for informado, a conversão considerará como UTF-8
        /// </summary>
        /// <param name="text">o texto a codificar</param>
        /// <param name="encoding">o encoding do texto</param>
        /// <returns></returns>
        public static string Encode64(string text, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            byte[] plainTextBytes = encoding.GetBytes(text);
            string text64 = Convert.ToBase64String(plainTextBytes);

            return text64;
        }

        static public string ComputeSha512Hash(string rawData)
        {
            // Create a SHA512   
            using (SHA512 sha512Hash = SHA512.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        #endregion // Encoding

        #region Toolset de strings
        /// <summary>
        /// Obtem um pedaço do texto com, no máximo, a quantidade especificada de caracteres.
        /// </summary>
        /// <param name="texto">texto a ser processado</param>
        /// <param name="start">posição inicial do sub texto a recuperar</param>
        /// <param name="qtChars">qtde máxima de caracteres a recuperar a partir da 
        /// posição fornecida.</param>
        /// <returns>o pedaço do texto recuperado.</returns>
        static public string GetSubstring(this string texto, int start, int qtChars)
        {
            string ret = texto;
            
            if (texto != null)
            {
                var lastPos = qtChars + start - 1;
                var qtos = (texto.Length > lastPos) ? lastPos : texto.Length - start;
                ret = (start > texto.Length) ? string.Empty : texto.Substring(start, qtos);
            }

            return ret;
        }
        /// <summary>
        /// Limita o tamanho do texto a, no máximo, maxChars caracteres
        /// </summary>
        /// <param name="texto">texto a ser verificado</param>
        /// <param name="maxChars">quantidade máxima de caracteres</param>
        /// <returns>o texto com a quantidade de caracteres limitada.</returns>
        static public string Max(this string texto, int maxChars)
        {
            string ret = texto;
            if (texto != null && texto.Length > maxChars)
                ret = texto.Substring(0, maxChars);
            return ret;
        }

        /// <summary>
        /// Ajusta o tamanho do texto a qtChars caracteres. Se o texto original é maior que esse tamanho,
        /// ele será truncado. Se for menor, será completado com brancos.
        /// </summary>
        /// <param name="texto">texto a ser verificado</param>
        /// <param name="qtChars">quantidade máxima de caracteres</param>
        /// <returns>o texto com a quantidade de caracteres limitada.</returns>
        static public string StrAntiga(string texto, int qtChars)
        {
            string ret = texto;
            if (texto != null)
            {
                if (texto.Length > qtChars)
                    ret = texto.Substring(0, qtChars);
                else
                if (texto.Length < qtChars)
                    ret = texto + Repeat(' ', qtChars - texto.Length);
            }
            return ret;
        }

        public static string Repeat(char ch, int count)
        {
            return new string(ch, count);
        }
        #endregion // Toolset de strings

        #region Zip / Unzip
        public static string CompressToBase64(string uncompressedString)
        {
            return Compress(uncompressedString, WBaseFormat.bfBase64);
        }

        public static string DecompressFromBase64(string compressedString)
        {
            return Decompress(compressedString, WBaseFormat.bfBase64);
        }

        public static string CompressToBase36(string uncompressedString)
        {
            return Compress(uncompressedString, WBaseFormat.bfBase36);
        }
        public static string DecompressFromBase36(string compressedString)
        {
            return Decompress(compressedString, WBaseFormat.bfBase36);
        }


        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        /// <param name="format">format of the encoding</param>
        public static string Compress(string uncompressedString, WBaseFormat format)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                using var compressedStream = new MemoryStream();
                // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, true))
                {
                    uncompressedStream.CopyTo(compressorStream);
                }

                // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                compressedBytes = compressedStream.ToArray();
            }

            if (format == WBaseFormat.bfBase36)
                return WBase36.Encode(compressedBytes);
            return Convert.ToBase64String(compressedBytes);
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        /// <param name="format"></param>
        public static string Decompress(string compressedString, WBaseFormat format)
        {
            byte[] decompressedBytes;
            byte[] compressedBytes = (format == WBaseFormat.bfBase36) ?
                WBase36.Decode(compressedString) :
                Convert.FromBase64String(compressedString);

            var compressedStream = new MemoryStream(compressedBytes);

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using var decompressedStream = new MemoryStream();
                decompressorStream.CopyTo(decompressedStream);
                decompressedBytes = decompressedStream.ToArray();
            }

            return Encoding.UTF8.GetString(decompressedBytes);
        }
        #endregion // Zip / Unzip
    }
}
