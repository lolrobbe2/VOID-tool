using BeleidsPlanApi.src.database.Repo;
using Microsoft.Extensions.Caching.Memory;
using NetCord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoxholeBot.repositories
{
    public class Guild
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
    }
    public class GuildChannel
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
    }
    public class DiscordRepository
    {
        private readonly NetCord.Gateway.GatewayClient _client;
        private Cache<Guild> _guildNameCache, _guildIdCache;

        private Cache<GuildChannel[]> _guildChannelCache;
        public DiscordRepository(NetCord.Gateway.GatewayClient client, IMemoryCache cache) { 
            _client = client;
            _guildIdCache = new Cache<Guild>(cache);
            _guildNameCache = new Cache<Guild>(cache);

            _guildChannelCache = new Cache<GuildChannel[]>(cache);
        }

        public async Task<Guild[]> GetGuilds()
        {
            List<Guild> result = new List<Guild>();

            await foreach (RestGuild guild in _client.Rest.GetCurrentUserGuildsAsync())
            {
                result.Add(new()
                {
                    Id = guild.Id,
                    Name = guild.Name
                });
            }

            return result.ToArray();
        }

        public async Task<Guild> GetGuild(ulong id)
        {
            if (_guildIdCache.CacheGet(id, out Guild guild))
                return guild;
            Guild[] guilds = await GetGuilds();
            return _guildIdCache.CacheSet(guilds.FirstOrDefault(guild => guild.Id == id), id);
        }

        public async Task<Guild> GetGuild(string name)
        {
            if (_guildNameCache.CacheGet(name, out Guild guild))
                return guild;
            Guild[] guilds = await GetGuilds();
            return _guildNameCache.CacheSet(guilds.FirstOrDefault(guild => guild.Name == name), name);
        }

        public async Task<GuildChannel[]>   GetChannels(Guild guild)
        {
            if(_guildChannelCache.CacheGet(guild.Id,out GuildChannel[] channels))
                return channels;

            var _channels = await _client.Rest.GetGuildChannelsAsync(guild.Id);
            return _guildChannelCache.CacheSet(_channels.Select(channel => new GuildChannel() { Id = channel.Id, Name = channel.Name }).ToArray(),guild.Id);
        }
        public async Task<GuildChannel> GetChannel(Guild guild, ulong id)
        {
            return (await GetChannels(guild)).FirstOrDefault(channel => channel.Id.Equals(id));
        }
        public async Task<GuildChannel> GetChannel(Guild guild, string name)
        {
            foreach(var channel in await GetChannels(guild))
            {
                Console.Write(channel.Name);
            }
            return (await GetChannels(guild)).FirstOrDefault(channel => channel.Name == name);
        }

        public async Task SendMessage(GuildChannel channel, MessageProperties properties)
        {

            await _client.Rest.SendMessageAsync(channel.Id, properties);
        }
        public async Task SendMessage(GuildChannel channel, string message)
        {
            MessageProperties properties = new MessageProperties();
            properties.Content = message;
            await _client.Rest.SendMessageAsync(channel.Id, properties);
        }
    }
}
