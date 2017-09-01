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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Emzi0767.CompanionCube.Services;
using Microsoft.Extensions.PlatformAbstractions;

namespace Emzi0767.CompanionCube.Modules
{
    [NotBlocked]
    public sealed class MiscCommandsModule
    {
        private DatabaseClient Database { get; }
        private SharedData Shared { get; }

        public MiscCommandsModule(DatabaseClient database, SharedData shared)
        {
            this.Database = database;
            this.Shared = shared;
        }

        [Command("about"), Aliases("info"), Description("Displays information about the bot.")]
        public async Task AboutAsync(CommandContext ctx)
        {
            var ccv = typeof(CompanionCubeCore)
                .GetTypeInfo()
                .Assembly
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ??

                typeof(CompanionCubeCore)
                .GetTypeInfo()
                .Assembly
                .GetName()
                .Version
                .ToString(3);
            
            var dsv = ctx.Client.VersionString;
            var ncv = PlatformServices.Default
                .Application
                .RuntimeFramework
                .Version
                .ToString(2);
            
            try
            {
                var a = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(xa => xa.GetName().Name == "System.Private.CoreLib");
                var pth = Path.GetDirectoryName(a.Location);
                pth = Path.Combine(pth, ".version");
                using (var fs = File.OpenRead(pth))
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                {
                    await sr.ReadLineAsync();
                    ncv = await sr.ReadLineAsync();
                }
            }
            catch { }

            var invuri = string.Concat("https://discordapp.com/oauth2/authorize?scope=bot&permissions=0&client_id=", ctx.Client.CurrentApplication.Id);

            var embed = new DiscordEmbedBuilder
            {
                Title = "About Companion Cube",
                Url = "https://emzi0767.com/bots/companion_cube",
                Description = string.Concat("Companion Cube is a bot made by Emzi0767#1837 (<@!181875147148361728>). The source code is available on ", Formatter.MaskedUrl("Emzi's GitHub",
                        new Uri("https://github.com/Emzi0767/Discord-Companion-Cube-Bot"), "Companion Cube's source code on GitHub"),
                    ".\n\nThis shard is currently servicing ", ctx.Client.Guilds.Count.ToString("#,##0"),
                    " guilds.\n\nClick ", Formatter.MaskedUrl("this invite link", new Uri(invuri), "Companion Cube invite link"), " to invite me to your guild!"),
                Color = new DiscordColor(0xD091B2)
            };

            embed.AddField("Bot Version", string.Concat(DiscordEmoji.FromName(ctx.Client, ":companion_cube:"), " ", Formatter.Bold(ccv)), true)
                .AddField("DSharpPlus Version", string.Concat(DiscordEmoji.FromName(ctx.Client, ":dsplus:"), " ", Formatter.Bold(dsv)), true)
                .AddField(".NET Core Version", string.Concat(DiscordEmoji.FromName(ctx.Client, ":dotnet:"), " ", Formatter.Bold(ncv)), true);

            await ctx.RespondAsync("", embed: embed.Build()).ConfigureAwait(false);
        }

        [Command("uptime"), Description("Display bot's uptime.")]
        public async Task UptimeAsync(CommandContext ctx)
        {
            var upt = DateTime.Now - this.Shared.ProcessStarted;
            var ups = this.Shared.TimeSpanToString(upt);
            await ctx.RespondAsync(string.Concat("\u200b", DiscordEmoji.FromName(ctx.Client, ":companion_cube:"), " The bot has been running for ", Formatter.Bold(ups), ".")).ConfigureAwait(false);
        }

        [Command("cleanup")]
        public async Task CleanupAsync(CommandContext ctx, [Description("Maximum number of messages to clean up.")] int max_count = 100)
        {
            var lid = 0ul;
            for (var i = 0; i < max_count; i += 100)
            {
                var msgs = await ctx.Channel.GetMessagesAsync(Math.Min(max_count - i, 100), before: lid != 0 ? (ulong?)lid : null).ConfigureAwait(false);
                var msgsf = msgs.Where(xm => xm.Author.Id == ctx.Client.CurrentUser.Id).OrderBy(xm => xm.Id);

                var lmsg = msgsf.FirstOrDefault();
                if (lmsg == null)
                    break;

                lid = lmsg.Id;

                try
                {
                    await ctx.Channel.DeleteMessagesAsync(msgsf).ConfigureAwait(false);
                }
                catch (UnauthorizedException)
                {
                    foreach (var xmsg in msgs)
                        await xmsg.DeleteAsync();
                }
            }

            var msg = await ctx.RespondAsync(DiscordEmoji.FromName(ctx.Client, ":msokhand:").ToString()).ConfigureAwait(false);
            await Task.Delay(2500).ContinueWith(t => msg.DeleteAsync());
        }
    }
}