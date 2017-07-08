using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services;

namespace YunoBotV2.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Utility : CustomModuleBase
    {

        [Command("role")]
        [Summary("Selfrole with the entered role")]
        public async Task RoleCommand([Remainder]string search)
        {

            var guild = Context.Guild;
            var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);

            if (role == null)
                await ReplyAsync($"The role `{search}` does not exist!");
            else if (!Database.IsSelfRole(role))
                await ReplyAsync($"**{role.Name}** is not self role-able.");
            else
            {

                await (Context.User as SocketGuildUser).AddRoleAsync(role);
                await ReplyAsync($"You have roled yourself as **{role.Name}**!");

            }

        }

        [Command("unrole")]
        [Summary("Unrole with the entered role")]
        public async Task UnroleCommand([Remainder]string search)
        {

            var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);
            var user = Context.User as SocketGuildUser;

            if (role == null)
                await ReplyAsync($"The role `{search}` does not exist!");
            else if (!Database.IsSelfRole(role))
                await ReplyAsync($"**{role.Name}** is not self role-able.");
            else if (!user.Roles.Contains(role))
                await ReplyAsync($"You do not have the role **{role.Name}**!");
            else
            {

                await (Context.User as SocketGuildUser).RemoveRoleAsync(role);
                await ReplyAsync($"You have unroled yourself from **{role.Name}**!");

            }

        }

        [Command("selfroles")]
        [Summary("List all the selfroles")]
        public async Task SelfRolesCommand()
        {

            var roles = Database.GetRoles(Context.Guild)?
                .Select(id => Context.Guild.GetRole(id))
                .Select(role => role.Name);

            if (roles != null)
            {

                var display = $"```\n{string.Join("\n", roles)}```";

                await ReplyAsync(display);

            }
            else
                await ReplyAsync("There are no selfroles set up!");

        }

        [Command("autoroles")]
        [Summary("List all the autoroles")]
        public async Task AutoRolesCommand()
        {

            var roles = Database.GetAutoRoles(Context.Guild)?
                .Select(id => Context.Guild.GetRole(id))
                .Select(role => role.Name);

            if (roles != null)
            {

                var display = $"```\n{string.Join("\n", roles)}```";

                await ReplyAsync(display);

            }
            else
                await ReplyAsync("There are no autoroles set up!");

        }

        [RequireUserPermission(GuildPermission.Administrator)]
        public class UtilityAdmin : CustomModuleBase
        {

            [Command("addselfrole")]
            [Summary("Add a self role!")]
            public async Task AddRoleCommand([Remainder]string search)
            {

                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);

                if (role == null)
                    await ReplyAsync($"There is no role with the name of **{role.Name}**!");
                else if (Database.IsSelfRole(role))
                    await ReplyAsync($"**{role.Name}** is already self role-able!");
                else
                {

                    await Database.AddSelfRole(role);
                    await ReplyAsync($"**{role.Name}** is now self role-able!");

                }


            }

            [Command("delselfrole")]
            [Summary("Add a self role!")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task DelRoleCommand([Remainder]string search)
            {

                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);

                if (role == null)
                    await ReplyAsync($"There is no role with the name of **{role.Name}**!");
                else if (!Database.IsSelfRole(role))
                    await ReplyAsync($"**{role.Name}** is not self role-able!");
                else
                {

                    await Database.RemoveSelfRole(role);
                    await ReplyAsync($"**{role.Name}** is no longer self role-able!");

                }

            }

            [Command("autorole")]
            [Summary("Add an autorole")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task AutoRoleCommand([Remainder]string search)
            {

                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);

                if (role == null)
                    await ReplyAsync($"There is no role with the name of **{role.Name}**!");
                else
                {

                    await Database.AddAutoRole(role);
                    await ReplyAsync($"Users will now be given **{role.Name}** automatically! Use `roleall <role name>` if you want to give all users the role!");

                }

            }

            [Command("unautorole")]
            [Summary("Disable an autorole")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task UnAutoRoleCommand([Remainder]string search)
            {

                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);

                if (role == null)
                    await ReplyAsync($"There is no role with the name of **{role.Name}**!");
                else
                {

                    await Database.RemoveAutoRole(role);
                    await ReplyAsync($"Users will not be given **{role.Name}** automatically!");

                }

            }

            [Command("roleall")]
            [Summary("Give all users in the guild a role")]
            public async Task RoleAllCommand([Remainder]string search)
            {

                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);

                if (role == null)
                    await ReplyAsync($"There is no role with the name of **{role.Name}**!");
                else
                {

                    await ReplyAsync($"Giving all users the role **{role.Name}**, please wait!");

                    await Context.Guild.DownloadUsersAsync();

                    foreach (var user in Context.Guild.Users)
                    {

                        await Task.Delay(1500);
                        await user.AddRoleAsync(role);

                    }

                    await ReplyAsync($"All users have been given the role **{role.Name}**!");

                }

            }

            //the reason why only this command has a role as an accepted parameter is because
            //the role may not be assigned to anyone, therefore, not mention them
            //but if stupidity trumphs the intended usage, well, nothing i can do
            [Command("roleall")]
            [Summary("Give all users in the guild a role")]
            public async Task RoleAllCommand(SocketRole role)
            {

                await ReplyAsync($"Giving all users the role **{role.Name}**, please wait!");

                await Context.Guild.DownloadUsersAsync();

                foreach (var user in Context.Guild.Users)
                {

                    await Task.Delay(1500);
                    await user.AddRoleAsync(role);

                }

                await ReplyAsync($"All users have been given the role **{role.Name}**!");

            }

            [Command("unroleall")]
            [Summary("Give all users in the guild a role")]
            public async Task UnroleAllCommand([Remainder]string search)
            {

                var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == search);

                if (role == null)
                    await ReplyAsync($"There is no role with the name of **{role.Name}**!");
                else
                {
                    await ReplyAsync($"Removing the role **{role.Name}** from all users, please wait!");

                    await Context.Guild.DownloadUsersAsync();

                    foreach (var user in Context.Guild.Users)
                    {

                        await Task.Delay(1500);
                        await user.RemoveRoleAsync(role);

                    }

                    await ReplyAsync($"All users have been removed of the role **{role.Name}**!");

                }

            }

        }

    }
}
