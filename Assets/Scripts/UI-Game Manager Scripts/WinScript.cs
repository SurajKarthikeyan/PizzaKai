using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject winCanvas;
    public List<GameObject> enemiesToDie;

    private void LateUpdate()
    {
        for (int i=0; i < enemiesToDie.Count; i++)
        {
            if (enemiesToDie[i] == null)
            {
                enemiesToDie.RemoveAt(i);
            }
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enemiesToDie.Count != 0) return;

        if (collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            winCanvas.SetActive(true);
        }
    }
}
