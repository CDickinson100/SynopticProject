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
    public class TakeCommandTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.MazeConfiguration = new MazeConfiguration();
            Program.CommandManager = new CommandManager();
            Program.Player = new Player("Test")
            {
                Room = new Room("Test Room", true, new Dictionary<string, int>
                {
                    {"Gold", 5}
                }, new Passage[] { })
            };
        }

        [Test]
        public void TestTakeCommand__InvalidSyntax()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("take");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Invalid syntax, correct syntax is: Take <Item>"
            }, output);
        }

        [Test]
        public void TestTakeCommand__ThreatPresent()
        {
            Program.Player.Room.Threat = new Threat();
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("take gold");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "You must defeat the Threat in this room in order to take any items."
            }, output);
        }

        [Test]
        public void TestTakeCommand__UnknownItem()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("take Invalid_Item");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Unknown Item: Invalid_Item"
            }, output);
        }

        [Test]
        public void TestTakeCommand__Success()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("take gold");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.IsFalse(Program.Player.Room.Treasures.ContainsKey("Gold"));
            Assert.AreEqual(5, Program.Player.Inventory["Gold"]);
            Assert.AreEqual(new[]
            {
                "You took 5 Gold(s) from the room"
            }, output);
        }
    }
}