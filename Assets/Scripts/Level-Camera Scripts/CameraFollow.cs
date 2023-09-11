using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //camera follow variables
    private BoxCollider2D cameraBox; //the camera's box collider
    private Transform player; //player's position
    private float cameraHeight;
    private float cameraWidth;

    //camera shake variables
    private float shakeDuration = 0.25f;
    public bool startShake = false;
    public float shakeStrength;

    private Vector3 newCamPos;

    // Start is called before the first frame update
    private void Start()
    {
        cameraBox = GetComponent<BoxCollider2D>();
        //gets the transform of the player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        EventManager.Instance.onCameraShake.AddListener(Shake);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    //AspectRatioBoxChange();
    //    //FollowPlayer();

    //    if (startShake)
    //    {
    //        StartCoroutine(Shake());
    //    }

       
    //}

    //void AspectRatioBoxChange() //changes the camera's collider based on the aspect ratio
    //{
    //    cameraHeight = (Camera.main.orthographicSize * 2) + 0.1f;
    //    cameraWidth = (Camera.main.orthographicSize * Camera.main.aspect * 2) + 0.1f;
    //    cameraBox.size = new Vector2(cameraWidth, cameraHeight);
        
    //}

    //void FollowPlayer()
    //{
    //    //Think of a better way to find boundaries, find is kinda slow
    //    if (GameObject.Find("Boundary"))
    //    {
    //        //changes the camera's position depending on where the player is in the boundary box
    //        transform.position = new Vector3(Mathf.Clamp(player.position.x, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.x + cameraBox.size.x / 2, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.x - cameraBox.size.x / 2),
    //                                         Mathf.Clamp(player.position.y, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.min.y + cameraBox.size.y / 2, GameObject.Find("Boundary").GetComponent<BoxCollider2D>().bounds.max.y - cameraBox.size.y / 2),
    //                                         transform.position.z);
    //    }
    //}


    public void Shake(AnimationCurve strength)
    {
        StartCoroutine(Shake_CR());
    }

    public IEnumerator Shake_CR()
    {
        //gets the current position so that it can return to it later
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            newCamPos = startPosition + Random.insideUnitSphere * shakeStrength;
            transform.position = new Vector3(newCamPos.x, newCamPos.y, startPosition.z);
            yield return new WaitForEndOfFrame();
        }

        transform.position = startPosition;
    }
}
