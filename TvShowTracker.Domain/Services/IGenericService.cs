using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IGenericService<T> where T : class
    {
        public Task<Result<T>> CreateAsync(T target);

        public Task<Result<T>> UpdateAsync(T target);

        public Task<Result<T>> GetByIdAsync(int id);

        public Task<Result<bool>> DeactivateAsync(int id);

        public Task<Result<IEnumerable<T>>> GetAllAsync(int? page = null, int? size = null);
    }
}
