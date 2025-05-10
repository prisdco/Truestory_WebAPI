using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Infrastructure.Interface;

namespace Truestory.Application.Extensions
{
    public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheBehavior<TRequest, TResponse>> _logger;

        public CacheBehavior(IMemoryCache cache, ILogger<CacheBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not ICachableRequest cachableRequest)
            {
                return await next(); // Not a cachable request
            }

            var cacheKey = cachableRequest.CacheKey;

            if (_cache.TryGetValue(cacheKey, out TResponse cachedResponse))
            {
                _logger.LogInformation($"[CACHE HIT] Key: {cacheKey}");
                return cachedResponse;
            }

            _logger.LogInformation($"[CACHE MISS] Key: {cacheKey}. Processing request.");
            var response = await next();

            _cache.Set(cacheKey, response, cachableRequest.AbsoluteExpiration ?? TimeSpan.FromMinutes(5));
            _logger.LogInformation($"[CACHE SET] Key: {cacheKey}");

            return response;
        }
    }
}
