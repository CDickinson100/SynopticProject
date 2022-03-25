using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Synoptic_Project;
using Synoptic_Project.Commands;

namespace Unit_Tests
{
    [TestFixture]
    public class PlayerTest
    {
        private Player _player;

        [SetUp]
        public void SetupBeforeEachTest()
        {
            Program.MazeConfiguration = new MazeConfiguration
            {
                Items = new[]
                {
                    new Item("TestItem", 1, 1),
                    new Item("OtherTestItem", 3, 2),
                    new Item("ValuableTestItem", 5, 3)
                }
            };
            _player = new Player("Test Player");
        }

        [Test]
        public void PlayerTest__GetWorth()
        {
            _player.Inventory = new Dictionary<string, int>
            {
                {"TestItem", 3},
                {"OtherTestItem", 2},
                {"ValuableTestItem", 4}
            };

            Assert.AreEqual(29, _player.GetWorth());
        }

        [Test]
        public void PlayerTest__GetDefence__Default()
        {
            Assert.AreEqual(1, _player.GetDefence());
        }

        [Test]
        public void PlayerTest__GetDefence__HasDefenceItems()
        {
            _player.Inventory = new Dictionary<string, int>
            {
                {"TestItem", 3},
                {"OtherTestItem", 2},
                {"ValuableTestItem", 4}
            };
            
            Assert.AreEqual(3, _player.GetDefence());
        }

        [Test]
        public void PlayerTest__AddItem__NoItemPreviously()
        {
            _player.AddItem("test", 1);
            
            Assert.AreEqual(1, _player.Inventory["test"]);
        }

        [Test]
        public void PlayerTest__AddItem__HadItemPreviously()
        {
            _player.Inventory = new Dictionary<string, int>
            {
                {"test", 5}
            };
            
            _player.AddItem("test", 1);
            
            Assert.AreEqual(6, _player.Inventory["test"]);
        }

        [Test]
        public void PlayerTest__RemoveItem__NotInInventory()
        {
            _player.RemoveItem("test");
            
            Assert.IsFalse(_player.Inventory.ContainsKey("test"));
        }

        [Test]
        public void PlayerTest__RemoveItem__RemovesFromInventory()
        {
            _player.Inventory = new Dictionary<string, int>
            {
                {"test", 1}
            };
            _player.RemoveItem("test");
            
            Assert.IsFalse(_player.Inventory.ContainsKey("test"));
        }

        [Test]
        public void PlayerTest__RemoveItem__ReducesAmountBy1()
        {
            _player.Inventory = new Dictionary<string, int>
            {
                {"test", 2}
            };
            _player.RemoveItem("test");
            
            Assert.AreEqual(1, _player.Inventory["test"]);
        }
    }
}