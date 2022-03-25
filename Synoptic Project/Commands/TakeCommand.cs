using System;
using System.Linq;

namespace Synoptic_Project.Commands
{
    public class TakeCommand : Command
    {
        public TakeCommand() : base(new[] {"take"}, "Take <Item>", "Take an item from your current room")
        {
        }

        public override void Execute(string[] arguments)
        {
            var room = Program.Player.Room;
            var input = string.Join(" ", arguments);
            if (input.Equals(""))
            {
                Console.WriteLine("Invalid syntax, correct syntax is: " + Syntax);
                return;
            }

            if (room.Threat != null)
            {
                Console.WriteLine("You must defeat the Threat in this room in order to take any items.");
                return;
            }

            foreach (var item in room.Treasures.Keys)
            {
                if (!item.ToLower().Equals(input.ToLower())) continue;
                var amount = room.Treasures[item];
                room.Treasures.Remove(item);
                Program.Player.AddItem(item, amount);
                Console.WriteLine("You took " + amount + " " + item + "(s) from the room");
                return;
            }

            Console.WriteLine("Unknown Item: " + input);
        }
    }
}