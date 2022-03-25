using System.Collections.Generic;

namespace Synoptic_Project
{
    public class Threat
    {
        public string Name;
        public double Health;
        public int Damage;
        public double MaxHealth;
        public Dictionary<string, Action> Actions;

        public Threat(string name, double health, int damage, Dictionary<string, Action> actions)
        {
            Name = name;
            Damage = damage;
            Health = health;
            MaxHealth = health;
            Actions = actions;
        }

        public Threat()
        {
        }
    }
}