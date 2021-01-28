using System;
using System.Threading.Tasks;
using az_lazy.Helpers;
using az_lazy.Manager;
using Spectre.Console;

namespace az_lazy.Commands.Queue.Executor
{
    public class WatchQueueExecutor : ICommandExecutor<QueueOptions>
    {
        private readonly ILocalStorageManager LocalStorageManager;
        private readonly IAzureQueueManager AzureStorageManager;

        public WatchQueueExecutor(
            ILocalStorageManager localStorageManager,
            IAzureQueueManager azureStorageManager)
        {
            this.LocalStorageManager = localStorageManager;
            this.AzureStorageManager = azureStorageManager;
        }

        public async Task Execute(QueueOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Watch))
            {
                await AnsiConsole
                    .Status()
                    .Spinner(Spinner.Known.Star)
                    .SpinnerStyle(Style.Parse("green bold"))
                    .StartAsync($"Starting to watch {opts.Watch} ... ", async _ =>
                    {
                        try
                        {
                            var selectedConnection = LocalStorageManager.GetSelectedConnection();
                            await AzureStorageManager.WatchQueue(selectedConnection.ConnectionString, opts.Watch);

                            AnsiConsole.MarkupLine($"Clearing poison queue {opts.CureQueue}-poison ... [bold green]Successful[/]");
                            AnsiConsole.MarkupLine($"Finished moving poison queue messages");
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"Starting to watch {opts.Watch} ... [bold red]Failed[/]");
                            AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
                        }
                    });

                ConsoleHelper.WriteInfoWaiting($"Starting to watch {opts.Watch}", true);

                try
                {
                    ConsoleHelper.WriteLineSuccessWaiting($"Watching queue {opts.Watch}");

                    var selectedConnection = LocalStorageManager.GetSelectedConnection();
                    await AzureStorageManager.WatchQueue(selectedConnection.ConnectionString, opts.Watch);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteLineFailedWaiting($"Failed to watch queue {opts.Watch}");
                    ConsoleHelper.WriteLineError(ex.Message);
                }
            }
        }
    }
}