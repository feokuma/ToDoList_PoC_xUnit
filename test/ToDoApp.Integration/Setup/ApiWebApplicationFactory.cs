using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Data;
using Xunit;

namespace ToDoApp.Integration.Setup
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>, IDisposable
    {
        public AppDbContext Context { get; private set; }
        public IServiceScope Scope { get; private set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationAppSettings = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Integration.json")
                    .Build();
                config.AddConfiguration(integrationAppSettings);
            });

            builder.ConfigureServices(services =>
            {
                Scope = services.BuildServiceProvider().CreateScope();
                Context = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
            });
        }

        // O código que vem aqui será executado depois de passar por todos os testes
        public new void Dispose()
        {
            Context.Dispose();
            Scope.Dispose();
        }
    }
}