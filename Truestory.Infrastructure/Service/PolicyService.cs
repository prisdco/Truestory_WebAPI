using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Infrastructure.Service
{
    using Polly;
    using Polly.Retry;
    using Microsoft.Extensions.Logging;
    using Truestory.Infrastructure.Interface;

    public class PolicyService : IPolicyService
    {
        private readonly ILogger<PolicyService> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public PolicyService(ILogger<PolicyService> logger)
        {
            _logger = logger;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(2),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception, "Retry {RetryCount} after {Delay}", retryCount, timeSpan);
                    });
        }

        public Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
        {
            return _retryPolicy.ExecuteAsync(action);
        }
    }

}
