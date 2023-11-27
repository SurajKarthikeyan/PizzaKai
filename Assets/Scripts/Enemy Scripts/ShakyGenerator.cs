using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakyGenerator : MonoBehaviour
{
    [Tooltip("The amount of distance from the normal location that the shaking can occur")]
    public float shakeAmount = 0.05f;

    Vector2 normalPos;
    Vector2 shakyPos;

    bool doShake;
    float shakeDuration;

    private void Start()
    {
        normalPos = gameObject.transform.position;
    }
    void FixedUpdate()
    {
        if (doShake)
        {//randomize x and y values 
            shakyPos = new Vector2(Random.Range(normalPos.x -shakeAmount,normalPos.x + shakeAmount), Random.Range(normalPos.y -shakeAmount,normalPos.y + shakeAmount));

            gameObject.transform.position = shakyPos;
        }
    }

    //this is what would be called from other scripts
    public void startShaking(float duration)
    {
        shakeDuration = duration;
        StartCoroutine("Shaking");
       
    }

    private IEnumerator Shaking()
    {
        doShake = true;

        yield return new WaitForSeconds(shakeDuration);

        doShake = false;
        gameObject.transform.position = normalPos;
    }

}
