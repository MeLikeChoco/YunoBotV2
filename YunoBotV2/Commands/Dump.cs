using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services.Extensions;

namespace YunoBotV2.Commands
{
    [Group("dump")]
    [RequireContext(ContextType.Guild)]
    public class Dump : CustomModuleBase
    {

        [Command("roles")]
        [Summary("Dump all the roles")]
        public async Task DumpRoles()
        {

            var roles = Context.Guild.Roles.Where(role => !role.IsEveryone).Select(role => role.Name).Aggregate((str, next) => str += $", {next}");

            if (roles.Length == 0)
                await ReplyAsync("There are no roles to list!");
            else
                await ReplyAsync($"```\n{roles}```");

        }

        [Command("users")]
        [Summary("Attempt to dump all the users")]
        public async Task DumpUsers()
        {

            var users = Context.Guild.Users.Take(1500) //i cant print out more than 2000 characters anyway
                .Select(user => user.Username)
                .Aggregate((str, next) => str += $";']- {next}"); //i needed a unique seperator, lmao

            if (users.Length > 2000)
            {

                users = "Too many users, only listing a few. " + users;
                users = users.Substring(0, 2000);
                var index = users.LastIndexOf(";']-");
                users = users.Substring(0, index);

            }

            await ReplyAsync($"```\n{users.Replace(";']-", ",")}```");

        }

        [Command("bans")]
        [Summary("Attempt to dump all the bans")]
        public async Task DumpBans()
        {

            var bans = await Context.Guild.GetBansAsync();

            if (bans.Count == 0)
            {

                await ReplyAsync("There are no bans to list!");
                return;

            }

            var banString = bans.Take(1500)
                .Select(ban => ban.User.Username)
                .Aggregate((str, next) => str += $";']- {next}");

            if (banString.Length > 2000)
            {

                banString = "Too many bans, only listing a few. " + banString;
                banString = banString.Substring(0, 2000);
                var index = banString.LastIndexOf(";']-");
                banString = banString.Substring(0, index);

            }

            await ReplyAsync($"```\n{banString.Replace(";']-", ",")}```");

        }

        [Command("text channels")]
        [Summary("Attempt to dump all the text channels")]
        public async Task DumpTextChannels()
        {

            var textChannels = Context.Guild.TextChannels.Take(1500)
                .Select(channel => channel.Name.Trim())
                .Aggregate((str, next) => str += $";']- {next}");

            if (textChannels.Length > 2000)
            {

                textChannels = "Too many text channels, only listing a few. " + textChannels;
                textChannels = textChannels.Substring(0, 2000);
                var index = textChannels.LastIndexOf(";']-");
                textChannels = textChannels.Substring(0, index);

            }

            await ReplyAsync($"```\n{textChannels.Replace(";']-", ",")}```");

        }

        [Command("voice channels")]
        [Summary("Attempt to dump all the text channels")]
        public async Task DumpVoiceChannels()
        {

            var voiceChannels = Context.Guild.VoiceChannels.Take(1500)
                .Select(channel => channel.Name.Trim())
                .Aggregate((str, next) => str += $";']- {next}");

            if (voiceChannels.Length > 2000)
            {

                voiceChannels = "Too many voice channels, only listing a few. " + voiceChannels;
                voiceChannels = voiceChannels.Substring(0, 2000);
                var index = voiceChannels.LastIndexOf(";']-");
                voiceChannels = voiceChannels.Substring(0, index);

            }

            await ReplyAsync($"```\n{voiceChannels.Replace(";']-", ",")}```");

        }

        [Command("guild")]
        [Summary("Dump guild info")]
        public async Task DumpGuild()
        {

            var guild = Context.Guild;
            var padAmount = 18;
            var emotes = guild.Emotes.Count == 0 ? "None" : string.Join(", ", guild.Emotes);
            var features = guild.Features.Count == 0 ? "None" : string.Join(", ", guild.Features);
            var roles = guild.Roles.Count == 0 ? "None" : string.Join(", ", guild.Roles);
            var builder = new StringBuilder("```fix\n");

            builder.AppendLPaddingChar($"Name: {guild.Name}", padAmount);
            builder.AppendLPaddingChar($"ID: {guild.Id}", padAmount);
            builder.AppendLPaddingChar($"Created: {GetCreatedString(guild.CreatedAt)} ({guild.CreatedAt})", padAmount);
            builder.AppendLPaddingChar($"Region: {guild.VoiceRegionId.ToUpper()}", padAmount);
            builder.AppendLPaddingChar($"Notification: {guild.DefaultMessageNotifications.ToString().InsertSpaces()}", padAmount);
            builder.AppendLPaddingChar($"Verification: {guild.VerificationLevel}", padAmount);
            builder.AppendLPaddingChar($"MFA Level: {guild.MfaLevel.ToString().InsertSpaces()}", padAmount);
            builder.AppendLPaddingChar($"Channels: {guild.TextChannels.Count} (Text) / {guild.VoiceChannels.Count} (Voice)", padAmount);
            builder.AppendLPaddingChar($"Default Channel: {guild.DefaultChannel}", padAmount);
            builder.AppendLPaddingChar($"AFK Channel: {guild.AFKChannel?.ToString() ?? "None"}", padAmount);
            builder.AppendLPaddingChar($"Afk Timeout: {guild.AFKTimeout} seconds", padAmount);
            builder.AppendLPaddingChar($"Embed Channel: {guild.EmbedChannel?.ToString() ?? "None"}", padAmount);
            builder.AppendLPaddingChar($"Members: {guild.MemberCount} ({guild.Users.Where(user => user.Status == UserStatus.Online).Count()} online)", padAmount);
            builder.AppendLPaddingChar($"Owner: {guild.Owner}", padAmount);
            builder.AppendLPaddingChar($"Features: {features}", padAmount);
            builder.AppendLPaddingChar($"Icon: {guild.IconUrl ?? "None"}", padAmount);
            builder.AppendLPaddingChar($"Splash: {guild.SplashUrl ?? "None"}", padAmount);
            builder.AppendLPaddingChar($"Embedabble: {guild.IsEmbeddable}", padAmount);
            builder.AppendLPaddingChar($"Roles: {roles}", padAmount);
            builder.AppendLPaddingChar($"Emotes: {emotes}", padAmount);
            builder.Append("```");

            await ReplyAsync(builder.ToString());

        }

        [Command("user")]
        [Summary("Dump guild info")]
        public async Task DumpUser(SocketGuildUser user = null)
        {

            if (user == null)
                user = Context.User as SocketGuildUser;

            var padAmount = 18;
            var roles = user.Roles.Count != 1 ? user.Roles.Where(role => !role.IsEveryone) : user.Roles;
            var joinedAt = user.JoinedAt.Value.UtcDateTime;
            var createdAt = user.CreatedAt.UtcDateTime;
            var builder = new StringBuilder("```fix\n");

            builder.AppendLPaddingChar($"Name: {user}", padAmount);

            if (!string.IsNullOrEmpty(user.Nickname))
                builder.AppendLPaddingChar($"Nickname: {user.Nickname}", padAmount);

            builder.AppendLPaddingChar($"Id: {user.Id}", padAmount);
            builder.AppendLPaddingChar($"Discriminator: {user.Discriminator}", padAmount);
            builder.AppendLPaddingChar($"Joined: {GetCreatedString(joinedAt)} ({joinedAt})", padAmount);
            builder.AppendLPaddingChar($"Created: {GetCreatedString(createdAt)} ({createdAt})", padAmount);
            builder.AppendLPaddingChar($"Hierarchy: {user.Hierarchy}", padAmount);

            if (user.VoiceChannel != null)
            {

                builder.AppendLPaddingChar($"Voice Channel: {user.VoiceChannel}", padAmount);
                builder.AppendLPaddingChar($"Voice Session Id: {user.VoiceSessionId}", padAmount);

            }

            builder.AppendLPaddingChar($"Status: {user.Status}", padAmount);

            if (user.Game.HasValue)
            {

                var game = user.Game.Value;

                builder.AppendLPaddingChar($"Game: {game.Name}", padAmount);
                
                if(game.StreamType == StreamType.Twitch)
                    builder.AppendLPaddingChar($"Stream: {game.StreamUrl}", padAmount);

            }

            builder.AppendLPaddingChar($"Is Bot: {user.IsBot}", padAmount);
            builder.AppendLPaddingChar($"Is Deafened: {user.IsDeafened}", padAmount);
            builder.AppendLPaddingChar($"Is Muted: {user.IsMuted}", padAmount);
            builder.AppendLPaddingChar($"Is Self Deafened: {user.IsSelfDeafened}", padAmount);
            builder.AppendLPaddingChar($"Is Self Muted: {user.IsSelfMuted}", padAmount);
            builder.AppendLPaddingChar($"Is Supressed: {user.IsSuppressed}", padAmount);
            builder.AppendLPaddingChar($"Is Webhook: {user.IsWebhook}", padAmount);
            builder.AppendLPaddingChar($"Avatar: {user.GetAvatarUrl().SubStringTo("?")}", padAmount);
            builder.AppendLPaddingChar($"Roles: {string.Join(", ", roles)}", padAmount);

            builder.Append("```");

            await ReplyAsync(builder.ToString());

        }

        [Command("role")]
        [Summary("Dump information on a role")]
        public async Task DumpRoleCommand(SocketRole role)
        {

            var padAmount = 17;
            var color = role.Color;
            var permissions = string.Join(", ", role.Permissions.ToList().Select(perm => perm.ToString()));
            var builder = new StringBuilder("```fix\n");

            builder.AppendLPaddingChar($"Name: {role.Name}", padAmount);
            builder.AppendLPaddingChar($"ID: {role.Id}", padAmount);
            builder.AppendLPaddingChar($"Created at: {GetCreatedString(role.CreatedAt)}", padAmount);
            builder.AppendLPaddingChar($"Members: {role.Members.Count()}", padAmount);
            builder.AppendLPaddingChar($"Position: {role.Position}", padAmount);
            builder.AppendLPaddingChar($"Color: {color.R}, {color.G}, {color.B}", padAmount);
            builder.AppendLPaddingChar($"Is Mentionable: {role.IsMentionable}", padAmount);
            builder.AppendLPaddingChar($"Is Managed: {role.IsManaged}", padAmount);
            builder.AppendLPaddingChar($"Is Hoisted: {role.IsHoisted}", padAmount);
            builder.AppendLPaddingChar($"Permissions: {permissions}", padAmount);

            builder.Append("```");

            await ReplyAsync(builder.ToString());

        }

        //worse calculation ever, but its close enough
        private string GetCreatedString(DateTimeOffset? creationDate)
        {

            if (creationDate.HasValue)
            {

                var history = DateTime.UtcNow.Subtract(creationDate.Value.UtcDateTime);
                var averageDays = 30.42;
                var str = "";

                if (history.TotalDays >= 356)
                {

                    var years = (int)(history.TotalDays / 365);
                    var months = (int)((history.TotalDays - (365 * years)) / averageDays);
                    var days = (int)(history.TotalDays - (365 * years) - (months * averageDays));
                    var yearOrYears = years == 1 ? "year" : "years";
                    var monthOrMonths = months == 1 ? "month" : "months";
                    var dayOrDays = days == 1 ? "day" : "days";
                    str += $"{years} {yearOrYears}";

                    if (months != 0)
                        str += $", {months} {monthOrMonths}";
                    if (days != 0)
                        str += $", {days} {dayOrDays}";

                }
                else if (history.TotalDays >= 30)
                {

                    var months = (int)(history.TotalDays / averageDays);
                    var days = (int)(history.TotalDays - (months * averageDays));
                    var monthOrMonths = months == 1 ? "month" : "months";
                    var dayOrDays = days == 1 ? "day" : "days";

                    if (months != 0)
                        str += $"{months} {monthOrMonths}";
                    if (days != 0)
                        str += $", {days} {dayOrDays}";

                }
                else
                {

                    var dayOrDays = history.TotalDays == 1 ? "day" : "days";
                    str += $"{history.TotalDays} {dayOrDays}";

                }

                return str;

            }
            else
                return null;

        }

    }
}
