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
    public class FleeCommandTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.MazeConfiguration = new MazeConfiguration();
            Program.CommandManager = new CommandManager();
            Program.Player = new Player("Test");
        }

        [Test]
        public void TestFleeCommand__NoThreat()
        {
            Program.Player.Room.Threat = null;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("flee");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "There is no current threat in this room."
            }, output);
        }

        [Test]
        public void TestFleeCommand__NoPreviousRoom()
        {
            Program.Player.Room.Threat = new Threat();
            Program.Player.PreviousRoom = null;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("flee");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "You do not have any previous room to go to."
            }, output);
        }

        [Test]
        public void TestFleeCommand__Success()
        {
            var room = new Room("Test Room", true, new Dictionary<string, int>(), new Passage[] { });
            Program.Player.Room.Threat = new Threat();
            Program.Player.PreviousRoom = room;
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("flee");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(room, Program.Player.Room);
            Assert.IsNull(Program.Player.PreviousRoom);

            Assert.AreEqual(new[]
            {
                "----------Test Room----------",
                "Items:",
                "    NONE",
                "Threat: NONE",
                "Directions:"
            }, output);
        }
    }
}