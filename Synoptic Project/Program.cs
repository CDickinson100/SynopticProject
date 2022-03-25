using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Synoptic_Project.Commands;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Synoptic_Project
{
    public static class Program
    {
        public static MazeConfiguration MazeConfiguration;
        public static CommandManager CommandManager;
        public static Player Player;

        public static bool IsPlaying = true;

        public static void Main()
        {
            while (true)
            {
                MainLoop();
                Console.WriteLine("");
                Console.WriteLine("Do you want to play again? (y/n)");
                var input = Console.ReadLine();
                while (input != "y" && input != "n" && input != "yes" && input != "no")
                {
                    Console.WriteLine("Input must be \"y\" or \"n\"");
                    Console.WriteLine("Do you want to play again? (y/n)");
                    input = Console.ReadLine();
                }

                if (input == "n" || input == "no")
                {
                    return;
                }

                Console.Clear();
            }
        }

        private static void MainLoop()
        {
            IsPlaying = true;
            SaveDefaultConfig();
            MazeConfiguration = GetMazeConfiguration();
            CommandManager = new CommandManager();
            Player = new Player(GetName());
            CommandManager.Execute("where");

            while (IsPlaying)
            {
                if (Player.Health <= 0)
                {
                    Console.WriteLine("You died!");
                    CommandManager.Execute("info");
                    return;
                }

                CommandManager.Execute(Console.ReadLine());
            }

            Console.WriteLine("Congratulations " + Player.Name + "!");
            Console.WriteLine("You exited the Maze with");
            Console.WriteLine("Health: " + Player.Health + "/100");
            Console.WriteLine("Inventory: ");
            var items = MazeConfiguration.Items
                .Where(item => item.DisplayInInventory)
                .Select(item => item.Name)
                .Where(item => Player.Inventory.ContainsKey(item))
                .ToArray();
            if (items.Length == 0)
            {
                Console.WriteLine("    NONE");
            }
            else
            {
                foreach (var item in items)
                {
                    Console.WriteLine("    " + item + " - " + Player.Inventory[item]);
                }
            }

            Console.WriteLine("Worth: " + Player.GetWorth());
        }

        private static string GetName()
        {
            while (true)
            {
                Console.WriteLine("What is your name? (3-15 chars)");
                var input = Console.ReadLine();
                if (input != null && input.Length >= 3 && input.Length <= 15)
                {
                    return input;
                }

                Console.WriteLine("Your name must be between 3 and 5 characters.");
            }
        }

        private static bool ValidateConfiguration(MazeConfiguration mazeConfiguration)
        {
            if (mazeConfiguration.Rooms.SelectMany(room => room.Passages).Count(passage => passage.IsExit) != 1)
            {
                Console.WriteLine("This configuration is invalid: There must only be one exit passage");
                return false;
            }

            foreach (var room in mazeConfiguration.Rooms)
            {
                foreach (var item in room.Treasures.Keys)
                {
                    if (mazeConfiguration.Items.Select(i => i.Name).Contains(item)) continue;
                    Console.WriteLine("This configuration is invalid: Unknown item " + item);
                    return false;
                }
            }

            return true;
        }

        private static void SaveDefaultConfig()
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            File.WriteAllText("default.yaml", serializer.Serialize(new MazeConfiguration()));
        }

        private static MazeConfiguration GetMazeConfiguration()
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            while (true)
            {
                Console.WriteLine("Enter Configuration Path: (default.yaml)");
                var path = Console.ReadLine();
                if (path == null || path.Equals("")) path = "default.yaml";
                if (!File.Exists(path))
                {
                    Console.WriteLine("Invalid file location");
                    continue;
                }

                try
                {
                    var config = deserializer.Deserialize<MazeConfiguration>(File.ReadAllText(path));
                    if (!ValidateConfiguration(config)) continue;
                    Console.WriteLine("Successfully loaded configuration file");
                    return config;
                }
                catch (Exception)
                {
                    Console.WriteLine("This is not a valid configuration file.");
                    Console.WriteLine("Format must contain valid yaml syntax.");
                }
            }
        }
    }
}