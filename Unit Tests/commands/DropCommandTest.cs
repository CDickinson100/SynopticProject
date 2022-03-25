using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Synoptic_Project;
using Synoptic_Project.Commands;

namespace Unit_Tests.commands
{
    [TestFixture]
    public class DropCommandTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.MazeConfiguration = new MazeConfiguration();
            Program.CommandManager = new CommandManager();
            Program.Player = new Player("Test")
            {
                Inventory =
                {
                    ["Gold"] = 10
                }
            };
        }

        [Test]
        public void TestDropCommand__InvalidInput()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("drop");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Invalid syntax, correct syntax is: Drop <Item>"
            }, output);
        }

        [Test]
        public void TestDropCommand__ThreatPresent()
        {
            Program.Player.Room.Threat = new Threat();
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("drop gold");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "You must defeat the Threat in this room in order to drop any items."
            }, output);
        }

        [Test]
        public void TestDropCommand__NotInInventory()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("drop InvalidItem");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "You do not have that item in your inventory."
            }, output);
        }

        [Test]
        public void TestDropCommand__Success__NoItemInRoom()
        {
            Program.Player.Room.Treasures = new Dictionary<string, int>();
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("drop Gold");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(9, Program.Player.Inventory["Gold"]);
            Assert.AreEqual(1, Program.Player.Room.Treasures["Gold"]);

            Assert.AreEqual(new[]
            {
                "You left 1 Gold in the room."
            }, output);
        }

        [Test]
        public void TestDropCommand__Success__AlreadyHasItemInRoom()
        {
            Program.Player.Room.Treasures = new Dictionary<string, int>
            {
                {"Gold", 1}
            };
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("drop Gold");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(9, Program.Player.Inventory["Gold"]);
            Assert.AreEqual(2, Program.Player.Room.Treasures["Gold"]);

            Assert.AreEqual(new[]
            {
                "You left 1 Gold in the room."
            }, output);
        }
    }
}