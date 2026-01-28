using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoxholeBot.src.Discord.shemas
{

    public enum OAuthScope
    {
        [EnumMember(Value = "bot")]
        Bot,

        [EnumMember(Value = "rpc")]
        Rpc,

        [EnumMember(Value = "identify")]
        Identify,

        [EnumMember(Value = "connections")]
        Connections,

        [EnumMember(Value = "email")]
        Email,

        [EnumMember(Value = "guilds")]
        Guilds,

        [EnumMember(Value = "guilds.join")]
        GuildsJoin,

        [EnumMember(Value = "guilds.members.read")]
        GuildsMembersRead,

        [EnumMember(Value = "gdm.join")]
        GdmJoin,

        [EnumMember(Value = "messages.read")]
        MessagesRead,

        [EnumMember(Value = "rpc.notifications.read")]
        RpcNotificationsRead,

        [EnumMember(Value = "rpc.voice.write")]
        RpcVoiceWrite,

        [EnumMember(Value = "rpc.voice.read")]
        RpcVoiceRead,

        [EnumMember(Value = "rpc.activities.write")]
        RpcActivitiesWrite,

        [EnumMember(Value = "webhook.incoming")]
        WebhookIncoming,

        [EnumMember(Value = "applications.commands")]
        ApplicationsCommands,

        [EnumMember(Value = "applications.builds.upload")]
        ApplicationsBuildsUpload,

        [EnumMember(Value = "applications.builds.read")]
        ApplicationsBuildsRead,

        [EnumMember(Value = "applications.store.update")]
        ApplicationsStoreUpdate,

        [EnumMember(Value = "applications.entitlements")]
        ApplicationsEntitlements,

        [EnumMember(Value = "relationships.read")]
        RelationshipsRead,

        [EnumMember(Value = "activities.read")]
        ActivitiesRead,

        [EnumMember(Value = "activities.write")]
        ActivitiesWrite,

        [EnumMember(Value = "dm_channels.read")]
        DmChannelsRead
    }

    public sealed class OAuthScopeJsonConverter : JsonConverter<OAuthScope>
    {
        public override OAuthScope Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString()!;

            foreach (FieldInfo field in typeof(OAuthScope).GetFields())
            {
                EnumMemberAttribute attribute = field.GetCustomAttribute<EnumMemberAttribute>();
                if (attribute != null && attribute.Value == value)
                {
                    return (OAuthScope)field.GetValue(null)!;
                }
            }

            throw new JsonException($"Unknown OAuth scope '{value}'");
        }

        public override void Write(Utf8JsonWriter writer, OAuthScope value, JsonSerializerOptions options)
        {
            FieldInfo field = typeof(OAuthScope).GetField(value.ToString())!;
            EnumMemberAttribute attribute = field.GetCustomAttribute<EnumMemberAttribute>();

            writer.WriteStringValue(attribute != null ? attribute.Value : value.ToString());
        }
    }
    public sealed class OAuthScopeArrayJsonConverter : JsonConverter<OAuthScope[]>
    {
        private readonly OAuthScopeJsonConverter _elementConverter = new OAuthScopeJsonConverter();

        public override OAuthScope[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            var list = new List<OAuthScope>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                list.Add(_elementConverter.Read(ref reader, typeof(OAuthScope), options));
            }

            return list.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, OAuthScope[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                _elementConverter.Write(writer, item, options);
            }
            writer.WriteEndArray();
        }
    }

}
