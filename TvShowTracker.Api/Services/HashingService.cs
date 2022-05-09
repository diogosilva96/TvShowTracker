using HashidsNet;

namespace TvShowTracker.Api.Services
{
    public class HashingService : IHashingService
    {
        private readonly Hashids _hashids;
        public HashingService(string saltKey)
        {
            _hashids = new Hashids(saltKey);
        }
        public int Decode(string hash) => _hashids.DecodeSingle(hash);
        public string Encode(int number) => _hashids.Encode(number);
    }
}
