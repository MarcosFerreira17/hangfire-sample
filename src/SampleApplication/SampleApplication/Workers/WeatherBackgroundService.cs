
using Hangfire;

namespace SampleApplication.Workers;

public class WeatherBackgroundService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Job em segundo plano
        BackgroundJob.Enqueue(() => Console.WriteLine("Este é um job em segundo plano."));

        // Job com atraso de 5 minutos
        BackgroundJob.Schedule(() => Console.WriteLine("Este é um job com atraso de 5 minutos."), TimeSpan.FromMinutes(5));

        // Job recorrente que é executado a cada 1 minuto
        RecurringJob.AddOrUpdate("job-recorrente-com-nome", () => Console.WriteLine("Este é um job recorrente com nome."), Cron.Minutely);

        // Job com parâmetros
        BackgroundJob.Enqueue(() => ExemploJobComParametros(10, "Hangfire"));

        // Job principal
        var jobId = BackgroundJob.Enqueue(() => JobPrincipal());

        // Job de continuação
        BackgroundJob.ContinueWith(jobId, () => JobDeContinuacao(), JobContinuationOptions.OnlyOnSucceededState);


        // Job que pode falhar
        BackgroundJob.Enqueue(() => JobQuePodeFalhar());




        return Task.CompletedTask;
    }

    public void JobQuePodeFalhar()
    {
        // Simulação de um erro que pode ocorrer durante a execução do job
        throw new Exception("Ocorreu um erro durante a execução do job.");
    }

    public void ExemploJobComParametros(int numero, string texto)
    {
        Console.WriteLine($"Número: {numero}, Texto: {texto}");
    }

    public void JobPrincipal()
    {
        Console.WriteLine("Este é o job principal.");
    }

    public void JobDeContinuacao()
    {
        Console.WriteLine("Este é o job de continuação.");
    }
}
