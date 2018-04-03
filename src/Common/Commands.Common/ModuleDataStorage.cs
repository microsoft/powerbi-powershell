/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using Microsoft.Extensions.Caching.Memory;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    public class ModuleDataStorage : IDataStorage
    {
        private static IMemoryCache Cache { get; set; }

        public ModuleDataStorage(IMemoryCache cache = null)
        {
            if(cache != null)
            {
                Cache = cache;
            }
            else
            {
                Cache = Cache ?? new MemoryCache(new MemoryCacheOptions());
            }
        }

        public bool TryGetItem<T>(string key, out T value) => Cache.TryGetValue<T>(key, out value);

        public T GetItemOrDefault<T>(string key)
        {
            if (this.TryGetItem<T>(key, out T tempValue))
            {
                return tempValue;
            }

            return default;
        }

        public T TryGetItemSet<T>(string key, T value)
        {
            if (this.TryGetItem<T>(key, out T tempValue))
            {
                return tempValue;
            }

            this.SetItem<T>(key, value);
            return value;
        }

        public void RemoveItem(string key) => Cache.Remove(key);

        public void SetItem<T>(string key, T value) => Cache.Set<T>(key, value);
    }
}