using System;

namespace TheBlogg.Services
{
    public class MyRandom : IMyRandom
    {
        private readonly Random _random;

        public MyRandom()
        {
            _random = new Random();
        }

        public int GetRandom(int length)
        {
            return _random.Next(length);
        }
    }
}
