using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Infrastructure.Interface
{
    public interface ICachableRequest
    {
        /// <summary>
        /// Unique key used to store and retrieve the cache.
        /// </summary>
        string CacheKey { get; }

        /// <summary>
        /// How long the cache should last (optional).
        /// </summary>
        TimeSpan? AbsoluteExpiration { get; }
    }
}
