using Game.Model;

namespace GameApplication.Model
{
    public class ApiRequest
    {
        public string GameType { get; set; }
        public List<Frame> Frames { get; set; }
    }
}
