using UnityEngine;

/// <summary>
/// Represents a duration of time, bundling both maximum time and elapsed time
/// for an event in one class.
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
    /// <param name="resetIfDone">If true, resets the duration</param>
    /// <returns>The new value of <see cref="IsDone"/>.</returns>
    public bool Increment(float deltaTime, bool resetIfDone)
    {
        elapsed += deltaTime;
        elapsed = Mathf.Clamp(elapsed, 0, maxTime);

        if (IsDone && resetIfDone)
        {
            Reset();
            return true;
        }

        return IsDone;
    }

    /// <summary>
    /// Increments <see cref="elapsed"/> by <see cref="Time.deltaTime"/>. Use
    /// this in Update() functions.
    /// </summary>
    /// <inheritdoc cref="Increment(float, bool)"/>
    public bool IncrementUpdate(bool resetIfDone)
    {
        return Increment(Time.deltaTime, resetIfDone);
    }

    /// <summary>
    /// Increments <see cref="elapsed"/> by <see cref="Time.fixedDeltaTime"/>. Use
    /// this in FixedUpdate() functions.
    /// </summary>
    /// <inheritdoc cref="Increment(float, bool)"/>
    public bool IncrementFixedUpdate(bool resetIfDone)
    {
        return Increment(Time.fixedDeltaTime, resetIfDone);
    }

    /// <summary>
    /// Resets the value of <see cref="elapsed"/>, if the duration is done.
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

    /// <summary>
    /// Sets the value of <see cref="elapsed"/> to <see cref="maxTime"/>, if the
    /// duration is not done.
    /// </summary>
    /// <returns>True on success, false otherwise.</returns>
    public bool Finish()
    {
        if (!IsDone)
        {
            elapsed = maxTime;
            return true;
        }

        return false;
    }
}