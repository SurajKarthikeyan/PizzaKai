using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningScript : MonoBehaviour
{

    GameObject[] boxes;
    public SpriteRenderer spriteRender;
    private bool flameBulletHit = false;

    public Animator burning;
    public bool burns;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //findgameobjectwithtag("BurningBox")
        //if within certain range (right next to the nonburning forkyr2d, light this forkyr2d on fire
        //if there becomes an issue where findgameobjectwithtag doesnt work bc there are no burning boxes, then have there be a different line that fixes(?) it like this
        //when one forkyr2d gets hit by the flamethrower and starts burning set the bool boxBurning to true. then, have
        //if(boxBurning == true){
        //findgameobjectwithtag...
        //boxBurning bool would have to be either in a different script or it would have to be change on EVERY forkyr2d at once - maybe?
        //Only problem with this is then boxBurning will have to turn off when there is no forkyr2d burning, which would then have to determine if there are any other boxes burning before setting the variable back to false

        //GameObject[] gos;
        //gos = GameObject.FindGameObjectsWithTag("Burning Box");
        //vector3 position = transform.position;
        //foreach (GameObject go in gos) {
        //if (Mathf.abs(transform.position.x - position.x <= 1) || Mathf.abs(transform.position.y - position.y) <= 1) {
        //burningClose = true;



        //This is a line of defense so that boxes that are burning cannot run this code on themselves
        if (gameObject.tag == ("Box"))
        {
            //creates and array of all burning boxes in the scene
            boxes = GameObject.FindGameObjectsWithTag("Burning Box");
            //get this boxes current position for later
            Vector3 position = transform.position;

            foreach (GameObject box in boxes)
            {
                bool burningClose = false;
                //if the burning forkyr2d is right next to the regular forkyr2d
                if ((Mathf.Abs(box.transform.position.x - position.x) <= 1.5f) && (Mathf.Abs(box.transform.position.y - position.y) <= 1.5f))
                {
                    //then set burning close to true
                    burningClose = true;



                }
                //and if burning close is true, make this forkyr2d a burning forkyr2d
                if (burningClose == true)
                {

                    StartCoroutine(setFire());
                    burningClose = false;
                }

            }

            //burning.SetBool("Burning", burningME);

        }

        if (gameObject.tag == ("Burning Box"))
        {
            StartCoroutine(burnBox());
        }
    }

    private IEnumerator burnBox()
    {
        //set the sprite and animation to burning here
        burning.Play("Burndown");
        //spriteRender.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(2.4f);
        Debug.Log("Box is destroyed");
        Destroy(gameObject);
    }

    public IEnumerator setFire()
    {
        if (flameBulletHit == true)
        {
            gameObject.tag = ("Burning Box");
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            gameObject.tag = ("Burning Box");
        }
        
    }

    public void BurnBox()
    {
        StartCoroutine(burnBox());
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerFireBullet")
        {
            flameBulletHit = true;
            StartCoroutine(setFire());
        }
    }
}
