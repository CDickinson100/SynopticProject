using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Synoptic_Project.Commands;

namespace Unit_Tests.commands
{
    [TestFixture]
    public class CommandManagerTest
    {
        private TestCommand _testCommand;
        private CommandManager _commandManager;
        
        [SetUp]
        public void SetupBeforeEachTest()
        {
            _testCommand = new TestCommand();
            _commandManager = new CommandManager
            {
                Commands = new Command[]
                {
                    _testCommand
                }
            };
        }

        [Test]
        public void TestCommandManager__ExecutesTestCommand()
        {
            Assert.IsFalse(_testCommand.HasBeenCalled);
            
            _commandManager.Execute("test");
            
            Assert.IsTrue(_testCommand.HasBeenCalled);
        }

        [Test]
        public void TestCommandManager__UnknownCommand()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            
            _commandManager.Execute("invalid command");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Invalid command, type \"help\" to see all available Commands."
            }, output);
        }
    }
}