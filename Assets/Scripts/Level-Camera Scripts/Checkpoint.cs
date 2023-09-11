using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    #region Variables
    [Header("Animation")]
    public Animator checkpointAnim;

    [AnimatorParam(nameof(checkpointAnim), AnimatorControllerParameterType.Bool)]
    public string animActivateBool;

    private RespawnModule associatedRespawner;

    #endregion

    #region Properties
    public RespawnModule AssociatedRespawner
    {
        get => associatedRespawner;
        set
        {
            associatedRespawner = value;

            if (checkpointAnim && !string.IsNullOrEmpty(animActivateBool))
            {
                checkpointAnim.SetBool(animActivateBool, value != null);
            }
        }
    }
    #endregion
}
