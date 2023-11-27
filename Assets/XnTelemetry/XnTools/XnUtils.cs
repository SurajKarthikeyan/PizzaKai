using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Array = System.Array;

namespace XnTools {

	// These are actually OUTSIDE of the Utils Class

	// TODO: Move Easing and XnHexGrid into their own classes. - 2022-06-28 JGB
	// TODO: Create XnTools namespace - 2022-06-28 JGB
	// TODO: Replace Bezier methods with much faster array versions

	public enum BoundsTest {
		center,		// Is the center of the GameObject on screen
		onScreen,	// Are the bounds entirely on screen
		offScreen	// Are the bounds entirely off screen
	}


	//============================ Reusable Generic Delegates ============================


	public class XnUtils : MonoBehaviour {
		static public bool DEBUG = true;

		// Returns the maximum value for a Vector3, which can be used to return a unique, identifiable Vector3 value
		static public Vector3 maxVector3 {
			get { return( new Vector3(float.MaxValue, float.MaxValue, float.MaxValue) ); }
		}

		
		// ====================== Utils Consts ======================
		
		static public Color		colClearBlack = new Color(0,0,0,0);
		static public Color 	colClearWhite = new Color(1,1,1,0);

	#region Bounds Functions
	//============================ Bounds Functions ============================\
		
		// Creates bounds that encapsulate the two Bounds passed in.
		public static Bounds BoundsUnion( Bounds b0, Bounds b1 ) {
			// If the size of one of the bounds is Vector3.zero, ignore that one
			if ( b0.size==Vector3.zero && b1.size!=Vector3.zero ) {
				return( b1 );
			} else if ( b0.size!=Vector3.zero && b1.size==Vector3.zero ) {
				return( b0 );
			} else if ( b0.size==Vector3.zero && b1.size==Vector3.zero ) {
				return( b0 );
			}
			// Stretch b0 to include the b1.min and b1.max
			b0.Encapsulate(b1.min);
			b0.Encapsulate(b1.max);
			return( b0 );
		}
		
		public static Bounds CombineBoundsOfChildren(GameObject go) {
			// Create an empty Bounds b
			Bounds b = new Bounds(Vector3.zero, Vector3.zero);
			// If this GameObject has a Renderer Component...
			if (go.GetComponent<Renderer>() != null) {
				// Expand b to contain the Renderer's Bounds
				b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
			}
			// If this GameObject has a Collider Component...
			if (go.GetComponent<Collider>() != null) {
				// Expand b to contain the Collider's Bounds
				b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
			}
			// Iterate through each child of this gameObject.transform
			foreach( Transform t in go.transform ) {
				// Expand b to contain their Bounds as well
				b = BoundsUnion( b, CombineBoundsOfChildren( t.gameObject ) );
			}
			
			return( b );
		}
		
		// Make a static read-only public property camBounds
		static public Bounds camBounds {
			get {
				// if _camBounds hasn't been set yet
				if (_camBounds.size == Vector3.zero) {
					// SetCameraBounds using the default Camera
					SetCameraBounds();
				}
				return( _camBounds );
			}
		}
		// This is the private static field that camBounds uses
		static private Bounds _camBounds;
		
		public static void SetCameraBounds(Camera cam=null) {
			// If no Camera was passed in, use the main Camera
			if (cam == null) cam = XnUtils.mainCamera;
			// This makes a couple important assumptions about the camera!:
			//   1. The camera is Orthographic
			//   2. The camera is at a rotation of R:[0,0,0]
			
			// Make Vector3s at the topLeft and bottomRight of the Screen coords
			Vector3 topLeft = new Vector3( 0, 0, 0 );
			Vector3 bottomRight = new Vector3( Screen.width, Screen.height, 0 );
			
			// Convert these to world coordinates
			Vector3 boundTLN = cam.ScreenToWorldPoint( topLeft );
			Vector3 boundBRF = cam.ScreenToWorldPoint( bottomRight );
			
			// Adjust the z to be at the near and far Camera clipping planes
			boundTLN.z += cam.nearClipPlane;
			boundBRF.z += cam.farClipPlane;
			
			// Find the center of the Bounds
			Vector3 center = (boundTLN + boundBRF)/2f;
			_camBounds = new Bounds( center, Vector3.zero );
			// Expand _camBounds to encapsulate the extents.
			_camBounds.Encapsulate( boundTLN );
			_camBounds.Encapsulate( boundBRF );
		}


	    static Camera S_mainCamera;
	    static public Camera mainCamera {
	        get {
	            if (S_mainCamera == null) {
	                S_mainCamera = Camera.main;
	            }
	            return S_mainCamera;
	        }
	    }




		// Get the location of the mouse in World coordinates (at z=0)
	//	static public Vector3 mouseLoc {
	//		get {
	//			Vector3 loc = Input.mousePosition;
	//			loc.z = -Utils.mainCamera.transform.position.z;
	//			loc = Utils.mainCamera.ScreenToWorldPoint(loc);
	//			return(loc);
	//		}
	//	}
		static public Vector3 MouseLoc {
			get {
				return(mouseLoc);
			}
		}

		static public Ray mouseRay {
			get {
				Vector3 loc = Input.mousePosition;
				Ray r = XnUtils.mainCamera.ScreenPointToRay(loc);
				return( r );
			}
		}
		static public Ray MouseRay {
			get { return( mouseRay ); }
		}
		
		
		
		// Test to see whether Bounds are on screen.
		public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center) {
			// Call the more generic BoundsInBoundsCheck with camBounds as bigB
			return( BoundsInBoundsCheck( camBounds, bnd, test ) );
		}
		
		// Tests to see whether lilB is inside bigB
		public static Vector3 BoundsInBoundsCheck( Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen ) {
			// Get the center of lilB
			Vector3 pos = lilB.center;
			
			// Initialize the offset at [0,0,0]
			Vector3 off = Vector3.zero;
			
			switch (test) {			
	// The center test determines what off (offset) would have to be applied to lilB to move its center back inside bigB
			case BoundsTest.center:
				// if the center is contained, return Vector3.zero
				if ( bigB.Contains( pos ) ) {
					return( Vector3.zero );
				}
				// if not contained, find the offset
				if (pos.x > bigB.max.x) {
					off.x = pos.x - bigB.max.x;
				} else  if (pos.x < bigB.min.x) {
					off.x = pos.x - bigB.min.x;
				}
				if (pos.y > bigB.max.y) {
					off.y = pos.y - bigB.max.y;
				} else  if (pos.y < bigB.min.y) {
					off.y = pos.y - bigB.min.y;
				}
				if (pos.z > bigB.max.z) {
					off.z = pos.z - bigB.max.z;
				} else  if (pos.z < bigB.min.z) {
					off.z = pos.z - bigB.min.z;
				}
				return( off );
				
	// The onScreen test determines what off would have to be applied to keep all of lilB inside bigB
			case BoundsTest.onScreen:
				// find whether bigB contains all of lilB
				if ( bigB.Contains( lilB.min ) && bigB.Contains( lilB.max ) ) {
					return( Vector3.zero );
				}
				// if not, find the offset
				if (lilB.max.x > bigB.max.x) {
					off.x = lilB.max.x - bigB.max.x;
				} else  if (lilB.min.x < bigB.min.x) {
					off.x = lilB.min.x - bigB.min.x;
				}
				if (lilB.max.y > bigB.max.y) {
					off.y = lilB.max.y - bigB.max.y;
				} else  if (lilB.min.y < bigB.min.y) {
					off.y = lilB.min.y - bigB.min.y;
				}
				if (lilB.max.z > bigB.max.z) {
					off.z = lilB.max.z - bigB.max.z;
				} else  if (lilB.min.z < bigB.min.z) {
					off.z = lilB.min.z - bigB.min.z;
				}
				return( off );
				
	// The offScreen test determines what off would need to be applied to move any tiny part of lilB inside of bigB
			case BoundsTest.offScreen:
				// find whether bigB contains any of lilB
				bool cMin = bigB.Contains( lilB.min );
				bool cMax = bigB.Contains( lilB.max );
				if ( cMin || cMax ) {
					return( Vector3.zero );
				}
				// if not, find the offset
				if (lilB.min.x > bigB.max.x) {
					off.x = lilB.min.x - bigB.max.x;
				} else  if (lilB.max.x < bigB.min.x) {
					off.x = lilB.max.x - bigB.min.x;
				}
				if (lilB.min.y > bigB.max.y) {
					off.y = lilB.min.y - bigB.max.y;
				} else  if (lilB.max.y < bigB.min.y) {
					off.y = lilB.max.y - bigB.min.y;
				}
				if (lilB.min.z > bigB.max.z) {
					off.z = lilB.min.z - bigB.max.z;
				} else  if (lilB.max.z < bigB.min.z) {
					off.z = lilB.max.z - bigB.min.z;
				}
				return( off );
				
			}
			
			return( Vector3.zero );
		}
	#endregion

		#region Transform Functions
	//============================ Transform Functions ============================\
		
		// This function will iteratively climb up the transform.parent tree
		//   until it either finds a parent with a tag != "Untagged" or no parent
		public static GameObject FindTaggedParent(GameObject go) {
			// If this gameObject has a tag
			if (go.tag != "Untagged") {
				// then return this gameObject
				return(go);
			}
			// If there is no parent of this Transform
			if (go.transform.parent == null) {
				// We've reached the end of the line with no interesting tag
				// So return null
				return( null );
			}
			// Otherwise, recursively climb up the tree
			return( FindTaggedParent( go.transform.parent.gameObject ) );
		}
		// This version of the function handles things if a Transform is passed in
		public static GameObject FindTaggedParent(Transform t) {
			return( FindTaggedParent( t.gameObject ) );
		}
		
		#endregion
		
		
		#region Materials Functions
	//============================ Materials Functions ============================
		
		// Returns a list of all Materials in this GameObject or its children
		static public Material[] GetAllMaterials( GameObject go ) {
			List<Material> mats = new List<Material>();
			if (go.GetComponent<Renderer>() != null) {
				mats.Add(go.GetComponent<Renderer>().material);
			}
			foreach( Transform t in go.transform ) {
				mats.AddRange( GetAllMaterials( t.gameObject ) );
			}
			return( mats.ToArray() );
		}
		
		#endregion
		
		#region Linear Interpolation
	//============================ Linear Interpolation ============================
		
		// The standard Vector Lerp functions in Unity don't allow for extrapolation
		//   (which is input u values <0 or >1), so we need to write our own functions
		static public Vector3 Lerp (Vector3 vFrom, Vector3 vTo, float u) {
			Vector3 res = (1-u)*vFrom + u*vTo;
			return( res );
		}
		// The same function for Vector2
		static public Vector2 Lerp (Vector2 vFrom, Vector2 vTo, float u) {
			Vector2 res = (1-u)*vFrom + u*vTo;
			return( res );
		}
		// The same function for float
		static public float Lerp (float vFrom, float vTo, float u) {
			float res = (1-u)*vFrom + u*vTo;
			return( res );
		}
		// The same function for Color
		static public Color Lerp (Color vFrom, Color vTo, float u) {
			Color res = (1-u)*vFrom + u*vTo;
			return( res );
		}
		#endregion
		
		#region Bézier Curves
	//============================ Bézier Curves ============================

	    // NOTE: I'm replacing all of these with versions that allocate much less memory. - JB
	    // NOTE: These could allocate even slightly less if the List was passed with the ref keyword, but it would require additional intermediate functions
		
		// While most Bézier curves are 3 or 4 points, it is possible to have
		//   any number of points using this recursive function
		// This uses the Utils.Lerp function because it needs to allow extrapolation
		static public Vector3 Bezier( float u, List<Vector3> vList, int n0=0, int n1=-1 ) {
	        // handle default n1 value
	        if (n1 == -1) n1 = vList.Count-1;

			// If there is only one element in vList, return it
	        if (n0 == n1) return vList[n0];

			// Recur left:  if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
			// Recur Right: if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
			// The result is the Lerp of these two shorter Lists
	        Vector3 res = Lerp( Bezier(u, vList, n0, n1-1), Bezier(u, vList, n0+1, n1), u );
			return( res );
		}
		
		// This version allows an Array or a series of Vector3s as input
		static public Vector3 Bezier( float u, params Vector3[] vecs ) {
			return( Bezier( u, new List<Vector3>(vecs) ) );
		}
		
		
		// The same two functions for Vector2
	    static public Vector2 Bezier( float u, List<Vector2> vList, int n0=0, int n1=-1 ) {
	        // handle default n1 value
	        if (n1 == -1) n1 = vList.Count-1;
	        
	        // If there is only one element in vList, return it
	        if (n0 == n1) return vList[n0];
	        
	        // Recur left:  if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
	        // Recur Right: if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
	        // The result is the Lerp of these two shorter Lists
	        Vector2 res = Lerp( Bezier(u, vList, n0, n1-1), Bezier(u, vList, n0+1, n1), u );
			return( res );
		}
		
		// This version allows an Array or a series of Vector2s as input
		static public Vector2 Bezier( float u, params Vector2[] vecs ) {
			return( Bezier( u, new List<Vector2>(vecs) ) );
		}
		
		
		// The same two functions for float
	    static public float Bezier( float u, List<float> vList, int n0=0, int n1=-1 ) {
	        // handle default n1 value
	        if (n1 == -1) n1 = vList.Count-1;
	        
	        // If there is only one element in vList, return it
	        if (n0 == n1) return vList[n0];
	        
	        // Recur left:  if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
	        // Recur Right: if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
	        // The result is the Lerp of these two shorter Lists
	        float res = Lerp( Bezier(u, vList, n0, n1-1), Bezier(u, vList, n0+1, n1), u );
	        return( res );
		}
		
		// This version allows an Array or a series of floats as input
		static public float Bezier( float u, params float[] vecs ) {
			return( Bezier( u, new List<float>(vecs) ) );
		}
		
		
		// The same two functions for Quaternion
	    static public Quaternion Bezier( float u, List<Quaternion> vList, int n0=0, int n1=-1 ) {
	        // handle default n1 value
	        if (n1 == -1) n1 = vList.Count-1;
	        
	        // If there is only one element in vList, return it
	        if (n0 == n1) return vList[n0];
	        
	        // Recur left:  if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
	        // Recur Right: if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
	        // The result is the Lerp of these two shorter Lists
	        Quaternion res = Quaternion.Slerp( Bezier(u, vList, n0, n1-1), Bezier(u, vList, n0+1, n1), u );
	        return( res );
		}
		
		// This version allows an Array or a series of floats as input
		static public Quaternion Bezier( float u, params Quaternion[] vecs ) {
			return( Bezier( u, new List<Quaternion>(vecs) ) );
		}


	    // OLD VERSIONS

	//    // While most Bézier curves are 3 or 4 points, it is possible to have
	//    //   any number of points using this recursive function
	//    // This uses the Utils.Lerp function because it needs to allow extrapolation
	//    static public Vector3 Bezier( float u, List<Vector3> vList ) {
	//        // If there is only one element in vList, return it
	//        if (vList.Count == 1) {
	//            return( vList[0] );
	//        }
	//        // Otherwise, create vListR, which is all but the 0th element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
	//        List<Vector3> vListR =  vList.GetRange(1, vList.Count-1);
	//        // And create vListL, which is all but the last element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
	//        List<Vector3> vListL = vList.GetRange(0, vList.Count-1);
	//        // The result is the Lerp of these two shorter Lists
	//        Vector3 res = Lerp( Bezier(u, vListL), Bezier(u, vListR), u );
	//        return( res );
	//    }
	    
	    
	//    // The same two functions for Vector2
	//    static public Vector2 Bezier( float u, List<Vector2> vList ) {
	//        // If there is only one element in vList, return it
	//        if (vList.Count == 1) {
	//            return( vList[0] );
	//        }
	//        // Otherwise, create vListR, which is all but the 0th element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
	//        List<Vector2> vListR =  vList.GetRange(1, vList.Count-1);
	//        // And create vListL, which is all but the last element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
	//        List<Vector2> vListL = vList.GetRange(0, vList.Count-1);
	//        // The result is the Lerp of these two shorter Lists
	//        Vector2 res = Lerp( Bezier(u, vListL), Bezier(u, vListR), u );
	//        return( res );
	//    }
	    
	    
	//    // The same two functions for float
	//    static public float Bezier( float u, List<float> vList ) {
	//        // If there is only one element in vList, return it
	//        if (vList.Count == 1) {
	//            return( vList[0] );
	//        }
	//        // Otherwise, create vListR, which is all but the 0th element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
	//        List<float> vListR =  vList.GetRange(1, vList.Count-1);
	//        // And create vListL, which is all but the last element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
	//        List<float> vListL = vList.GetRange(0, vList.Count-1);
	//        // The result is the Lerp of these two shorter Lists
	//        float res = Lerp( Bezier(u, vListL), Bezier(u, vListR), u );
	//        return( res );
	//    }
	    
	    
	//    // The same two functions for Quaternion
	//    static public Quaternion Bezier( float u, List<Quaternion> vList ) {
	//        // If there is only one element in vList, return it
	//        if (vList.Count == 1) {
	//            return( vList[0] );
	//        }
	//        // Otherwise, create vListR, which is all but the 0th element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
	//        List<Quaternion> vListR =  vList.GetRange(1, vList.Count-1);
	//        // And create vListL, which is all but the last element of vList
	//        // e.g. if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
	//        List<Quaternion> vListL = vList.GetRange(0, vList.Count-1);
	//        // The result is the Slerp of these two shorter Lists
	//        // It's possible that Quaternion.Slerp may clamp u to [0..1] :(
	//        Quaternion res = Quaternion.Slerp( Bezier(u, vListL), Bezier(u, vListR), u );
	//        return( res );
	//    }

	#endregion
		
		#region Trace & Logging Functions
		//============================ Trace & Logging Functions ============================

	    static public string PARAMS_JOIN(params object[] objs) {
	        System.Text.StringBuilder sb = new System.Text.StringBuilder();
	        sb.Append(objs[0].ToString());
	        for (int i=1; i<objs.Length; i++) {
	            sb.Append('\t');
	            sb.Append(objs[i].ToString());
	            //NOTE: Could add switch statement for various object types here
	        }
	        return sb.ToString();
	    }

		static public void tr(params object[] objs) {
	        print( PARAMS_JOIN(objs) );
			//string s = objs[0].ToString();
			//for (int i=1; i<objs.Length; i++) {
			//	s += "\t"+objs[i].ToString();
			//}
			//print (s);
		}

		static public void trd(params object[] objs) {
			if (DEBUG) {
				tr (objs);
			}
		}

	    static public void LogError(params object[] objs) {
	        Debug.LogError( PARAMS_JOIN(objs) );
	    }

	    static public void LogWarning(params object[] objs) {
	        Debug.LogWarning( PARAMS_JOIN(objs) );
	    }

	#endregion

		
		#region Math Functions
		//============================ Math Functions ============================

		static public float RoundToPlaces(float f, int places=2) {
			float mult = Mathf.Pow(10,places);
			f *= mult;
			f = Mathf.Round (f);
			f /= mult;
			return(f);
		}

		static public string AddCommasToNumber(float f) {
			return AddCommasToNumber(f,2);
		}
		static public string AddCommasToNumber(float f, int places) {
			int n = Mathf.RoundToInt(f);
			f -= n;
			f = RoundToPlaces(f,places);
			string str = AddCommasToNumber( n );
			str += "."+(f*Mathf.Pow(10,places));
			return( str );
		}
		static public string AddCommasToNumber(int n) {
			int rem;
			int div;
			string res = "";
			string rems;
			while (n>0) {
				rem = n % 1000;
				div = n / 1000;
				rems = rem.ToString();
				
				while (div>0 && rems.Length<3) {
					rems = "0"+rems;
				}
				// TODO: I think there must be a faster way to concatenate strings. Maybe I could do this with an array or something
				if (res == "") {
					res = rems;
				} else {
					res = rems + "," + res.ToString();
				}
				n = div;
			}
			if (res == "") res = "0";
			return( res );
		}

		#endregion


		#region Utils Statics and Instance Methods
		//--------------------------------------------------------------------------------------------//
		//--------------------------------------------------------------------------------------------//
		//--------------------------------------------------------------------------------------------//
		
		static private XnUtils	_S;
		
		static private bool		SINGLETON_INITED = false;
		static public bool		DEBUG_DEFAULT = true;
		
		static public string	_LOG_HEADER = "";
		static public string	LOG = "";
		static public List<string>	LOGS;
		static public List<string>	LOG_HEADERS;
		
		static public bool		TRACE_LOG = false;
		static public bool		CLEAR_LOG_ON_PRINT = true;
		// TODO: Look this number up so that I'm correct!!!
		static public int		MAX_STRING_LENGTH = 999999;
		static public KeyCode	PRINT_KEY_DEFAULT = KeyCode.BackQuote;
		static public bool		SPLIT_VECTOR_COMPONENTS_ON_LOG = true;
		
		public bool				debug = DEBUG_DEFAULT;
		public KeyCode			printKey = KeyCode.BackQuote;
		public bool				printThisFrame = false;
		
		
		// INSTANCE FUNCTIONS - An instance is created by any function which runs in the background to do things like print the log
		public void Update() {
			printThisFrame = Input.GetKeyDown(printKey);
		}
		
		
		public void LateUpdate() {
			if (printThisFrame) {
				PrintLog();
				printThisFrame = false;
			}
			// NOTE: This prevents the LOG from exceeding the max length in memory
			if (LOG.Length >= MAX_STRING_LENGTH) {
				PrintLog();
				LOG = "";
			}
		}
		
		// STATIC FUNCTIONS
		
		// ====================== Utils Singleton Functions ======================
		static public XnUtils S {
			get {
				if (_S != null) return(_S);
				MakeUtilsGameObject();
				return(_S);
			}
			set {
				_S = value;
			}
		}
		
		// Generate a singleton of Utils to handle keyboard input and such
		static void MakeUtilsGameObject() {
			if (SINGLETON_INITED) return;
			SINGLETON_INITED = true;
			GameObject go = new GameObject("Utils");
			go.AddComponent<XnUtils>();
			_S = go.GetComponent<XnUtils>();
			if (LOGS == null) LOGS = new List<string>();
			if (LOG_HEADERS == null) LOG_HEADERS = new List<string>();
		}
		
		#endregion
		
		
		#region Camera Functions
		// ====================== Camera Functions ======================

		static public Rect screenRect {
			get {
				Rect r = new Rect(0,0,Screen.width,Screen.height);
				return(r);
			}
		}

		static public Rect GetBoundsAt0() {
			Rect r = new Rect();
			Camera cam = XnUtils.mainCamera;
			Vector3 bl = cam.ScreenToWorldPoint ( new Vector3 (0,0,-cam.transform.position.z) );
			Vector3 tr = cam.ScreenToWorldPoint ( new Vector3 (cam.pixelWidth,cam.pixelHeight,-cam.transform.position.z) );
			r.xMin = bl.x;
			r.yMin = bl.y;
			r.xMax = tr.x;
			r.yMax = tr.y;
			return(r);
		}
		
		static public Rect CameraBounds() {
			return( GetBoundsAt0() );
		}
		
		static public Rect cameraBounds {
			get {
				return( CameraBounds() );
			}
		}
		
		
		static public Vector3 mouseLoc {
			get {
				return( GetMouseLoc() );
			}
		}
		static public Vector3 GetMouseLoc(Camera cam=null, float dist=0) {
			if (cam == null) cam = XnUtils.mainCamera;
			if (dist == 0) dist = -cam.transform.position.z;
			Vector3 pos = Input.mousePosition;
			Vector3 loc = cam.ScreenToWorldPoint( new Vector3 (pos.x, pos.y, dist) );
			return(loc);
		}
		
		#endregion
		
		#region Color Functions
		// ====================== Color Functions ======================
		
		static public Color ColorMixAdditive( Color c0, Color c1 ) {
			Color c = c0;
			if (c1.r > c.r) c.r = c1.r;
			if (c1.g > c.g) c.g = c1.g;
			if (c1.b > c.b) c.b = c1.b;
			return( c );
		}
		
		// Returns a float describing how different the two colors are from each other
		// 0 is identical, 1 is black vs. white
		static public float ColorDifference( Color c0, Color c1, bool checkAlpha=false) {
			Color c;
			c.r = Mathf.Abs(c0.r - c1.r);
			c.g = Mathf.Abs(c0.g - c1.g);
			c.b = Mathf.Abs(c0.b - c1.b);
			float diff;
			if (checkAlpha) {
				c.a = Mathf.Abs(c0.a - c1.a);
				// Average the channels
				diff = (c.r + c.g + c.b + c.a) / 4f;
			} else {
				c.a = 0;
				// Average the channels
				diff = (c.r + c.g + c.b) / 3f;
			}
			return( diff );
		}
		
		static public Color ColorInvisible(Color c) {
			// NOTE: Color is passed by value
			c.a = 0;
			return( c );
		}



		public static Color ColorFrom255(int r, int g, int b, int a=255) {
			float f255 = 1f / 255f;
			float fR = r * f255;
			float fG = g * f255;
			float fB = b * f255;
			float fA = a * f255;
			return new Color(fR, fG, fB, fA);
	    }


		// The HEX to and from Color code is from https://www.c-sharpcorner.com/article/hex-colors-in-C-Sharp/
		/// <summary>
		/// Converts a "#FF00FF" or "FF00FF" style hex color to a Color.
		/// </summary>
		/// <param name="hexString">Hex Color (e.g., "#FF00FF" or "FF00FF")</param>
		/// <returns>A Color, or Color.magenta if there was an error</returns>
		public static Color HexToColor(string hexString) {
			// Translates a html hexadecimal definition of a color into a .NET Framework Color.
			// The input string must start with a '#' character and be followed by 6 hexadecimal
			// digits. The digits A-F are not case sensitive. If the conversion was not successfull
			// the color white will be returned.
			Color col;
			int r, g, b, a;
			r = 0;
			g = 0;
			b = 0;
			a = 255;
			int offset = (hexString[0] == '#') ? 1 : 0;
			int len = hexString.Length - offset;
			if (len == 6 || len == 8) {
				r = int.Parse(hexString.Substring(offset + 0, 2), System.Globalization.NumberStyles.HexNumber);
				g = int.Parse(hexString.Substring(offset + 2, 2), System.Globalization.NumberStyles.HexNumber);
				b = int.Parse(hexString.Substring(offset + 4, 2), System.Globalization.NumberStyles.HexNumber);
				if (len == 8) {
					a = int.Parse(hexString.Substring(offset + 6, 2), System.Globalization.NumberStyles.HexNumber);
				}
				col = ColorFrom255(r, g, b, a);
			} else {
				return Color.magenta;
			}
			return col;
		}



		/// <summary>
		/// Returns the hexadecimal web representation of the color (e.g., #FF00FF)
		/// </summary>
		/// <param name="c">The Color</param>
		/// <returns>A hex string like #FF00FF</returns>
		public static string ColorToHex(Color c, bool prependHash=true) {
			string hash = prependHash ? "#" : "";
			int r = Mathf.RoundToInt(c.r * 255);
			int g = Mathf.RoundToInt(c.g * 255);
			int b = Mathf.RoundToInt(c.b * 255);
			if (c.a != 1) {
				int a = Mathf.RoundToInt(c.b * 255);
				return $"{hash}{r:X2}{g:X2}{b:X2}{a:X2}";
			}
			return $"{hash}{r:X2}{g:X2}{b:X2}";
		}

	#endregion


		#region Randomization Functions
		// ====================== Randomization Functions ======================

		// Generates a random int which is from 0 (inclusive) to limit (exclusive)
		static public int RandInt( int limit ) {
			float rF = Random.value * (float) limit;
			int rI = Mathf.FloorToInt( rF );
			if (rI == limit) rI--;
			return( rI );
		}
		
		static public float GetRandInRange( float min, float max ) {
			if (min > max) {
				float f = min;
				min = max;
				max = f;
			}
			return( (max - min) * Random.value + min );
		}
		static public int GetRandInRange( int min, int max ) {
			float f = GetRandInRange( (float) min, (float) max );
			int n = Mathf.RoundToInt(f);
			n = Mathf.Clamp(n,min,max);
			return( n );
		}
		static public float GetRandInRange( Vector2 rng ) {
			return( GetRandInRange(rng.x, rng.y) );
		}

		
		static public Vector3 GetRandInRect( Rect r ) {
			Vector3 v = Vector3.zero;
			v.x = GetRandInRange( r.xMin, r.xMax );
			v.y = GetRandInRange( r.yMin, r.yMax );
			return( v );
		}
		static public Vector2 GetRandInRectV2( Rect r ) {
			Vector3 v = GetRandInRect( r );
			Vector2 v2 = new Vector2( v.x, v.y );
			return( v2 );
		}
		
		static public float GetNumWithVariance( float num, float variance ) {
			return( num-variance + (Random.value * 2 * variance) );
		}

		static public bool randomBool {
			get { return ( Random.value < 0.5f ); }
        }
		
		#endregion
		
		#region String and Logging Functions
		// ====================== String and Logging Functions ======================
		static public string Str(object o) {
			string s = "";
			if (o == null) return "null";

			if ( o.GetType().IsArray ) {
				s = KnownArrayToTabbedString( o );
			} else {
				s = o.ToString();
				
				if (o is Vector3 && SPLIT_VECTOR_COMPONENTS_ON_LOG) {
					Vector3 v3 = (Vector3) o;
					s = v3.x+"\t"+v3.y+"\t"+v3.z+"\t"; // Inserts a double-tab between Vector3s
				}
				if (o is Vector2) {
					Vector2 v2 = (Vector2) o;
					s = "( "+v2.x+", "+v2.y+" )";
				}
				
				
			}
			
			/*
			//Debug.Log(typeof(o));
			//if ( typeof(o) == typeof(object[]) ) {
			//if ( typeof(o) == 
			
			if ( o is Vector3[] ) {
				s = Vector3ArrayToTabbedString(o as Vector3[]);
			} else if ( o is int[] ) {
				s = IntArrayToTabbedString(o as int[]);
			} else if ( o is Vector2[] ) {
				s = Vector2ArrayToTabbedString(o as Vector2[]);
			} else {
				s = o.ToString();
			}
			*/
			
			return(s);
		}
		
		static public string KnownArrayToTabbedString( object o ) {
			string str = "";
			int ndx = 0;
			foreach (object obj in (Array)o) {
				if (ndx == 0) {
					str += Str( obj );
				} else {
					str += "\t" + Str( obj );
				}
				ndx++;
			}
			/*
			Array ss = (Array) o;
			string str = Str(ss[0]);
			for (int i=1; i<ss.Length; i++) {
				str += "\t"+Str(ss[i]);
			}
			*/
			return(str);
		}
		
		/*
		static public string IntArrayToTabbedString(int[] ss) {
			if (ss.Length == 0) return("");
			//string str = (string) ss[0];
			string str = Str(ss[0]);//ss[0].ToString();
			for (int i=1; i<ss.Length; i++) {
				str += "\t"+Str(ss[i]);
			}
			return(str);
		}
		
		
		static public string Vector2ArrayToTabbedString(Vector2[] ss) {
			if (ss.Length == 0) return("");
			//string str = (string) ss[0];
			string str = Str(ss[0]);//ss[0].ToString();
			for (int i=1; i<ss.Length; i++) {
				str += "\t"+Str(ss[i]);
			}
			return(str);
		}
		
		
		static public string Vector3ArrayToTabbedString(Vector3[] ss) {
			if (ss.Length == 0) return("");
			//string str = (string) ss[0];
			string str = Str(ss[0]);//ss[0].ToString();
			for (int i=1; i<ss.Length; i++) {
				str += "\t"+Str(ss[i]);
			}
			return(str);
		}
		
		static public string ArrayListToTabbedString(ArrayList ss) {
			if (ss.Count == 0) return("");
			//string str = (string) ss[0];
			string str = Str(ss[0]);//ss[0].ToString();
			for (int i=1; i<ss.Count; i++) {
				str += "\t"+Str(ss[i]);
			}
			return(str);
		}
		*/
		
		
		
		static public string ArrayToTabbedString(params object[] ss) {
			if (ss.Length == 0) return("");
			//string str = (string) ss[0];
			string str = Str(ss[0]);//ss[0].ToString();
			for (int i=1; i<ss.Length; i++) {
				str += "\t"+Str(ss[i]);
			}
			return(str);
		}
		
		static public void Log(params object[] ss) {
			Log(0, ss);
		}
		static public void Log(int ndx, params object[] ss) {
			MakeUtilsGameObject();
			
			string str = ArrayToTabbedString(ss);
			str += "\r";
			while (ndx >= LOGS.Count) {
				LOGS.Add("");
			}
			LOGS[ndx] += str;
			if (TRACE_LOG) Debug.Log(str);
		}
		
		static public void PrintLog() {
			PrintLog(0);
		}
		static public void PrintLog(int ndx) {
			if (LOGS == null || LOGS.Count == 0) return;
			MakeUtilsGameObject();
			string str = "=====LOG "+ndx+"=====\tFrame\ttime\tfixedTime\n";
			str += ArrayToTabbedString("", Time.frameCount, Time.time, Time.fixedTime)+"\n\n";
			if (LOG_HEADER.Length>0) str += LOG_HEADER+"\n";
			Debug.Log(str+LOGS[ndx]);
			LOGS[ndx] += "\n\n\n\n\n";
			if (CLEAR_LOG_ON_PRINT) {
				LOGS[ndx] = "";
				LOG_HEADER = "";
			}
		}

		static public void ClearLog() {
			ClearLog(0);
		}
		static public void ClearLog(int ndx) {
			if (LOGS == null) LOGS = new List<string>();
			if (LOGS.Count > ndx) {
				LOGS[ndx] = "";
			}
		}
		
		static public void SetLogHeader(params object[] ss) {
			MakeUtilsGameObject();
			SetLogHeader( 0, ss );
			//LOG_HEADER = ArrayToTabbedString(ss);
		}
		static public void SetLogHeader(int ndx, params object[] ss) {
			MakeUtilsGameObject();
			while (ndx >= LOG_HEADERS.Count) {
				LOG_HEADERS.Add("");
			}
			LOG_HEADERS[ndx] = ArrayToTabbedString(ss);
		}
		
		static public string LOG_HEADER {
			get {
				return(_LOG_HEADER);
			}
			set {
				if (value != _LOG_HEADER) {
					_LOG_HEADER = value;
				}
			}
		}

		/*
		static public void SetLogHeader(params object[] ss) {
			LOG_HEADER = ArrayToTabbedString(ss);
		}
		
		static public string LOG_HEADER {
			get {
				return(_LOG_HEADER);
			}
			set {
				if (value != _LOG_HEADER) {
					_LOG_HEADER = value;
				}
			}
		}
		
		/*
		static public void Log(string s) {
			MakeUtilsGameObject();
			
			Log(new string[] {s});
		}
		static public void Log(params object[] ss) {
			MakeUtilsGameObject();
			
			string str = ArrayToTabbedString(ss);
			str += "\r";
			LOG += str;
			if (TRACE_LOG) Debug.Log(str);
		}
		
		static public void PrintLog() {
			MakeUtilsGameObject();
			string str = "=====LOG=====\tFrame\ttime\tfixedTime\r";
			str += ArrayToTabbedString("", Time.frameCount, Time.time, Time.fixedTime)+"\r\r";
			if (LOG_HEADER.Length>0) str += LOG_HEADER+"\r";
			Debug.Log(str+LOG);
			LOG += "\r\r\r\r\r";
			if (CLEAR_LOG_ON_PRINT) {
				LOG = "";
				LOG_HEADER = "";
			}
		}
		
		static public void SetLogHeader(params object[] ss) {
			LOG_HEADER = ArrayToTabbedString(ss);
		}
		
		static public string LOG_HEADER {
			get {
				return(_LOG_HEADER);
			}
			set {
				if (value != _LOG_HEADER) {
					_LOG_HEADER = value;
				}
			}
		}
		*/
		
		
		// These identical functions bypass the log entirely but still take params as input and output to Debug.Log
		static public void Print(params object[] ss) {
			MakeUtilsGameObject();
	        System.DateTime dt = System.DateTime.Now;
	        string s = ArrayToTabbedString(ss);
	        //DebugPanelToggle.Add(dt.ToString("HH:mm:ss.ffff")+"\n"+s);
	        Debug.Log(s+"\n"+dt.ToString("HH:mm:ss.ffff"));
		}
		static public void Tr(params object[] ss) {
			MakeUtilsGameObject();
			
			Debug.Log(ArrayToTabbedString(ss)+"\r");
		}
		
		
	//	static public string AddCommasToNumber(float n) {
	//		// TODO: Actually add the decimals to these numbers
	//		return( AddCommasToNumber( Mathf.RoundToInt(n) ) );
	//	}
	//	static public string AddCommasToNumber(int n) {
	//		int rem;
	//		int div;
	//		string res = "";
	//		string rems;
	//		while (n>0) {
	//			rem = n % 1000;
	//			div = n / 1000;
	//			rems = rem.ToString();
	//			
	//			while (div>0 && rems.Length<3) {
	//				rems = "0"+rems;
	//			}
	//			// TODO: I think there must be a faster way to concatenate strings. Maybe I could do this with an array or something
	//			if (res == "") {
	//				res = rems;
	//			} else {
	//				res = rems + "," + res.ToString();
	//			}
	//			n = div;
	//		}
	//		return( res );
	//	}
		
		/* Old Versions
		static public void Log(string s) {
			Log(new string[] {s});
		}
		static public void Log(params string[] ss) {
			string str = ss[0];
			for (int i=1; i<ss.Length; i++) {
				str += "\t"+ss[i];
			}
			str += "\r";
			LOG += str;
			if (TRACE_LOG) print(str);
		}
		
		static public void PrintLog() {
			print(LOG);
			LOG += "\r\r\r\r";
			if (CLEAR_LOG_ON_PRINT) LOG = "";
		}
		*/

		
		
		static public float[] ParseStringListToFloatArray( string eAtt ) {
			string[] sArr = eAtt.Split(',');
			float[] fArr = new float[sArr.Length];
			for (int i=0; i<sArr.Length; i++) {
				fArr[i] = float.Parse( sArr[i] );
			}
			return( fArr );
		}
		
		
		static public int[] ParseStringListToIntArray( string eAtt ) {
			string[] sArr = eAtt.Split(',');
			int[] iArr = new int[sArr.Length];
			for (int i=0; i<sArr.Length; i++) {
				iArr[i] = int.Parse( sArr[i] );
			}
			return( iArr );
		}
		
		
		static public List<char> MostCommonChars( string s ) {
			// Count how many instances we have of each letter
			int[] letterCounts = new int[26];
			string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			foreach (char cW in s) {
				for (int i=0; i<letters.Length; i++) {
					if (cW == letters[i]) {
						letterCounts[i]++;
					}
				}
			}
			// Find the biggest letterCounts
			int numLetters = 0;
			List<char> commonChars = new List<char>();
			for (int i=0; i<letterCounts.Length; i++) {
				if (letterCounts[i] > numLetters) {
					numLetters = letterCounts[i];
					commonChars.Clear();
				}
				if (letterCounts[i] == numLetters) commonChars.Add(letters[i]);
			}
			
			return(commonChars);
		}
		
		
		static public string CharListToString( List<char> lC ) {
			string s = "";
			foreach (char c in lC) {
				s += c;
			}
			return(s);
		}
	#endregion
		
		
	#region Mathematical Functions
		// ====================== Mathematical Functions ======================
		static public bool IsEven(int i) {
			return( i%2 == 0 );
		}
		static public bool IsOdd(int i) {
			return( i%2 == 1 );
		}
		
		static public float FloatListMean( List<float> l ) {
			if (l.Count == 0) return(0);
			float tot = 0;
			int count = 0;
			foreach (float f in l) {
				tot += f;
				count++;
			}
			tot /= count;
			return( tot );
		}
		
		
		static public Vector3 Vector3ListMean( params Vector3[] vv ) {
			return( Vector3ListMean( new List<Vector3>(vv) ));
		}
		static public Vector3 Vector3ListMean( List<Vector3> l ) {
			if (l.Count == 0) return(Vector3.zero);
			Vector3 tot = Vector3.zero;
			int count = 0;
			foreach (Vector3 v3 in l) {
				tot += v3;
				count++;
			}
			tot /= count;
			return( tot );
		}
		
		
	    /// <summary>
	    /// Converts a number to Roman numerals.
	            /// NOTE: If you're going to use this a lot, you should convert it to a StringBuilder! - JB
	    /// </summary>
	    /// <returns>The roman.</returns>
	    /// <param name="number">Number.</param>
		static public string ToRoman(int number) {
	//		string str = "";
			if ((number < 0) || (number > 3999)) {
				Debug.LogError("Utils.ToRoman: input of out range [1..3999]\t"+number.ToString());
				return("");
			}
			if (number < 1) return("");
			if (number >= 1000)	return("M" + ToRoman(number - 1000));
			if (number >= 900)	return("CM" + ToRoman(number - 900));
			if (number >= 500)	return("D" + ToRoman(number - 500));
			if (number >= 400)	return("CD" + ToRoman(number - 400));
			if (number >= 100)	return("C" + ToRoman(number - 100));           
			if (number >= 90)	return("XC" + ToRoman(number - 90));
			if (number >= 50)	return("L" + ToRoman(number - 50));
			if (number >= 40)	return("XL" + ToRoman(number - 40));
			if (number >= 10)	return("X" + ToRoman(number - 10));
			if (number >= 9)	return("IX" + ToRoman(number - 9));
			if (number >= 5)	return("V" + ToRoman(number - 5));
			if (number >= 4)	return("IV" + ToRoman(number - 4));
			if (number >= 1)	return("I" + ToRoman(number - 1));
			
			return("");
		}
		
		
		static public string PadLeadingZeroes(int n, int toPlaces) {
			string s = n.ToString();
			while (s.Length < toPlaces) {
				s = "0"+s;
			}
			return( s );
		}

		
		static public Vector2 Vector3to2( Vector3 v3, bool useXZ = false ) {
			if (!useXZ) {
				return( new Vector2( v3.x, v3.y ) );
			} else {
				return( new Vector2( v3.x, v3.z ) );
			}
		}
	#endregion

	#region Rotation Functions
		// ====================== Rotation Functions ======================
		
		static public float LimitRotation(float r) {
			while (r < -180) {
				r += 360;
			}
			while (r > 180) {
				r -= 360;
			}
			return(r);
		}
		
		static public float LimitRotation(float r, float r0) {
			while (r-r0 < -180) {
				r += 360;
			}
			while (r-r0 > 180) {
				r -= 360;
			}
			return(r);
		}
		
		static public float RotDiff(float r0, float r1) {
			r1 = LimitRotation( r1, r0 );
			float r = r1-r0;
			return(r);
		}
	#endregion
		
		#region Fuzzy Equality Functions
		// ====================== Fuzzy Equality Functions ======================

		static public bool Eq( Vector3 v0, Vector3 v1, float toWithin=0.1f, bool checkDimensionsSeparately=false ) {
			return Eq3(v0, v1, toWithin, checkDimensionsSeparately);
		}

		static public bool Eq3( Vector3 v0, Vector3 v1, float toWithin=0.1f, bool checkDimensionsSeparately=false ) {
			Vector3 v01 = v1-v0;
			if ( !checkDimensionsSeparately ) {
				return ( v01.magnitude <= toWithin );
			} else {
				if (v01.x > -toWithin && v01.x < toWithin) {
					if (v01.y > -toWithin && v01.y < toWithin) {
						if (v01.z > -toWithin && v01.z < toWithin) {
							return(true);
						}
					}
				}
			}
	//		if (v01.x > -toWithin && v01.x < toWithin) {
	//			if (v01.y > -toWithin && v01.y < toWithin) {
	//				if (v01.z > -toWithin && v01.z < toWithin) {
	//					return(true);
	//				}
	//			}
	//		}
			return(false);
		}
		
		static public bool Eq( Vector2 v0, Vector2 v1, float toWithin=0.1f, bool checkDimensionsSeparately=false ) {
			Vector2 v01 = v1-v0;
			if ( !checkDimensionsSeparately ) {
				return ( v01.magnitude <= toWithin );
			} else {
				if (v01.x > -toWithin && v01.x < toWithin) {
					if (v01.y > -toWithin && v01.y < toWithin) {
						return(true);
					}
				}
			}
	//		if (v01.x > -toWithin && v01.x < toWithin) {
	//			if (v01.y > -toWithin && v01.y < toWithin) {
	//				return(true);
	//			}
	//		}
			return(false);
		}

		static public bool Eq( float f0, float f1, float toWithin=0.1f ) {
			return (Mathf.Abs(f0-f1) < toWithin);
		}
		
		#endregion
		
		#region Mesh Functions
		// ====================== Mesh Functions ======================
		public static void AddBackfacesToMesh( Mesh m ) {
			int len = m.triangles.Length;
			int[] triangles = new int[len*2];
			for (int i=0; i<m.triangles.Length; i += 3) {
				triangles[i]   = m.triangles[i];
				triangles[i+1] = m.triangles[i+1];
				triangles[i+2] = m.triangles[i+2];
				
				triangles[len+i]   = m.triangles[i];
				triangles[len+i+1] = m.triangles[i+2];
				triangles[len+i+2] = m.triangles[i+1];
			}
			m.triangles = triangles;
		}
		
		public static void MeshRemoveDuplicateVertices( Mesh m, bool testUV=false, bool testNormal=false) {
	//		Mesh m2 = new Mesh();
			// Find and remove identical Vertices
			Vector3 v, n;
			Vector2 uv;
			XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG = false;
			//Utils.Print(m.vertices);
			XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG = true;
			List<int> triangles = new List<int>(m.triangles);
			List<int> identicalVerts = new List<int>();
			List<int> vertsToRemove = new List<int>();
			List<Vector3> verts = new List<Vector3>(m.vertices);
			List<Vector2> uvs = new List<Vector2>(m.uv);
			List<Vector3> normals = new List<Vector3>(m.normals);
			// Go through all of the vertices
			for (int i=0; i<m.vertexCount; i++) {
				if (vertsToRemove.IndexOf(i) != -1) continue; // Don't check if we're already removing this vert
				v = m.vertices[i];
				uv = m.uv[i];
				n = m.normals[i];
				identicalVerts.Clear();
				for (int j=i+1; j<m.vertexCount; j++) {
					// If another vertex is identical, then add it to the list to replace
					if (!Eq3(m.vertices[j], v, 0.1f)) continue;
					if (testUV && !Eq3(m.uv[j], uv, 0.1f)) continue;
					if (testNormal && !Eq3(m.normals[j], n, 0.1f)) continue;
					/*
					if (m.vertices[j] != v) continue;
					if (testUV && m.uv[j] != uv) continue;
					if (testNormal && m.normals[j] != n) continue;
					*/
					identicalVerts.Add(j);
					vertsToRemove.Add(j);
				}
				if (identicalVerts.Count > 0) {
					//Debug.Log("break");
				}
				for (int k=0; k<triangles.Count; k++) {
					// If a triangle used one of the identicalVerts, replace it with the base vert
					if (identicalVerts.IndexOf(triangles[k]) > -1) {
						triangles[k] = i;
					}
				}
				/*
				for (int k=0; k<m.triangles.Length; k++) {
					// If a triangle used one of the identicalVerts, replace it with the base vert
					if (identicalVerts.IndexOf(m.triangles[k]) > -1) m.triangles[k] = i;
				}
				*/
			}
			// Ripple-delete the verts (which is going to take time!)
			vertsToRemove.Sort();
			int ndx;
			for (int i=vertsToRemove.Count-1; i>=0; i--) {
				ndx = vertsToRemove[i];
				verts.RemoveAt(ndx);
				uvs.RemoveAt(ndx);
				normals.RemoveAt(ndx);
			}
			// Now we have culled verts, uvs, and normals
			// Rearrange the triangles to point at the right verts
			int gt;
			for (int i=0; i<triangles.Count; i++) {
				// find out how many vertsToRemove this reference is larger than
				gt=0;
				ndx = triangles[i];
				for (int j=0; j<vertsToRemove.Count; j++) {
					if (vertsToRemove[j] < ndx)
						gt++;
					else
						break;
				}
				triangles[i] = ndx-gt;
			}
			
			
			/*
			// Now we have culled verts, uvs, and normals
			// Rearrange the triangles to point at the right verts
			for (int i=0; i<m.triangles.Length; i++) {
				// Find the m.vertex referred to in m.triangles[i]
				v = m.vertices[ m.triangles[i] ];
				for (int j=0; j<verts.Count; j++) {
					// Search through verts to find an identical Vector3
					if (v == verts[j]) {
						// Set the triangle to be the index of that Vector3
						m.triangles[i] = j;
						break;
					}
				}
			}
			*/
			m.triangles = triangles.ToArray();
			m.vertices = verts.ToArray();
			m.uv = uvs.ToArray();
			m.normals = normals.ToArray();
			
			MeshRemoveDuplicateTriangles(m);
		}
		
		public struct TriInt3 {
			public int x, y, z;
			
			public TriInt3 ( int eX, int eY, int eZ ) {
				x = eX;
				y = eY;
				z = eZ;
			}
			
			public static bool operator == (TriInt3 e1, TriInt3 e2) {
				// NOTE: This assumes that x, y, & z are different!!!
				// If each of the three verts in e1 match one of the verts in e2, they are equal
				// NOTE: This ignores vert ordering for backface culling and such
				if (e1.x==e2.x || e1.x==e2.y || e1.x==e2.z) {
					if (e1.y==e2.x || e1.y==e2.y || e1.y==e2.z) {
						if (e1.z==e2.x || e1.z==e2.y || e1.z==e2.z) {
							return( true );
						}
					}
				}
				return( false );
			}
			
			public static bool operator != (TriInt3 e1, TriInt3 e2) {
				// NOTE: This assumes that x, y, & z are different!!!
				// If each of the three verts in e1 match one of the verts in e2, they are equal
				// NOTE: This ignores vert ordering for backface culling and such
				if (e1.x==e2.x || e1.x==e2.y || e1.x==e2.z) {
					if (e1.y==e2.x || e1.y==e2.y || e1.y==e2.z) {
						if (e1.z==e2.x || e1.z==e2.y || e1.z==e2.z) {
							return( false );
						}
					}
				}
				return( true );
			}

	        public override bool Equals(object o) {
	            if (o==null) return false;
	            TriInt3 ti = (TriInt3) o;
	            if (ti.x != x || ti.y != y || ti.z != z) {
	                return false;
	            }
	            return true;
	        }

	        public override int GetHashCode()
	        {
	            return (x + y + z)%int.MaxValue;
	        }
		}
		
		
		public static void MeshRemoveDuplicateTriangles( Mesh m ) {
			List<TriInt3> triInts = new List<TriInt3>();
			// Create a grouping of the triangles
			for (int i=0; i<m.triangles.Length; i+=3) {
				triInts.Add( new TriInt3( m.triangles[i], m.triangles[i+1], m.triangles[i+2] ) );
			}
			// Search backwards through the triangles to find duplicates
			TriInt3 ti;
			for (int i=triInts.Count-1; i>=0; i--) {
				ti = triInts[i];
				for (int j=i-1; j>=0; j--) {
					if (triInts[j] == ti) {
						// If an earlier TriInt3 == this one, remove this one
						triInts.RemoveAt(i);
						break;
					}
				}
			}
			// Spit this information back out to m.triangles
			List<int> triangles = new List<int>();
			foreach (TriInt3 ti3 in triInts) {
				triangles.Add(ti3.x);
				triangles.Add(ti3.y);
				triangles.Add(ti3.z);
			}
			m.triangles = triangles.ToArray();
		}
		
		/* This is supposed to be covered by Instantiate(Mesh)
		public static Mesh CloneMesh(Mesh m) {
			Mesh c = new Mesh();
			c.vertices = m.vertices.Clone();
			c.normals = m.normals.Clone();
			c.
		}
		*/
		
		#endregion
		
		
		#region Transform Functions
		// ====================== Transform Functions ======================
		
		// This finds a much better version of lossyScale than Unity provides!
		public static Vector3 LossyScale(Transform t) {
			if (t.parent == null) return(Vector3.one);
			Vector3 v = LossyScale(t.parent);
			v.x *= t.localScale.x;
			v.y *= t.localScale.y;
			v.z *= t.localScale.z;
			return(v);
		}
		
		public static Vector3 ScaleVectorByVector(Vector3 v, Vector3 scale) {
			v.x *= scale.x;
			v.y *= scale.y;
			v.z *= scale.z;
			return(v);
		}
		
	#endregion






	#region Other Functions
	    // ====================== Other Functions ======================


	    // BreakPoint can be called from anywhere to cause a breakpoint
	    static public void BreakPoint() {


	        Debug.Log("BreakPoint!!!");


	    }

	    static public T ValidateEnum<T>(T eEnumItem)
	    {
	        if (System.Enum.IsDefined(typeof(T), eEnumItem) == true)
	            return eEnumItem;
	        else
	            return default(T);
	    }
	    

	    /// <summary>
	    /// Removes the diacritics from a string of text (i.e. removes accent marks and such). - JB
	    /// Modified from: http://www.levibotelho.com/development/c-remove-diacritics-accents-from-a-string
	    /// </summary>
	    /// <returns>The diacritics.</returns>
	    /// <param name="text">Text.</param>
	    public static string RemoveDiacritics(string text)
	    {
	        if (string.IsNullOrEmpty(text)) // IsNullOrWhiteSpace(text))
	            return text;

	        text = text.Normalize(System.Text.NormalizationForm.FormD);
	        System.Text.StringBuilder sb = new System.Text.StringBuilder();

	        //        char[] chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
	        //        return new string(chars).Normalize(System.Text.NormalizationForm.FormC);

	        for (int i=0; i<text.Length; i++) {
	            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(text[i]) != System.Globalization.UnicodeCategory.NonSpacingMark) {
	                sb.Append(text[i]);
	            }
	        }

	        text = sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
	        return text;
	    }
	    
	    #endregion
	}






	#region Easing Class
	//============================ Easing Classes ============================
	[System.Serializable]
	public class EasingCachedCurve {
		public List<string>		curves =	new List<string>();
		public List<float>		mods = 		new List<float>();
	}

	/// <summary>
	/// Class that can be used to create complex easing curves for linear interpolation functions.
	/// Terms include: Linear, In, Out, InOut, Sin, SinIn, & SinOut. All can be followed by a | and 
	/// a modifier number, and multiple easing operations can be comma separated.
	/// </summary>
	public class Easing {
		//[Tooltip("From Easing Terms include: Linear, In, Out, InOut, Sin, SinIn, & SinOut. " +
		//"All can be followed by a | and a modifier number, " +
		//"and multiple easing operations can be comma separated.")]
		static public string Linear = 		",Linear|";
		static public string In = 			",In|";
		static public string Out =			",Out|";
		static public string InOut = 		",InOut|";
		static public string Sin =			",Sin|";
		static public string SinIn =		",SinIn|";
		static public string SinOut =		",SinOut|";
		
		static public Dictionary<string,EasingCachedCurve> cache;
		// This is a cache for the information contained in the complex strings
		//   that can be passed into the Ease function. The parsing of these
		//   strings is most of the effort of the Ease function, so each time one
		//   is parsed, the result is stored in the cache to be recalled much 
		//   faster than a parse would take.
		// Need to be careful of memory leaks, which could be a problem if several
		//   million unique easing parameters are called
		
		static public float Ease( float u, params string[] curveParams ) {
			// Set up the cache for curves
			if (cache == null) {
				cache = new Dictionary<string, EasingCachedCurve>();
			}
			
			float u2 = u;
			foreach ( string curve in curveParams ) {
				// Check to see if this curve is already cached
				if (!cache.ContainsKey(curve)) {
					// If not, parse and cache it
					EaseParse(curve);
				} 
				// Call the cached curve
				u2 = EaseP( u2, cache[curve] );
			}
			return( u2 );
			/*	
				
				// It's possible to pass in several comma-separated curves
				string[] curvesA = curves.Split(',');
				foreach (string curve in curvesA) {
					if (curve == "") continue;
					//string[] curveA = 
				}
				
			}
			//string[] curve = func.Split(',');
			
			foreach (string curve in curves) {
				
			}
			
			string[] funcSplit;
			foreach (string f in funcs) {
				funcSplit = f.Split('|');
				
			}
			*/
		}
		
		static private void EaseParse( string curveIn ) {
			EasingCachedCurve ecc = new EasingCachedCurve();
			// It's possible to pass in several comma-separated curves
			string[] curves = curveIn.Split(',');
			foreach (string curve in curves) {
				if (curve == "") continue;
				// Split each curve on | to find curve and mod
				string[] curveA = curve.Split('|');
				ecc.curves.Add(curveA[0]);
				if (curveA.Length == 1 || curveA[1] == "") {
					ecc.mods.Add(float.NaN);
				} else {
					float parseRes;
					if ( float.TryParse(curveA[1], out parseRes) ) {
						ecc.mods.Add( parseRes );
					} else {
						ecc.mods.Add( float.NaN );
					}
				}	
			}
			cache.Add(curveIn, ecc);
		}
		
		
		static public float Ease( float u, string curve, float mod ) {
			return( EaseP( u, curve, mod ) );
		}
		
		static private float EaseP( float u, EasingCachedCurve ec ) {
			float u2 = u;
			for (int i=0; i<ec.curves.Count; i++) {
				u2 = EaseP( u2, ec.curves[i], ec.mods[i] );
			}
			return( u2 );
		}
		
		// TODO: Mathf.Pow is VERY slow. There are several places where it could be avoided here. - 2022-06-27 JGB
		static private float EaseP( float u, string curve, float mod ) {
			float u2 = u;
			
			switch (curve) {
			case "In":
				if (float.IsNaN(mod)) mod = 2;
				u2 = Mathf.Pow(u, mod);
				break;
				
			case "Out":
				if (float.IsNaN(mod)) mod = 2;
				u2 = 1 - Mathf.Pow( 1-u, mod );
				break;
				
			case "InOut":
				if (float.IsNaN(mod)) mod = 2;
				if ( u <= 0.5f ) {
					u2 = 0.5f * Mathf.Pow( u*2, mod );
				} else {
					u2 = 0.5f + 0.5f * (  1 - Mathf.Pow( 1-(2*(u-0.5f)), mod )  );
				}
				break;
				
			case "Sin":
				if (float.IsNaN(mod)) mod = 0.15f;
				u2 = u + mod * Mathf.Sin( 2*Mathf.PI*u );
				break;
				
			case "SinIn":
				// mod is ignored for SinIn
				if ( float.IsNaN( mod ) ) mod = 0f;
				u2 = 1 - Mathf.Cos( u * Mathf.PI * 0.5f );
				u2 += Mathf.Sin( u * Mathf.PI ) * mod;
				break;
				
			case "SinOut":
				// mod is ignored for SinOut
				if ( float.IsNaN( mod ) ) mod = 0f;
				u2 = Mathf.Sin( u * Mathf.PI * 0.5f );
				u2 += Mathf.Sin( u * Mathf.PI ) * mod;
				break;
				
			case "Linear":
			default:
				// u2 already equals u
				break;
			}
			
			return( u2 );
		}
		
	}
	#endregion


	#region HexGrid class
	//============================ HexGrid ============================
	/// <summary>
	/// This is a helper class designed to minimize the effort needed to create and use
	/// a hexagonal grid. Currently, it only works as a flat-top odd-q grid, but it can
	/// be adapted to other hex grid forms later.
	/// See the excellent discussion of hex grids here: https://www.redblobgames.com/grids/hexagons/
	/// </summary>
	public class XnHexGrid {
		public const float sqrt3 = 1.7320508f;

		private float _size;

		public float size {
			get { return _size; }
			set {
				_size = value;
				sX = _size * 1.5f;
				sY = size  * sqrt3;
				sYh = sY   * 0.5f;

			}
		}
		public float sX  { get; private set; } // Space X between hexes 
		public float sY  { get; private set; } // Space Y between hexes
		public float sYh { get; private set; } // Half Space Y
		public bool  row0isTop = true;

		public XnHexGrid(float hexSize = 1) {
			size = hexSize;
		}

		public Vector2 Pos( float col, float row, bool addOddY=true ) {
			Vector2 vec = Vector2.zero;
			vec.x = sX * col;
			float oddY = addOddY ? Mathf.Abs( ( ( col + 1 ) % 2f ) - 1 ) : 0; // if addOddY == false, oddY = 0
			vec.y = sY * row + sYh * oddY;
			if ( row0isTop ) vec.y = -vec.y;
			return vec;
		}
		
		// This was to test the calculation of oddY in the editor.
	// #region HexGridTestArea
	// 	[Range( 0, 10 )]
	// 	public float x, oddY;
	//     
	// 	void OnValidate() { // Continuously calculate oddY;
	// 		oddY = Mathf.Abs( ( (x +1) % 2f ) - 1 );
	// 	}
	// #endregion
		
		//TODO: Add much more to this, like finding the nearest col and row to a specific point. But for now, it's good!
	}
	#endregion

	#region XnUtilsExtensionMethods
	public static class XnUtilsExtensionMethods{
		#region float Extensions
		static public bool IsApproximately(this float a, float b, float resolution = 0) {
			if (resolution == 0) {
				return Mathf.Approximately( a, b );
            }
			return ( Mathf.Abs( a - b ) <= resolution );
        }

		static public float LerpTo(this float a, float b, float u) {
			return ( 1 - u ) * a + u * b;
        }

        #endregion

        #region Binary Number to String Extension Methods
        static public string ToStringBinary( this ulong u, char zeroChar = '_' ) {
			string s = System.Convert.ToString( (long) u, 2 );
			if ( zeroChar != '0' ) s = s.Replace( '0', zeroChar );
			s = s.PadLeft( 64, zeroChar );
			return s;
        }

		static public string ToStringBinary( this uint u, char zeroChar = '_' ) {
			string s = System.Convert.ToString( u, 2 );
			if ( zeroChar != '0' ) s = s.Replace( '0', zeroChar );
			s = s.PadLeft( 32, zeroChar );
			return s;
		}

		static public string ToStringBinary( this int u, char zeroChar = '_' ) {
			string s = System.Convert.ToString( u, 2 );
			if ( zeroChar != '0' ) s = s.Replace( '0', zeroChar );
			s = s.PadLeft( 32, zeroChar );
			return s;
		}
        #endregion

        #region Hamming Weight Extension Methods
        static public int HammingWeight( this ulong u ) {
			u = u - ( ( u >> 1 ) & 0x5555555555555555UL );
			u = ( u & 0x3333333333333333UL ) + ( ( u >> 2 ) & 0x3333333333333333UL );
			return (int) ( unchecked(( ( u + ( u >> 4 ) ) & 0xF0F0F0F0F0F0F0FUL ) * 0x101010101010101UL) >> 56 );
		}

		static public int HammingWeight( this uint u ) {
			u = u - ( ( u >> 1 ) & 0x55555555U );
			u = ( u & 0x33333333U ) + ( ( u >> 2 ) & 0x33333333U );
			return (int) ( unchecked(( ( u + ( u >> 4 ) ) & 0xF0F0F0FU ) * 0x1010101U) >> 24 );
		}

		static public int HammingWeight( this int u) {
			return ( (uint) u ).HammingWeight();
        }

        static public int HammingWeight( this LayerMask lm ) {
            return ( (uint) (int) lm ).HammingWeight();
        }

		static public void TestHammingWeight() {
			int numTestsPerType = 100;
			int numBits, numBitsLeft;
			int possibleBits;
			int hammingWeight;
			int correctChecks;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			// ulong check
			sb.AppendLine( "__________ ulong tests __________" );
			ulong ul;
			possibleBits = 64;
			correctChecks = 0;
            for ( int i = 0; i < numTestsPerType; i++ ) {
				numBits = Random.Range( 0, possibleBits+1 );
				numBitsLeft = numBits;
				ul = 0UL;
                for ( int j = possibleBits-1; j >= 0; j-- ) {
					if ( numBitsLeft == 0 ) continue;
					if ( (numBitsLeft == j + 1) || XnUtils.randomBool ) {
						ul |= ( 1UL << j );
						numBitsLeft--;
					}
                }
				hammingWeight = ul.HammingWeight();
				if (hammingWeight == numBits) {
					sb.AppendLine( $"_pass_ - {ul.ToStringBinary()} has {hammingWeight} bits" );
					correctChecks++;
				} else {
					sb.AppendLine( $"FAILED - {ul.ToStringBinary()} should have {numBits} bits, but HammingWeight counted {hammingWeight} bits" );
				}
            }
			Debug.LogWarning( $"{correctChecks} / {numTestsPerType} tests passed ({(float) correctChecks / numTestsPerType * 100}%)\n" + sb.ToString() );

			// uint checks
			sb.Clear();
			sb.AppendLine( "__________ uint tests __________" );
			uint ui;
			possibleBits = 32;
			correctChecks = 0;
			for ( int i = 0; i < numTestsPerType; i++ ) {
				numBits = Random.Range( 0, possibleBits + 1 );
				numBitsLeft = numBits;
				ui = 0U;
				for ( int j = possibleBits-1; j >= 0; j-- ) {
					if ( numBitsLeft == 0 ) continue;
					if ( ( numBitsLeft == j + 1 ) || XnUtils.randomBool ) {
						ui |= ( 1U << j );
						numBitsLeft--;
					}
				}
				hammingWeight = ui.HammingWeight();
				if ( hammingWeight == numBits ) {
					sb.AppendLine( $"_pass_ - {ui.ToStringBinary()} has {hammingWeight} bits" );
					correctChecks++;
				} else {
					sb.AppendLine( $"FAILED - {ui.ToStringBinary()} should have {numBits} bits, but HammingWeight counted {hammingWeight} bits" );
				}
			}
			Debug.LogWarning( $"{correctChecks} / {numTestsPerType} tests passed ({(float) correctChecks / numTestsPerType * 100}%)\n" + sb.ToString() );

			// uint checks
			sb.Clear();
			sb.AppendLine( "__________ int tests __________" );
			int n;
			possibleBits = 32;
			correctChecks = 0;
			for ( int i = 0; i < numTestsPerType; i++ ) {
				numBits = Random.Range( 0, possibleBits + 1 );
				numBitsLeft = numBits;
				n = 0;
				for ( int j = possibleBits-1; j >= 0; j-- ) {
					if ( numBitsLeft == 0 ) continue;
					if ( ( numBitsLeft == j + 1 ) || XnUtils.randomBool ) {
						n |= ( 1 << j );
						numBitsLeft--;
					}
				}
				hammingWeight = n.HammingWeight();
				if ( hammingWeight == numBits ) {
					sb.AppendLine( $"_pass_ - {n.ToStringBinary()} has {hammingWeight} bits" );
					correctChecks++;
				} else {
					sb.AppendLine( $"FAILED - {n.ToStringBinary()} should have {numBits} bits, but HammingWeight counted {hammingWeight} bits" );
				}
			}
			Debug.LogWarning( $"{correctChecks} / {numTestsPerType} tests passed ({(float) correctChecks / numTestsPerType * 100}%)\n" + sb.ToString() );
		}
		#endregion

	}
    #endregion

}