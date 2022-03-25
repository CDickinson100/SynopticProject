using System;
using System.Collections.Generic;
using System.Linq;

namespace Synoptic_Project
{
    public class Player
    {
        public readonly string Name;
        public Room Room;
        public Room PreviousRoom;
        public Dictionary<string, int> Inventory;
        public double Health;

        public Player(string name)
        {
            Name = name;
            var random = new Random();
            var mazeConfiguration = Program.MazeConfiguration;
            var availableSpawningRooms = mazeConfiguration.Rooms.Where(room => room.Spawnable).ToArray();
            Room = availableSpawningRooms[random.Next(availableSpawningRooms.Length)];
            Health = 100;
            Inventory = mazeConfiguration.StartingItems;
        }

        public int GetWorth()
        {
            var worth = 0;
            foreach (var item in Program.MazeConfiguration.Items)
            {
                if (Inventory.ContainsKey(item.Name))
                {
                    worth += item.Worth * Inventory[item.Name];
                }
            }

            return worth;
        }

        public double GetDefence()
        {
            double defenceModifier = 1;
            foreach (var item in Program.MazeConfiguration.Items)
            {
                if (!Inventory.ContainsKey(item.Name)) continue;
                if (defenceModifier < item.DefenceModifier) defenceModifier = item.DefenceModifier;
            }

            return defenceModifier;
        }

        public void AddItem(string item, int amount)
        {
            if (Inventory.Keys.Contains(item))
            {
                Inventory[item] += amount;
            }
            else
            {
                Inventory[item] = amount;
            }
        }

        public void RemoveItem(string item)
        {
            if (!Inventory.Keys.Contains(item)) return;
            if (Inventory[item] > 1)
            {
                Inventory[item] -= 1;
            }
            else
            {
                Inventory.Remove(item);
            }
        }
    }
}