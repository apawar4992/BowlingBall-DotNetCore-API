using Game.Model;
using Game.Repository;

namespace Game.Manager
{
    public class GameFactory : IGameFactory
    {
        private readonly IEnumerable<IGameRepository> _repos;
        public GameFactory(IEnumerable<IGameRepository> repos)
        {
            _repos = repos;
        }
        public IGameRepository GetGameInstance(string gameType)
        {
            var serviceResolver = _repos.FirstOrDefault(item=> item.GameName.Equals(gameType));
            if (serviceResolver == null)
            {
                throw new GameNotFoundException();
            }

            return serviceResolver;
        }
    }
}
