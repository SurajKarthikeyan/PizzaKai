using UnityEngine;

public class ForkyOven : MonoBehaviour
{
    #region Vars

    [SerializeField] private Generator[] generators;
    [SerializeField] private Alarm alarm;

    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the player enters the oven, kill the player
        if(collision.gameObject.tag == "Player")
        {
            //There doesnt appear to be a way to get player maxhealth so I just picked a big number
            collision.gameObject.GetComponent<Character>().TakeDamage(999999);
        }
        else
        {

            if(collision.gameObject.GetComponent<OilBarrel>() != null)
            {
                foreach(Generator generator in generators)
                {
                    if(generator != null)
                    {
                        alarm.AlarmSystem();
                        generator.SetVulnerability();
                    }
                }
            }
            Destroy(collision.gameObject);
        }

    }
}
