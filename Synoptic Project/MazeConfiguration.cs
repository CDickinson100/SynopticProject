using System.Collections.Generic;

namespace Synoptic_Project
{
    public class MazeConfiguration
    {
        public Dictionary<string, int> StartingItems = new Dictionary<string, int>
        {
            {"Punch", 1}
        };

        public Item[] Items =
        {
            new Item("Punch", 0, 1, false),
            new Item("Silver", 1, 0),
            new Item("Gold", 5, 0),
            new Item("Enchanted Treasure", 50, 0),
            new Item("Exit Key", 0, 0),
            new Item("Sword", 10, 2),
            new Item("Armor", 10, 0),
            new Item("Water Bucket", 0, 0),
            new Item("Health Potion", 5, 0, 
                new Action(ActionType.HealSelf, 25, "You used a health potion and healed yourself.", true))
        };

        public Room[] Rooms =
        {
            new Room("Treasure Room", false, new Dictionary<string, int>
            {
                {"Sword", 1},
                {"Gold", 15}
            }, new[]
            {
                new Passage("South", "North Hallway", "", false, false)
            }),
            new Room("North Hallway", false, new Dictionary<string, int>
            {
                {"Silver", 5},
                {"Gold", 1}
            }, new[]
            {
                new Passage("North", "Treasure Room", "", false, false),
                new Passage("East", "East Hallway", "", false, false),
                new Passage("West", "West Hallway", "", false, false),
                new Passage("South", "Boss Room", "", false, false)
            }, new Threat("Fire", 100, 25, new Dictionary<string, Action>
            {
                {
                    "Punch",
                    new Action(ActionType.DamageSelf, 10, "You punched the fire... that hurt.", false)
                },
                {
                    "Sword",
                    new Action(ActionType.None, 0, "You attacked the fire... why did you think that would work?", false)
                },
                {
                    "Water Bucket",
                    new Action(ActionType.KillThreat, 0, "You threw water on the fire and extinguished it.", true)
                }
            })),
            new Room("West Hallway", true, new Dictionary<string, int>
            {
                {"Silver", 7},
                {"Gold", 2}
            }, new[]
            {
                new Passage("North", "North Hallway", "", false, false),
                new Passage("East", "Boss Room", "", false, false),
                new Passage("South", "South Hallway", "", false, false)
            }),
            new Room("Boss Room", false, new Dictionary<string, int>
            {
                {"Exit Key", 1},
                {"Enchanted Treasure", 1}
            }, new[]
            {
                new Passage("North", "North Hallway", "", false, false),
                new Passage("East", "East Hallway", "", false, false),
                new Passage("South", "South Hallway", "", false, false),
                new Passage("West", "West Hallway", "", false, false)
            }, new Threat("Orc King", 100, 50, new Dictionary<string, Action>
            {
                {
                    "Punch",
                    new Action(ActionType.DamageThreat, 7, "You punched the orc king, it did a bit of damage", false)
                },
                {
                    "Sword",
                    new Action(ActionType.DamageThreat, 40, "You attacked the orc king, it did a lot of damage", false)
                },
                {
                    "Water Bucket",
                    new Action(ActionType.None, 0, "You threw water at the orc king, it looks at you in confusion.", false)
                }
            })),
            new Room("East Hallway", true, new Dictionary<string, int>
            {
                {"Silver", 3},
                {"Health Potion", 1}
            }, new[]
            {
                new Passage("North", "North Hallway", "", false, false),
                new Passage("West", "Boss Room", "", false, false),
                new Passage("South", "Armory", "", false, false)
            }),
            new Room("Armory", false, new Dictionary<string, int>
            {
                {"Armor", 1},
                {"Health Potion", 1}
            }, new[]
            {
                new Passage("North", "East Hallway", "", false, false),
                new Passage("West", "South Hallway", "", false, false)
            }, new Threat("Orc Soldier", 100, 15, new Dictionary<string, Action>
            {
                {
                    "Punch",
                    new Action(ActionType.DamageThreat, 10, "You punched the orc soldier, it did a bit of damage", false)
                },
                {
                    "Sword",
                    new Action(ActionType.DamageThreat, 50, "You attacked the orc soldier, it did a lot of damage", false)
                },
                {
                    "Water Bucket",
                    new Action(ActionType.None, 0, "You threw water at the orc soldier, it looks at you in confusion.", false)
                }
            })),
            new Room("South Hallway", true, new Dictionary<string, int>
            {
                {"Gold", 3},
                {"Water Bucket", 1}
            }, new[]
            {
                new Passage("North", "Boss Room", "", false, false),
                new Passage("East", "Armory", "", false, false),
                new Passage("West", "West Hallway", "", false, false),
                new Passage("South", "", "Exit Key", true, true)
            })
        };
    }
}