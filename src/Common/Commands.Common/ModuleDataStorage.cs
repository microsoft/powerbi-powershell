/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System.Collections.Generic;
using Microsoft.PowerBI.Common.Abstractions.Interfaces;

namespace Microsoft.PowerBI.Commands.Common
{
    public class ModuleDataStorage : IDataStorage
    {
        private static IDictionary<string, object> Cache { get; set; }

        public ModuleDataStorage(IDictionary<string, object> cache = null)
        {
            if (cache != null)
            {
                Cache = cache;
            }
            else
            {
                Cache = Cache ?? new Dictionary<string, object>();
            }
        }

        public bool TryGetItem<T>(string key, out T value) where T : class
        {
            var result = Cache.TryGetValue(key, out object tempValue);
            value = result ? tempValue as T : null;
            return result;
        }

        public T GetItemOrDefault<T>(string key) where T : class
        {
            if (this.TryGetItem<T>(key, out T tempValue))
            {
                return tempValue;
            }

            return default;
        }

        public T TryGetItemSet<T>(string key, T value) where T : class
        {
            if (this.TryGetItem<T>(key, out T tempValue))
            {
                return tempValue;
            }

            this.SetItem<T>(key, value);
            return value;
        }

        public void RemoveItem(string key) => Cache.Remove(key);

        public void SetItem<T>(string key, T value) where T : class
        {
            if(Cache.ContainsKey(key))
            {
                Cache[key] = value;
            }
            else
            {
                Cache.Add(key, value);
            }
        }

    }
}