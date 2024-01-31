using Game.Model;
using Game.Repository;
using Moq;
using BowlingGame = Game.Model.BowlingGame;
using GameResult = Game.Repository.GameResult;

namespace Game.Manager.Tests
{
    public class GameManagerTests
    {
        private Mock<IGameRepository> _gameRepo;
        private Mock<IGameFactory> _gameFactory;
        private Model.Game game = null;
        private BowlingGame bowlingGame;

        [SetUp]
        public void Setup()
        {
            _gameRepo = new Mock<IGameRepository>();
            _gameFactory = new Mock<IGameFactory>();

            game = new BowlingGame();
            game.GameType = Constants.BOWLINGBALL;
            game.GameId = 1;
            bowlingGame = (BowlingGame)game;
        }

        [Test]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 10, 9, 1, 5, 5, 7, 2, 10, 10, 10, 9, 0, 8, 2, 9, 1, 10 }, 187)]
        [TestCase(new int[] { 1, 3, 7, 3, 10, 1, 7, 5, 2, 5, 3, 8, 2, 8, 2, 10, 9, 0 }, 131)]
        [TestCase(new int[] { 1, 3, 7, 3, 10, 1, 7, 5, 2, 5, 3, 8, 2, 8, 2, 10, 9, 1, 10 }, 143)]
        public async Task BowlingBall_GameManager_GetResultTests(int[] arr, int result)
        {
            // Arrange
            var managerLogger = TestHelper.GetLoggerInstance<GameManager>();

            _gameFactory.Setup(x => x.GetGameInstance(It.IsAny<string>())).Returns(_gameRepo.Object);
            _gameRepo.Setup(x => x.CalculateScore(It.IsAny<Repository.Game>())).ReturnsAsync(new GameResult()
            {
                Score = result
            });

            // Act
            var gameManager = new GameManager(managerLogger, _gameFactory.Object);

            bowlingGame.BowlingFrames = TestHelper.AddRolls(arr);
            var gameResult = await gameManager.GetResult(game);

            // Assert
            Assert.NotNull(gameResult);
            Assert.That(gameResult.Score, Is.EqualTo(result));
        }

        [Test]
        public Task BowlingBall_GameManager_GetGameInstanceThrowsException()
        {
            // Arrange
            _gameFactory.Setup(x => x.GetGameInstance(It.IsAny<string>()))
                        .Throws(new GameNotFoundException());
            var managerLogger = TestHelper.GetLoggerInstance<GameManager>();
            var gameManager = new GameManager(managerLogger, _gameFactory.Object);

            // Act & Assert
            Assert.ThrowsAsync<GameNotFoundException>(async () => await gameManager.GetResult(game));
            return Task.CompletedTask;
        }
    }
}