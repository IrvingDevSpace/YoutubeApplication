using System.Windows.Input;

namespace YoutubeApplication.Common
{
    internal static class ICommandExtensions
    {
        public static async Task ExecuteAsync(this ICommand? command, object? parameter)
        {
            if (command == null) return;

            if (command is IAsyncCommand asyncCommand)
                await asyncCommand.ExecuteAsync(parameter);
            else
                command.Execute(parameter);
        }
    }
}
