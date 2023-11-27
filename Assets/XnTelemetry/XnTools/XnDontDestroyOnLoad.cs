using UnityEngine;

public class XnDontDestroyOnLoad : MonoBehaviour {
    void Awake() {
        DontDestroyOnLoad( this.gameObject );
    }
}