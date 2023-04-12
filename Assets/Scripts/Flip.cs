using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public Vector3 newScale;
    public float newScaleX;
    public PlayerMovement player;

    

    public Shotgun shotgun;
    public TommyGun tommy;
    public Flamethrower flamethrower;

    public GameObject tommyLeft;
    public GameObject tommyRight;

    public GameObject tommyFirepointRight;
    public GameObject tommyFirepointLeft;

    public GameObject shotgunLeft;
    public GameObject shotgunRight;
    
    public GameObject shotgunFirepointRight;
    public GameObject shotgunFirepointLeft;

    public GameObject flameLeft;
    public GameObject flameRight;

    public GameObject flameFirepointRight;
    public GameObject flameFirepointLeft;

    public UICode UIScript;

    

    public float rotZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        newScale = transform.localScale;
        newScaleX = newScale.x;
        FlipSprite();

        if (UIScript.tommy.activeSelf == true)
        {
            rotZ = tommy.gunScriptableObject.rotZ;
        }

        if (UIScript.shotgun.activeSelf == true)
        {
            //Change to shotgunscriptable object
            rotZ = shotgun.gunScriptableObject.rotZ;
        }

        if (UIScript.flamethrower.activeSelf == true)
        {
            //Change to flamethrowerscriptable object
            rotZ = flamethrower.gunScriptableObject.rotZ;
        }

    }

    public void FlipSprite()
    {
        if ((player.facingRight && (rotZ > 90 || rotZ < -90)) || (!player.facingRight && (-90 < rotZ && rotZ < 90)))
        {
            player.facingRight = !player.facingRight;
            // Vector3 localScale = transform.localScale;
            newScale.x *= -1f;
            transform.localScale = newScale;

            if (tommyLeft.activeSelf == true)
            {
                UIScript.tommy.transform.position = new Vector3(UIScript.tommy.transform.position.x - .864f, UIScript.tommy.transform.position.y - .027f, 0f);

                tommyLeft.SetActive(false);
                tommyRight.SetActive(true);
                tommyFirepointLeft.SetActive(false);
                tommyFirepointRight.SetActive(true);
            }
            else if (tommyRight.activeSelf == true)
            {
                UIScript.tommy.transform.position = new Vector3(UIScript.tommy.transform.position.x + .864f, UIScript.tommy.transform.position.y + .027f, 0f);
                tommyRight.SetActive(false);
                tommyLeft.SetActive(true);
                tommyFirepointLeft.SetActive(true);
                tommyFirepointRight.SetActive(false);
            }

            if (shotgunLeft.activeSelf == true)
            {
                UIScript.shotgun.transform.position = new Vector3(UIScript.shotgun.transform.position.x - .864f, UIScript.shotgun.transform.position.y - .027f, 0f);
                
                shotgunLeft.SetActive(false);
                shotgunRight.SetActive(true);
                shotgunFirepointLeft.SetActive(false);
                shotgunFirepointRight.SetActive(true);

            }
            else if (shotgunRight.activeSelf == true)
            {
                UIScript.shotgun.transform.position = new Vector3(UIScript.shotgun.transform.position.x + .864f, UIScript.shotgun.transform.position.y + .027f, 0f);
                
                shotgunRight.SetActive(false);
                shotgunLeft.SetActive(true);
                shotgunFirepointLeft.SetActive(true);
                shotgunFirepointRight.SetActive(false);
            }

            if (flameLeft.activeSelf == true)
            {
                UIScript.flamethrower.transform.position = new Vector3(UIScript.flamethrower.transform.position.x - .864f, UIScript.flamethrower.transform.position.y - .027f, 0f);

                flameLeft.SetActive(false);
                flameRight.SetActive(true);
                flameFirepointLeft.SetActive(false);
                flameFirepointRight.SetActive(true);

            }
            else if (flameRight.activeSelf == true)
            {
                UIScript.flamethrower.transform.position = new Vector3(UIScript.flamethrower.transform.position.x + .864f, UIScript.flamethrower.transform.position.y + .027f, 0f);

                flameRight.SetActive(false);
                flameLeft.SetActive(true);
                flameFirepointLeft.SetActive(true);
                flameFirepointRight.SetActive(false);
            }
        }
    }
}
