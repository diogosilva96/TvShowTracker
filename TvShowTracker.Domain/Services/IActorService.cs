using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IActorService
    {
        public Task<Result<IEnumerable<ActorModel>>> GetAllAsync(GetActorsFilter filter);

        public Task<Result<ActorModel>> GetByIdAsync(int id);
    }
}
