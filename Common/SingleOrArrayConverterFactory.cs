using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace OldenEraTemplateEditor.Common;

public class SingleOrArrayConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
            return false;

        return typeToConvert.GetGenericTypeDefinition() == typeof(List<>);
    }

    public override JsonConverter CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        Type itemType = typeToConvert.GetGenericArguments()[0];

        Type converterType =
            typeof(SingleOrArrayConverter<>).MakeGenericType(itemType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}
public class SingleOrArrayConverter<T>
    : JsonConverter<List<T>>
{
    public override List<T> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        List<T> result = new();

        // 数组
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                T? item = JsonSerializer.Deserialize<T>(
                    ref reader,
                    options);

                if (item != null)
                    result.Add(item);
            }
        }
        // 单值
        else
        {
            T? item = JsonSerializer.Deserialize<T>(
                ref reader,
                options);

            if (item != null)
                result.Add(item);
        }

        return result;
    }

    public override void Write(
        Utf8JsonWriter writer,
        List<T> value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }

        writer.WriteEndArray();
    }
}