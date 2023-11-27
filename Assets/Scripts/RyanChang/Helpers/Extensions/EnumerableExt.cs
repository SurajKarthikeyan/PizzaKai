using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains methods pertaining to C# list, dictionaries, or other IEnumerable
/// classes.
/// </summary>
public static class EnumerableExt
{
    #region Index
    /// <summary>
    /// Returns true if <paramref name="index"/> is a valid indexer into
    /// <paramref name="collection"/>. If <paramref name="collection"/> is null
    /// or empty, returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection to evaluate.</param>
    /// <param name="index">The index to evaluate.</param>
    /// <returns>True if <paramref name="index"/> is a valid indexer into
    /// <paramref name="collection"/>, false otherwise.</returns>
    public static bool IndexInRange<T>(this IEnumerable<T> collection,
        int index)
    {
        if (collection == null)
            return false;

        return index >= 0 && index < collection.Count();
    }

    /// <summary>
    /// Ensures that <paramref name="index"/> ranges from 0 to <paramref
    /// name="collection"/> count. If it lies outside of that bound, then wrap
    /// it.
    /// </summary>
    public static int WrapAroundLength<T>(this int index, ICollection<T> collection)
    {
        return index.WrapAroundLength(collection.Count);
    }

    /// <summary>
    /// Ensures that <paramref name="index"/> ranges from 0 to <paramref
    /// name="length"/>. If it lies outside of that bound, then wrap it.
    /// </summary>
    /// <remarks>
    /// Adapted from https://stackoverflow.com/a/1082938.
    /// </remarks>
    public static int WrapAroundLength(this int index, int length)
    {
        return (index % length + length) % length;
    }

    /// <summary>
    /// Ensures that <paramref name="index"/> ranges from <paramref
    /// name="from"/> to <paramref name="to"/> (including <paramref
    /// name="to"/>). If it lies outside of that bound, then wrap it.
    /// </summary>
    public static int WrapAround(this int index, int from, int to)
    {
        return index.WrapAroundLength(to - from + 1) + from;
    }
    #endregion

    #region List
    /// <summary>
    /// Swaps the values in the collection at <paramref name="indexA"/> and
    /// <paramref name="indexB"/>.
    /// </summary>
    /// <param name="collection">The list/array to operate on.</param>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    public static void Swap<T>(this IList<T> collection, int indexA, int indexB)
    {
        (collection[indexB], collection[indexA]) =
            (collection[indexA], collection[indexB]);
    }

    /// <summary>
    /// Removes everything past (and including) index from.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to modify.</param>
    /// <param name="from">Index to remove from.</param>
    public static void TrimToEnd<T>(this IList<T> list, int from)
    {
        while (from < list.Count)
        {
            list.RemoveAt(from);
        }
    }

    /// <summary>
    /// If index is a valid index in list, then replace the element at index
    /// with obj. Otherwise, add obj to the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to modify.</param>
    /// <param name="obj">Object to add.</param>
    /// <param name="index">Index to add the object at.</param>
    public static void AddOrReplace<T>(this IList<T> list, T obj, int index)
    {
        if (index < list.Count)
            list[index] = obj;
        else
            list.Add(obj);
    }

    /// <summary>
    /// If index is a valid index in list, then replace the element at index
    /// with obj. Otherwise, add obj to the list, buffering new elements with
    /// defaults.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">List to modify.</param>
    /// <param name="obj">Object to add.</param>
    /// <param name="index">Index to add the object at.</param>
    public static void AddOrReplaceWithBuffer<T>(this IList<T> list, T obj,
        int index)
    {
        if (index < list.Count)
            list[index] = obj;
        else
        {
            while (index > list.Count)
            {
                list.Add(default);
            }

            list.Add(obj);
        }
    }
    #endregion

    #region Dictionary
    /// <summary>
    /// Tries to get the value from <paramref name="dictionary"/>. If found,
    /// returns that value. Otherwise, return the value assigned to <paramref
    /// name="defaultValue"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of key in the dictionary.</typeparam>
    /// <typeparam name="TVal">The type of value in the dictionary.</typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static TVal GetValueOrDefault<TKey, TVal>(
        this IDictionary<TKey, TVal> dictionary,
        TKey key, TVal defaultValue = default)
    {
        if (dictionary.TryGetValue(key, out TVal value))
        {
            return value;
        }

        return defaultValue;
    }

    /// <summary>
    /// Initializes an internal list in <paramref name="dictionary"/>.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="T">List member object type.</typeparam>
    /// <param name="dictionary">The dictionary of lists.</param>
    /// <param name="key">The key for <paramref name="dictionary"/>.</typeparam>
    public static void InitializeListInDictList<K, T>(
        this IDictionary<K, List<T>> dictionary, K key)
    {
        if (!dictionary.ContainsKey(key))
            dictionary[key] = new();
        else
            dictionary[key] ??= new();
    }

    /// <summary>
    /// Adds <paramref name="obj"/> to an internal list in a dictionary of lists
    /// <paramref name="dictionary"/>.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="T">Object to add type.</typeparam>
    /// <param name="dictionary">The dictionary of lists.</param>
    /// <param name="key">The key that targets the list to add to.</typeparam>
    /// <param name="obj">The object to add to the list.</param>
    public static void AddToDictList<K, T>(
        this IDictionary<K, List<T>> dictionary, K key, T obj)
    {
        dictionary.InitializeListInDictList(key);

        dictionary[key].Add(obj);
    }

    /// <summary>
    /// Adds or replaces <paramref name="obj"/> to an internal list in a
    /// dictionary of lists <paramref name="dictionary"/>. See <see
    /// cref="AddOrReplace{T}(IList{T}, T, int)"/>.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="T">Object to add type.</typeparam>
    /// <param name="dictionary">The dictionary of lists.</param>
    /// <param name="key">The key that targets the list to add to.</typeparam>
    /// <param name="obj">The object to add to the list.</param>
    /// <param name="index">Index to add the object at.</param>
    public static void AddOrReplaceToDictList<K, T>(
        this IDictionary<K, List<T>> dictionary, K key, T obj, int index)
    {
        dictionary.InitializeListInDictList(key);

        dictionary[key].AddOrReplace(obj, index);
    }
    #endregion

    #region Misc
    /// <summary>
    /// Returns true if <paramref name="collection"/> is null or contains no
    /// elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">Collection to check.</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection == null || collection.Count <= 0;
    }
    #endregion
}