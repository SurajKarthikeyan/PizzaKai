using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a dictionary of enums, where each enum is mapped to a value.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public class EnumDictionary<TEnum, TValue> :
    IDictionary<TEnum, TValue>
    where TEnum : Enum
{
    /// <summary>
    /// The dictionary explicitly used by the editor.
    /// </summary>
    [SerializeField]
    private UnityDictionary<TEnum, TValue> editorDict = new();

    #region Helper Methods
    /// <summary>
    /// Used by <see cref="EnumDictionaryDrawerUIE"/> to fix the enum, if
    /// needed.
    /// </summary>
    /// <returns>True if no changes were made, false otherwise.</returns>
    public bool FixEditorDict()
    {
        var enumValues = Enum.GetValues(typeof(TEnum));

        bool noErrors = true;

        foreach (TEnum enumIndex in enumValues)
        {
            if (!editorDict.ContainsKey(enumIndex))
            {
                // No key. Fix it.
                editorDict[enumIndex] = default;
                noErrors = false;
            }
        }

        return noErrors;
    }
    #endregion

    #region IDictionary Implementation
    public TValue this[TEnum key] { get => ((IDictionary<TEnum, TValue>)editorDict)[key]; set => ((IDictionary<TEnum, TValue>)editorDict)[key] = value; }

    public ICollection<TEnum> Keys => ((IDictionary<TEnum, TValue>)editorDict).Keys;

    public ICollection<TValue> Values => ((IDictionary<TEnum, TValue>)editorDict).Values;

    public int Count => ((ICollection<KeyValuePair<TEnum, TValue>>)editorDict).Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<TEnum, TValue>>)editorDict).IsReadOnly;

    public void Add(TEnum key, TValue value)
    {
        ((IDictionary<TEnum, TValue>)editorDict).Add(key, value);
    }

    public void Add(KeyValuePair<TEnum, TValue> item)
    {
        ((ICollection<KeyValuePair<TEnum, TValue>>)editorDict).Add(item);
    }

    public void Clear()
    {
        ((ICollection<KeyValuePair<TEnum, TValue>>)editorDict).Clear();
    }

    public bool Contains(KeyValuePair<TEnum, TValue> item)
    {
        return ((ICollection<KeyValuePair<TEnum, TValue>>)editorDict).Contains(item);
    }

    public bool ContainsKey(TEnum key)
    {
        return ((IDictionary<TEnum, TValue>)editorDict).ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TEnum, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TEnum, TValue>>)editorDict).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TEnum, TValue>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<TEnum, TValue>>)editorDict).GetEnumerator();
    }

    public bool Remove(TEnum key)
    {
        return ((IDictionary<TEnum, TValue>)editorDict).Remove(key);
    }

    public bool Remove(KeyValuePair<TEnum, TValue> item)
    {
        return ((ICollection<KeyValuePair<TEnum, TValue>>)editorDict).Remove(item);
    }

    public bool TryGetValue(TEnum key, out TValue value)
    {
        return ((IDictionary<TEnum, TValue>)editorDict).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)editorDict).GetEnumerator();
    }
    #endregion
}