using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chat.Common.Converter
{
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var @string = reader.GetString();
            if (string.IsNullOrWhiteSpace(@string))
            {
                return null;
            }
            return DateTime.Parse(@string);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.Value.ToString("o"));
            }
        }
    }
}