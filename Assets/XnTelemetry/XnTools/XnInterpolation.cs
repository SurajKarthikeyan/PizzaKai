using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XnTools {
	public enum CurveType {
	    linear,
	    square,
	    power,
	    squareInv,
	    powInv,
	    sin,
	    sinStart,
	    sinStop,
	    slowMid,
	    inOut, 
	    other // other can enable the use of a string to define a curve in places where this enum is used to define the curve - JGB 2022-10-06
	}

	public class XnInterpolation {
		
		public const string LINEAR		= "Linear";
		public const string SQUARE		= "Square";
		public const string POWER		= "Power|";
		public const string SQUARE_INV	= "Square_Inv";
		public const string POW_INV		= "Pow_Inv|";
		public const string SIN 		= "Sin|";
		public const string SIN_START	= "Sin_Start|";
		public const string SIN_STOP	= "Sin_Stop|";
		public const string SLOW_MID	= "Slow_Mid|";
		public const string IN_OUT      = "In_Out|";
			
		
		static public Vector3 Interpolate( ArrayList arr, float u ) {
			if (arr.Count > 1) {
				ArrayList left  = arr.GetRange(0, arr.Count-1);
				ArrayList right = arr.GetRange(1, arr.Count-1);
				Vector3 p = Lerp( Interpolate(left, u), Interpolate(right, u), u );
				return(p);
			} else {
				if (arr[0] is GameObject) {
					return( (arr[0] as GameObject).transform.position );
				}
				return( (Vector3) arr[0] );
			}
		}
		
		
		/*
		static public Vector3 Lerp( List<Vector3> l, float u, string curve ) {
			u = Curve( u, curve );
			return( Lerp( l, u ) );
		}
		static public Vector3 Lerp( List<Vector3> l, float u ) {
			if (l.Count > 1) {
				List<Vector3> left  = l.GetRange(0, l.Count-1);
				List<Vector3> right = l.GetRange(1, l.Count-1);
				Vector3 p = Lerp( Lerp( left, u ), Lerp( right, u ), u );
				return(p);
			} else {
				return( l[0] );
			}
		}
		*/
		/*
		static public T LerpList<T>( List<T> l, float u, string curve ) {
			u = Curve( u, curve );
			return( LerpList( l, u ) );
		}
		*/
		
		static public Vector3 LerpList( List<Vector3> l, float u, string curve ) {
			u = Curve( u, curve );
			return( LerpList( l, u ) );
		}
		static public Vector3 LerpList( List<Vector3> l, float u ) {
			List<Vector3> l2;
			if (l.Count > 2) {
				List<Vector3> left  = l.GetRange(0, l.Count-1);
				List<Vector3> right = l.GetRange(1, l.Count-1);
				l2 = new List<Vector3>();
				l2.Add( LerpList( left, u ) );
				l2.Add( LerpList( right, u ) );
			} else if (l.Count == 2) {
				l2 = l;
			} else {
				return( l[0] );
			}
			return( Lerp( l2[0], l2[1], u) );
		}
		
		
		static public float LerpList( List<float> l, float u, string curve ) {
			u = Curve( u, curve );
			return( LerpList( l, u ) );
		}
		static public float LerpList( List<float> l, float u ) {
			List<float> l2;
			if (l.Count > 2) {
				List<float> left  = l.GetRange(0, l.Count-1);
				List<float> right = l.GetRange(1, l.Count-1);
				l2 = new List<float>();
				l2.Add( LerpList( left, u ) );
				l2.Add( LerpList( right, u ) );
			} else if (l.Count == 2) {
				l2 = l;
			} else {
				return( l[0] );
			}
			return( Lerp( l2[0], l2[1], u) );
		}
		
		/*
		static public T LerpList<T>( List<T> l, float u ) {
			List<T> l2;
			if (l.Count > 2) {
				List<T> left  = l.GetRange(0, l.Count-1);
				List<T> right = l.GetRange(1, l.Count-1);
				l2 = new List<T>();
				l2.Add( LerpList( left, u ) );
				l2.Add( LerpList( right, u ) );
			} else if (l.Count == 2) {
				l2 = l;
			} else {
				return( l[0] );
			}
			
			System.Type t = (l2[0]).GetType();
				
			if (t == typeof(Vector3)) {
				return( Lerp( (Vector3) l2[0], (Vector3) l2[1], u ) );
			}
			if (t == typeof(float)) {
				return( Lerp( (float) l2[0], (float) l2[1], u ) );
			}
			return( l[0] );
		}
		*/
		static public Vector3 Lerp( Vector3 p0, Vector3 p1, float u, string curve ) {
			u = Curve(u, curve);
			return( Lerp(p0, p1, u) );
		}
		static public Vector3 Lerp( Vector3 p0, Vector3 p1, float u ) {
			/*
			Vector3 p01 = Vector3.zero;
			p01.x = (1-u)*p0.x + u*p1.x;
			p01.y = (1-u)*p0.y + u*p1.y;
			p01.z = (1-u)*p0.z + u*p1.z;
			*/
			Vector3 p01 = (1-u)*p0 + u*p1;
			if ( float.IsNaN(p01.x) || float.IsNaN(p01.y) || float.IsNaN(p01.z) ) {
				bool tSplit = XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG;
				XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG = false;
				XnUtils.Print("ERROR: Interp.Lerp(",p0, p1, u,") : Result includes NaN!!!", p01);
				XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG = tSplit;
			}
			return(p01);
		}
		
		
		static public Color LerpList( List<Color> l, float u, string curve ) {
			u = Curve( u, curve );
			return( LerpList( l, u ) );
		}
		static public Color LerpList( List<Color> l, float u ) {
			List<Color> l2;
			if (l.Count> 2) {
				List<Color> left  = l.GetRange(0, l.Count-1);
				List<Color> right = l.GetRange(1, l.Count-1);
				l2 = new List<Color>();
				l2.Add( LerpList( left,  u ) );
				l2.Add( LerpList( right, u ) );
			} else if (l.Count == 2) {
				l2 = l;
			} else {
				return(l[0]);
			}
			return( Lerp( l2[0], l2[1], u ) );
		}
		static public Color Lerp( Color t0, Color t1, float u ) {
			Color t01 = t0*(1f-u) + t1*u;
			return( t01 );
		}
		
		/*==== Generic Method Version
		// This would have worked, except the compiler balked at T * float
		
		static public T LerpList<T>( List<T> l, float u, string curve ) {
			u = Curve( u, curve );
			return( LerpList( l, u ) );
		}
		static public T LerpList<T>( List<T> l, float u ) {
			List<T> l2;
			if (l.Count> 2) {
				List<T> left  = l.GetRange(0, l.Count-1);
				List<T> right = l.GetRange(1, l.Count-1);
				l2 = new List<T>();
				l2.Add( LerpList( left,  u ) );
				l2.Add( LerpList( right, u ) );
			} else if (l.Count == 2) {
				l2 = l;
			} else {
				return(l[0]);
			}
			return( Lerp( l2[0], l2[1], u ) );
		}
		static public T Lerp<T>( T t0, T t1, float u ) {
			T t01 = t0*(1f-u) + t1*u;
			return( t01 );
		}
		*/
		
		
		static public float Lerp( float f0, float f1, float u, string curve ) {
			u = Curve(u, curve);
			return( Lerp(f0, f1, u) );
		}
		static public float Lerp( float f0, float f1, float u ) {
			return( (1-u)*f0 + u*f1 );
		}
		
		
		static public float GetCurveExp( string[] curveArray, float defaultF = 2.0f ) {
			float exp = defaultF;
			if (curveArray.Length == 2 && curveArray[1] != "") {
				exp = float.Parse(curveArray[1] as string);
			}
			return( exp );
		}
		

	    public struct CurveItem {
	        public CurveType   type;
	        public float       num;
	        public CurveItem(CurveType ct=CurveType.linear, float f=1f) {
	            type = ct;
	            num = f;
	        }
	    }

	    static Dictionary<string, List<CurveItem>> curveCache;

	    static void CacheCurve(string eCurve) {
	        List<CurveItem> curveCacheMaker = new List<CurveItem>();
	        string[] curvesArray = eCurve.Split(',');
	        float exp = 2;
	        foreach (string curve in curvesArray) {
	            string[] curveArray = curve.Split(("|")[0]);
	            CurveType ct = CurveType.linear;
	            switch (curveArray[0]) {
	                case "Square":
	                    ct = CurveType.square;
	                    break;
	                case "Power":
	                    ct = CurveType.power;
	                    exp = GetCurveExp( curveArray, 2 );
	                    break;
	                case "Square_Inv":
	                    ct = CurveType.squareInv;
	                    break;
	                case "Pow_Inv":
	                    ct = CurveType.powInv;
	                    exp = GetCurveExp( curveArray, 2 );
	                    break;
	                case "Sin":
	                    ct = CurveType.sin;
	                    exp = GetCurveExp( curveArray, -0.2f );
	                    break;
	                case "Sin_Start":
	                    ct = CurveType.sinStart;
	                    exp = GetCurveExp( curveArray, 0.75f );
	                    break;
	                case "Sin_Stop":
	                    ct = CurveType.sinStop;
	                    exp = GetCurveExp( curveArray, 0.75f );
	                    break;
	                case "Slow_Mid":
	                    ct = CurveType.slowMid;
	                    exp = GetCurveExp( curveArray, 2 );
	                    break;
	                case "In_Out":
		                ct = CurveType.inOut;
		                exp = GetCurveExp(curveArray, 2f);
		                break;
	            }
	            curveCacheMaker.Add(  new CurveItem( ct, exp )  );
	        }
	        curveCache.Add(eCurve, curveCacheMaker);
	    }

	    static public float Curve(float u, CurveType ct, float exp) {
		    return EvaluateCurveItem(u, new CurveItem(ct, exp));
	    }

	    static public float EvaluateCurveItem(float u, CurveItem ci) {
		    switch (ci.type) {
			    case CurveType.linear:
				    return u;
			    case CurveType.square:
				    u = u*u;
				    break;
			    case CurveType.power:
				    // This is because Mathf.Pow( <0, anything ) is NaN
				    if (u < 0) u = 0;
				    u = Mathf.Pow(u, ci.num);
				    break;
			    case CurveType.squareInv: // "Square_Inv":
				    u = 1.0f - ( (1.0f-u) * (1.0f-u) );
				    break;
			    case CurveType.powInv: // "Pow_Inv":
				    if (u > 1) u = 1;
				    u = 1.0f - Mathf.Pow( (1f-u), ci.num);
				    break;
			    case CurveType.sin: // "Sin":
				    u = u + Mathf.Sin(u*Mathf.PI*2.0f)*ci.num;
				    break;
			    case CurveType.sinStart: // "Sin_Start":
				    u = u - Mathf.Sin(u*Mathf.PI)*(ci.num * (1-u));
				    break;
			    case CurveType.sinStop: // "Sin_Stop":
				    u = u + Mathf.Sin(u*Mathf.PI)*(ci.num * (u));
				    break;
			    case CurveType.slowMid: // "Slow_Mid":
				    if (u < 0.5f) {
					    u = 0.5f - ( Mathf.Pow( 1-2*u, ci.num )/2 );
				    } else {
					    u = 0.5f + ( Mathf.Pow( 2*u-1, ci.num )/2 );
				    }
				    break;
			    case CurveType.inOut: // "In_Out": // Interpolates between Pow and Pow_Inv
				    float uIn = Mathf.Pow(u, ci.num);
				    float uOut = 1.0f - Mathf.Pow( (1f-u), ci.num);
				    u = (1 - u) * uIn + u * uOut;
				    break;
		    }
		    return u;
	    }
	    
		static public float Curve(float u, string eCurve) {
	        if (curveCache == null) {
	            curveCache = new Dictionary<string, List<CurveItem>>();
	        }
	        if ( !curveCache.ContainsKey(eCurve) ) {
	            CacheCurve(eCurve);
	        }

	        float u2 = u;
	//        float exp = 2;
	        foreach (CurveItem ci in curveCache[eCurve]) {
		        u2 = EvaluateCurveItem(u2, ci);

	            if ( float.IsNaN(u2) ) {
	                bool tSplit = XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG;
	                XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG = false;
	                XnUtils.Print("ERROR: Interp.Curve(",u, eCurve, ") : Result includes NaN!!!", u2);
	                XnUtils.SPLIT_VECTOR_COMPONENTS_ON_LOG = tSplit;
	            }
	        }

	        return u2;
	    }

	        /*
	        //float tF;
			string[] curvesArray = eCurve.Split(',');
			float exp = 2; // This is the default exponent for all curve functions
			foreach (string curve in curvesArray) {
				string[] curveArray = curve.Split(("|")[0]);
				switch (curveArray[0] as string) {
				case "Square":
					u2 = u2*u2;
					break;
				case "Power":
					// This is because Mathf.Pow( <0, anything ) is NaN
					if (u2 < 0) u2 = 0;
					exp = GetCurveExp( curveArray, 2 );
					u2 = Mathf.Pow(u2, exp);
					break;
				case "Square_Inv":
					u2 = 1.0f - ( (1.0f-u2) * (1.0f-u2) );
					break;
				case "Pow_Inv":
					//if (u2 > 1) Utils.BreakPoint();
					// This is because Mathf.Pow( <0, anything ) is NaN
					if (u2 > 1) u2 = 1;
					exp = GetCurveExp( curveArray, 2 );
					u2 = 1.0f - Mathf.Pow( (1f-u2), exp);
					break;
				case "Sin":
					exp = GetCurveExp( curveArray, -0.2f );
					//if (curveArray[1] == "") curveArray[1] = "-0.2";
					u2 = u2 + Mathf.Sin(u2*Mathf.PI*2.0f)*float.Parse(curveArray[1] as string);
					break;
				case "Sin_Start":
					exp = GetCurveExp( curveArray, 0.75f );
					//if (curveArray[1] == "") curveArray[1] = "0.75";
					//u2 = u - Mathf.Sin(u*Mathf.PI)*float.Parse(curveArray[1] as string);
					u2 = u2 - Mathf.Sin(u2*Mathf.PI)*(float.Parse(curveArray[1] as string) * (1-u2));
					break;
				case "Sin_Stop":
					exp = GetCurveExp( curveArray, 0.75f );
					//if (curveArray[1] == "") curveArray[1] = "0.75";
					u2 = u2 + Mathf.Sin(u2*Mathf.PI)*(float.Parse(curveArray[1] as string) * (u2));
					break;
					/*
					case "Linear":
					default:
						u2 = u;
						break;
					*
				case "Slow_Mid":
					exp = GetCurveExp( curveArray, 2 );
					//if (curveArray[1] == "") curveArray[1] = "2";
					if (u2 < 0.5f) {
						u2 = 0.5f - ( Mathf.Pow( 1-2*u2, exp )/2 );
					} else {
						u2 = 0.5f + ( Mathf.Pow( 2*u2-1, exp )/2 );
					}
					break;
				}
				
				if ( float.IsNaN(u2) ) {
					bool tSplit = Utils.SPLIT_VECTOR_COMPONENTS_ON_LOG;
					Utils.SPLIT_VECTOR_COMPONENTS_ON_LOG = false;
					Utils.Print("ERROR: Interp.Curve(",u, eCurve, ") : Result includes NaN!!!", u2);
					Utils.SPLIT_VECTOR_COMPONENTS_ON_LOG = tSplit;
				}
			}
			
			return(u2);	
		}
		*/
		
		
		
	}


	public class Bezier {
		public List<Vector3>	pts;
		public string			curve = XnInterpolation.LINEAR;
		public List<Vector3>	localScales;
		
		public Bezier() {
			pts = new List<Vector3>();
		}
		
		public Bezier( System.Collections.Generic.IEnumerable<UnityEngine.Vector3> eL ) {
			pts = new List<Vector3>(eL);
		}
		
		public Vector3 Lerp( float u ) {
			return( XnInterpolation.LerpList( pts, u, curve ) );
		}
		public Vector3 Lerp( float u, string eCurve ) {
			return( XnInterpolation.LerpList( pts, u, eCurve ) );
		}
		
		public Vector3 LerpLocalScale( float u ) {
			if (localScales == null) return( Vector3.one );
			return( XnInterpolation.LerpList( localScales, u, curve ) );
		}
		public Vector3 LerpLocalScale( float u, string eCurve ) {
			if (localScales == null) return( Vector3.one );
			return( XnInterpolation.LerpList( localScales, u, eCurve ) );
		}
	}


	public class Spline {
		public List<Vector3>		pts;
		public List<Vector3>		tangents;
		
		private bool				readyForMovePixels = false;
		public float				lastU = 0;
		public float				lastDU = 0.01f;
		public Vector3				lastLoc;
		static public int			movePixelsRecursion = 16;
		
		private float				_eccentricity = 0.5f; // 0.5 is the value for Catmull-Rom splines
		
		private List<float>			_distances;
		public List<float>			uVals;
		public float				magnitude;
		
		public bool					useRelativeUs = true;
		// useRelativeUs determines whether this will evenly divide u among all the spline segments or whether it will 
		// adjust the u for each segment to be relative to the distance between the points. true should make movement
		// along the spline smoother than otherwise.
		
		public List<SplineSubDiv>[]	subDivs;
		
		public Spline() {
			pts = new List<Vector3>();
			tangents = new List<Vector3>();
		}
		
		public Spline( System.Collections.Generic.IEnumerable<UnityEngine.Vector3> ePts ) {
			pts = new List<Vector3>( ePts );
			GenerateTangents();
		}
		public Spline( System.Collections.Generic.IEnumerable<UnityEngine.Vector3> ePts, float eEcc ) {
			pts = new List<Vector3>( ePts );
			GenerateTangents(eEcc);
		}
		public Spline( System.Collections.Generic.IEnumerable<UnityEngine.Vector3> ePts, 
						System.Collections.Generic.IEnumerable<UnityEngine.Vector3> eTans ) {
			pts = new List<Vector3>( ePts );
			tangents = new List<Vector3>( eTans );
			GenerateUVals();
		}
		
		public void GenerateTangents( float eEcc ) {
			_eccentricity = eEcc;
			GenerateTangents();
		}
		public void GenerateTangents() {
			tangents = new List<Vector3>();
			Vector3 p0;
			Vector3 p1;
			Vector3 pT;
			for (int i=0; i<pts.Count; i++) {
				if (i == 0) {
					p0 = pts[i];
					p1 = pts[i+1];
				} else if (i == pts.Count-1) {
					p0 = pts[i-1];
					p1 = pts[i];
				} else {
					p0 = pts[i-1];
					p1 = pts[i+1];
				}
				pT = (p1-p0)*_eccentricity;
				//pT += pts[i];
				tangents.Add(pT);
			}
			GenerateUVals();
			//SubdivideSplineToMaxDistance( 5	 );
		}
		
		public void GenerateUVals() {
			// This function generates the minimum distance (i.e. @ eccentricity = 0) between points to help with smooth movement
			magnitude = 0;
			_distances = new List<float>();
			uVals = new List<float>();
			Vector3 delta;
			float f;
			// Calculate the raw distances between each pair of points.
			for (int i=1; i<pts.Count; i++) {
				delta = pts[i] - pts[i-1];
				_distances.Add(delta.magnitude);
				magnitude += delta.magnitude;
			}
			// Now we can find the various uVals for these
			for (int i=0; i<_distances.Count; i++) {
				f = _distances[i] / magnitude;
				uVals.Add(f);
			}
		}
		
		public Vector3 Lerp( float u, string curve ) {
			u = XnInterpolation.Curve( u, curve );
			return( Lerp( u ) );
		}
		public Vector3 Lerp( float u0 ) {
			// These would prevent extrapolation
			//if (u<=0) return(pts[0]);
			//if (u>=1) return(pts[pts.Count-1]);
			int n = 0;
			float subU = 0;
			float accU = 0;
			
			if (u0 <= 0) {
				n = 0;
				subU = 0;
			} else if (u0 >= 1) {
				n = pts.Count-2;
				subU = 1;
			} else {
				if (useRelativeUs) {
					if (uVals == null) GenerateUVals();
					accU = 0;
					n = -1;
					for (int i=0; i<uVals.Count; i++) {
						if (uVals[i] == 0) continue; // Avoids divide by zero error, but shouldn't exist regardless
						if (accU + uVals[i] > u0) {
							n = i;
							subU = (u0-accU)/uVals[i];
							break;
						}
						accU += uVals[i];
					}
					if (n == -1) {
						XnUtils.Print("ERROR","Interp.Lerp( "+u0+" )","Never found a valid u value.");
						n = 0;
					}
				} else {
					subU = u0 * pts.Count-1;
					n = Mathf.FloorToInt(subU);
					subU -= n; // This removes the n component, leaving a u = [0..1]
				}
			}
			
			Vector3 p = LerpSegment( n, subU );
			return(p);
		}
		
		
		public Vector3 LerpSegment( int n, float u ) {
			// n is the number of the segment
			// u is the interpolation within that segment
			
			// Determine which points we should be Lerping between
			Vector3 p0, p1, t0, t1;p0 = pts[n];
			p1 = pts[n+1];
			t0 = tangents[n];
			t1 = tangents[n+1];
			// Generate Spline basis function values
			float u2 = Mathf.Pow(u,2);
			float u3 = Mathf.Pow(u,3);
			
			float h1 =  2*u3 - 3*u2 + 1;	// calculate basis function 1
			float h2 = -2*u3 + 3*u2;		// calculate basis function 2
			float h3 =    u3 - 2*u2 + u;	// calculate basis function 3
			float h4 =    u3 -   u2;		// calculate basis function 4
			
			Vector3 p =	h1*p0 +				// multiply and sum all funtions
		         		h2*p1 +				// together to build the interpolated
		         		h3*t0 +				// point along the curve.
		         		h4*t1;
			
			//Utils.Log(new string[] { u0.ToString(), n.ToString(), u.ToString(), h1.ToString(), h2.ToString(), h3.ToString(), h4.ToString() });
			
			return(p);						// draw to calculated point on the curve
			
		}
		
		public float eccentricity {
			get {
				return(_eccentricity);
			}
			set {
				GenerateTangents(value);
			}
		}
		
		// First Attempt at MovePixels
		// This attempts to move a set distance along the Spline from the previous position
		// There may be a better way to do this, but for now, it just tries to narrow down the answer movePixelsRecursion times
		public Vector3 MovePixels(float ePix) {
			if (!readyForMovePixels) {
				// Initial setup
				lastU = 0;
				lastDU = 0.1f;
				lastLoc = Lerp(0);
				readyForMovePixels = true;
			}
			float u = lastU + lastDU;
			float dU = lastDU;
			float dDU = dU;
			float mag=0;
			Vector3 loc = Lerp(u);
			for (int i=0; i<movePixelsRecursion; i++) {
				dDU *= 0.5f;
				mag = (loc-lastLoc).magnitude;
				if ( mag > ePix ) {
					u -= dDU;
				} else {
					u += dDU;
				}
				loc = Lerp(u);
			}
	// TODO: This assumes a loop. That probably shouldn't be the case.
			if (u > 1) u-=1;
			lastLoc = loc;
			lastDU = u - lastU;
			Debug.Log("u="+u+"\tdU="+lastDU+"\tePix="+ePix+"\tmag="+mag);
			lastU = u;
			return( loc );
		}
		
		/* This was a bad idea. Actually caused a hiccup in Unity's frame rate on my mac.
		// This will iteratively subdivide the spline into straight segments that are guaranteed to be shorter than maxDist.
		// Enables constant-speed movement along the (approximated) spline.
	// TODO: Make this able to progressively approach the spline rather than calculating the entire thing @ once
		public void SubdivideSplineToMaxDistance( float maxDist ) {
			subDivs = new List<SplineSubDiv>[pts.Count-1];
			SplineSubDiv ssd;
			float maxDistFromPrev;
			float subDivAmt = 0.5f;
			for (int segNum=0; segNum<pts.Count-1; segNum++) {
				if (subDivs[segNum] == null) subDivs[segNum] = new List<SplineSubDiv>();
				// First, generate at least subU = [0, 0.25, 0.5, 0.75, 1] subDivs
				subDivs[segNum].Add( CalculateSplineSubDiv( segNum, 0f ) );
				subDivs[segNum].Add( CalculateSplineSubDiv( segNum, 0.5f ) );
				subDivs[segNum].Add( CalculateSplineSubDiv( segNum, 1f ) );
				maxDistFromPrev = CalculateSplineSubDivDistances( segNum );
				int insertIndex = 0;
				while (maxDistFromPrev > maxDist) {
	// TODO: Should there be another way to break out of this loop?
					subDivAmt *= 0.5f;
					insertIndex = 1;
					for (float f=subDivAmt; f<1; f+=(subDivAmt*2)) { // NOTE: In over 25 years of programming, this is the first float-based for loop I've ever written.
						// NOTE: This skips the ones that have already been created
						//subDivs[segNum].Add( CalculateSplineSubDiv( segNum, f ) );
						try {
							subDivs[segNum].Insert(insertIndex, CalculateSplineSubDiv( segNum, f ) );
						} catch {
							Debug.Log("Error");
						}
						insertIndex += 2;
					}
					// Use LINQ to re-sort the List by subU
					// This may not be needed, and LINQ may be slow
					//subDivs[segNum] = subDivs[segNum].OrderBy(subDiv => subDiv.subU).ToList(); //Example: tris = tris.OrderBy(tr => tr.transform.position.x).ToArray();
					maxDistFromPrev = CalculateSplineSubDivDistances( segNum );
				}
			}
		}
		
		public SplineSubDiv CalculateSplineSubDiv( int segNum, float u ) {
			SplineSubDiv ssd = new SplineSubDiv();
			ssd.pos = LerpSegment( segNum, u );
			ssd.subU = u;
			return( ssd );
		}
		
		public float CalculateSplineSubDivDistances( int segNum ) {
			if (segNum >= subDivs.Length || subDivs[segNum] == null || subDivs[segNum].Count < 2) {
	// TODO: Handle this a little better for the case where subDivs[segNum].Length < 2
				return(0);
			}
			Vector3 prevPos = subDivs[segNum][0].pos;
			Vector3 pos;
			float dist;
			float maxDist = 0;
			SplineSubDiv ssd;
			for (int i=1; i<subDivs[segNum].Count; i++) {
				ssd = subDivs[segNum][i];
				pos = ssd.pos;
				dist = (pos - prevPos).magnitude;
				ssd.distFromPrev = dist;
				subDivs[segNum][i] = ssd;
				maxDist = Mathf.Max( maxDist, dist );
				prevPos = pos;
			}
			return( maxDist );
		}
		*/
					
		
		/*
		// This attempts to move a set distance along the Spline from the previous position
		// Uses uVals to estimate and then tries to narrow down the answer movePixelsRecursion times
		public Vector3 MovePixels(float ePix) {
			if (!readyForMovePixels) {
				// Initial setup
				lastU = 0;
				lastDU = 0.1f;
				lastLoc = Lerp(0);
				readyForMovePixels = true;
			}
			float u = lastU + lastDU;
			float dU = lastDU;
			float dDU = dU;
			float mag=0;
			Vector3 loc = Lerp(u);
			for (int i=0; i<movePixelsRecursion; i++) {
				dDU *= 0.5f;
				mag = (loc-lastLoc).magnitude;
				if ( mag > ePix ) {
					u -= dDU;
				} else {
					u += dDU;
				}
				loc = Lerp(u);
			}
	// TODO: This assumes a loop. That probably shouldn't be the case.
			if (u > 1) u-=1;
			lastLoc = loc;
			lastDU = u - lastU;
			Debug.Log("u="+u+"\tdU="+lastDU+"\tePix="+ePix+"\tmag="+mag);
			lastU = u;
			return( loc );
		}
		*/
	}


	public struct SplineSubDiv {
		public Vector3		pos;
		public float		subU;
		public float		distFromPrev;
	}



}









