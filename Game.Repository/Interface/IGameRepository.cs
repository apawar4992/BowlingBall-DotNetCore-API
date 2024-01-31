namespace Game.Repository
{
    public interface IGameRepository
    {
        string GameName { get; }
        Task<GameResult> CalculateScore(Game game);
    }
}
