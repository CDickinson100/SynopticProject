using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Synoptic_Project;
using Synoptic_Project.Commands;

namespace Unit_Tests.commands
{
    [TestFixture]
    public class HelpCommandTest
    {
        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.CommandManager = new CommandManager();
        }

        [Test]
        public void TestHelpCommand()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("help");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");
            
            Assert.AreEqual(new[]
            {
                "----------Available Commands----------",
                "Help - Displays a list of all available commands",
                "Where - Displays information based on your current location.",
                "Go <Direction> - Move in a specified direction.",
                "Flee - Abandons the current fight and retreats to the previous room.",
                "Info - Displays current information about your player",
                "Take <Item> - Take an item from your current room",
                "Use <Item> - Use an item from your inventory",
                "Drop <Item> - Drop an item in the room"
            },output);
        }
    }
}