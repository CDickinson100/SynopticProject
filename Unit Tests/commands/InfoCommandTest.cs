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
    public class InfoCommandTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.MazeConfiguration = new MazeConfiguration();
            Program.CommandManager = new CommandManager();
            Program.Player = new Player("Test")
            {
                Room = new Room("Test Room", true, new Dictionary<string, int>(), new Passage[]{})
            };
        }

        [Test]
        public void TestInfoCommand__WithoutInventory()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("info");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");
            
            Assert.AreEqual(new[]
            {
                "----------Test----------",
                "Health: 100/100",
                "Inventory:",
                "    NONE",
                "Worth: 0",
                "Location: Test Room",
                "Defence: 1"
            },output);
        }

        [Test]
        public void TestInfoCommand__WithInventory()
        {
            Program.Player.Inventory["Gold"] = 5;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("info");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");
            
            Assert.AreEqual(new[]
            {
                "----------Test----------",
                "Health: 100/100",
                "Inventory:",
                "    Gold - 5",
                "Worth: 25",
                "Location: Test Room",
                "Defence: 1"
            },output);
        }
    }
}