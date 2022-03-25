using System;

namespace Synoptic_Project.Commands
{
    public class HelpCommand : Command
    {
        public HelpCommand() : base(new[] {"Help"}, "Help", "Displays a list of all available commands")
        {
        }

        public override void Execute(string[] arguments)
        {
            Console.WriteLine("----------Available Commands----------");
            foreach (var command in Program.CommandManager.Commands)
            {
                Console.WriteLine(command.Syntax + " - " + command.Description);
            }
        }
    }
}