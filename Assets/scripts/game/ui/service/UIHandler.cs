using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : HandlerBase<UIHandler>
{
    public override void LoadData()
    {
        CollectionUI = new Dictionary<string, UIBase>();
        EventsUpdateFrame = new List<UIBase>();
        EventsUpdateFrameEnd = new List<UIBase>();
        EventsUpdateFixed = new List<UIBase>();

        EventsUpdateOnce = new List<UIBase>();

        UIBase[] foundUI = SceneHandler.Instance.RootUI.GetComponentsInChildren<UIBase>();
        for ( int i = 0; i < foundUI.Length; i++ )
        {
            foundUI[ i ].Initialization();
        }
    }

    public Dictionary<string, UIBase> CollectionUI;

    public List<UIBase> EventsUpdateFrame;
    public List<UIBase> EventsUpdateFrameEnd;
    public List<UIBase> EventsUpdateFixed;

    public List<UIBase> EventsUpdateOnce;

    public string[] FirstShowedUI;

    public List<UIBase> UIOpened;
    public List<UIBase> UIRolledUP;

    public List<UIBase> Temp = new List<UIBase>();


    public UIBase UIGet( string uiName )
    {
        if ( string.IsNullOrEmpty( uiName ) == true )
        {
            throw new ArgumentNullException( "UI name " );
        }

        if ( CollectionUI.ContainsKey( uiName ) == true )
        {
            return CollectionUI[ uiName ];
        }
        else
        {
            HandlerRegistry.Instance.LogError( "UI " + uiName + " not in scene." );
        }
        return null;
    }

    public T UIGet<T>( string uiName ) where T : class
    {
        return UIGet( uiName ) as T;
    }

    public void UIAdd( UIBase ui )
    {
        if ( null == ui )
        {
            throw new ArgumentNullException( "UI" );
        }
        lock ( CollectionUI )
        {
            if ( CollectionUI.ContainsKey( ui.UIName ) == true )
            {
                throw new ArgumentException( "UI is already registered", ui.UIName );
            }
            CollectionUI.Add( ui.UIName, ui );
            Temp.Add( ui );
        }
    }

    public void UIRemove( string uiName )
    {
        if ( string.IsNullOrEmpty( uiName ) == true )
        {
            throw new ArgumentNullException( "UI name" );
        }
        lock ( CollectionUI )
        {
            if ( CollectionUI.ContainsKey( uiName ) == true )
            {
                CollectionUI.Remove( uiName );
            }
        }
    }


    public void DoLoadAwake()
    {
    }

    public void DoLoadStart()
    {
    }

    public void ShowFirstUI()
    {
        for ( int i = 0; i < FirstShowedUI.Length; i++ )
        {
            UIBase ui = UIGet( FirstShowedUI[ i ] );
            if ( null != ui )
            {
                ui.Show();
            }
        }
    }

    public void UIUpdateEveryFrame()
    {
        for ( int i = 0; i < EventsUpdateFrame.Count; i++ )
        {
            EventsUpdateFrame[ i ].UIUpdateEveryFrame();
        }
    }

    public void UIUpdateAfterFrame()
    {
        for ( int i = 0; i < EventsUpdateFrameEnd.Count; i++ )
        {
            EventsUpdateFrameEnd[ i ].UIUpdateAfterFrame();
        }
        for ( int i = 0; i < EventsUpdateOnce.Count; i++ )
        {
            System.Action continuationAction = EventsUpdateOnce[ i ].UIUpdateOnce;
            EventsUpdateOnce.RemoveAt( i );
            continuationAction();
        }
    }

    public void UIUpdatePhysics()
    {
        for ( int i = 0; i < EventsUpdateFixed.Count; i++ )
        {
            EventsUpdateFixed[ i ].UIUpdatePhysics();
        }
    }

    public void UIUpdateFrameAdd( UIBase ui )
    {
        EventsUpdateFrame.Add( ui );
    }

    public void UIUpdateFrameEndAdd( UIBase ui )
    {
        EventsUpdateFrameEnd.Add( ui );
    }

    public void UIUpdateFixedAdd( UIBase ui )
    {
        EventsUpdateFixed.Add( ui );
    }

    public void UIUpdateOnceAdd( UIBase ui )
    {
        EventsUpdateOnce.Add( ui );
    }

    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    ///

    public void AddOpenedUI( UIBase uIBase )
    {
        if ( UIOpened.Contains( uIBase ) == false )
        {
            UIOpened.Add( uIBase );
        }
    }

    public void RemoveOpenedUI( UIBase uIBase )
    {
        if ( UIOpened.Contains( uIBase ) )
        {
            UIOpened.Remove( uIBase );
        }
    }

    public void RollUpAllButThis( UIBase uIBase, string withoutUI = null )
    {
        IEnumerable<KeyValuePair<string, UIBase>> filteredUI = null;
        if ( string.IsNullOrEmpty( withoutUI ) == false )
        {
            filteredUI = CollectionUI.Where( ui => ui.Value.IsShowed == true && ui.Value.IsChild == false && ui.Value.UIName != uIBase.UIName
            && ui.Value.UIName != withoutUI );
        }
        else
        {
            filteredUI = CollectionUI.Where( ui => ui.Value.IsShowed == true && ui.Value.IsChild == false && ui.Value.UIName != uIBase.UIName );
        }

        //var filteredUI = CollectionUI.Where(ui => ui.Value.IsShowed == true && ui.Value.IsChild == false && ui.Value.uiName != uIBase.uiName);

        //if (string.IsNullOrEmpty(withoutUI) == false)
        //{
        //    filteredUI.Where(ui => ui.Value.uiName != withoutUI);
        //}

        foreach ( var filtered in filteredUI )
        {
            UIBase ui = filtered.Value;
            ui.Hide();
            UIRolledUP.Add( ui );
        }
    }

    public void ShowRolledUp()
    {
        foreach ( UIBase ui in UIRolledUP )
        {
            ui.Show();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////
    ///

    public void BaseShow( UIBase uIBase, bool addToOpened = false )
    {
        if ( null != uIBase )
        {
            uIBase.UIPanel.SetActive( true );
            if ( addToOpened == true )
            {
                AddOpenedUI( uIBase );
            }
        }
    }

    public void BaseHide( UIBase uIBase, bool removeFromOpened = false )
    {
        if ( null != uIBase )
        {
            uIBase.UIPanel.SetActive( false );
            if ( removeFromOpened == true )
            {
                RemoveOpenedUI( uIBase );
            }
        }
    }

    public void BaseHideAll()
    {
        if ( null != UIOpened )
        {
            UIOpened.ForEach( x =>
            {
                //x.UIPanel.SetActive( false );
                x.Hide();
            } );
            UIOpened.Clear();
        }
    }
}