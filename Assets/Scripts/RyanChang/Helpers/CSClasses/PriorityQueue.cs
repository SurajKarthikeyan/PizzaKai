using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Priority queue with a min heap. Allows for updates to any priority.
/// Assumes all values are unique.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PriorityQueue<T>
{
    #region Structs
    /// <summary>
    /// Priority queue element.
    /// </summary>
    public struct PriorityElement
    {
        /// <summary>
        /// Priority of the element. Smaller = higher priority.
        /// </summary>
        public float priority;

        /// <summary>
        /// The element's value.
        /// </summary>
        public T value;

        /// <summary>
        /// Constructs a new priority queue element.
        /// </summary>
        /// <param name="priority">The priority to assign to this.</param>
        /// <param name="value">The element's value.</param>
        public PriorityElement(float priority, T value)
        {
            this.priority = priority;
            this.value = value;
        }

        public override string ToString()
        {
            return $"PQElement: priority {priority}, {value}";
        }
    }

    /// <summary>
    /// Used to locate <see cref="PriorityElement"/>.
    /// </summary>
    private struct LocatorElement : IComparable
    {
        /// <summary>
        /// Element this locator is pointing at.
        /// </summary>
        public PriorityElement pqElement;

        /// <summary>
        /// A counter value to do tie breaks.
        /// </summary>
        private int tieBreaker;

        /// <summary>
        /// The static counter value. See <paramref name="tieBreaker"/>.
        /// </summary>
        private static int tieBreakerCounter = 0;

        /// <summary>
        /// Index of <paramref name="pqElement"/> in <paramref name="data"/>.
        /// </summary>
        public int index;

        /// <summary>
        /// Creates a new locator.
        /// </summary>
        /// <param name="pqElement">The priority queue element this points to.</param>
        /// <param name="index">Index of <paramref name="pqElement"/> in <paramref name="data"/>.</param>
        public LocatorElement(PriorityElement pqElement, int index)
        {
            this.pqElement = pqElement;
            this.index = index;

            tieBreaker = tieBreakerCounter;
            tieBreakerCounter++;
        }

        public int CompareTo(object other)
        {
            if (other == null) return 1;

            var otherLocator = (LocatorElement)other;

            if (pqElement.priority == otherLocator.pqElement.priority)
            {
                return tieBreaker - otherLocator.tieBreaker;
            }
            else
            {
                return Mathf.RoundToInt(otherLocator.pqElement.priority - pqElement.priority);
            }
        }

        public override string ToString()
        {
            return $"LocatorElement: index {index}, {pqElement}";
        }
    }
    #endregion

    #region Variables
    /// <summary>
    /// Underlying container for this queue.
    /// </summary>
    private List<PriorityElement> data;

    /// <summary>
    /// Dictionary to locate the data to update.
    /// </summary>
    private Dictionary<T, LocatorElement> locator;
    #endregion

    #region Properties
    /// <summary>
    /// Objects in this queue.
    /// </summary>
    public int Count => data.Count;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new empty <see cref="PriorityQueue{T}"/>.
    /// </summary>
    public PriorityQueue()
    {
        data = new();
        locator = new();
    }

    /// <summary>
    /// Creates a <see cref="PriorityQueue{T}"/> with the specified elements.
    /// </summary>
    /// <param name="elements">The collection of elements to add.</param>
    public PriorityQueue(IEnumerable<PriorityElement> elements) : this()
    {
        foreach (var elem in elements)
        {
            Enqueue(elem);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Enqueues a value with a priority.
    /// </summary>
    /// <param name="priority">Priority of the value. Smaller = more
    /// priority.</param>
    /// <param name="value">Value to insert.</param>
    public void Enqueue(float priority, T value)
    {
        var element = new PriorityElement(priority, value);
        data.Add(element);
        locator.Add(value, new LocatorElement(element, Count - 1));
        PercolateUp(Count - 1);
    }

    /// <summary>
    /// Enqueues a <see cref="PriorityElement"/> directly.
    /// </summary>
    /// <param name="element"></param>
    public void Enqueue(PriorityElement element)
    {
        data.Add(element);
        locator.Add(element.value, new LocatorElement(element, Count - 1));
        PercolateUp(Count - 1);
    }

    /// <summary>
    /// Returns the element at the front of the queue.
    /// </summary>
    /// <returns></returns>
    public T Peek() => data[0].value;

    /// <summary>
    /// Removes and returns the element and its priority at the front of the
    /// queue.
    /// </summary>
    /// <returns>A tuple of (priority, value).</returns>
    public PriorityElement DequeueElement()
    {
        T value = data[0].value;
        var valueElement = data[0];

        // Get values of the datas.
        var currData = data[0];
        var swapData = data[Count - 1];

        // Get values of the locators so they can be updated later.
        var currLocator = locator[currData.value];
        var swapLocator = locator[swapData.value];
        currLocator.index = Count - 1;  // Ends up in the end of list
        swapLocator.index = 0;          // Ends up in the front of list

        // Swap elements so that the top is now at the bottom
        data[0] = swapData;
        data[Count - 1] = currData;

        // Reassign locators
        locator[currData.value] = currLocator;
        locator[swapData.value] = swapLocator;

        // Pop last element.
        data.RemoveAt(Count - 1);
        locator.Remove(value);

        // Swapped element needs to be correctly ordered.
        PercolateDown(0);

        return valueElement;
    }

    /// <summary>
    /// Removes and returns the element at the front of the queue.
    /// </summary>
    /// <returns>The minimum value.</returns>
    public T DequeueValue()
    {
        return DequeueElement().value;
    }

    /// <summary>
    /// Updates the key.
    /// </summary>
    /// <param name="newPriority">The new priority to assign.</param>
    /// <param name="key">The value to assign the new priority to.</param>
    /// <returns>True if update successful.</returns>
    public bool Update(float newPriority, T key)
    {
        if (!locator.ContainsKey(key))
            return false;

        var currLocator = locator[key];
        var swapLocator = locator[data[0].value];

        var currData = data[currLocator.index];
        var swapData = data[0];
        data[currLocator.index] = swapData;
        data[0] = currData;

        swapLocator.index = currLocator.index;
        currLocator.index = 0;
        locator[key] = currLocator;
        locator[swapData.value] = swapLocator;

        DequeueValue();

        Enqueue(newPriority, currLocator.pqElement.value);

        PercolateDown(0);
        PercolateUp(swapLocator.index);

        return true;
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Returns the left child index of the element at <paramref
    /// name="currentIndex"/>.
    /// </summary>
    /// <param name="currentIndex">Current index of an element.</param>
    /// <returns>The left child index, if it exists. Otherwise throws
    /// IndexOutOfRangeException.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the left child of
    /// the element at <paramref name="currentIndex"/> does not
    /// exist.</exception>
    private int GetLeftChildIndex(int currentIndex)
    {
        int childI = 2 * currentIndex + 1;

        if (childI < Count)
            return childI;
        else
            throw new IndexOutOfRangeException(
                $"Child index {childI} of current index {currentIndex} is out " +
                $"of range of list with length {Count}."
                );
    }

    /// <summary>
    /// Returns the right child index of the element at <paramref
    /// name="currentIndex"/>.
    /// </summary>
    /// <param name="currentIndex">Current index of an element.</param>
    /// <returns>The right child index, if it exists. Otherwise throws
    /// IndexOutOfRangeException.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the right child
    /// of the element at <paramref name="currentIndex"/> does not
    /// exist.</exception>
    private int GetRightChildIndex(int currentIndex)
    {
        int childI = 2 * currentIndex + 2;

        if (childI < Count)
            return childI;
        else
            throw new IndexOutOfRangeException(
                $"Child index {childI} of current index {currentIndex} is out " +
                $"of range of list with length {Count}."
                );
    }

    /// <summary>
    /// Returns the index of the smallest child of the element at <paramref
    /// name="currentIndex"/>.
    /// </summary>
    /// <param name="currentIndex">Current index of an element.</param>
    /// <returns>The smallest child index, if it exists. Otherwise throws
    /// IndexOutOfRangeException.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the smallest
    /// child of the element at <paramref name="currentIndex"/> does not
    /// exist.</exception>
    private int GetMinChildIndex(int currentIndex)
    {
        int childIR = 2 * currentIndex + 2;
        int childIL = childIR - 1;

        if (childIL < Count && childIR < Count)
        {
            return (data[childIL].priority < data[childIR].priority) ? childIL : childIR;
        }
        else if (childIL < Count)
        {
            return childIL;
        }
        else if (childIR < Count)
        {
            return childIR;
        }
        else
        {
            return -1;
            // throw new IndexOutOfRangeException(
            //     $"Child indexes {childIR} and {childIL} of current index {currentIndex} is out " +
            //     $"of range of list with length {Count}."
            //     );
        }
    }

    /// <summary>
    /// Returns the parent index of the element at <paramref name="childIndex"/>.
    /// </summary>
    /// <param name="childIndex">Current index of an element.</param>
    /// <returns>The parent index, if it exists. Otherwise throws IndexOutOfRangeException.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the parent of the element at 
    /// <paramref name="childIndex"/> does not exist.</exception>
    private int GetParentIndex(int childIndex)
    {
        if (childIndex == 0)
            throw new IndexOutOfRangeException(
                $"Parent index {0} of child index {childIndex} is out " +
                $"of range of list with length {Count}."
                );

        int parentI = Mathf.FloorToInt((childIndex - 1) / 2);

        if (parentI < Count && 0 <= parentI)
            return parentI;
        else
            throw new IndexOutOfRangeException(
                $"Parent index {parentI} of child index {childIndex} is out " +
                $"of range of list with length {Count}."
                );
    }

    /// <summary>
    /// Percolates up the value at index to its valid spot in the heap.
    /// </summary>
    /// <param name="index">Index to start the percolation at.</param>
    private void PercolateUp(int index)
    {
        while (index > 0 && index < Count)
        {
            var parentI = GetParentIndex(index);

            if (parentI == index)
                break;

            if (data[index].priority < data[parentI].priority)
            {
                // Parent's child has more priority, Swap elements.
                (data[parentI], data[index]) = (data[index], data[parentI]);

                // Update locators.
                var parentLocator = locator[data[parentI].value];
                var currentLocator = locator[data[index].value];
                parentLocator.index = parentI;
                currentLocator.index = index;
                locator[data[parentI].value] = parentLocator;
                locator[data[index].value] = currentLocator;
            }

            index = parentI;
        }
    }

    /// <summary>
    /// Percolates down the value at index to its valid spot in the heap.
    /// </summary>
    /// <param name="index">Index to start the percolation at.</param>
    private void PercolateDown(int index)
    {
        while (index >= 0 && index < Count)
        {
            var childI = GetMinChildIndex(index);

            if (childI >= 0 && data[childI].priority < data[index].priority)
            {
                // Child has more priority. Swap elements.
                (data[childI], data[index]) = (data[index], data[childI]);

                // Update locators.
                var childLocator = locator[data[childI].value];
                var currentLocator = locator[data[index].value];
                childLocator.index = childI;
                currentLocator.index = index;
                locator[data[childI].value] = childLocator;
                locator[data[index].value] = currentLocator;
            }

            index = childI;
        }
    }
    #endregion
}