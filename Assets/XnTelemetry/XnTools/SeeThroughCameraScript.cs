using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XnTools;


public class SeeThroughCameraScript : MonoBehaviour {
    static public float FOV2;
    static public Dictionary<GameObject, int> ORIGINAL_LAYERS = new Dictionary<GameObject, int>();
    static public List<GameObject> OLD_HITS = new List<GameObject>();
    static public List<GameObject> NEW_HITS = new List<GameObject>();

    const int PLAYER_LAYER_NUM = 6;
    const int ALWAYS_OPAQUE_LAYER_NUM = 18;
    const int SEE_THROUGH_LAYER_NUM = 19;

    [Tooltip("If you're having performance issues when the see-through camera activates, you can try turning this off.")]
    public bool seeThroughEnabled = true;
    public Camera mainCamera, seeThroughCamera;
    private GameObject _seeThroughCameraGO;
    public GameObject seeThroughQuad;
    public Transform poi;
    public float distReduction = 0.5f;
    public bool showDebugLines = false;
    [Tooltip("Even though this is a LayerMask, it is only allowed to be set to a single layer.")]
    public LayerMask seeThroughLayer = 1 << SEE_THROUGH_LAYER_NUM; // SeeThrough
    private LayerMask _seeThroughLayerOld = 1 << SEE_THROUGH_LAYER_NUM; 
    public LayerMask alwaysOpaqueLayers = (1 << PLAYER_LAYER_NUM ) | (1 << ALWAYS_OPAQUE_LAYER_NUM ); // Player and AlwaysOpaque

    private Vector3[] dirs;
    Transform camTrans;

    void Start() {
        FOV2 = mainCamera.fieldOfView / 4f;
        camTrans = mainCamera.transform;

        dirs = new Vector3[4];
        dirs[0] = Vector3.forward;
        dirs[1] = Quaternion.Euler(-FOV2, 0, 0 ) * Vector3.forward; // up
        dirs[2] = Quaternion.Euler( 0, FOV2, 0 ) * Vector3.forward; // right
        dirs[3] = Quaternion.Euler( 0,-FOV2, 0 ) * Vector3.forward; // left
        //dirs[1] = Quaternion.Euler( FOV2, 0, 0 ) * Vector3.forward;

        _seeThroughCameraGO = seeThroughCamera.gameObject;
        SetSeeThroughActive( false, true );
    }

    private void OnValidate() {
        if ( seeThroughLayer.HammingWeight() > 1 ) {
            seeThroughLayer = seeThroughLayer ^ _seeThroughLayerOld;
        }
        _seeThroughLayerOld = seeThroughLayer;
    }


    void Update() {
        OLD_HITS = NEW_HITS;
        NEW_HITS = new List<GameObject>();
        // Look at the poi
        camTrans.LookAt( poi.position, Vector3.up );
        // Get dir and dist to poi
        Vector3 poiDelta = poi.position - camTrans.position;
        float poiDist = poiDelta.magnitude - distReduction;
        Vector3 poiDir = poiDelta / poiDist;

        Vector3 localDir;
        RaycastHit[] hits;
        GameObject hitGO;
        foreach (Vector3 dir in dirs) {
            localDir = camTrans.TransformDirection( dir );
            hits = Physics.RaycastAll( camTrans.position, localDir, poiDist, ~alwaysOpaqueLayers );
            foreach (RaycastHit hit in hits) {
                hitGO = hit.collider.gameObject;
                if ( !NEW_HITS.Contains( hitGO ) ) NEW_HITS.Add( hitGO );
                if (!ORIGINAL_LAYERS.ContainsKey(hitGO)) {
                    ORIGINAL_LAYERS.Add( hitGO, hitGO.layer );
                }
            }
            if (showDebugLines) {
                Debug.DrawLine( camTrans.position, camTrans.position + localDir * poiDist, Color.cyan );
            }
        }
        List<GameObject> addedGOs = NEW_HITS.Except( OLD_HITS ).ToList();
        foreach (GameObject go in addedGOs) {
            if ( go == null ) continue;
            go.layer = SEE_THROUGH_LAYER_NUM;
        }
        List<GameObject> removedGOs = OLD_HITS.Except( NEW_HITS ).ToList();
        foreach ( GameObject go in removedGOs ) {
            if ( go == null ) continue;
            go.layer = ORIGINAL_LAYERS[go];
        }

        seeThroughActive = ( NEW_HITS.Count > 0 );
    }

    private bool _seeThroughActive = true;
    public bool seeThroughActive {
        get { return _seeThroughActive; }
        set { SetSeeThroughActive( value ); }
    }

    void SetSeeThroughActive( bool b, bool requireToSet = false) {
        if (requireToSet || _seeThroughActive != b ) {
            _seeThroughActive = b;
            _seeThroughCameraGO.SetActive( b );
            seeThroughQuad.SetActive( b );
        }
    }
}
