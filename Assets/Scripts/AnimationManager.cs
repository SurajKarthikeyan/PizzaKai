using System.Collections;
using UnityEngine;

/// <summary>
/// System to handle and manage all animation triggers and calls
/// Can be expanded to handle other animations and animation controllers
/// </summary>
public class AnimationManager : MonoBehaviour
{
    #region Vars

    public static AnimationManager animManager;
    #endregion


    #region UnityMethods

    void Start()
    {
        if (animManager == null)
        {
            animManager = this;
        }
        else
        {
            Debug.LogError("You done goofed and made two AnimationManagers in the scene");
            // Like seriously, how do you manage this?
        }
    }

    #endregion



    #region PrivateMethods
    

    #endregion


    #region CoroutineAnimation

    //Single Coroutine that takes in the wait time along with the name of the boolean animation to flip its bool
    // Function meant for boolean animations that want a specific end time but have no way of triggering that end time outside of time passed
    // If there is another way, use that method as coroutines are not very efficient relative other aspects in unity, esspecialyl the physics engine
    // That being said, if time is the only method of ending an animation that use the below coroutine ( if its a boolean, if its a trigger then it should end itself)

    IEnumerator BooleanAnimation(float waitTime, string animationName, bool face)
    {
        if (face)
        {
            //faceAnimator.SetBool(animationName, true);
            yield return new WaitForSeconds(waitTime);
            //faceAnimator.SetBool(animationName, false);
        }

        else
        {
            //bodyAnimator.SetBool(animationName, true);
            yield return new WaitForSeconds(waitTime);
            //bodyAnimator.SetBool(animationName, false);
        }
    }

    #endregion



}

