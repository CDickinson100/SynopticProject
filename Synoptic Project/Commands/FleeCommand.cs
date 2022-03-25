using System;

namespace Synoptic_Project.Commands
{
    public class FleeCommand : Command
    {
        public FleeCommand() : base(new []{"Flee", "Retreat"}, "Flee", "Abandons the current fight and retreats to the previous room.")
        {
        }

        public override void Execute(string[] arguments)
        {
            var room = Program.Player.Room;
            if (room.Threat==null)
            {
                Console.WriteLine("There is no current threat in this room.");
                return;
            }
            if (Program.Player.PreviousRoom==null)
            {
                Console.WriteLine("You do not have any previous room to go to.");
                return;
            }

            Program.Player.Room = Program.Player.PreviousRoom;
            Program.Player.PreviousRoom = null;
            Program.CommandManager.Execute("where");
        }
    }
}