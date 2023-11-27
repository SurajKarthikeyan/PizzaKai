using System;
using System.Collections.Generic;

/// <summary>
/// Can be used in a sorted list, dictionary, or set in order to define priority
/// for its values.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[Serializable]
public class PriorityKey<TPriority> : IComparable<PriorityKey<TPriority>> where TPriority : IComparable<TPriority>
{
    /// <summary>
    /// Based on convention, a lower value for <see cref="priority"/> usually
    /// denotes a higher priority, so lower values of <see cref="priority"/>
    /// means that the key will be selected ahead of keys with higher values of
    /// <see cref="priority"/>.
    /// </summary>
    public TPriority priority;

    /// <summary>
    /// The ID of the key, used to distinguish between different keys. It is
    /// intended that this be set to the InstanceID of a Unity GameObject, but
    /// it can be set to any unique value. See the constructors of this class
    /// for more information.
    /// </summary>
    public int id;

    /// <summary>
    /// Optional tag to distinguish between keys with the same ID. You could,
    /// for example, use it to denote which method this key is used in.
    /// </summary>
    public string tag;

    /// <summary>
    /// Creates a new priority key.
    /// </summary>
    /// <param name="priority">Priority of the key. A lower value is a higher
    /// priority.</param>
    /// <param name="id">ID of the key. See <see cref="id"/>.</param>
    /// <param name="tag">Optional tag to distinguish between different keys
    /// with the same ID. See <see cref="tag"/>.</param>
    public PriorityKey(TPriority priority, int id, string tag = "default")
    {
        this.priority = priority;
        this.id = id;
        this.tag = tag;
    }

    /// <summary>
    /// Creates a new priority key, using a Unity GameObject to generate the ID.
    /// </summary>
    /// <param name="unityObject">Object used for ID. See <see
    /// cref="id"/>.</param>
    /// <inheritdoc cref="PriorityKey{TPriority}(TPriority, int, string)"/>
    public PriorityKey(TPriority priority, UnityEngine.Object unityObject,
        string tag = "default")
    {
        this.priority = priority;
        this.id = unityObject.GetInstanceID();
        this.tag = tag;
    }

    /// <summary>
    /// Creates a new priority key, using a Unity GameObject to generate the ID,
    /// with the default priority.
    /// </summary>
    /// <inheritdoc cref="PriorityKey(TPriority, UnityEngine.Object, string)"/>
    public PriorityKey(UnityEngine.Object unityObject, string tag = "default")
    {
        this.priority = default;
        this.id = unityObject.GetInstanceID();
        this.tag = tag;
    }

    public int CompareTo(PriorityKey<TPriority> other)
    {
        // Lower value == higher priority.
        int cmp = priority.CompareTo(other.priority);

        if (cmp == 0)
        {
            if (id == other.id)
                return tag.CompareTo(other.tag);

            return id.CompareTo(other.id);
        }
        else
            return cmp;
    }

    public override string ToString()
    {
        return $"PriorityKey [{priority}] ({id}:{tag})";
    }
}