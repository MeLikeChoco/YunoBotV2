using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects.Database.Guilds
{
    public class GuildSettingContext : DbContext
    {

        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source = Databases/Guilds.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Guild>().Property<string>("_autoRoleString").HasColumnName("AutoRoles");
            modelBuilder.Entity<Guild>().Property<string>("_selfRoleString").HasColumnName("SelfRoles");

        }

        public GuildSettingContext(IEnumerable<SocketGuild> guilds)
        {

            foreach (var guild in guilds)
            {

                if (Guilds.Find(guild.Id) == null)
                {

                    Guilds.Add(new Guild(guild.Id));
                    
                }

            }

            SaveChanges();

        }

        public GuildSettingContext() { }

    }
}
