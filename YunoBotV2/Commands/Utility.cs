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
    public class Utility : CustomModuleBase
    {

        [Command("role")]
        [Summary("Selfrole with the entered role")]
        [RequireContext(ContextType.Guild)]
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
        [RequireContext(ContextType.Guild)]
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
        [RequireContext(ContextType.Guild)]
        public async Task SelfRolesCommand()
        {

            var roles = Database.GetRoles(Context.Guild).Select(id => Context.Guild.GetRole(id)).Select(role => role.Name);
            var display = $"```\n{string.Join("\n", roles)}```";

            await ReplyAsync(display);

        }

        [Command("addrole")]
        [Summary("Add a self role!")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
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

        [Command("delrole")]
        [Summary("Add a self role!")]
        [RequireContext(ContextType.Guild)]
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

    }
}
