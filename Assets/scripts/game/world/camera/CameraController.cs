using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Camera CurrentCamera;

    public Ray ray;
    public RaycastHit lastHit;
    private float mNextRaycast = 0f;

    private readonly float _mouseDragThreshold = 4f;
    private readonly float _mouseClickThreshold = 10f;
    private readonly float _touchDragThreshold = 80f;
    private readonly float _touchClickThreshold = 80f;
    private readonly float _rangeDistance = -1f;
    public LayerMask eventReceiverMask = -1;

    private MouseOrTouch[] mMouse = new MouseOrTouch[] { new MouseOrTouch(), new MouseOrTouch(), new MouseOrTouch() };
    private GameObject mHover;

    public MouseOrTouch currentTouch = null;
    public GameObject hoveredObject;
    public Vector3 lastTouchPosition = Vector3.zero;

    public delegate void MoveDelegate( Vector3 delta );
    public delegate void VoidDelegate( GameObject go );
    public delegate void BoolDelegate( GameObject go, bool state );
    public delegate void FloatDelegate( GameObject go, float delta );
    public delegate void VectorDelegate( GameObject go, Vector3 delta );
    public delegate void ObjectDelegate( GameObject go, GameObject obj );
    public delegate void KeyCodeDelegate( GameObject go, KeyCode key );

    public VoidDelegate OnClick;
    public VoidDelegate OnDoubleClick;
    public BoolDelegate OnHover;
    public BoolDelegate OnPress;
    public BoolDelegate OnSelect;
    public VectorDelegate OnDrag;
    public VoidDelegate OnDragStart;
    public ObjectDelegate OnDragOver;
    public ObjectDelegate OnDragOut;
    public VoidDelegate OnDragEnd;
    public ObjectDelegate OnDrop;
    public MoveDelegate OnMouseMove;

    static public bool isDragging = false;
    private bool _dragIsCanceled;
    private bool _moveToObject;

    private bool _moveStartCamera;
    public float moveSlowDownTime;
    private bool _moveIsStarted;
    public float MoveStartedMagnitude = 0.001f;
    private Vector3 _moveTouchPosition;
    private Vector3 _moveTouchDelta;
    private Vector3 _moveDelta;

    private bool _onDraggedFromObject;

    static GameObject mCurrentSelection = null;
    static GameObject mNextSelection = null;


    public float deltaMultiplaier = 1f;
    public float CameraZoomMultiplaier = 25f;

    private Tween _cameraReposition;

    static public GameObject SelectedObject
    {
        get
        {
            return mCurrentSelection;
        }
        set
        {
            SetSelection( value );
        }
    }

    static protected void SetSelection( GameObject go )
    {
        if ( mNextSelection != null )
        {
            mNextSelection = go;
        }
        else if ( mCurrentSelection != go )
        {
            mNextSelection = go;
        }
    }

    private bool _notifying = false;

    public void Initialization()
    {
    }

    public void DoUpdateFrame()
    {
        if ( IsPointerOverUIElement( GetEventSystemRaycastResults() ) == false )
        {
            ProcessMouse();
            ProccesMoveCamera();
        }
    }

    private bool IsPointerOverUIElement( List<RaycastResult> eventSystemRaysastResults )
    {
        for ( int index = 0; index < eventSystemRaysastResults.Count; index++ )
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[ index ];
            if ( curRaysastResult.gameObject.layer == 5 )
            {
                return true;
            }
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData( EventSystem.current )
        {
            position = Input.mousePosition
        };
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );
        return raysastResults;
    }

    void ProccesMoveCamera()
    {
        if ( Input.GetMouseButton( 0 ) == true )
        {
            if ( _moveStartCamera == false )
            {
                _moveStartCamera = true;
                _moveTouchPosition = Input.mousePosition;
                _moveTouchDelta = Vector3.zero;
                _moveIsStarted = false;
            }
            else
            {
                lastTouchPosition = Input.mousePosition;
                lastTouchPosition.z = _moveTouchPosition.z = CurrentCamera.transform.position.y;//200f;//44.4f;
                _moveTouchDelta = CurrentCamera.ScreenToWorldPoint( lastTouchPosition ) - CurrentCamera.ScreenToWorldPoint( _moveTouchPosition );
                _moveTouchPosition = Input.mousePosition;

                if ( _moveTouchDelta.sqrMagnitude > MoveStartedMagnitude && _moveIsStarted == false )
                {
                    _moveIsStarted = true;
                    UIHandler.Instance.BaseHideAll();

                    if ( null != mCurrentSelection )
                    {
                        OnSelect?.Invoke( mCurrentSelection, false );
                        Notify( mCurrentSelection, "OnSelect", false );
                        mNextSelection = null;
                        mCurrentSelection = null;
                    }
                }
            }
        }
        else if ( Input.GetMouseButtonUp( 0 ) == true )
        {
            _moveStartCamera = false;
        }
        _moveDelta = _moveTouchDelta * deltaMultiplaier;// mPressed ? _moveTouchDelta : Vector3.Lerp( _moveDelta, Vector3.zero, Time.deltaTime * moveSlowDownTime );

        Vector3 offset = Vector3.Scale( _moveDelta, -Vector3.one );

        Vector3 cameraCurrentPosition = CurrentCamera.transform.position;
        cameraCurrentPosition += new Vector3( offset.x, 0f, offset.z );

        CurrentCamera.transform.position = cameraCurrentPosition;

        float scrollWheelChange = Input.GetAxis( "Mouse ScrollWheel" );
        if ( scrollWheelChange != 0 )
        {
            ZoomByDistanceConstrain( scrollWheelChange );
            UIHandler.Instance.BaseHideAll();
        }
        CurrentCamera.transform.position = MoveBorderConstrain( CurrentCamera.transform.position );
    }

    public void ProcessMouse()
    {
        mMouse[ 0 ].delta = Input.mousePosition - ( Vector3 )mMouse[ 0 ].pos;
        mMouse[ 0 ].pos = Input.mousePosition;
        bool posChanged = mMouse[ 0 ].delta.sqrMagnitude > 0.001f;
        for ( int i = 1; i < 3; ++i )
        {
            mMouse[ i ].pos = mMouse[ 0 ].pos;
            mMouse[ i ].delta = mMouse[ 0 ].delta;
        }
        bool isPressed = false;
        for ( int i = 0; i < 3; ++i )
        {
            if ( Input.GetMouseButtonDown( i ) == true )
            {
                isPressed = true;
            }
            else if ( Input.GetMouseButton( i ) == true )
            {
                isPressed = true;
            }
        }
        if ( isPressed == true || posChanged == true || mNextRaycast < RealTime.time )
        {
            mNextRaycast = RealTime.time + 0.02f;
            if ( Raycast( Input.mousePosition ) == false )
            {
                //hoveredObject = fallThrough;
            }
            for ( int i = 0; i < 3; ++i )
            {
                mMouse[ i ].current = hoveredObject;
            }
        }
        bool highlightChanged = ( mMouse[ 0 ].last != mMouse[ 0 ].current );
        if ( highlightChanged == true )
        {
        }
        if ( isPressed == true )
        {
        }
        if ( OnMouseMove != null )
        {
            currentTouch = mMouse[ 0 ];
            OnMouseMove( currentTouch.delta );
            currentTouch = null;
        }
        if ( mHover != null && highlightChanged == true )
        {
            currentTouch = mMouse[ 0 ];
            OnHover?.Invoke( mHover, false );
            Notify( mHover, "OnHover", false );
            mHover = null;
        }
        for ( int i = 0; i < 3; ++i )
        {
            bool pressed = Input.GetMouseButtonDown( i );
            bool unpressed = Input.GetMouseButtonUp( i );
            if ( pressed || unpressed )
            {
            }
            currentTouch = mMouse[ i ];
            if ( pressed == true )
            {
                currentTouch.pressedCam = CurrentCamera;
            }
            else if ( currentTouch.pressed != null )
            {
                //CurrentCamera = currentTouch.pressedCam;
            }
            ProcessTouch( pressed, unpressed );
        }
        if ( isPressed == false && highlightChanged == true )
        {
            mHover = mMouse[ 0 ].current;
            currentTouch = mMouse[ 0 ];
            OnHover?.Invoke( mHover, true );
            Notify( mHover, "OnHover", true );
        }
        currentTouch = null;
        mMouse[ 0 ].last = mMouse[ 0 ].current;
        for ( int i = 1; i < 3; ++i )
        {
            mMouse[ i ].last = mMouse[ 0 ].last;
        }
    }

    public void ProcessTouch( bool pressed, bool unpressed )
    {
        bool isMouse = false;
        float drag = isMouse ? _mouseDragThreshold : _touchDragThreshold;
        float click = isMouse ? _mouseClickThreshold : _touchClickThreshold;
        drag *= drag;
        click *= click;
        currentTouch.totalDelta += currentTouch.delta;

        if ( pressed == true )
        {
            currentTouch.pressStarted = true;
            OnPress?.Invoke( currentTouch.pressed, false );
            Notify( currentTouch.pressed, "OnPress", false );
            currentTouch.pressed = currentTouch.current;
            currentTouch.dragged = currentTouch.current;
            currentTouch.totalDelta = Vector2.zero;
            currentTouch.dragStarted = false;
            OnPress?.Invoke( currentTouch.pressed, true );
            Notify( currentTouch.pressed, "OnPress", true );
            if ( currentTouch.pressed != mCurrentSelection )
            {
                SelectedObject = currentTouch.pressed;
            }
        }
        else if ( currentTouch.pressed != null )
        {
            float mag = currentTouch.totalDelta.sqrMagnitude;
            bool justStarted = false;
            if ( !currentTouch.dragStarted && currentTouch.last != currentTouch.current )
            {
                justStarted = true;//myy
                currentTouch.dragStarted = true;
                currentTouch.delta = currentTouch.totalDelta;
                isDragging = true;
                OnDragStart?.Invoke( currentTouch.dragged );
                Notify( currentTouch.dragged, "OnDragStart", null );
                OnDragOver?.Invoke( currentTouch.last, currentTouch.dragged );
                Notify( currentTouch.last, "OnDragOver", currentTouch.dragged );
                isDragging = false;
            }
            else if ( currentTouch.dragStarted == false )
            {
                justStarted = true;
                currentTouch.dragStarted = true;
                currentTouch.delta = currentTouch.totalDelta;
            }
            if ( currentTouch.dragStarted == true )
            {
                isDragging = true;
                if ( justStarted == true )
                {
                    OnDragStart?.Invoke( currentTouch.dragged );
                    Notify( currentTouch.dragged, "OnDragStart", null );
                    OnDragOver?.Invoke( currentTouch.last, currentTouch.dragged );
                    Notify( currentTouch.current, "OnDragOver", currentTouch.dragged );
                }
                else if ( currentTouch.last != currentTouch.current )
                {
                    OnDragStart?.Invoke( currentTouch.dragged );
                    Notify( currentTouch.last, "OnDragOut", currentTouch.dragged );
                    OnDragOver?.Invoke( currentTouch.last, currentTouch.dragged );
                    Notify( currentTouch.current, "OnDragOver", currentTouch.dragged );
                }
                OnDrag?.Invoke( currentTouch.dragged, currentTouch.delta );
                Notify( currentTouch.dragged, "OnDrag", currentTouch.pos );
                currentTouch.last = currentTouch.current;
                isDragging = false;
            }
            if ( drag < currentTouch.totalDelta.sqrMagnitude && _onDraggedFromObject == false )
            {
                OnDragEnd?.Invoke( currentTouch.dragged );
                Notify( currentTouch.dragged, "OnDragEnd", null );
                OnPress?.Invoke( currentTouch.pressed, false );
                Notify( currentTouch.pressed, "OnPress", false );
                currentTouch.dragStarted = false;
                isDragging = false;
                _dragIsCanceled = true;
            }
        }
        if ( unpressed == true )
        {
            currentTouch.pressStarted = false;
            if ( currentTouch.pressed != null )
            {
                if ( currentTouch.dragStarted == true )
                {
                    OnDragOut?.Invoke( currentTouch.last, currentTouch.dragged );
                    Notify( currentTouch.last, "OnDragOut", currentTouch.dragged );
                    OnDragEnd?.Invoke( currentTouch.dragged );
                    Notify( currentTouch.dragged, "OnDragEnd", null );
                }
                if ( _dragIsCanceled == false )
                {
                    OnPress?.Invoke( currentTouch.pressed, false );

                    Notify( currentTouch.pressed, "OnPress", false );
                }
                if ( isMouse == true )
                {
                    OnHover?.Invoke( currentTouch.current, true );
                    Notify( currentTouch.current, "OnHover", true );
                }
                mHover = currentTouch.current;
                if ( currentTouch.dragged == currentTouch.current || ( currentTouch.totalDelta.sqrMagnitude < drag ) )
                {
                    if ( currentTouch.pressed != mCurrentSelection )
                    {
                        if ( null != mCurrentSelection )
                        {
                            OnSelect?.Invoke( mCurrentSelection, false );
                            Notify( mCurrentSelection, "OnSelect", false );
                            mNextSelection = null;
                            mCurrentSelection = null;
                        }
                        mNextSelection = null;
                        mCurrentSelection = currentTouch.pressed;
                        OnSelect?.Invoke( currentTouch.pressed, true );
                        Notify( currentTouch.pressed, "OnSelect", true );
                    }
                    else
                    {
                        mNextSelection = null;
                        mCurrentSelection = currentTouch.pressed;
                    }
                    if ( currentTouch.pressed == currentTouch.current && _dragIsCanceled == false )
                    {
                        float time = RealTime.time;
                        OnClick?.Invoke( currentTouch.pressed );
                        if ( _onDraggedFromObject == false )//temp
                        {
                            Notify( currentTouch.pressed, "OnClick", null );
                        }
                        if ( currentTouch.clickTime + 0.35f > time )
                        {
                            OnDoubleClick?.Invoke( currentTouch.pressed );
                            _moveToObject = true;
                            Notify( currentTouch.pressed, "OnDoubleClick", null );
                        }
                        currentTouch.clickTime = time;
                    }
                }
                else if ( currentTouch.dragStarted )
                {
                    OnDrop?.Invoke( currentTouch.current, currentTouch.dragged );
                    Notify( currentTouch.current, "OnDrop", currentTouch.dragged );
                }
            }
            else if ( currentTouch.totalDelta == Vector2.zero )
            {
                UIHandler.Instance.BaseHideAll();
            }
            currentTouch.dragStarted = false;
            currentTouch.pressed = null;
            currentTouch.dragged = null;
            _onDraggedFromObject = false;

        }
        _dragIsCanceled = false;
    }

    public bool Raycast( Vector3 inPos )
    {
        //Vector3 pos = CurrentCamera.ScreenToViewportPoint( inPos );
        ray = CurrentCamera.ScreenPointToRay( inPos );
        int mask = CurrentCamera.cullingMask & ( int )eventReceiverMask;
        float dist = ( _rangeDistance > 0f ) ? _rangeDistance : CurrentCamera.farClipPlane - CurrentCamera.nearClipPlane;
        if ( Physics.Raycast( ray, out lastHit, dist, mask ) == true )
        {
            //Vector3 lastWorldPosition = lastHit.point;
            if ( lastHit.collider.gameObject != null )
            {
                hoveredObject = lastHit.collider.gameObject;
                return true;
            }
        }
        return false;
    }

    public void Notify( GameObject go, string funcName, object obj )
    {
        if ( go == null || _notifying )
        {
            return;
        }
        _notifying = true;
        if ( go.activeSelf == true )
        {
            go.SendMessage( funcName, obj, SendMessageOptions.DontRequireReceiver );
        }
        _notifying = false;
    }

    public void CameraRepositionToPoint( Vector3 position )
    {
        if ( _moveToObject == true )
        {
            return;
        }
        _moveToObject = true;

        float b = ( ( CurrentCamera.transform.position.y ) - CurrentCamera.transform.position.y * -0.14f ) / Mathf.Tan( 45f );
        position = new Vector3( position.x - b, CurrentCamera.transform.position.y, position.z - b );
        _cameraReposition = CurrentCamera.transform.DOMove( position, 0.45f ).OnComplete( () => { _moveToObject = false; } );
    }

    public void ZoomByDistanceConstrain( float scrollWheelChange )
    {
        float R = scrollWheelChange * CameraZoomMultiplaier;
        float PosX = CurrentCamera.transform.eulerAngles.x + 90;
        float PosY = -1 * ( CurrentCamera.transform.eulerAngles.y - 90 );
        PosX = PosX / 180 * Mathf.PI;
        PosY = PosY / 180 * Mathf.PI;
        float X = R * Mathf.Sin( PosX ) * Mathf.Cos( PosY );
        float Z = R * Mathf.Sin( PosX ) * Mathf.Sin( PosY );
        float Y = R * Mathf.Cos( PosX );
        CurrentCamera.transform.position = new Vector3( CurrentCamera.transform.position.x + X, CurrentCamera.transform.position.y + Y, CurrentCamera.transform.position.z + Z );
    }

    Vector3 MoveBorderConstrain( Vector3 position )
    {
        Vector2 conXRes = new Vector2(
                ( -600f - ( position.z / 180f * Mathf.PI ) ) * ( 750f / position.y ), //( position.z * 0.14f )
                ( 100f + ( position.z / 180f * Mathf.PI ) ) * ( 750f / position.y ) //( position.z * 0.14f )
                );
        Vector2 conZRes = new Vector2(
                ( -700f - ( position.z / 180f * Mathf.PI ) ) * ( 750f / position.y ), //( position.z * 0.14f )
                ( 100f + ( position.z / 180f * Mathf.PI ) ) * ( 750f / position.y ) //( position.z * 0.14f )
                );
        return new Vector3(
            Mathf.Clamp( position.x, conXRes.x, conXRes.y ),
            Mathf.Clamp( position.y, 150f, 750f ),
            Mathf.Clamp( position.z, conZRes.x, conZRes.y ) );
    }
}

[Serializable]
public class MouseOrTouch
{
    public Vector2 pos;
    public Vector2 lastPos;
    public Vector2 delta;
    public Vector2 totalDelta;
    public Camera pressedCam;
    public GameObject last;
    public GameObject current;
    public GameObject pressed;
    public GameObject dragged;
    public float clickTime = 0f;
    public bool touchBegan = true;
    public bool pressStarted = false;
    public bool dragStarted = false;
}