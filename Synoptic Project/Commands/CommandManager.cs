using System;
using System.Linq;

namespace Synoptic_Project.Commands
{
    public class CommandManager
    {
        public Command[] Commands;

        public CommandManager()
        {
            Commands = new Command[]
            {
                new HelpCommand(),
                new WhereCommand(),
                new GoCommand(),
                new FleeCommand(),
                new InfoCommand(),
                new TakeCommand(),
                new UseCommand(),
                new DropCommand()
            };
        }

        public void Execute(string input)
        {
            var arguments = input.Split(' ');
            foreach (var command in Commands)
            {
                if (!command.Alias.Any(alias => alias.ToLower().Equals(arguments[0].ToLower()))) continue;
                command.Execute(arguments.Skip(1).ToArray());
                return;
            }

            Console.WriteLine("Invalid command, type \"help\" to see all available Commands.");
        }
    }
}