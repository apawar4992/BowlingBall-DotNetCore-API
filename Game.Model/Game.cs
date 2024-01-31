using System.Text.Json.Serialization;

namespace Game.Model
{
    public class Game
    {
        public string GameType { get; set; }
        public int GameId { get; set; }
        public string GameCategory { get; set; }
    }
}
