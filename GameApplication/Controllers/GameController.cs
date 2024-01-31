using Game.Manager;
using Game.Model;
using GameApplication.Model;
using Microsoft.AspNetCore.Mvc;

namespace GameApplication.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        public readonly ILogger<GameController> _logger;
        public readonly IGameManager _gameManager;
        public GameController(ILogger<GameController> gameLogger, IGameManager gameManager)
        {
            _logger = gameLogger;
            _gameManager = gameManager;
        }

        [HttpPost]
        [Route("GetResult")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CalculateResult([FromBody] ApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                _logger.LogError("Invalid input parameter.");
                throw new ArgumentNullException();
            }

            Game.Model.Game game = null;
            if (apiRequest.GameType == Constants.BOWLINGBALL)
            {
                game = new BowlingGame()
                {
                    BowlingFrames = apiRequest.Frames
                };
                game.GameType = Constants.BOWLINGBALL;
            }

            // Manager call
            var gameResult = await _gameManager.GetResult(game);
            if (gameResult == null)
            {
                _logger.LogInformation($"Something went wrong while calculating score for gametype:{game?.GameType}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                var response = new ApiResponse<string>
                {
                    StatusCode = 200,
                    Data = $"Calculated score:{gameResult?.Score}"
                };

                _logger.LogInformation($"Calculated score:{gameResult?.Score} for game type:{game?.GameType}");
                return Ok(response);
            }
        }
    }
}
