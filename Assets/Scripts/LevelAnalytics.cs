using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAnalytics : MonoBehaviour
{
    public WeaponMasterModule WMM;
    public WeaponModule currentWeapon;
    public bool levelCompleted = false;
    public string levelName;
    private float startTime, endTime, totalTime;
    private float tommyTime, shotgunTime, flamethrowerTime, sniperTime;

    // Start is called before the first frame update
    void Start()
    {
        levelName = SceneManager.GetActiveScene().name;
        startTime = Time.fixedTime;
        WMM = FindObjectOfType<WeaponMasterModule>();
        currentWeapon = WMM.CurrentWeapon;
    }

    private void FixedUpdate()
    {
        currentWeapon = WMM.CurrentWeapon;
        switch (currentWeapon)
        {
            case TommyGunWeapon:
                tommyTime += Time.fixedDeltaTime;
                break;
            case ShotGunWeapon:
                shotgunTime += Time.fixedDeltaTime;
                break;
            case FlameThrowerWeapon:
                flamethrowerTime += Time.fixedDeltaTime;
                break;
            //case Sniper
            //sniperTime += Time.fixedDeltaTime;
            //case future weapons
        }
    }

    //this function ideally would only be called when the player leaves the scene either through the pause menu or through finishing the level
    public void PostLevelAnalytics()
    {
        endTime = Time.fixedTime;
        totalTime = endTime - startTime;

        WWWForm form = new WWWForm();

        form.AddField("Level Name", levelName);
        form.AddField("Time Spent (s)", $"{totalTime:0.00}" );
        form.AddField("Level Finished?", levelCompleted.ToString());
        form.AddField("Tommygun use Time (s)", $"{tommyTime:0.00}");
        form.AddField("Shotgun use Time (s)", $"{shotgunTime:0.00}");
        form.AddField("Flamethrower use Time (s)", $"{flamethrowerTime:0.00}");
        form.AddField("Sniper use Time (s)", $"{sniperTime:0.00}");

        XnAnalytics.POST(form);
    }
}
