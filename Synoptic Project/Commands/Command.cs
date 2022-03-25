namespace Synoptic_Project.Commands
{
    public abstract class Command
    {
        public readonly string[] Alias;
        public readonly string Syntax;
        public readonly string Description;

        protected Command(string[] alias, string syntax, string description)
        {
            Alias = alias;
            Syntax = syntax;
            Description = description;
        }
        public abstract void Execute(string[] arguments);
    }
}