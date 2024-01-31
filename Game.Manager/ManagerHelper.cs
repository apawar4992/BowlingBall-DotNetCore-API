using Game.Model;

namespace Game.Manager
{
    public static class ManagerHelper
    {
        public static Repository.Game ToGame(this Model.Game game)
        {
            Repository.Game repoGame;
            BowlingGame bGame;
            if (game is BowlingGame child)
            {
                BowlingGame bowlingGame = (BowlingGame)game;
            }

            if (game is BowlingGame)
            {
                bGame = (BowlingGame)game;
                repoGame = bGame.ToBowlingGame();
                if (repoGame == null)
                    return null;
                repoGame.GameId = Guid.NewGuid().ToString();
                repoGame.GameType = Constants.BOWLINGBALL;
            }
            else
            {
                throw new GameNotFoundException();
            }

            return repoGame;
        }

        public static Repository.BowlingGame ToBowlingGame(this BowlingGame bowlingGame)
        {
            if (bowlingGame == null)
                return null;

            Repository.BowlingGame bowling = new Repository.BowlingGame()
            {
                BowlingFrames = bowlingGame.BowlingFrames.ToFrame(),
                GameCategory = "InDoor",
            };

            return bowling;
        }

        public static List<Repository.Frame> ToFrame(this List<Frame> frames)
        {
            if (frames == null)
                return null;
            List<Repository.Frame> repoFrames = frames.Select(Item => new Repository.Frame(Item.FirstBowl)
            {
                SecondBowl = Item.SecondBowl,
            }).ToList();

            return repoFrames;
        }
    }
}
