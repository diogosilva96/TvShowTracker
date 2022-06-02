using HashidsNet;
using Microsoft.Extensions.Logging;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;

namespace TvShowTracker.Infrastructure.Services
{
    public class HashingService : IHashingService
    {
        private readonly ILogger<IHashingService> _logger;
        private readonly Hashids _hashManager;
        public HashingService(string saltKey, ILogger<IHashingService> logger)
        {
            _logger = logger;
            _hashManager = new Hashids(saltKey);
        }

       
        public int? Decode(string hash)
        {
            try
            {
                return _hashManager.DecodeSingle(hash);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while decoding {hash}, error details: {details}",hash, ex.ToString());
                return null;
            }
        }

     
        public string? Encode(int number)
        {
            try
            {
                return _hashManager.Encode(number);

            }
            catch (Exception ex)
            {
                // this throw will probably never happen, but better safe than sorry
                _logger.LogError("Error occurred while encoding {id}, error details: {details}", number, ex.ToString());
                return null;
            }
        }
    }
}
