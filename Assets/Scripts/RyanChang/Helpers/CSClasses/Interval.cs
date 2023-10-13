using UnityEngine;

[System.Serializable]
[System.Obsolete("Use Duration instead")]
public class Interval
{
    [Tooltip("The range used.")]
    public Range range;

    [Tooltip("The timer used for the interval. Informational only. Do not change in inspector.")]
    public float timer;

    /// <summary>
    /// Constructs an interval with a single range.
    /// </summary>
    /// <param name="value">The value to set to.</param>
    public Interval(float value)
    {
        range = new(value);
    }

    /// <summary>
    /// Creates a new interval with a linear range with max and min.
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximal value</param>
    public Interval(float min, float max)
    {
        range = new(min, max);
    }

    /// <summary>
    /// Updates the interval.
    /// </summary>
    /// <param name="deltaTime">Amount of time that has passed since last update.</param>
    /// <returns>True if the interval has reached its internal timer. Most likely Time.deltaTime.</returns>
    public bool UpdateInterval(float deltaTime)
    {
        if (timer >= range.Select())
        {
            range.Reset();
            timer = 0;
            return true;
        }
        else
        {
            timer += deltaTime;
            return false;
        }
    }

    /// <summary>
    /// Ensure that the next call to UpdateInterval returns true
    /// by setting the timer to its maximum selected value.
    /// </summary>
    public void WindToEnd()
    {
        timer = range.Select() + 1;
    }

    /// <summary>
    /// Reset timer to zero without resetting the timer's
    /// time selection.
    /// </summary>
    public void WindToStart()
    {
        timer = 0;
    }

    /// <summary>
    /// Does WindToStart (resets timer), then resets the range
    /// (selects a new time for the timer).
    /// </summary>
    public void Restart()
    {
        WindToStart();
        range.Reset();
    }
}