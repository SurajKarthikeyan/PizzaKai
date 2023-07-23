using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Used to display a dictionary in the unity inspector. Use if you want to
/// allow the user to modify a dictionary that won't be changed anywhere else.
/// </summary>
/// <typeparam name="TKey">A unity-serializable value.</typeparam>
/// <typeparam name="TValue">A unity-serializable value.</typeparam>
[Serializable]
public class UnityDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    #region Structs
    [Serializable]
    public struct InspectorKeyValuePair
    {
        #region Variables
        /// <summary>
        /// The key associated with this <see cref="InspectorKeyValuePair"/>.
        /// </summary>
        [SerializeField]
        private TKey key;

        /// <summary>
        /// The value associated with this <see cref="InspectorKeyValuePair"/>.
        /// </summary>
        [SerializeField]
        private TValue value;
        #endregion

        #region Properties
        /// <inheritdoc cref="key"/>
        public TKey Key => key;

        /// <inheritdoc cref="value"/>
        public TValue Value => value;
        #endregion

        public InspectorKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        /// <summary>
        /// Creates a new <see cref="InspectorKeyValuePair"/> from <paramref
        /// name="keyValuePair"/>.
        /// </summary>
        /// <param name="keyValuePair">The <see cref="KeyValuePair{TKey,
        /// TValue}"/> to use.</param>
        public InspectorKeyValuePair(KeyValuePair<TKey, TValue> keyValuePair)
        {
            this.key = keyValuePair.Key;
            this.value = keyValuePair.Value;
        }
    }
    #endregion

    #region Variables
    /// <summary>
    /// The list of <see cref="InspectorKeyValuePair"/> used by the inspector.
    /// </summary>
    [SerializeField]
    private List<InspectorKeyValuePair> keyValuePairs;
    private int prevKVPsHash;

    private Dictionary<TKey, TValue> internalDict;

    private Dictionary<TKey, TValue> BufferedDict
    {
        get
        {
            // This is fairly cheap to run, and (most of the time) runs in O(1).
            GenerateInternalDict();
            return internalDict;
        }
    }
    #endregion

    #region Helper Functions
    private void GenerateInternalDict()
    {
        if (keyValuePairs == null)
        {
            ResetInternalDict();
            prevKVPsHash = keyValuePairs.GetHashCode();
        }
        else
        {
            // Check the hash to avoid unneeded updates.
            int KVPsHash = keyValuePairs.GetHashCode();
            if (internalDict == null || KVPsHash != prevKVPsHash)
            {
                ResetInternalDict();
                prevKVPsHash = KVPsHash;
            }
        }
    }

    private void ResetInternalDict()
    {
        internalDict = new();

        keyValuePairs ??= new();

        foreach (var keyValue in keyValuePairs)
        {
            internalDict[keyValue.Key] = keyValue.Value;
        }
    }

    private void UpdateKeyValuePairs()
    {
        keyValuePairs = BufferedDict.
            Select(kvp => new InspectorKeyValuePair(kvp)).
            ToList();
    }
    #endregion

    #region IDictionary Implementation
    public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)BufferedDict).Keys;

    public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)BufferedDict).Values;

    public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).IsReadOnly;

    public TValue this[TKey key]
    {
        get
        {
            return ((IDictionary<TKey, TValue>)BufferedDict)[key];
        }
        set
        {
            ((IDictionary<TKey, TValue>)BufferedDict)[key] = value;
            UpdateKeyValuePairs();
        }
    }

    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)BufferedDict).Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return ((IDictionary<TKey, TValue>)BufferedDict).ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        bool removed = ((IDictionary<TKey, TValue>)BufferedDict).Remove(key);
        UpdateKeyValuePairs();
        return removed;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return ((IDictionary<TKey, TValue>)BufferedDict).TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Add(item);
        UpdateKeyValuePairs();
    }

    public void Clear()
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Clear();
        UpdateKeyValuePairs();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).CopyTo(array, arrayIndex);
        UpdateKeyValuePairs();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        bool removed = ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Remove(item);
        UpdateKeyValuePairs();
        return removed;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<TKey, TValue>>)BufferedDict).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)BufferedDict).GetEnumerator();
    }
    #endregion
}