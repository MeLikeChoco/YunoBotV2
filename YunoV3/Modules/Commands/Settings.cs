using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using YunoV3.Objects.Database.Guilds;
using Discord.WebSocket;
using Discord;

namespace YunoV3.Modules.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Settings : CustomBase
    {

        public GuildSettingContext DbContext { get; set; }

        private Guild _guild;

        protected override void BeforeExecute(CommandInfo command)
            => _guild = DbContext.Find<Guild>(Context.Guild.Id);

        protected override void AfterExecute(CommandInfo command)
            => DbContext.SaveChanges();

        [Command("role")]
        [Summary("Role yourself with one of the self roles!")]
        public async Task SelfRole([Remainder]string input)
        {

            var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == input);

            if (role == null)
                await NoResultError("roles", input);
            else if (_guild.SelfRoles.Contains(role.Id))
            {
                                
                await (Context.User as SocketGuildUser).AddRoleAsync(role);
                await ReplyAsync($"You have been given **{role.Name}**!");

            }
            else
                await ReplyAsync($"**{role.Name}** is not a self role! Use `{_guild.Prefix} selfroles` to check the guild's self roles!");

        }

        [Command("unrole")]
        [Summary("Unrole yourself from one of the self roles!")]
        public async Task Unrole([Remainder]string input)
        {

            var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == input);

            if (role == null)
                await NoResultError("roles", input);
            else if (_guild.SelfRoles.Contains(role.Id))
            {

                if ((Context.User as SocketGuildUser).Roles.Any(r => r.Id == role.Id))
                {

                    await (Context.User as SocketGuildUser).RemoveRoleAsync(role);
                    await ReplyAsync($"**{role.Name}** has been removed from you!");

                }
                else
                    await ReplyAsync($"You do not have **{role.Name}**!");

            }
            else
                await ReplyAsync($"**{role.Name}** is not a self role! Use `{_guild.Prefix} selfroles` to check the guild's self roles!");

        }

        [Command("autoroles")]
        [Summary("Gets the auto roles!")]
        public async Task ListAutoRoles()
        {

            var roles = _guild.AutoRoles;

            if(roles.Count() == 0)
            {

                await ReplyAsync("You do not have any auto roles setup!");
                return;

            }

            var counter = 1;
            var builder = new StringBuilder("```fix");
            builder.AppendLine();

            foreach(var role in roles)
            {

                var name = Context.Guild.GetRole(role).Name;

                builder.AppendLine($"{counter}. {name}");
                counter++;

            }

            builder.Append("```");
            await ReplyAsync(builder.ToString());

        }

        [Command("addautorole")]
        [Summary("Adds an auto role!")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddAutoRoles(params SocketRole[] input)
        {

            var roles = input.Where(r => !_guild.AutoRoles.Contains(r.Id)).ToArray();
            var existing = input.Where(r => _guild.AutoRoles.Contains(r.Id)).ToArray();

            foreach (var role in roles)
            {

                _guild.AddAutoRole(role);

            }

            await ReplyAsync($"{roles.Length} auto roles have been added! (**{existing.Length}** ommitted)");

        }

        [Command("delautorole")]
        [Summary("Delete an auto role!")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DelAutoRoles(params SocketRole[] input)
        {

            var roles = input.Where(r => _guild.AutoRoles.Contains(r.Id)).ToArray();
            var nonexisting = input.Where(r => !_guild.AutoRoles.Contains(r.Id)).ToArray();

            foreach (var role in roles)
            {

                _guild.AddAutoRole(role);

            }

            await ReplyAsync($"{roles.Length} auto roles have been removed! (**{nonexisting.Length}** ommitted)");

        }

        [Command("selfroles")]
        [Summary("Get all the self roles!")]
        public async Task ListSelfRoles()
        {

            var roles = _guild.SelfRoles;

            if(roles.Count() == 0)
            {

                await ReplyAsync("You do not have any self roles setup!");
                return;

            }

            var counter = 1;
            var builder = new StringBuilder("```fix");
            builder.AppendLine();

            foreach (var role in roles)
            {

                var name = Context.Guild.GetRole(role).Name;

                builder.AppendLine($"{counter}. {name}");
                counter++;

            }

            builder.Append("```");
            await ReplyAsync(builder.ToString());

        }
        
        [Command("addselfrole")]
        [Summary("Add a self role!")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddSelfRoles(params SocketRole[] input)
        {

            var roles = input.Where(r => !_guild.SelfRoles.Contains(r.Id)).ToArray();
            var existing = input.Where(r => _guild.SelfRoles.Contains(r.Id)).ToArray();

            foreach (var role in roles)
            {

                _guild.AddSelfRole(role);

            }

            await ReplyAsync($"**{roles.Length}** self roles have been added! (**{existing.Length}** ommitted)");

        }

        [Command("delselfrole")]
        [Summary("Delete a self role!")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DelSelfRoles(params SocketRole[] input)
        {

            var roles = input.Where(r => _guild.SelfRoles.Contains(r.Id)).ToArray();
            var nonexisting = input.Where(r => !_guild.SelfRoles.Contains(r.Id)).ToArray();

            foreach (var role in roles)
            {

                _guild.RemoveSelfRole(role);

            }

            await ReplyAsync($"**{roles.Length}** self roles have been removed! (**{nonexisting.Length}** ommitted)");

        }

        [Command("prefix")]
        [Summary("Set the prefix for the bot on this guild!")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public Task SetPrefix([Remainder]string prefix)
        {

            _guild.Prefix = prefix;

            return ReplyAsync($"The prefix has been set to `{prefix}`!");

        }

    }
}
