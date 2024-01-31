namespace Game.Model
{
    public class GameNotFoundException : Exception
    {
        public override string Message
        {
            get
            {
                return Constants.GAMENOTFOUNDEXCEPTION;
            }
        }
    }
}
