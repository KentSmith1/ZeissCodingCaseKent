using MachineStreamBackend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MachineStreamBackend
{
    public class MessagesContext : DbContext
    {
        public DbSet<StreamMessagePayload> MessagePayloads { get; set; }

        public string DbPath { get; }

        public MessagesContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "messages.db");
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
