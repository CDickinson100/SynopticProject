namespace Synoptic_Project
{
    public class Item
    {
        public string Name;
        public int Worth;
        public double DefenceModifier;
        public bool DisplayInInventory;
        public Action DefaultAction;

        public Item(string name, int worth, double defenceModifier, Action defaultAction)
        {
            Name = name;
            Worth = worth;
            DefenceModifier = defenceModifier;
            DisplayInInventory = true;
            DefaultAction = defaultAction;
        }

        public Item(string name, int worth, double defenceModifier, bool displayInInventory)
        {
            Name = name;
            Worth = worth;
            DefenceModifier = defenceModifier;
            DisplayInInventory = displayInInventory;
        }

        public Item(string name, int worth, double defenceModifier)
        {
            Name = name;
            Worth = worth;
            DefenceModifier = defenceModifier;
            DisplayInInventory = true;
        }

        public Item()
        {
        }
    }
}