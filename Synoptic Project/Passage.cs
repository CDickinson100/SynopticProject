namespace Synoptic_Project
{
    public class Passage
    {
        public string Direction;
        public string Destination;
        public string UnlockedBy;
        public bool Locked;
        public bool IsExit;

        public Passage(string direction, string destination, string unlockedBy, bool locked, bool isExit)
        {
            Direction = direction;
            Destination = destination;
            UnlockedBy = unlockedBy;
            Locked = locked;
            IsExit = isExit;
        }
        
        public Passage()
        {
            
        }
    }
}