namespace TvShowTracker.Api.Services
{
    public interface IHashingService
    {
        public int Decode(string hash);

        public string Encode(int number);
    }
}
