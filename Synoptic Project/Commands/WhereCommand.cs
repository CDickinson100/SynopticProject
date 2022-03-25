using System;

namespace Synoptic_Project.Commands
{
    public class WhereCommand : Command
    {
        public WhereCommand() : base(
            new[] {"where", "whereami"}, "Where", "Displays information based on your current location.")
        {
        }

        public override void Execute(string[] arguments)
        {
            var room = Program.Player.Room;
            Console.WriteLine("----------" + room.Name + "----------");
            Console.WriteLine("Items:");
            if (room.Treasures.Keys.Count == 0)
            {
                Console.WriteLine("    NONE");
            }
            else
            {
                foreach (var item in room.Treasures.Keys)
                {
                    Console.WriteLine("    " + item + " - " + room.Treasures[item]);
                }
            }

            Console.WriteLine("Threat: " + (room.Threat == null ? "NONE" : room.Threat.Name));
            Console.WriteLine("Directions:");
            foreach (var passage in room.Passages)
            {
                Console.WriteLine("    " + passage.Direction);
            }
        }
    }
}