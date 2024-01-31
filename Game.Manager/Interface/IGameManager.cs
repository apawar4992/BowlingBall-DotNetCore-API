using Game.Model;

namespace Game.Manager
{
    public interface IGameManager
    {
        public Task<GameResult> GetResult(Model.Game game);
    }
}
