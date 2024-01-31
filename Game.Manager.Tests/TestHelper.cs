using AutoFixture.AutoMoq;
using AutoFixture;
using Game.Model;
using Microsoft.Extensions.Logging;

namespace Game.Manager.Tests
{
    public static class TestHelper
    {
        public static List<Frame> AddRolls(int[] pins)
        {
            bool isFirstBowlStrike = true;
            List<Frame> frames = null;
            try
            {
                if (pins != null && pins.Count() > 0)
                {
                    frames = new List<Frame>();
                    for (int index = 0; index < pins.Length; index++)
                    {
                        if (isFirstBowlStrike)
                        {
                            // for first bowl.
                            frames.Add(new Frame(pins[index]));
                            isFirstBowlStrike = pins[index] == 10;
                        }
                        else
                        {
                            // for second bowl.
                            var last = frames.LastOrDefault();
                            last.SecondBowl = pins[index];
                            isFirstBowlStrike = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return frames;
        }

        public static ILogger<T> GetLoggerInstance<T>()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            return fixture.Create<ILogger<T>>();
        }
    }
}
