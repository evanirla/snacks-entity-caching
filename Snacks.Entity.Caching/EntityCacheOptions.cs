using Microsoft.Extensions.Caching.Distributed;
using System;

namespace Snacks.Entity.Caching
{
    /// <summary>
    /// Configuration for EntityCacheService
    /// </summary>
    public class EntityCacheOptions
    {
        /// <summary>
        /// The action executed for every entry into the distributed cache.
        /// </summary>
        public Action<DistributedCacheEntryOptions> EntryAction { get; set; }
    }
}
