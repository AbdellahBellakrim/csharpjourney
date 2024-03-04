using GameStore.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.data;

public static class DataExtentions
{
    public static void InitializeDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services, IConfiguration configuration
    ){
    var connString = configuration.GetConnectionString("GameStoreContext");
    services.AddSqlServer<GameStoreContext>(connString).AddScoped<IGamesRepository, EntityFrameworkGamesRepository>();

    return services;
    }
}



// ConnectionStrings:GameStoreContext = Server=localhost;Database=GameStore;User Id=sa;Password=Your_password123;TrustServerCertificate=True
// builder.Services.AddSingleton<IGamesRepository, InMemGamesRepository>();