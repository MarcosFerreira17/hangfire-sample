using Hangfire;
using Hangfire.SqlServer;

namespace SampleApplication.Extensions;

public static class ServiceExtensions
{
    public static void AddBaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureExtensions(configuration);
        services.HangfireExtensions(configuration);
    }

    private static void AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
    }

    private static void HangfireExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(connectionString,
            new SqlServerStorageOptions
            {
                //SchemaName = "Hangfire", // Esquema do banco de dados
                PrepareSchemaIfNecessary = true, // Preparar o esquema do banco de dados, se necessário
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5), // Tempo máximo de espera por lote de comando
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5), // Timeout de invisibilidade deslizante
                QueuePollInterval = TimeSpan.Zero, // Intervalo de pesquisa de fila, se zero, as filas serão consultadas o mais rápido possível
                UseRecommendedIsolationLevel = true, // Utilize o nível de isolamento recomendado
                DisableGlobalLocks = true // Desative travas globais (mais eficiente para escalabilidade)
            });

            // Configuração avançada para o Hangfire Dashboard
            config.UseDashboardMetric(SqlServerStorage.ActiveConnections); // Adicione métricas personalizadas ao Dashboard
            config.UseDashboardMetric(SqlServerStorage.TotalConnections); // Adicione métricas personalizadas ao Dashboard
        });

        // Configuração avançada para o Hangfire Server
        services.AddHangfireServer(options =>
        {
            options.Queues = new[] { "default", "critical" }; // Defina as filas que o servidor Hangfire irá monitorar
            options.ServerName = "Worker"; // Defina o nome do servidor Hangfire
            options.WorkerCount = Environment.ProcessorCount * 5; // Defina o número de trabalhadores Hangfire
            options.SchedulePollingInterval = TimeSpan.FromSeconds(15); // Intervalo de verificação de agendamento
            options.ServerTimeout = TimeSpan.FromMinutes(30); // Tempo limite do servidor
            options.ServerCheckInterval = TimeSpan.FromSeconds(10); // Intervalo de verificação do servidor
            options.HeartbeatInterval = TimeSpan.FromSeconds(10); // Intervalo de batimento cardíaco
            options.ServerTimeout = TimeSpan.FromMinutes(5); // Tempo limite do servidor Hangfire
        });


        // Opcional: Adicione autorização ao Dashboard
        services.AddAuthorization(options =>
        {
            options.AddPolicy("HangfirePolicy", policy =>
            {
                // Implemente a lógica de autorização conforme necessário
                policy.RequireAuthenticatedUser();
            });
        });
    }
}
