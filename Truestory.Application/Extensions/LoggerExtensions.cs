using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Truestory.Application.Extensions
{
    public static class LoggerExtensions
    {
        #region recursive log with exception
        public static void RecursivelyLogWrtException<T>(this ILogger<T> logger, Exception exception, IConfiguration configuration = null)
        {
            try
            {
                logger.LogError(exception, exception.Message);
                logger.LogInformation(JsonSerializer.Serialize(exception));

                Exception innerException = exception.InnerException;
                int countWrtInnerException = 0;
                int maximumCountWrtInnerExceptionLogging = 10;
                if (!string.IsNullOrEmpty(configuration?["MaximumCountWrtInnerExceptionLogging"]))
                {
                    maximumCountWrtInnerExceptionLogging = Convert.ToInt32(configuration["MaximumCountWrtInnerExceptionLogging"]);
                }

                while (innerException != null
                    && ++countWrtInnerException <= maximumCountWrtInnerExceptionLogging)
                {
                    logger.LogInformationWrtInnerExceptionWrtCount(countWrtInnerException);
                    logger.LogError(innerException, innerException.Message);
                    logger.LogInformation(JsonSerializer.Serialize(innerException));

                    innerException = innerException.InnerException;
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error occurred while logging exception(s)...");
                logger.LogError(ex, ex.Message);
            }
        }
        #endregion


        #region log informatio with inner exception
        private static void LogInformationWrtInnerExceptionWrtCount<T>(this ILogger<T> logger, int count)
        {
            logger.LogInformation($"Logging inner exception ({count})...");
        }
        #endregion
    }
}
