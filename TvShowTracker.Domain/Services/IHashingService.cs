namespace TvShowTracker.Domain.Services
{
    public interface IHashingService
    {
        /// <summary>
        /// Converts a hash to a number
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>The number if the hash is valid, otherwise null</returns>
        public int? Decode(string hash);

        /// <summary>
        /// Encodes a number to a hash
        /// </summary>
        /// <param name="number"></param>
        /// <returns>The string if the number is valid, otherwise null</returns>
        public string? Encode(int number);
    }
}
