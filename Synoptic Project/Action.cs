namespace Synoptic_Project
{
    public class Action
    {
        public ActionType ActionType;
        public int Damage;
        public string Message;
        public bool Consume;

        public Action(ActionType actionType, int damage, string message, bool consume)
        {
            ActionType = actionType;
            Damage = damage;
            Message = message;
            Consume = consume;
        }
        
        public Action()
        {
            
        }
    }
}