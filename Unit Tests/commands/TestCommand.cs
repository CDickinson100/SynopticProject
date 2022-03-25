using Synoptic_Project.Commands;

namespace Unit_Tests.commands
{
    public class TestCommand : Command
    {
        public bool HasBeenCalled = false;

        public TestCommand() : base(new[] {"test"}, "test", "test")
        {
        }

        public override void Execute(string[] arguments)
        {
            HasBeenCalled = true;
        }
    }
}