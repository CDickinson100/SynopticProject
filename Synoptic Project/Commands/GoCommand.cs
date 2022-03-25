using System;
using System.Linq;

namespace Synoptic_Project.Commands
{
    public class GoCommand : Command
    {
        public GoCommand() : base(new[] {"go"}, "Go <Direction>", "Move in a specified direction.")
        {
        }

        public override void Execute(string[] arguments)
        {
            if (arguments.Length != 1)
            {
                Console.WriteLine("Invalid syntax, correct syntax is: " + Syntax);
                return;
            }

            var room = Program.Player.Room;

            if (room.Threat != null)
            {
                Console.WriteLine("You must defeat the Threat in this room to progress any further. Try \"Flee\"");
                return;
            }

            var passages = room.Passages
                .Where(p => p.Direction.ToLower().Equals(arguments[0].ToLower()))
                .ToArray();
            if (passages.Length == 0)
            {
                Console.WriteLine("Invalid Direction.");
                return;
            }

            var passage = passages[0];
            if (passage.Locked)
            {
                Console.WriteLine("It seems that passage is locked.");
                return;
            }

            if (passage.IsExit)
            {
                Program.IsPlaying = false;
                return;
            }

            var newRoom = Program.MazeConfiguration.Rooms
                .Where(r => r.Name.ToLower().Equals(passage.Destination.ToLower()))
                .ToArray()[0];
            Program.Player.PreviousRoom = room;
            Program.Player.Room = newRoom;
            Program.CommandManager.Execute("where");
        }
    }
}