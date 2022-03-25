using System;
using System.Linq;

namespace Synoptic_Project.Commands
{
    public class DropCommand : Command
    {
        public DropCommand() : base(new[] {"drop"}, "Drop <Item>", "Drop an item in the room")
        {
        }

        public override void Execute(string[] arguments)
        {
            var player = Program.Player;
            var room = player.Room;
            var input = string.Join(" ", arguments);
            if (input.Equals(""))
            {
                Console.WriteLine("Invalid syntax, correct syntax is: " + Syntax);
                return;
            }

            if (room.Threat != null)
            {
                Console.WriteLine("You must defeat the Threat in this room in order to drop any items.");
                return;
            }

            foreach (var item in Program.Player.Inventory.Keys)
            {
                if (!item.ToLower().Equals(input.ToLower())) continue;
                const int amount = 1;
                if (room.Treasures.Keys.Contains(item))
                {
                    room.Treasures[item] += amount;
                }
                else
                {
                    room.Treasures[item] = amount;
                }

                player.RemoveItem(item);
                Console.WriteLine("You left " + amount + " " + item + " in the room.");
                return;
            }

            Console.WriteLine("You do not have that item in your inventory.");
        }
    }
}