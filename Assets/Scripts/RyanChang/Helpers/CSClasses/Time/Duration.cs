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
        maxTime = duration;
    }

    /// <summary>
    /// True if the elapsed time is greater than the maximal time.
    /// </summary>
    public bool IsDone => elapsed >= maxTime;

    /// <summary>
    /// What percentage is this duration done by?
    /// </summary>
    public float Percent => IsDone ? 1 : elapsed / maxTime;

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
            ResetIfDone();
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
    /// Increments <see cref="elapsed"/> by <see cref="Time.fixedDeltaTime"/>.
    /// Use this in FixedUpdate() functions.
    /// </summary>
    /// <inheritdoc cref="Increment(float, bool)"/>
    public bool IncrementFixedUpdate(bool resetIfDone)
    {
        return Increment(Time.fixedDeltaTime, resetIfDone);
    }

    /// <summary>
    /// Resets the value of <see cref="elapsed"/>, if the duration is done. This
    /// doesn't do anything if the timer is not done, however.
    /// </summary>
    /// <returns>True if <see cref="IsDone"/> was true when the function was
    /// ran. False otherwise.</returns>
    public bool ResetIfDone()
    {
        if (IsDone)
        {
            Reset();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resets the value of <see cref="elapsed"/> to zero, regardless of whether
    /// or not the timer is done.
    /// </summary>
    public void Reset() => elapsed = 0;

    /// <summary>
    /// Sets the value of <see cref="elapsed"/> to <see cref="maxTime"/>, if the
    /// duration is not done.
    /// </summary>
    /// <returns>False if <see cref="IsDone"/> is already true, true
    /// otherwise.</returns>
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