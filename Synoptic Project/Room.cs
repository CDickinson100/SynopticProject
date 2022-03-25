using System.Collections.Generic;

namespace Synoptic_Project
{
    public class Room
    {
        public string Name;
        public bool Spawnable;
        public Dictionary<string, int> Treasures;
        public Passage[] Passages;
        public Threat Threat;

        public Room(string name, bool spawnable, Dictionary<string, int> treasures, Passage[] passages, Threat threat)
        {
            Name = name;
            Spawnable = spawnable;
            Treasures = treasures;
            Passages = passages;
            Threat = threat;
        }

        public Room(string name, bool spawnable, Dictionary<string, int> treasures, Passage[] passages)
        {
            Name = name;
            Spawnable = spawnable;
            Treasures = treasures;
            Passages = passages;
        }

        public Room()
        {
            
        }
    }
}