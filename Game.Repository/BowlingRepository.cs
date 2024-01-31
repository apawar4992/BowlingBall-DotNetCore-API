using Game.Model;
using Microsoft.Extensions.Logging;

namespace Game.Repository
{
    public class BowlingRepository : IGameRepository
    {
        private readonly ILogger<BowlingRepository> _logger;
        string IGameRepository.GameName => Constants.BOWLINGBALL;

        public BowlingRepository(ILogger<BowlingRepository> logger)
        {
            _logger = logger;
        }

        #region Public Methods  

        public async Task<GameResult> CalculateScore(Game game)
        {
            if (!(game is BowlingGame))
            {
                _logger.LogError("Invalid game.");
                throw new InvalidGameException();
            }

            GameResult gameResult = new GameResult();
            BowlingGame bowlingGame = (BowlingGame)game;
            if (bowlingGame.BowlingFrames != null && bowlingGame.BowlingFrames.Count > 0)
            {
                gameResult.Score = await Calculate(bowlingGame);
            }

            _logger.LogInformation("Final Score:{0}", gameResult.Score);
            return gameResult;
        }

        #endregion

        #region Private Methods

        private async Task<int> Calculate(BowlingGame bowlingGame)
        {
            int score = 0;
            for (var index = 0; index < 10; index++)
            {
                Frame frame = bowlingGame.BowlingFrames[index];
                if (frame.IsStrike)
                {
                    // Add strike bonus.
                    score += 10 + await GetStrikeBonus(bowlingGame.BowlingFrames, index);
                }
                else if (frame.IsSpare)
                {
                    // Add spare bonus.
                    score += 10 + bowlingGame.BowlingFrames[index + 1].FirstBowl;
                }
                else
                    score += frame.Score;
            }

            return score;
        }

        private async Task<int> GetStrikeBonus(IList<Frame> rolls, int roll)
        {
            int bonus;
            // The bonus for strike is the value of the next two balls rolled.
            // Get first bowl
            bonus = rolls[roll + 1].FirstBowl;

            // Check if the next bowl is strike.
            if (bonus == 10)
            {
                // If next roll is strike.
                bonus += rolls[roll + 2].FirstBowl;
            }
            else
            {
                // If next frame is spare.
                bonus += rolls[roll + 1].SecondBowl;
            }

            return bonus;
        }

        #endregion
    }
}
