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
    public class GoCommandTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.MazeConfiguration = new MazeConfiguration();
            Program.CommandManager = new CommandManager();
            Program.Player = new Player("Test")
            {
                Room = new Room("Test Room", true, new Dictionary<string, int>(), new[]
                {
                    new Passage("north", "Boss Room", "", false, false)
                })
            };
        }

        [Test]
        public void TestGoCommand__InvalidSyntax()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("go");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Invalid syntax, correct syntax is: Go <Direction>"
            }, output);
        }

        [Test]
        public void TestGoCommand__ThreatPresent()
        {
            Program.Player.Room.Threat = new Threat();
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("go north");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "You must defeat the Threat in this room to progress any further. Try \"Flee\""
            }, output);
        }

        [Test]
        public void TestGoCommand__InvalidDirection()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("go south");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Invalid Direction."
            }, output);
        }

        [Test]
        public void TestGoCommand__PassageLocked()
        {
            Program.Player.Room.Passages[0].Locked = true;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("go north");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "It seems that passage is locked."
            }, output);
        }

        [Test]
        public void TestGoCommand__PassageExit()
        {
            Program.Player.Room.Passages[0].IsExit = true;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("go north");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new string[] { }, output);
            Assert.IsFalse(Program.IsPlaying);
        }

        [Test]
        public void TestGoCommand__Success()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("go north");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual("Test Room", Program.Player.PreviousRoom.Name);
            Assert.AreEqual("Boss Room", Program.Player.Room.Name);
            Assert.AreEqual(new[]
            {
                "----------Boss Room----------",
                "Items:",
                "    Exit Key - 1",
                "    Enchanted Treasure - 1",
                "Threat: Orc King",
                "Directions:",
                "    North",
                "    East",
                "    South",
                "    West"
            }, output);
        }
    }
}