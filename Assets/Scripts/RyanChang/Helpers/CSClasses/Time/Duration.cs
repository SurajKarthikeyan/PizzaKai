using UnityEngine;

/// <summary>
/// Represents a duration of time, bundling both maximum time and elapsed time
/// for an event in one class..
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
[System.Serializable]
public class Duration
{
    [Tooltip("How long is this duration?")]
    public float maxTime = 1f;

    [HideInInspector]
    public float elapsed = 0f;

    public Duration(float duration)
    {
        this.maxTime = duration;
    }

    /// <summary>
    /// True if the elapsed time is greater than the maximal time.
    /// </summary>
    public bool IsDone => elapsed >= maxTime;

    /// <summary>
    /// Increments <see cref="elapsed"/> by the specified <paramref
    /// name="deltaTime"/>.
    /// </summary>
    /// <param name="deltaTime">How much to increment <see cref="elapsed"/>
    /// by.</param>
    /// <returns>The new value of <see cref="IsDone"/>.</returns>
    public bool Increment(float deltaTime)
    {
        elapsed += deltaTime;
        elapsed = Mathf.Clamp(elapsed, 0, maxTime);
        return IsDone;
    }

    /// <summary>
    /// Increments <see cref="elapsed"/> by <see cref="Time.deltaTime"/>. Use
    /// this in Update() functions.
    /// </summary>
    /// <returns>The new value of <see cref="IsDone"/>.</returns>
    public bool IncrementUpdate()
    {
        return Increment(Time.deltaTime);
    }

    /// <summary>
    /// Increments <see cref="elapsed"/> by <see cref="Time.fixedDeltaTime"/>. Use
    /// this in FixedUpdate() functions.
    /// </summary>
    /// <returns>The new value of <see cref="IsDone"/>.</returns>
    public bool IncrementFixedUpdate()
    {
        return Increment(Time.fixedDeltaTime);
    }

    // /// <summary>
    // /// Increments <see cref="elapsed"/> by the specified <paramref
    // /// name="deltaTime"/>. Then, calls <see cref="Reset"/> if <see
    // /// cref="IsDone"/> is now true.
    // /// </summary>
    // /// <param name="deltaTime">How much to increment <see cref="elapsed"/>
    // /// by.</param>
    // /// <returns>The new value of <see cref="IsDone"/>.</returns>
    // public bool Next(float deltaTime)
    // {
    //     elapsed += deltaTime;
    //     elapsed = Mathf.Clamp(elapsed, 0, maxTime);

    //     if (IsDone)
    //     {
    //         elapsed = 0;
    //         return true;
    //     }

    //     return false;
    // }

    // /// <summary>
    // /// Increments <see cref="elapsed"/> by <see cref="Time.deltaTime"/>. Then,
    // /// calls <see cref="Reset"/> if <see cref="IsDone"/> is now true. Use this
    // /// in Update() functions.
    // /// </summary>
    // /// <returns>The new value of <see cref="IsDone"/>.</returns>
    // public bool NextUpdate()
    // {
    //     return Next(Time.deltaTime);
    // }

    // /// <summary>
    // /// Increments <see cref="elapsed"/> by <see cref="Time.fixedDeltaTime"/>.
    // /// Then, calls <see cref="Reset"/> if <see cref="IsDone"/> is now true.
    // /// Use this in FixedUpdate() functions.
    // /// </summary>
    // /// <returns>The new value of <see cref="IsDone"/>.</returns>
    // public bool NextFixedUpdate()
    // {
    //     return Next(Time.fixedDeltaTime);
    // }


    /// <summary>
    /// Attempts to reset the value of <see cref="elapsed"/>.
    /// </summary>
    /// <returns>The value of <see cref="IsDone"/>.</returns>
    public bool Reset()
    {
        if (IsDone)
        {
            elapsed = 0;
            return true;
        }

        return false;
    }
}