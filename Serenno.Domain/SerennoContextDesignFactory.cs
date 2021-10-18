using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Serenno.Domain.Models;

namespace Serenno.Domain;

    public class SerennoContextDesignFactory : IDesignTimeDbContextFactory<SerennoContext>
    {
        public SerennoContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<SerennoContextDesignFactory>()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SerennoContext>()
                .UseSqlServer(configuration.GetValue<string>(nameof(SerennoConfig.DbConnection)));

            return new SerennoContext(optionsBuilder.Options);
        }
    }
