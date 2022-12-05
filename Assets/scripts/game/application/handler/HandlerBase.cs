using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HandlerBase<T> : MonoBehaviour where T : UnityEngine.Object
{
    private static T _instance = null;
    private static System.Object _sLock = new System.Object();

    public bool IsInited;
    public bool LogEnabled = false;

    public static T Instance
    {
        get
        {
            if ( null != _instance )
            {
                return _instance;
            }
            lock ( _sLock )
            {
                UnityEngine.Object[] foundObjects = UnityEngine.Object.FindObjectsOfType( typeof( T ) );
                if ( null == foundObjects || foundObjects.Length == 0 )
                {
                    HandlerRegistry.Instance.LogError( "Handler does not found : " + typeof( T ).Name );
                    return null;
                }
                _instance = foundObjects[ 0 ] as T;
                if ( foundObjects.Length >= 2 )
                {
                }
                return _instance;
            }
        }
    }

    public void Initialization()
    {
        if ( IsInited == false )
        {
            IsInited = true;
            LoadData();
        }
    }

    public virtual void LoadData()
    {
    }

    public void Log( object _message )
    {
        if ( LogEnabled == true )
        {
            Debug.Log( _message );
        }
    }

    public void LogError( object _message )
    {
        if ( LogEnabled == true )
        {
            Debug.LogError( _message );
        }
    }

    public void LogWarning( object _message )
    {
        if ( LogEnabled == true )
        {
            Debug.LogWarning( _message );
        }
    }
}