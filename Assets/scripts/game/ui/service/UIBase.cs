using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIBase : MonoBehaviour
{
    public string UIName;

    public UIPanelPrefab prefab;
    [HideInInspector]
    public bool IsInited;
    [HideInInspector]
    public bool IsShowed;
    [HideInInspector]
    public bool IsResizeble;

    [HideInInspector]
    public bool IsChild;
    [HideInInspector]
    public UIBase PreviousUI;

    public GameObject UIPanel;

    [HideInInspector]
    public bool LogEnabled = false;


    public UIBase()
    {
    }

    public void Initialization()
    {
        if ( IsInited == false )
        {
            if ( string.IsNullOrEmpty( UIName ) == true )
            {
                HandlerRegistry.Instance.LogError( "UI name is empty." );
            }
            prefab.SetUI( this );
            UIHandler.Instance.UIAdd( this );

            if ( prefab.ShowInitialize == true )
            {
                return;
            }
            LoadData();
        }
    }

    public virtual void LoadData()
    {
        IsInited = true;
        prefab.LoadData();
    }

    public virtual void Show()
    {
        IsShowed = true;
        prefab.Show();
    }

    public virtual void Show<T>( T data )
    {
        IsShowed = true;
        prefab.Show();
    }

    public virtual void Hide()
    {
        IsShowed = false;
        prefab.Hide();
    }

    public virtual void Remove()
    {
        UIHandler.Instance.UIRemove( UIName );
        UnityEngine.Object.Destroy( UIPanel );
    }

    public virtual void UIUpdateEveryFrame()
    {
        if ( IsInited == false )
        {
            return;
        }
    }

    public virtual void UIUpdateAfterFrame()
    {

    }

    public virtual void UIUpdatePhysics()
    {

    }

    public virtual void UIUpdateOnce()
    {
        if ( IsInited == false )
        {
            return;
        }
    }

    public virtual void UIUpdateLanguage()
    {
        if ( IsInited == false )
        {
            return;
        }
    }

    public virtual void UIUpdateSize()
    {
        if ( IsInited == false )
        {
            return;
        }
    }

    public virtual void UIPrefabLoadComplete()
    {
        if ( IsInited == false )
        {
            return;
        }
    }

    public virtual void UIPrefabRemoveComplete()
    {
    }

    public virtual void UIDataSet( System.Object uiData )
    {

    }



    public void Log( object message )
    {
        if ( LogEnabled == true )
        {
            UIHandler.Instance.Log( message );
        }
    }

    public void LogError( object message )
    {
        if ( LogEnabled == true )
        {
            UIHandler.Instance.LogError( message );
        }
    }

    public void LogWarning( object message )
    {
        if ( LogEnabled == true )
        {
            UIHandler.Instance.LogWarning( message );
        }
    }
}