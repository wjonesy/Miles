﻿using Miles.MassTransit.EntityFramework.Configurations.Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.EntityFramework.MessageDeduplication;
using Miles.MassTransit.MessageDeduplication;
using Miles.Sample.Domain.Command.Fixtures;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System.Data.Entity;
using System.Reflection;

namespace Miles.Sample.Persistence.EF
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext() : base("Miles.Sample")
        { }

        public DbSet<Domain.Command.Fixtures.Fixture> Fixtures { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<IncomingMessage> IncomingMessages { get; set; }
        public DbSet<OutgoingMessage> OutgoingMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.AddFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Configurations.Add(new IncomingMessageConfiguration());
            modelBuilder.Configurations.Add(new OutgoingMessageConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
