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
            // The function only regenerates the internal dictionary if a
            // difference has been found.
            GenerateInternalDict();
            return internalDict;
        }
    }
    #endregion

    #region Input Checking
    /// <summary>
    /// Checks if the <see cref="keyValuePairs"/> are valid.
    /// </summary>
    public UnityDictionaryErrorCode ValidateKVPs()
    {
        UnityDictionaryErrorCode code = UnityDictionaryErrorCode.None;

        if (keyValuePairs == null)
        {
            return code;
        }

        if (keyValuePairs.Count != keyValuePairs.Distinct(inspectorKVPKeyComparer).Count())
        {
            code |= UnityDictionaryErrorCode.DuplicateKeys;
        }

        if (typeof(TKey).IsClass)
        {
            if (keyValuePairs.Any(kvp => kvp.Key == null))
            {
                code |= UnityDictionaryErrorCode.NullKeys;
            }
        }

        return code;
    }

    private static readonly InspectorKVPKeyComparer inspectorKVPKeyComparer = new();

    private class InspectorKVPKeyComparer : IEqualityComparer<InspectorKeyValuePair>
    {
        public bool Equals(InspectorKeyValuePair x, InspectorKeyValuePair y)
        {
            return x.Key.Equals(y.Key);
        }

        public int GetHashCode(InspectorKeyValuePair obj)
        {
            return obj.GetHashCode();
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

#if UNITY_EDITOR
            // This code only runs in the editor as that is the only time when
            // keyValuePairs is modifiable.
            int i = keyValuePairs.FindIndex(kvp => kvp.Key.Equals(key));

            if (i >= 0)
                keyValuePairs[i] = new(keyValuePairs[i].Key, value);
            else
                keyValuePairs.Add(new(key, value));

            // Recompute the hash so internalDict doesn't get rebuilt.
            prevKVPsHash = keyValuePairs.GetHashCode();
#endif
        }
    }

    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)BufferedDict).Add(key, value);

#if UNITY_EDITOR
        keyValuePairs.Add(new(key, value));
#endif
    }

    public bool ContainsKey(TKey key)
    {
        return ((IDictionary<TKey, TValue>)BufferedDict).ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        bool removed = ((IDictionary<TKey, TValue>)BufferedDict).Remove(key);

#if UNITY_EDITOR
        // We only need to manage the keyValuePairs in the editor. When the game
        // is compiled, keyValuePairs can no longer be edited.
        if (removed)
            keyValuePairs.RemoveAll(kvp => kvp.Key.Equals(key));
#endif

        return removed;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return ((IDictionary<TKey, TValue>)BufferedDict).TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Add(item);

#if UNITY_EDITOR
        keyValuePairs.Add(new(item));
#endif
    }

    public void Clear()
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Clear();

#if UNITY_EDITOR
        keyValuePairs.Clear();
#endif
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        bool removed = ((ICollection<KeyValuePair<TKey, TValue>>)BufferedDict).Remove(item);

#if UNITY_EDITOR
        if (removed)
            keyValuePairs.RemoveAll(kvp => kvp.Key.Equals(item.Key));
#endif

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

#region Enums
[Flags]
public enum UnityDictionaryErrorCode
{
    /// <summary>
    /// <see cref="keyValuePairs"/> has been successfully validated.
    /// </summary>
    None = 0,
    /// <summary>
    /// <see cref="keyValuePairs"/> has at least 2 duplicate keys.
    /// </summary>
    DuplicateKeys = 1,
    /// <summary>
    /// <see cref="keyValuePairs"/> has at least 1 null key.
    /// </summary>
    NullKeys = 2
}
#endregion