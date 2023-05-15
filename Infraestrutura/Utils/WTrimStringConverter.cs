using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infraestrutura.Utils
{
    public class WTrimStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString().TrimEnd();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.TrimEnd());
        }
    }
}
