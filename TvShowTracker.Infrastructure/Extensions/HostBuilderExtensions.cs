using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace TvShowTracker.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures Serilog logging
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configAction"></param>
        /// <returns>If config action is null will use default behaviour</returns>
        public static IHostBuilder ConfigureSerilog(this IHostBuilder self, Action<LoggerConfiguration>? configAction = null)
        {
            self.UseSerilog((_, loggerConfiguration) =>
            {
                if (configAction is not null)
                {
                    configAction(loggerConfiguration);
                    return;
                }
                
                loggerConfiguration.WriteTo.Console()
                                   .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);
            });
            return self;
        }
    }
}
