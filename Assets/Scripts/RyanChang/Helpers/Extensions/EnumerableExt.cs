using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains methods pertaining to C# list, dictionaries, or other IEnumerable
/// classes.
/// </summary>
public static class EnumerableExt
{
    /// <summary>
    /// Returns true if the IEnumerable is null or contains no elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable">IEnumerable to check.</param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }

    /// <summary>
    /// Adds an addition to a list in the dictionary. Creates the list and adds
    /// the addition if the specified key does not exist in the dictionary.
    /// <typeparam name="TKey">The key.</typeparam>
    /// <typeparam name="TMem">The member of the list within the
    /// dictionary.</typeparam>
    /// <param name="dict">The dictionary to add to.</param>
    /// <param name="key">The specified key.</param>
    /// <param name="addition">What to add to the list.</param>
    /// </summary>
    public static void CreateAndAddList<TKey, TMem>(this Dictionary<TKey,
        List<TMem>> dict, TKey key, TMem addition)
    {
        dict.GetOrCreate(key).Add(addition);
    }

    /// <summary>
    /// Creates a new instance of TMem if the specified key does not exist in
    /// the provided dictionary.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TMem">The member type.</typeparam>
    /// <param name="dict">The provided dictionary.</param>
    /// <param name="key">The specified key.</param>
    /// <returns>The newly created or found instance of TMem.</returns>
    public static TMem GetOrCreate<TKey, TMem>(this Dictionary<TKey,
        TMem> dict, TKey key) where TMem : class, new()
    {
        if (!dict.ContainsKey(key))
            dict[key] = new();

        return dict[key];
    }

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
    /// Returns true if <paramref name="index"/> is a valid indexer into
    /// <paramref name="array"/>. If <paramref name="array"/> is null
    /// or empty, returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array to evaluate.</param>
    /// <param name="index">The index to evaluate.</param>
    /// <returns>True if <paramref name="index"/> is a valid indexer into
    /// <paramref name="array"/>, false otherwise.</returns>
    public static bool IndexInRange<T>(this T[] array, int index)
    {
        if (array == null)
            return false;

        return index >= 0 && index < array.Length;
    }

    #region List
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
}