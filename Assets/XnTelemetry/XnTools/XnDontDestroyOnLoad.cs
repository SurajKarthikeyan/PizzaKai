using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XnDontDestroyOnLoad : MonoBehaviour {
    void Awake() {
        DontDestroyOnLoad( this.gameObject );
    }
}