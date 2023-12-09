using NaughtyAttributes;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    #region Variables
    [Tooltip("The collider trigger for the checkpoint.")]
    [SerializeField]
    [ReadOnly]
    private Collider2D trigger;

    [Tooltip("The animator for the checkpoint.")]
    [SerializeField]
    [ReadOnly]
    private Animator flagAnimator;

    [SerializeField]
    [AnimatorParam(nameof(flagAnimator), AnimatorControllerParameterType.Bool)]
    private string flagActiveAnimKey = "Activated";

    [SerializeField] private AudioSource checkpointAudioSource;
    #endregion

    #region Instantiation
    private void Awake()
    {
        SetVars();
    }

    private void OnValidate()
    {
        SetVars();
    }

    private void SetVars()
    {
        this.RequireComponent(out flagAnimator);
        this.RequireComponent(out trigger);
        trigger.isTrigger = true;
    }
    #endregion

    #region Main Behavior
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.HasComponentInChildren(out RespawnModule respawn))
        {
            AudioDictionary.aDict.PlayAudioClipRemote("checkpointFlag", checkpointAudioSource);
            flagAnimator.SetBool(flagActiveAnimKey, true);
            respawn.lastRespawnPoint = transform;
            trigger.enabled = false;
        }
    } 
    #endregion
}
