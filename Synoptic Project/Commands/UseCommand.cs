using System;
using System.Linq;

namespace Synoptic_Project.Commands
{
    public class UseCommand : Command
    {
        public static bool CalledUseItem;
        public UseCommand() : base(new[] {"use"}, "Use <Item>", "Use an item from your inventory")
        {
            CalledUseItem = false;
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


            if (!player.Inventory.Keys.Any(s => s.ToLower().Equals(input.ToLower())))
            {
                Console.WriteLine("You do not have that item in your inventory.");
                return;
            }
            
            foreach (var item in Program.MazeConfiguration.Items)
            {
                if (!item.Name.ToLower().Equals(input.ToLower())) continue;
                if (room.Threat != null && room.Threat.Actions.ContainsKey(item.Name))
                {
                    var action = room.Threat.Actions[item.Name];
                    UseItem(item, action);
                    return;
                }
                
                foreach (var passage in room.Passages)
                {
                    if (!passage.Locked || !passage.UnlockedBy.Equals(item.Name)) continue;
                    Console.WriteLine("You unlocked a passage using the " + passage.UnlockedBy);
                    passage.Locked = false;
                    player.RemoveItem(item.Name);
                    return;
                }

                if (item.DefaultAction == null) continue;
                UseItem(item, item.DefaultAction);
                return;
            }
            Console.WriteLine("You cannot use that item here.");
        }

        public static void UseItem(Item item, Action action)
        {
            CalledUseItem = true;
            var player = Program.Player;
            var room = player.Room;
            var attack = action.Damage;
            if (room.Threat == null && (action.ActionType == ActionType.DamageThreat || action.ActionType == ActionType.KillThreat))
            {
                Console.WriteLine("You cannot use that item here.");
                return;
            }

            if (action.Consume)
            {
                player.RemoveItem(item.Name);
            }

            switch (action.ActionType)
            {
                case ActionType.DamageThreat:
                    room.Threat.Health -= attack;
                    break;
                case ActionType.DamageSelf:
                    player.Health -= action.Damage / player.GetDefence();
                    break;
                case ActionType.HealSelf:
                    player.Health = Math.Min(player.Health + action.Damage / player.GetDefence(), 100);
                    break;
                case ActionType.KillThreat:
                    room.Threat.Health = 0;
                    break;
                case ActionType.KillSelf:
                    player.Health = 0;
                    break;
                case ActionType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Console.WriteLine(action.Message);

            if (room.Threat == null || (action.ActionType != ActionType.DamageThreat && action.ActionType != ActionType.KillThreat)) return;
            if (room.Threat.Health <= 0)
            {
                if (action.ActionType == ActionType.DamageThreat)
                {
                    Console.WriteLine("You killed the " + room.Threat.Name+".");
                }

                room.Threat = null;
                return;
            }

            player.Health -= room.Threat.Damage / player.GetDefence();
            Console.WriteLine("The " + room.Threat.Name + " Damaged you for " + room.Threat.Damage / player.GetDefence() + " damage");
            Console.WriteLine("");
            Console.WriteLine("Your Health: " + player.Health + "/100");
            Console.WriteLine(room.Threat.Name + " Health: " + room.Threat.Health + "/" + room.Threat.MaxHealth);
        }
    }
}