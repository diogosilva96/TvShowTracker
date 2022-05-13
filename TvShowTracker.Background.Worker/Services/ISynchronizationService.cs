using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Background.Worker.Services
{
    public interface ISynchronizationService
    {
        /// <summary>
        /// Executes the synchronization
        /// </summary>
        /// <returns></returns>
        public Task ExecuteAsync();
    }
}
