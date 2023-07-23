using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject winCanvas;
    public List<GameObject> enemiesToDie;

    private void LateUpdate()
    {
        // Removes all enemies that are null or have been destroyed. It's
        // probably sufficient to just use enemy => !enemy, but this way makes
        // it clearer.
        enemiesToDie.RemoveAll(enemy => enemy.IsNullOrUnityNull());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && enemiesToDie.Count <= 0)
        {
            Time.timeScale = 0;
            winCanvas.SetActive(true);
        }
    }
}
