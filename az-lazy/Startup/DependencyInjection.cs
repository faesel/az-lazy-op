using az_lazy.Commands;
using az_lazy.Commands.Queue;
using az_lazy.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace az_lazy.Startup
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAzRunner, AzRunner>();

            //Command Runners
            serviceCollection.AddSingleton<IConnectionRunner<ConnectionOptions>, ConnectionRunner>();
            serviceCollection.AddSingleton<IConnectionRunner<QueueOptions>, QueueRunner>();

            //Managers
            serviceCollection.AddSingleton<ILocalStorageManager, LocalStorageManager>();

            return serviceCollection;
        }
    }
}