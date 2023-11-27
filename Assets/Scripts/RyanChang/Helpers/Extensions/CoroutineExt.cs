using UnityEngine;

/// <summary>
/// Contains methods that extend <see cref="Coroutine"/>.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class CoroutineExt
{
    /// <summary>
    /// For each coroutine in <paramref name="coroutines"/>, stops each
    /// individual coroutine if that coroutine is not null. not null.
    /// </summary>
    /// <param name="behaviour">The MonoBehavior that owns the
    /// coroutine.</param>
    /// <param name="coroutines">The coroutines to evaluate and stop.</param>
    public static void StopCoroutineIfExists(this MonoBehaviour behaviour,
        params Coroutine[] coroutines)
    {
        foreach (var cr in coroutines)
        {
            if (cr != null)
            {
                behaviour.StopCoroutine(cr);
            } 
        }
    }
}