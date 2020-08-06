using System;
using kafkaAndDbPairing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject.Utils
{
    public static class DataContextFactory
    {
        public static DataContext CreateTestDb()
        {

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();


            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString("d"))
                .UseInternalServiceProvider(serviceProvider);

            return new DataContext(builder.Options);
        }
    }
}
