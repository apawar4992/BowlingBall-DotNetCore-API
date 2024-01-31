
namespace Game.Model
{
    public class InvalidGameException : Exception
    {
        public override string Message
        {
            get
            {
                return Constants.INVALIDGAMEEXCEPTIONMESSAGE;
            }
        }
    }
}
