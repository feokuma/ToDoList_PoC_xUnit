using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;
using ToDoApp.Data;
using Xunit;

namespace ToDoApp.Integration.Setup
{
    public abstract class TestBase : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
    {
        public HttpClient Client { get; set; }
        public AppDbContext Context { get; }

        public readonly Checkpoint Checkpoint = new()
        {
            TablesToIgnore = new[]{
                "_EFMigrationsHistory"
            },
            DbAdapter = DbAdapter.Postgres
        };

        // Este código será executado antes de cada teste e antes do InitializeAsync
        public TestBase(ApiWebApplicationFactory factory)
        {
            Client = factory.CreateClient();
            Context = factory.Context;
        }

        // Este código será executado antes de cada teste e depois do construtor TestBase e do construtor da classe filha
        public async Task InitializeAsync()
        {
            await Context.Database.EnsureCreatedAsync();
            Context.ChangeTracker.Clear();
            using var npgsqlConnection = new NpgsqlConnection(Context.ConnectionString);
            await npgsqlConnection.OpenAsync();
            await Checkpoint.Reset(npgsqlConnection);
        }

        // O DisposeAsync será executado depois de cada teste
        public async Task DisposeAsync() => await Task.CompletedTask;
    }
}