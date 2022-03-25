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
    public class UseCommandTest
    {
        private Passage _passage;

        [SetUp]
        public void SetupBeforeEachTest()
        {
            _passage = new Passage("", "", "Gold", false, false);
            Program.MazeConfiguration = new MazeConfiguration();
            Program.CommandManager = new CommandManager();
            Program.Player = new Player("Test")
            {
                Inventory = new Dictionary<string, int>
                {
                    {"Gold", 5}
                },
                Room = new Room("Test Room", true, new Dictionary<string, int>(), new[]
                {
                    _passage
                })
            };
        }

        [Test]
        public void TestUseCommand__Execute__InvalidSyntax()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("use");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Invalid syntax, correct syntax is: Use <Item>"
            }, output);
        }

        [Test]
        public void TestTakeCommand__Execute__NotInInventory()
        {
            Program.Player.Inventory = new Dictionary<string, int>();
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("use gold");

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
        public void TestTakeCommand__Execute__ThreatPresent__Execute__CallsMethod()
        {
            Program.Player.Room.Threat = new Threat("Test", 10, 10, new Dictionary<string, Action>
            {
                {"Gold", new Action(ActionType.DamageThreat, 5, "Triggered", false)}
            });

            Program.CommandManager.Execute("use gold");

            Assert.IsTrue(UseCommand.CalledUseItem);
        }

        [Test]
        public void TestTakeCommand__Execute__UnlockPassage()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            _passage.Locked = true;

            Program.CommandManager.Execute("use gold");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.IsFalse(_passage.Locked);

            Assert.AreEqual(4, Program.Player.Inventory["Gold"]);

            Assert.AreEqual(new[]
            {
                "You unlocked a passage using the Gold"
            }, output);
        }

        [Test]
        public void TestTakeCommand__Execute__NoThreatPresentWithNoDefaultAction()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("use gold");

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "You cannot use that item here."
            }, output);

            Assert.IsFalse(UseCommand.CalledUseItem);
        }

        [Test]
        public void TestTakeCommand__Execute__NoThreatPresentWithDefaultAction()
        {
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Health Potion", 1}
            };
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Program.CommandManager.Execute("use Health Potion");

            Assert.IsTrue(UseCommand.CalledUseItem);
        }

        [Test]
        public void TestTakeCommand__UseItem__NoThreat()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            UseCommand.UseItem(new Item(), new Action(ActionType.DamageThreat, 10, "", false));

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "You cannot use that item here."
            }, output);
        }

        [Test]
        public void TestTakeCommand__UseItem__ConsumeItem()
        {
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.None, 10, "", true));

            Assert.IsFalse(Program.Player.Inventory.ContainsKey("Item"));
        }

        [Test]
        public void TestTakeCommand__UseItem__Action__DamageThreat__KillThreat()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };
            var threat = new Threat("Threat", 100, 0, new Dictionary<string, Action>());
            Program.Player.Room = new Room("Test", false, new Dictionary<string, int>(), new Passage[] { }, threat);

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.DamageThreat, 150, "Message", false));

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Message",
                "You killed the Threat."
            }, output);

            Assert.IsNull(Program.Player.Room.Threat);
        }

        [Test]
        public void TestTakeCommand__UseItem__Action__DamageThreat()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };
            var threat = new Threat("Threat", 100, 10, new Dictionary<string, Action>());
            Program.Player.Room = new Room("Test", false, new Dictionary<string, int>(), new Passage[] { }, threat);

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.DamageThreat, 10, "Message", false));

            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Message",
                "The Threat Damaged you for 10 damage",
                "Your Health: 90/100",
                "Threat Health: 90/100"
            }, output);
            Assert.AreEqual(90, threat.Health);
            Assert.AreEqual(90, Program.Player.Health);
        }

        [Test]
        public void TestTakeCommand__UseItem__Action__DamageSelf()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.DamageSelf, 10, "Message", false));
            
            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Message"
            }, output);

            Assert.AreEqual(90, Program.Player.Health);
        }

        [Test]
        public void TestTakeCommand__UseItem__Action__HealSelf()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };
            Program.Player.Health = 50;

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.HealSelf, 10, "Message", false));
            
            var output = stringWriter.ToString()
                .Replace("\r", "")
                .Split('\n')
                .Where(s => s != "");

            Assert.AreEqual(new[]
            {
                "Message"
            }, output);

            Assert.AreEqual(60, Program.Player.Health);
        }

        [Test]
        public void TestTakeCommand__UseItem__Action__HealSelf__Max100Health()
        {
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };
            Program.Player.Health = 95;

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.HealSelf, 10, "Message", false));

            Assert.AreEqual(100, Program.Player.Health);
        }

        [Test]
        public void TestTakeCommand__UseItem__Action__KillThreat()
        {
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };
            var threat = new Threat("Threat", 100, 0, new Dictionary<string, Action>());
            Program.Player.Room = new Room("Test", false, new Dictionary<string, int>(), new Passage[] { }, threat);

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.KillThreat, 10, "Message", false));

            Assert.IsNull(Program.Player.Room.Threat);
            Assert.AreEqual(0, threat.Health);
        }

        [Test]
        public void TestTakeCommand__UseItem__Action__KillSelf()
        {
            Program.Player.Inventory = new Dictionary<string, int>
            {
                {"Item", 1}
            };

            UseCommand.UseItem(new Item("Item", 0, 0), new Action(ActionType.KillSelf, 10, "Message", false));

            Assert.AreEqual(0, Program.Player.Health);
        }
    }
}