using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameHandler : HandlerBase<GameHandler>
{
    public override void LoadData()
    {
        UIHandler.Instance.Initialization();

        UIHandler.Instance.UIGet( "UIPanelMainMenu" ).Show();

        UIHandler.Instance.UIGet( "UIPanelLoading" ).Hide();
    }

    public void ContinueLoadData()
    {

        CameraHandler.Instance.Initialization();

        MapHandler.Instance.Initialization();

        InventoryHandler.Instance.Initialization();

        TimeActionHandler.Instance.Initialization();

        LoadGame();

        this.Delay( () => { UIHandler.Instance.UIGet( "UIPanelLoading" ).Hide(); }, 0.5f );
    }

    public void DoLoadAwake()
    {
        UIHandler.Instance.DoLoadAwake();
    }

    public void DoLoadStart()
    {
        UIHandler.Instance.DoLoadStart();
    }

    public void DoUpdateFrame()
    {
        UIHandler.Instance.UIUpdateEveryFrame();
        CameraHandler.Instance.DoUpdateFrame();
        TimeActionHandler.Instance.DoUpdateFrame();
        CharacterHandler.Instance.DoUpdateFrame();
    }

    public void DoUpdateFrameEnd()
    {
        UIHandler.Instance.UIUpdateAfterFrame();
    }

    public void DoUpdateFixed()
    {
        UIHandler.Instance.UIUpdatePhysics();
    }

    ////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////



    public void LoadGame()
    {
        MapHandler.Instance.Initialization();
        //MapHandler.Instance.SetMapCellsCount( 15, 15 );
        MapHandler.Instance.LoadMap();
    }
}