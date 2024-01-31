using Game.Repository;

namespace Game.Manager
{
    public interface IGameFactory
    {
        public IGameRepository GetGameInstance(string gameType);
    }
}
