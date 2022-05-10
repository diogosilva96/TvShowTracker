using HashidsNet;

namespace TvShowTracker.Api.Services
{
    public class HashingService : IHashingService
    {
        private readonly Hashids _hashManager;
        public HashingService(string saltKey)
        {
            _hashManager = new Hashids(saltKey);
        }
        public int Decode(string hash) => _hashManager.DecodeSingle(hash);
        public string Encode(int number) => _hashManager.Encode(number);
    }
}
