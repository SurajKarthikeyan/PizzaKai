using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject winCanvas;
    public List<GameObject> enemiesToDie;

    private void Start()
    {
        EventManager.Instance.onCharacterDeath.AddListener(CharacterDeath);
    }

    private void CharacterDeath(Character character)
    {
        enemiesToDie.Remove(character.gameObject);
    }

    private void LateUpdate()
    {
        enemiesToDie = enemiesToDie
            .Where(e => e)      // Null check
            .ToList();
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
