/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

namespace Microsoft.PowerBI.Common.Abstractions.Interfaces
{
    /// <summary>
    /// Data storage.
    /// </summary>
    public interface IDataStorage
    {
        /// <summary>
        /// Retreive an item if it exists in storage.
        /// </summary>
        /// <typeparam name="T">Type of the item to return.</typeparam>
        /// <param name="key">Lookup key to retrieve the item.</param>
        /// <param name="value">Result of the item if found; null if not found.</param>
        /// <returns>True if hte item is found; false otherwise.</returns>
        bool TryGetItem<T>(string key, out T value);

        /// <summary>
        /// Retrieve an item from storage, if not found the default value for the item type is returned.
        /// </summary>
        /// <typeparam name="T">Type of the item to return.</typeparam>
        /// <param name="key">Lookup key to retrieve the item.</param>
        /// <returns>Item found in storage or the default value of the type of item if not found.</returns>
        T GetItemOrDefault<T>(string key);

        /// <summary>
        /// Retrieve an item from storage if it exists, otherwise place the value in storage and return that as its default.
        /// </summary>
        /// <typeparam name="T">Type of the item to return.</typeparam>
        /// <param name="key">Lookup key to retrieve the item or to store item value with this key name.</param>
        /// <param name="value">Item to place in storage if the an item with the given key does not exist.</param>
        /// <returns>Item found in storage or the item passed in the value if not found.</returns>
        T TryGetItemSet<T>(string key, T value);

        /// <summary>
        /// Removes an item from storage.
        /// </summary>
        /// <param name="key">Lookup key to find the item to remove.</param>
        void RemoveItem(string key);

        /// <summary>
        /// Adds or updates (if it already exists) an item in storage.
        /// </summary>
        /// <typeparam name="T">Type of the item to store in storage.</typeparam>
        /// <param name="key">Lookup key to place item in storage and for future retrieval.</param>
        /// <param name="value">Value of the item to place in storage.</param>
        void SetItem<T>(string key, T value);
    }
}