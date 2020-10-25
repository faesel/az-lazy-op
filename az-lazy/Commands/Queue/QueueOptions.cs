using CommandLine;

namespace az_lazy.Commands.Queue
{
    [Verb("queue", HelpText = "Manage azure storage connections")]
    public class QueueOptions : ICommandOptions
    {
        [Option('l', "list", Required = false, HelpText = "List all connections available")]
        public bool List { get; set; }

        [Option('r', "remove", Required = false, HelpText = "Queue name to remove")]
        public string RemoveQueue { get; set; }
    }
}