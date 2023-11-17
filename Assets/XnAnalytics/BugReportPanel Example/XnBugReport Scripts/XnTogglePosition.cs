using UnityEngine;
using UnityEngine.Events;

namespace XnBugReport
{
    public class XnTogglePosition : MonoBehaviour {
        public enum eState {
            pos0, pos1, moving0,
            moving1, undefined
        };

        public Vector3 pos0,         pos1;
        public float   scale0   = 1, scale1 = 1;
        public float   duration = 1;
        [Tooltip( "From Easing Terms include: Linear, In, Out, InOut, Sin, SinIn, & SinOut. " +
                  "All can be followed by a | and a modifier number, "                        +
                  "and multiple easing operations can be comma separated." )]
        public string easing = "Sin|-0.2";
        public eState     state         = eState.pos0;
        public bool       toggleOnClick = true;
        public UnityEvent alsoToggle;

        protected Transform     trans;
        protected RectTransform rTrans;
        protected float         t0;

        protected virtual void Awake() {
            trans = transform;
            rTrans = GetComponent<RectTransform>();
        }

        public void Toggle() {
            Toggle( eState.undefined );
        }

        public void Toggle( eState toState ) {
            if ( !enabled ) return;
            switch ( toState ) {
            case eState.pos0:
            case eState.pos1:
                state = ( toState == eState.pos0 ) ? eState.moving0 : eState.moving1;
                alsoToggle.Invoke();
                t0 = Time.time;
                break;

            case eState.undefined:
                switch ( state ) {
                case eState.pos0:
                    state = eState.moving1;
                    alsoToggle.Invoke();
                    t0 = Time.time;
                    break;

                case eState.pos1:
                    state = eState.moving0;
                    alsoToggle.Invoke();
                    t0 = Time.time;
                    break;

                default:
                    // Don't do anything while moving
                    return;
                }

                break;

            default:
                Debug.LogError( $"Invalid toState of " + toState );
                break;
            }
        }

        protected virtual void OnMouseUpAsButton() {
            // block button clicks that have already been handled by uGUI
            if ( XnPointerOverUI.OVER_UI ) return;

            if ( !enabled ) return;
            if ( toggleOnClick ) Toggle();
        }

        protected virtual void Update() {
            if ( state == eState.moving0 || state == eState.moving1 ) {
                float u = ( Time.time - t0 ) / duration;
                if ( u < 0 ) u = 0;
                eState nextState = state;
                if ( u >= 1 ) {
                    u = 1;
                    nextState = ( state == eState.moving0 ) ? eState.pos0 : eState.pos1;
                }

                u = Easing.Ease( u, easing );
                Vector3 pos;
                float scale;
                if ( state == eState.moving0 ) {
                    pos = ( 1   - u ) * pos1   + u * pos0;
                    scale = ( 1 - u ) * scale1 + u * scale0;
                } else {
                    pos = ( 1   - u ) * pos0   + u * pos1;
                    scale = ( 1 - u ) * scale0 + u * scale1;
                }

                if ( rTrans == null ) {
                    trans.localPosition = pos;
                    trans.localScale = Vector3.one * scale;
                } else {
                    rTrans.anchoredPosition = pos;
                    rTrans.localScale = Vector3.one * scale;
                }

                state = nextState;
            }
        }

        public bool isMoving {
            get { return ( state == eState.moving0 || state == eState.moving1 ); }
        }
    }
}