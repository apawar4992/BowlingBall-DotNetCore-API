using AutoFixture;
using AutoFixture.AutoMoq;
using Game.Model;
using Game.Repository;
using Microsoft.Extensions.Logging;
using BowlingGame = Game.Model.BowlingGame;

namespace Game.Manager.Tests
{
    public class GameManagerIntegrationTests
    {
        private BowlingGame bowlingGame;
        private Model.Game game;

        [SetUp]
        public void Setup()
        {
            game = new BowlingGame();
            game.GameType = Constants.BOWLINGBALL;
            game.GameId = 1;
            bowlingGame = (BowlingGame)game;
        }

        [Test]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { 10, 9, 1, 5, 5, 7, 2, 10, 10, 10, 9, 0, 8, 2, 9, 1, 10 }, 187)]
        public async Task BowlingBall_ShouldReturnValidScore_ForValidInput(int[] arr, int expectedScore)
        {
            // Arrange
            var managerLogger = TestHelper.GetLoggerInstance<GameManager>();
            var bowlingRepositoryLogger = TestHelper.GetLoggerInstance<BowlingRepository>();

            var gameFactory = new GameFactory(new List<IGameRepository>
            {
                new BowlingRepository(bowlingRepositoryLogger)
            });

            // Act
            var gameManager = new GameManager(managerLogger, gameFactory);

            bowlingGame.BowlingFrames = TestHelper.AddRolls(arr);
            Model.GameResult gameResult = await gameManager.GetResult(game);

            // Assert
            Assert.NotNull(gameResult);
            Assert.That(gameResult.Score, Is.EqualTo(expectedScore));
        }

        [Test]
        [TestCase("")]
        [TestCase("Carrom")]
        public Task BowlingBall_ShouldReturnValidScore_ForInvalidGameType(string gameType)
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var managerLogger = fixture.Create<ILogger<GameManager>>();

            var gameFactory = new GameFactory(new List<IGameRepository>());
            game.GameType = gameType;

            // Act & Assert
            var gameManager = new GameManager(managerLogger, gameFactory);
            Assert.ThrowsAsync<GameNotFoundException>(async () => await gameManager.GetResult(game));
            return Task.CompletedTask;
        }
    }
}
