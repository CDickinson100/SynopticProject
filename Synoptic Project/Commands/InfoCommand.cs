using System;
using System.Linq;

namespace Synoptic_Project.Commands
{
    public class InfoCommand : Command
    {
        public InfoCommand() : base(new[] {"info", "inventory", "health"}, "Info", "Displays current information about your player")
        {
        }

        public override void Execute(string[] arguments)
        {
            var player = Program.Player;
            Console.WriteLine("----------" + player.Name + "----------");
            Console.WriteLine("Health: " + player.Health + "/100");
            Console.WriteLine("Inventory:");
            var items = Program.MazeConfiguration.Items
                .Where(item => item.DisplayInInventory)
                .Select(item => item.Name)
                .Where(item => player.Inventory.ContainsKey(item))
                .ToArray();
            if (items.Length == 0)
            {
                Console.WriteLine("    NONE");
            }
            else
            {
                foreach (var item in items)
                {
                    Console.WriteLine("    " + item + " - " + player.Inventory[item]);
                }
            }

            Console.WriteLine("Worth: " + player.GetWorth());
            Console.WriteLine("Location: " + player.Room.Name);
            Console.WriteLine("Defence: " + player.GetDefence());
        }
    }
}