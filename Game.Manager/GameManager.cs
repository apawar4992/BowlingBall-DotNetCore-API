using Microsoft.Extensions.Logging;

namespace Game.Manager
{
    public class GameManager : IGameManager
    {
        public readonly ILogger<GameManager> _logger;
        private readonly IGameFactory _gameFactory;

        public GameManager(ILogger<GameManager> logger, IGameFactory gameFactory)
        {
            _logger = logger;
            _gameFactory = gameFactory;
        }

        public async Task<Model.GameResult> GetResult(Model.Game game)
        {
            // Call factory by game type.
            var gameRepository = _gameFactory.GetGameInstance(game.GameType);
            _logger.LogInformation("Game is of type:{0}", game.GameType);

            // Convert model to repo model.
            var repoGame = game.ToGame();
            var responseScore = await gameRepository.CalculateScore(repoGame);

            _logger.LogInformation("Calculated score for type:{0}", game.GameType);

            return new Model.GameResult()
            {
                Score = responseScore.Score,
            };
        }
    }
}
