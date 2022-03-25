using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Synoptic_Project;
using Synoptic_Project.Commands;
using Action = Synoptic_Project.Action;

namespace Unit_Tests.commands
{
    [TestFixture]
    public class WhereCommandTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.MazeConfiguration = new MazeConfiguration();
            Program.CommandManager = new CommandManager();
            Program.Player = new Player("Test")
            {
                Room = new Room("Test Room", true, new Dictionary<string, int>(), new Passage[] { })
            };
        }

        [Test]
        public void TestWhereCommandWithThreat()
        {
            Program.Player.Room.Threat = new Threat("Threat", 10, 10, new Dictionary<string, Action>());
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("where");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "----------Test Room----------",
                "Items:",
                "    NONE",
                "Threat: Threat",
                "Directions:"
            }, output);
        }

        [Test]
        public void TestWhereCommandNoThreat()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("where");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "----------Test Room----------",
                "Items:",
                "    NONE",
                "Threat: NONE",
                "Directions:"
            }, output);
        }

        [Test]
        public void TestWhereCommandItems()
        {
            Program.Player.Room.Treasures = new Dictionary<string, int>
            {
                {"Item1", 5},
                {"Item6", 1}
            };
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("where");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "----------Test Room----------",
                "Items:",
                "    Item1 - 5",
                "    Item6 - 1",
                "Threat: NONE",
                "Directions:"
            }, output);
        }
    }
}