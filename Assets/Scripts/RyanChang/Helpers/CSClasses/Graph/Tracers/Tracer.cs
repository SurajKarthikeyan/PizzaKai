using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// A unified tracer class responsible for creating graph and path
/// visualizations.
///
/// <br/>
///
/// Authors: Ryan Chang (2023)
/// </summary>
/// <typeparam name="T">The vertex type of the graph or path.</typeparam>
[System.Serializable]
public class Tracer<T> where T : IEquatable<T>
{
    #region Delegate
    public delegate Vector3 ConvertToVector3(Vertex<T> vertex);
    #endregion

    #region Variables
    public GraphPathTracerData tracerData;
    public ConvertToVector3 converter;

    /// <summary>
    /// The thing being used to generate the visuals.
    /// </summary>
    public ITraceable<T> target;

    [SerializeField]
    private Transform traceContainer;
    #endregion

    #region Instantiation
    /// <summary>
    /// Traces the tracer.
    /// </summary>
    /// <param name="target">What to trace?</param>
    /// <param name="traceContainer">Where to put the traces.</param>
    /// <param name="converter">Converts from <see cref="T"/> to a <see
    /// cref="Vector3"/>.</param>
    public void Trace(ITraceable<T> target, ConvertToVector3 converter)
    {
        this.converter = converter;
        this.target = target;

        ValidateFields();
        Clear();

        Transform subcontainer = new GameObject(
            DateTime.Now.ToLongDateTimeString()
        ).transform;

        float maxWeight = 0;

        using (var edgeEnumerator = target.GetTraces())
        {
            while (edgeEnumerator.MoveNext())
            {
                maxWeight = Mathf.Max(edgeEnumerator.Current.weight, maxWeight);
            }
        }

        using (var edgeEnumerator = target.GetTraces())
        {
            while (edgeEnumerator.MoveNext())
            {
                BuildRenderers(subcontainer, edgeEnumerator.Current, maxWeight);
            }
        }

        subcontainer.transform.SetParent(traceContainer);
        subcontainer.transform.Localize();
    }

    public void Clear()
    {
        if (traceContainer)
        {
            traceContainer.DestroyAllChildren(true);
        }
    }

    #endregion

    #region Helpers
    protected void BuildRenderers(Transform traceContainer,
        GraphEdge<T> edge,
        float maxWeight)
    {
        maxWeight = Mathf.Max(maxWeight, 1);

        LineRenderer line = tracerData.tracerRenderer.InstantiateComponent(
            Vector3.zero, Quaternion.identity,
            traceContainer
        );
        line.useWorldSpace = true;
        line.transform.Localize();

        float percent = edge.weight / maxWeight;
        percent = Mathf.Clamp01(percent);
        Color selectedColor = tracerData.tracerGradient.Evaluate(
            percent
        );
        selectedColor.a = 0.8f;

        line.startColor = selectedColor;
        line.endColor = selectedColor;

        Vector3 fromVec = converter(edge.from);
        Vector3 toVec = converter(edge.to);

        Vector3 disp = toVec - fromVec;
        disp = new(disp.y, disp.x);
        disp.Normalize();
        disp *= 0.1f;

        fromVec += disp;
        toVec += disp;

        Vector3[] renderPositions = { fromVec, toVec };
        line.SetPositions(renderPositions);
        line.enabled = true;
    }

    protected void ValidateFields()
    {
        tracerData.LoadIfMissing("Pathfinding/Visual/DefaultTracerData");
    }
    #endregion
}