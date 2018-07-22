// This file is part of Emzi0767.CompanionCube project
//
// Copyright 2017 Emzi0767
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Emzi0767.CompanionCube.Exceptions;
using Emzi0767.CompanionCube.Modules;

namespace Emzi0767.CompanionCube.Services
{
    public sealed class DatabaseClient
    {
        private readonly Dictionary<ulong, string> _channelPrefixes = new Dictionary<ulong, string>();
        private readonly Dictionary<ulong, string> _guildPrefixes = new Dictionary<ulong, string>();
        private readonly Dictionary<ulong, long> _currency = new Dictionary<ulong, long>();
        private readonly Dictionary<(ulong channelId, string name), (ulong authorId, string contents)> _tags = new Dictionary<(ulong channelId, string name), (ulong authorId, string contents)>();
        private readonly Dictionary<ulong, bool> _musicEnabled = new Dictionary<ulong, bool>();
        public const string SchemaVersion = "1";

        public DatabaseClient(CompanionCubeDatabaseConfig config)
        {
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task<IReadOnlyDictionary<ulong, string>> GetChannelPrefixesAsync() 
            => Task.FromResult<IReadOnlyDictionary<ulong, string>>(_channelPrefixes);

        public Task<IReadOnlyDictionary<ulong, string>> GetGuildPrefixesAsync()
            => Task.FromResult<IReadOnlyDictionary<ulong, string>>(_guildPrefixes);

        public Task SetChannelPrefixAsync(ulong channelId, string prefix)
        {
            _channelPrefixes[channelId] = prefix;
            return Task.CompletedTask;
        }

        public Task SetGuildPrefixAsync(ulong guildId, string prefix)
        {
            _guildPrefixes[guildId] = prefix;
            return Task.CompletedTask;
        }

        public Task ResetChannelPrefixAsync(ulong channelId)
        {
            _channelPrefixes.Remove(channelId);
            return Task.CompletedTask;
        }

        public Task ResetGuildPrefixAsync(ulong guildId)
        {
            _channelPrefixes.Remove(guildId);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<ulong>> GetBlockedUsersAsync()
            => Task.FromResult<IReadOnlyList<ulong>>(ImmutableList<ulong>.Empty);

        public Task<IReadOnlyList<ulong>> GetBlockedChannelsAsync()
            => Task.FromResult<IReadOnlyList<ulong>>(ImmutableList<ulong>.Empty);

        public Task<IReadOnlyList<ulong>> GetBlockedGuildsAsync()
            => Task.FromResult<IReadOnlyList<ulong>>(ImmutableList<ulong>.Empty);

        public Task BlockUserAsync(ulong _)
        {
            throw new NotImplementedException("fail");
        }

        public Task UnblockUserAsync(ulong _)
        {
            throw new NotImplementedException("fail");
        }

        public Task BlockChannelAsync(ulong _)
        {
            throw new NotImplementedException("fail");
        }

        public Task UnblockChannelAsync(ulong _)
        {
            throw new NotImplementedException("fail");
        }

        public Task BlockGuildAsync(ulong _)
        {
            throw new NotImplementedException("fail");
        }

        public Task UnblockGuildAsync(ulong _)
        {
            throw new NotImplementedException("fail");
        }

        public Task IssueCurrencyAsync(ulong user, long amount)
        {
            if (!_currency.TryAdd(user, amount))
            {
                _currency[user] += amount;
            }

            return Task.CompletedTask;
        }

        public Task<long> GetCurrencyAsync(ulong user)
        {
            return Task.FromResult(_currency.TryGetValue(user, out var v) ? v : 0);
        }

        public Task TransferCurrencyAsync(ulong source, ulong target, long amount)
        {
            if (amount == 0)
                throw new Exception("Nothing to trade");
            if (!_currency.TryGetValue(source, out var senderCash) || senderCash < amount)
                throw new Exception("Sender not enough money");
            IssueCurrencyAsync(target, -amount); // yeah not race condition safe deal with it
            IssueCurrencyAsync(target, amount);
            
            return Task.CompletedTask;
        }

        public Task<bool> CreateTagAsync(ulong authorId, ulong channelId, string name, string contents)
        {
            if (_tags.ContainsKey((channelId, name)))
                return Task.FromResult(false);
            _tags[(channelId, name)] = (authorId, contents);
            return Task.FromResult(true);
        }

        public Task<TagResult> GetTagAsync(ulong channelId, string name)
        {
            var result = new TagResult { IsSuccess = false };
            if (!_tags.TryGetValue((channelId, name), out var tag)) return Task.FromResult(result);
            
            result.IsSuccess = true;
            result.ResultTag = new Tag
            {
                Id = 7 * "I don't really care".GetHashCode() ^ 420, // very secure
                Name = name,
                Contents = new[] { tag.contents },
                EditingUserIds = new ulong[] {},
                Edits = new DateTimeOffset[] {},
                ChannelId = channelId,
                OwnerId = tag.authorId,
                Uses = (long) (new Random().Next() * new Random().NextDouble()) / 4, // chosen by fair dice roll
                IsHidden = new Random().Next() > 0.5
            };
            return Task.FromResult(result);
        }

        public Task IncrementTagUsageAsync(long tagId)
        {
            return Task.CompletedTask;
        }

        public Task<TagResult> ListTagsAsync(ulong channelId, string name)
        {
            // fuck off
            return Task.FromResult(new TagResult());
        }

        public Task SetTagHiddenFlagAsync(ulong channelId, string name, bool flag)
        {
            // why
            return Task.CompletedTask;
        }

        public Task<bool> DeleteTagAsync(long id, ulong userId, bool force)
        {
            throw new Exception("Go away");
        }

        public Task<bool> EditTagAsync(long id, ulong userId, string contents, bool force)
        {
            throw new Exception("I'm tired");
        }

        public Task<Dictionary<ulong, double>> GetShekelRatesAsync()
        {
            // WHAT THE FUCK IS THIS??????????
            var dict = new Dictionary<ulong, double>();
            return Task.FromResult(dict);
        }

        public Task SetShekelRateAsync(ulong guildId, double rate)
        {
            return Task.CompletedTask;
        }

        public Task ResetShekelRateAsync(ulong guildId)
        {
            return Task.CompletedTask;
        }

        public Task SetMusicOptionAsync(ulong guildId, bool enabled)
        {
            _musicEnabled[guildId] = enabled;
            return Task.CompletedTask;
        }

        public Task<bool> GetMusicOptionAsync(ulong guildId)
        {
            return Task.FromResult(_musicEnabled.TryGetValue(guildId, out var enabled) && enabled);
        }
    }
}