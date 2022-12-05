using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapFieldTree : MapField
{
    public new void Initialization()
    {
        base.Initialization();
    }

    public new void OnClick()
    {
        base.OnClick();

        CameraHandler.Instance.EnvironmentCameraController.CameraRepositionToPoint( this.transform.position );
        UIHandler.Instance.UIGet( "UIPanelProductionProgressBar" ).Hide();
        UIHandler.Instance.UIGet( "UIPanelFieldProduction" ).Hide();

        CharacterHandler.Instance.CurrentCharacter.SetDestination( () => { }, transform.position );
    }

    public new void OnSelect( bool selected )
    {
        base.OnSelect( selected );
        Debug.Log( "OnSelect " + selected );
    }
}