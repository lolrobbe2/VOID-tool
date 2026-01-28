using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace FoxholeBot.src.Discord.shemas.commands
{
    public abstract record SendCommandPayload<TCommand, TArgs>
        where TCommand : Enum;

    public record SendCommandPayloadStandard<TCommand, TArgs>
    : SendCommandPayload<TCommand, TArgs>
    where TCommand : Enum
    {

        [JsonPropertyName("cmd")]
        public required TCommand Cmd { get; init; }
        [JsonPropertyName("args")]
        public TArgs? Args { get; init; }
        [JsonPropertyName("transfer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object[]? Transfer { get; init; }
    }

    public sealed record SendCommandPayloadSubscription<TArgs>
    : SendCommandPayload<Commands, TArgs>
    {
        [JsonPropertyName("cmd")]
        public required Commands Cmd { get; init; } // must be SUBSCRIBE or UNSUBSCRIBE
        [JsonPropertyName("args")]
        public TArgs? Args { get; init; }
        [JsonPropertyName("transfer")]
        public required string Evt { get; init; }
    }




    public class JsonStringEnumConverterGeneric<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (value == null)
                throw new JsonException($"Expected string value for enum {typeof(TEnum).Name}");

            if (Enum.TryParse<TEnum>(value, ignoreCase: true, out var result))
                return result;

            throw new JsonException($"Unknown value '{value}' for enum {typeof(TEnum).Name}");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

}
