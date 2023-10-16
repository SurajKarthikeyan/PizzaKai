using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class AITargeting
{
    #region Flags
    public enum Flags
    {
        Nothing,
        TargetPoint,
        TargetTransform,
        TargetPlayer,
    }
    #endregion

    #region variables
    public Flags flags;

    [AllowNesting]
    [ShowIf(nameof(flags), Flags.TargetPoint)]
    public Vector3 targetPoint;

    [AllowNesting]
    [ShowIf(nameof(flags), Flags.TargetTransform)]
    public Transform targetTransform;
    #endregion

    public TargetToken GetTarget()
    {
        return flags switch
        {
            Flags.TargetPlayer => new(GameManager.Instance.Player.transform),
            Flags.TargetTransform => new(targetTransform),
            Flags.TargetPoint => new(targetPoint),
            _ => null
        };
    }

    public void InitializeTargeting(EnemyControlModule enemy)
    {
        Debug.Log("");
    }
}